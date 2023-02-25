using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Pokepanion.Library.Helpers;

namespace Pokepanion.Library.GameProcessing;

public class GameWindow {

    private readonly Process process;

    /// <returns>The title of the window.</returns>
    public string Title => process.MainWindowTitle;

    /// <returns>The window's client area rect, not scaled for zoomed displays.</returns>
    public Rectangle ClientRect => Win32Api.GetClientRect(process.MainWindowHandle);

    /// <value>The window's client area rect, scaled according to the nearest display's zoom level.</value>
    public Rectangle ScaledClientRect {
        get {
            Rectangle clientRect = ClientRect;

            var getMonitorFlag = Win32Api.DefaultMonitorFlag.DEFAULT_TO_NEAREST;
            var monitorPtr = Win32Api.MonitorFromWindow(process.MainWindowHandle, getMonitorFlag);

            var scalingFac = Win32Api.GetScaleFactorForMonitor(monitorPtr);

            return new Rectangle(
                (int)Math.Floor(clientRect.X * scalingFac),
                (int)Math.Floor(clientRect.Y * scalingFac),
                (int)Math.Floor(clientRect.Width * scalingFac),
                (int)Math.Floor(clientRect.Height * scalingFac)
            );
        }
    }

    public delegate void OnGameCloseDelegate();
    public event OnGameCloseDelegate? OnGameClosed;

    /// <summary>
    /// Gets whether or not the window is minimized.
    /// </summary>
    /// <returns>True if the window is minimized, else false.</returns>
    public bool IsMinimized => Win32Api.IsIconic(process.MainWindowHandle);

    /// <summary>
    /// Gets whether or not the window is maximized.
    /// </summary>
    /// <returns>True if the window is maximized, else false.</returns>
    public bool IsMaximized => Win32Api.IsZoomed(process.MainWindowHandle);

    /// <summary>
    /// Tries to locate a process whose Main Window title matches the provided string.
    /// </summary>
    /// <param name="windowTitle">The name of the main window of the game to locate.</param>
    /// <exception cref="NoMatchingWindowException">Thrown when no window could be found</exception>
    /// <exception cref="TooManyMatchingWindowsException">Thrown when more than one window was found</exception>
    public GameWindow(string windowTitle) {
        var candidateProcesses = Process
            .GetProcesses()
            .Where(proc => proc.MainWindowTitle.Equals(windowTitle))
            .ToArray();

        if (candidateProcesses.Length == 0) {
            throw new NoMatchingWindowException(windowTitle);
        } else if (candidateProcesses.Length > 1) {
            throw new TooManyMatchingWindowsException(windowTitle);
        }

        process = candidateProcesses[0];

        Task.Factory.StartNew(() => {
            process.WaitForExit();
        }).ContinueWith((t) => {
            OnGameClosed?.Invoke();
        });
    }

    /// <summary>
    /// "Taps" a key (sends a press and release event).
    /// </summary>
    /// <param name="key">The key to tap</param>
    /// <param name="ctrl">Whether or not Ctrl should be held for this keypress</param>
    /// <param name="shift">Whether or not Shift should be held for this keypress</param>
    /// <remark>The window cannot be minimized, and will be brought to the foreground.</remark>
    public void SendKeyTap(Win32Api.ScanCode key, bool ctrl = false, bool shift = false) {
        if (!Win32Api.SetForegroundWindow(process.MainWindowHandle)) {
            throw new Exception("Failed to move window to foreground. Is it minimized?");
        }

        static void TryInsert(Win32Api.ScanCode code, bool release) {
            if (Win32Api.SendKeyboardInput(code, release) == 0) {
                throw new Exception($"Failed to send input for scan code {code} (release: {release})");
            }
        }

        if (ctrl) {
            TryInsert(Win32Api.ScanCode.CONTROL, false);
        }

        if (shift) {
            TryInsert(Win32Api.ScanCode.SHIFT, false);
        }

        // Insert press and release events for the core keypress
        TryInsert(key, false);
        TryInsert(key, true);


        if (shift) {
            TryInsert(Win32Api.ScanCode.SHIFT, true);
        }

        if (ctrl) {
            TryInsert(Win32Api.ScanCode.CONTROL, true);
        }

    }

    /// <summary>
    /// Captures a region of the client area of the window and stores it in a <see cref="Bitmap" />.
    /// </summary>
    /// <param name="region">The region to capture. Must fit inside <see cref="ClientRect" />.</param>
    /// <param name="existingBmp">A bitmap to draw into. Must be at least as large as the region.</param>
    /// <returns>A bitmap containing the specified region of the client area.</returns>
    public Bitmap CaptureScreen(Rectangle? region = null, Bitmap? existingBmp = null) {
        Rectangle clientRect = ClientRect;
        Rectangle captureRect = region ?? clientRect;

        // Grab window graphics
        using Graphics windowGraphics = Graphics.FromHwnd(process.MainWindowHandle);

        Bitmap target = existingBmp ?? new Bitmap(captureRect.Width, captureRect.Height, windowGraphics);

        if (target.Width < captureRect.Width || target.Height < captureRect.Height) {
            throw new Exception("Bitmap not large enough");
        }

        if (IsMinimized) {
            throw new ScreenCaptureFailedException(process.MainWindowTitle, "window is minimized");
        }

        if (!clientRect.Contains(captureRect)) {
            throw new ArgumentOutOfRangeException(
                    nameof(region),
                    region,
                    $"Region must be entirely contained by {clientRect}");
        }

        // Create a bitmap with the dimensions of the request region and the same
        // graphics format of the window, and grab a reference to its Graphics.

        // Bitmap outputBmp = new(captureRect.Width, captureRect.Height, windowGraphics);
        using Graphics outGraphics = Graphics.FromImage(target);

        // Get Device Context Handles to the source and destination
        var destPtr = outGraphics.GetHdc();
        var sourcePtr = windowGraphics.GetHdc();

        // Perform a blit from the specified region of source to destination
        var blitOk = Win32Api.BitBlt(
            destPtr,
            0, 0,
            captureRect.Width, captureRect.Height,
            sourcePtr,
            captureRect.X, captureRect.Y,
            Win32Api.RasterOperation.SRCCOPY);

        // Release the DC handles
        windowGraphics.ReleaseHdc(destPtr);
        outGraphics.ReleaseHdc(sourcePtr);

        // Throw exception if blit failed
        if (!blitOk) {
            throw new ScreenCaptureFailedException(process.MainWindowTitle, "unable to blit device context");
        }

        return target;
    }

    /// <summary>
    /// Tries to capture the window's client area. <paramref name="exception" /> is updated
    /// with the exception that occurred, if any.
    /// </summary>
    /// <param name="exception">The exception that occurred while trying to capture the screen, else null.</param>
    /// <returns>A bitmap containing the contents of the screen, if successful, else null.</returns>
    public Bitmap? TryCaptureScreen(out Exception? exception) {
        return TryCaptureScreen(null, out exception);
    }

    /// <summary>
    /// Tries to capture a region of the window's client area. <paramref name="exception" /> is updated
    /// with the exception that occurred, if any.
    /// </summary>
    /// <param name="exception">The exception that occurred while trying to capture the screen, else null.</param>
    /// <returns>A bitmap containing the contents of the screen, if successful, else null.</returns>
    public Bitmap? TryCaptureScreen(Rectangle? region, out Exception? exception) {
        try {
            var bmp = CaptureScreen(region);

            exception = null;
            return bmp;
        } catch (Exception e) {
            exception = e;
            return null;
        }
    }

    /// <summary>
    /// Generates a human-readable representation of this instance.
    /// </summary>
    /// <returns>A string of the following format: 
    /// <c>GameWindow (title = '{windowTitle}', handle = {processHandle})</c></returns>
    public override string ToString() {
        return $"GameWindow (title = '{process.MainWindowTitle}', handle = {process.MainWindowHandle})";
    }

    private class NoMatchingWindowException : Exception {
        public NoMatchingWindowException(string windowTitle)
            : base($"No window with title '{windowTitle}' appears to be running") { }
    }

    private class TooManyMatchingWindowsException : Exception {
        public TooManyMatchingWindowsException(string windowTitle)
            : base($"More than one window with title '{windowTitle}' appears to be running") { }
    }

    private class ScreenCaptureFailedException : Exception {
        public ScreenCaptureFailedException(string windowTitle)
            : base($"Failed to capture contents of '${windowTitle}'") { }

        public ScreenCaptureFailedException(string windowTitle, string reason)
            : base($"Failed to capture contents of '{windowTitle}': {reason}") { }
    }
}
