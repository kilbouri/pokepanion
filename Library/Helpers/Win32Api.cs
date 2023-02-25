using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Pokepanion.Library.Helpers;

public static class Win32Api {

    /// <summary>
    /// Flags that describe the default return value of <see cref="MonitorFromWindow" /> when
    /// the window of interest does not intersect any display.
    /// </summary>
    public enum DefaultMonitorFlag {
        DEFAULT_TO_NULL = 0,
        DEFAULT_TO_PRIMARY = 1,
        DEFAULT_TO_NEAREST = 2,
    }

    /// <summary>
    /// Valid raster options for <see cref="BitBlt" />.
    /// </summary>
    public enum RasterOperation : uint {
        BLACKNESS = 66,
        CAPTUREBLT = 1073741824,
        DSTINVERT = 5570569,
        MERGECOPY = 12583114,
        MERGEPAINT = 12255782,
        NOMIRRORBITMAP = 2147483648,
        NOTSRCCOPY = 3342344,
        NOTSRCERASE = 1114278,
        PATCOPY = 15728673,
        PATINVERT = 5898313,
        PATPAINT = 16452105,
        SRCAND = 8913094,
        SRCCOPY = 13369376,
        SRCERASE = 4457256,
        SRCINVERT = 6684742,
        SRCPAINT = 15597702,
        WHITENESS = 16711778,
    }

    /// <summary>
    /// ScanCodes for SendInput.
    /// </summary>
    /// <remark></remark>
    public enum ScanCode : short {
#pragma warning disable CA1069
        LBUTTON = 0,
        RBUTTON = 0,
        CANCEL = 70,
        MBUTTON = 0,
        XBUTTON1 = 0,
        XBUTTON2 = 0,
        BACK = 14,
        TAB = 15,
        CLEAR = 76,
        RETURN = 28,
        SHIFT = 42,
        CONTROL = 29,
        MENU = 56,
        PAUSE = 0,
        CAPITAL = 58,
        KANA = 0,
        HANGUL = 0,
        JUNJA = 0,
        FINAL = 0,
        HANJA = 0,
        KANJI = 0,
        ESCAPE = 1,
        CONVERT = 0,
        NONCONVERT = 0,
        ACCEPT = 0,
        MODECHANGE = 0,
        SPACE = 57,
        PRIOR = 73,
        NEXT = 81,
        END = 79,
        HOME = 71,
        LEFT = 75,
        UP = 72,
        RIGHT = 77,
        DOWN = 80,
        SELECT = 0,
        PRINT = 0,
        EXECUTE = 0,
        SNAPSHOT = 84,
        INSERT = 82,
        DELETE = 83,
        HELP = 99,
        KEY_0 = 11,
        KEY_1 = 2,
        KEY_2 = 3,
        KEY_3 = 4,
        KEY_4 = 5,
        KEY_5 = 6,
        KEY_6 = 7,
        KEY_7 = 8,
        KEY_8 = 9,
        KEY_9 = 10,
        KEY_A = 30,
        KEY_B = 48,
        KEY_C = 46,
        KEY_D = 32,
        KEY_E = 18,
        KEY_F = 33,
        KEY_G = 34,
        KEY_H = 35,
        KEY_I = 23,
        KEY_J = 36,
        KEY_K = 37,
        KEY_L = 38,
        KEY_M = 50,
        KEY_N = 49,
        KEY_O = 24,
        KEY_P = 25,
        KEY_Q = 16,
        KEY_R = 19,
        KEY_S = 31,
        KEY_T = 20,
        KEY_U = 22,
        KEY_V = 47,
        KEY_W = 17,
        KEY_X = 45,
        KEY_Y = 21,
        KEY_Z = 44,
        LWIN = 91,
        RWIN = 92,
        APPS = 93,
        SLEEP = 95,
        NUMPAD0 = 82,
        NUMPAD1 = 79,
        NUMPAD2 = 80,
        NUMPAD3 = 81,
        NUMPAD4 = 75,
        NUMPAD5 = 76,
        NUMPAD6 = 77,
        NUMPAD7 = 71,
        NUMPAD8 = 72,
        NUMPAD9 = 73,
        MULTIPLY = 55,
        ADD = 78,
        SEPARATOR = 0,
        SUBTRACT = 74,
        DECIMAL = 83,
        DIVIDE = 53,
        F1 = 59,
        F2 = 60,
        F3 = 61,
        F4 = 62,
        F5 = 63,
        F6 = 64,
        F7 = 65,
        F8 = 66,
        F9 = 67,
        F10 = 68,
        F11 = 87,
        F12 = 88,
        F13 = 100,
        F14 = 101,
        F15 = 102,
        F16 = 103,
        F17 = 104,
        F18 = 105,
        F19 = 106,
        F20 = 107,
        F21 = 108,
        F22 = 109,
        F23 = 110,
        F24 = 118,
        NUMLOCK = 69,
        SCROLL = 70,
        LSHIFT = 42,
        RSHIFT = 54,
        LCONTROL = 29,
        RCONTROL = 29,
        LMENU = 56,
        RMENU = 56,
        BROWSER_BACK = 106,
        BROWSER_FORWARD = 105,
        BROWSER_REFRESH = 103,
        BROWSER_STOP = 104,
        BROWSER_SEARCH = 101,
        BROWSER_FAVORITES = 102,
        BROWSER_HOME = 50,
        VOLUME_MUTE = 32,
        VOLUME_DOWN = 46,
        VOLUME_UP = 48,
        MEDIA_NEXT_TRACK = 25,
        MEDIA_PREV_TRACK = 16,
        MEDIA_STOP = 36,
        MEDIA_PLAY_PAUSE = 34,
        LAUNCH_MAIL = 108,
        LAUNCH_MEDIA_SELECT = 109,
        LAUNCH_APP1 = 107,
        LAUNCH_APP2 = 33,
        OEM_1 = 39,
        OEM_PLUS = 13,
        OEM_COMMA = 51,
        OEM_MINUS = 12,
        OEM_PERIOD = 52,
        OEM_2 = 53,
        OEM_3 = 41,
        OEM_4 = 26,
        OEM_5 = 43,
        OEM_6 = 27,
        OEM_7 = 40,
        OEM_8 = 0,
        OEM_102 = 86,
        PROCESSKEY = 0,
        PACKET = 0,
        ATTN = 0,
        CRSEL = 0,
        EXSEL = 0,
        EREOF = 93,
        PLAY = 0,
        ZOOM = 98,
        NONAME = 0,
        PA1 = 0,
        OEM_CLEAR = 0,
#pragma warning restore CA1069
    }

    // Currently unused - may be needed in the future.
    private enum VirtualKey : short {
#pragma warning disable CA1069
        LBUTTON = 0x01,
        RBUTTON = 0x02,
        CANCEL = 0x03,
        MBUTTON = 0x04,
        XBUTTON1 = 0x05,
        XBUTTON2 = 0x06,
        BACK = 0x08,
        TAB = 0x09,
        CLEAR = 0x0C,
        RETURN = 0x0D,
        SHIFT = 0x10,
        CONTROL = 0x11,
        MENU = 0x12,
        PAUSE = 0x13,
        CAPITAL = 0x14,
        KANA = 0x15,
        HANGUL = 0x15,
        JUNJA = 0x17,
        FINAL = 0x18,
        HANJA = 0x19,
        KANJI = 0x19,
        ESCAPE = 0x1B,
        CONVERT = 0x1C,
        NONCONVERT = 0x1D,
        ACCEPT = 0x1E,
        MODECHANGE = 0x1F,
        SPACE = 0x20,
        PRIOR = 0x21,
        NEXT = 0x22,
        END = 0x23,
        HOME = 0x24,
        LEFT = 0x25,
        UP = 0x26,
        RIGHT = 0x27,
        DOWN = 0x28,
        SELECT = 0x29,
        PRINT = 0x2A,
        EXECUTE = 0x2B,
        SNAPSHOT = 0x2C,
        INSERT = 0x2D,
        DELETE = 0x2E,
        HELP = 0x2F,
        KEY_0 = 0x30,
        KEY_1 = 0x31,
        KEY_2 = 0x32,
        KEY_3 = 0x33,
        KEY_4 = 0x34,
        KEY_5 = 0x35,
        KEY_6 = 0x36,
        KEY_7 = 0x37,
        KEY_8 = 0x38,
        KEY_9 = 0x39,
        KEY_A = 0x41,
        KEY_B = 0x42,
        KEY_C = 0x43,
        KEY_D = 0x44,
        KEY_E = 0x45,
        KEY_F = 0x46,
        KEY_G = 0x47,
        KEY_H = 0x48,
        KEY_I = 0x49,
        KEY_J = 0x4A,
        KEY_K = 0x4B,
        KEY_L = 0x4C,
        KEY_M = 0x4D,
        KEY_N = 0x4E,
        KEY_O = 0x4F,
        KEY_P = 0x50,
        KEY_Q = 0x51,
        KEY_R = 0x52,
        KEY_S = 0x53,
        KEY_T = 0x54,
        KEY_U = 0x55,
        KEY_V = 0x56,
        KEY_W = 0x57,
        KEY_X = 0x58,
        KEY_Y = 0x59,
        KEY_Z = 0x5A,
        LWIN = 0x5B,
        RWIN = 0x5C,
        APPS = 0x5D,
        SLEEP = 0x5F,
        NUMPAD0 = 0x60,
        NUMPAD1 = 0x61,
        NUMPAD2 = 0x62,
        NUMPAD3 = 0x63,
        NUMPAD4 = 0x64,
        NUMPAD5 = 0x65,
        NUMPAD6 = 0x66,
        NUMPAD7 = 0x67,
        NUMPAD8 = 0x68,
        NUMPAD9 = 0x69,
        MULTIPLY = 0x6A,
        ADD = 0x6B,
        SEPARATOR = 0x6C,
        SUBTRACT = 0x6D,
        DECIMAL = 0x6E,
        DIVIDE = 0x6F,
        F1 = 0x70,
        F2 = 0x71,
        F3 = 0x72,
        F4 = 0x73,
        F5 = 0x74,
        F6 = 0x75,
        F7 = 0x76,
        F8 = 0x77,
        F9 = 0x78,
        F10 = 0x79,
        F11 = 0x7A,
        F12 = 0x7B,
        F13 = 0x7C,
        F14 = 0x7D,
        F15 = 0x7E,
        F16 = 0x7F,
        F17 = 0x80,
        F18 = 0x81,
        F19 = 0x82,
        F20 = 0x83,
        F21 = 0x84,
        F22 = 0x85,
        F23 = 0x86,
        F24 = 0x87,
        NUMLOCK = 0x90,
        SCROLL = 0x91,
        LSHIFT = 0xA0,
        RSHIFT = 0xA1,
        LCONTROL = 0xA2,
        RCONTROL = 0xA3,
        LMENU = 0xA4,
        RMENU = 0xA5,
        BROWSER_BACK = 0xA6,
        BROWSER_FORWARD = 0xA7,
        BROWSER_REFRESH = 0xA8,
        BROWSER_STOP = 0xA9,
        BROWSER_SEARCH = 0xAA,
        BROWSER_FAVORITES = 0xAB,
        BROWSER_HOME = 0xAC,
        VOLUME_MUTE = 0xAD,
        VOLUME_DOWN = 0xAE,
        VOLUME_UP = 0xAF,
        MEDIA_NEXT_TRACK = 0xB0,
        MEDIA_PREV_TRACK = 0xB1,
        MEDIA_STOP = 0xB2,
        MEDIA_PLAY_PAUSE = 0xB3,
        LAUNCH_MAIL = 0xB4,
        LAUNCH_MEDIA_SELECT = 0xB5,
        LAUNCH_APP1 = 0xB6,
        LAUNCH_APP2 = 0xB7,
        OEM_1 = 0xBA,
        OEM_PLUS = 0xBB,
        OEM_COMMA = 0xBC,
        OEM_MINUS = 0xBD,
        OEM_PERIOD = 0xBE,
        OEM_2 = 0xBF,
        OEM_3 = 0xC0,
        OEM_4 = 0xDB,
        OEM_5 = 0xDC,
        OEM_6 = 0xDD,
        OEM_7 = 0xDE,
        OEM_8 = 0xDF,
        OEM_102 = 0xE2,
        PROCESSKEY = 0xE5,
        PACKET = 0xE7,
        ATTN = 0xF6,
        CRSEL = 0xF7,
        EXSEL = 0xF8,
        EREOF = 0xF9,
        PLAY = 0xFA,
        ZOOM = 0xFB,
        NONAME = 0xFC,
        PA1 = 0xFD,
        OEM_CLEAR = 0xFE
#pragma warning restore CA1069
    }

    [Flags]
    private enum Win32MouseXButtons : uint {
        Nothing = 0x00000000,
        XBUTTON1 = 0x00000001,
        XBUTTON2 = 0x00000002
    }

    [Flags]
    private enum Win32MouseEventFlags : uint {
        ABSOLUTE = 0x8000,
        HWHEEL = 0x01000,
        MOVE = 0x0001,
        MOVE_NOCOALESCE = 0x2000,
        LEFTDOWN = 0x0002,
        LEFTUP = 0x0004,
        RIGHTDOWN = 0x0008,
        RIGHTUP = 0x0010,
        MIDDLEDOWN = 0x0020,
        MIDDLEUP = 0x0040,
        VIRTUALDESK = 0x4000,
        WHEEL = 0x0800,
        XDOWN = 0x0080,
        XUP = 0x0100
    }

    [Flags]
    private enum Win32KeyEventFlags : uint {
        EXTENDEDKEY = 0x0001,
        KEYUP = 0x0002,
        SCANCODE = 0x0008,
        UNICODE = 0x0004
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct Win32Rect {
        public int Left, Top, Right, Bottom;

        public Win32Rect(int l, int t, int r, int b) => (Left, Top, Right, Bottom) = (l, t, r, b);

        public static implicit operator Rectangle(Win32Rect o) => new(o.Left, o.Top, o.Right - o.Left, o.Bottom - o.Top);
        public static implicit operator Win32Rect(Rectangle o) => new(o.Left, o.Top, o.Right, o.Bottom);
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct Win32Point {
        public int X, Y;

        public Win32Point(int x, int y) => (X, Y) = (x, y);

        public static implicit operator Point(Win32Point o) => new(o.X, o.Y);
        public static implicit operator Win32Point(Point o) => new(o.X, o.Y);
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct Win32Input {
        public uint type;
        public Win32InputUnion U;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct Win32InputUnion {
        [FieldOffset(0)]
        public Win32MouseInput mi;
        [FieldOffset(0)]
        public Win32KeyboardInput ki;
        [FieldOffset(0)]
        public Win32HardwareInput hi;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct Win32MouseInput {
        public int dx;
        public int dy;
        public Win32MouseXButtons mouseData;
        public Win32MouseEventFlags dwFlags;
        public uint time;
        public UIntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct Win32HardwareInput {
        public int uMsg;
        public short wParamL;
        public short wParamH;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct Win32KeyboardInput {
        public VirtualKey wVk;
        public ScanCode wScan;
        public Win32KeyEventFlags dwFlags;
        public int time;
        public UIntPtr dwExtraInfo;
    }

    /// <summary>
    /// Returns a <see cref="Rectange" /> representing the un-scaled client area of the window pointed to
    /// by <paramref name="hWnd" />.
    /// </summary>
    /// <param name="hWnd">A handle for the window to get the client area for.</param>
    /// <returns>An unscaled <see cref="Rectangle" /> containing the dimensions of the window pointed to
    /// by <see cref="hWnd" /></returns>
    /// <remark>https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getclientrect</remark>
    public static Rectangle GetClientRect(IntPtr hWnd) {

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetClientRect(IntPtr hWnd, out Win32Rect lpRect);

        if (!GetClientRect(hWnd, out Win32Rect clientRect)) {
            throw new Win32Exception(GetLastError(), $"Unable to get client rect for handle {hWnd}");
        }

        int width = clientRect.Right - clientRect.Left;
        int height = clientRect.Bottom - clientRect.Top;

        return new Rectangle(clientRect.Left, clientRect.Top, width, height);
    }

    /// <summary>
    /// Returns a handle to the monitor that has the largest area of intersection with the bounding box of the
    /// given window.
    /// </summary>
    /// <param name="hWnd">A handle to the window of interest</param>
    /// <param name="dwFlags">Determines the return value if the window does not intersect any monitor.</param>
    /// <returns>If the window intersects multiple monitors, then the return value is a handle to the display that
    /// has the largest area of intersection. Otherwise, the return value is determined by 
    /// <paramref name="dwFlags" /></returns>
    /// <remark>https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-monitorfromwindow</remark>
    public static IntPtr MonitorFromWindow(IntPtr hWnd, DefaultMonitorFlag dwFlags) {

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr MonitorFromWindow(IntPtr hWnd, uint dwFlags);

        return MonitorFromWindow(hWnd, (uint)dwFlags);
    }

    /// <summary>
    /// Gets the scale factor of a specific monitor.
    /// </summary>
    /// <param name="hMon">The monitor's handle.</param>
    /// <returns>A floating point factor representative of the display scale. <c>1.0</c> is 100%, 
    /// <c>1.5</c> is 150%, etc.</returns>
    /// <remark>https://learn.microsoft.com/en-us/windows/win32/api/shellscalingapi/nf-shellscalingapi-getscalefactorformonitor</remark>
    public static float GetScaleFactorForMonitor(IntPtr hMon) {

        [DllImport("shcore.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Error)]
        static extern uint GetScaleFactorForMonitor(IntPtr hMon, out int pScale);

        uint result = GetScaleFactorForMonitor(hMon, out int monitorScalePercent);

        if (result != 0) {
            throw new Win32Exception(GetLastError(), $"Unable to get monitor scaling for handle {hMon}");
        }

        return monitorScalePercent * 0.01f;
    }

    /// <summary>
    /// Determines whether the specified window is minimized (iconic).
    /// </summary>
    /// <param name="hWnd">A handle to the window of interest.</param>
    /// <returns>True if the window is minimized (iconic), otherwise false.</returns>
    /// <remark>https://learn.microsoft.com/en-ca/windows/win32/api/winuser/nf-winuser-isiconic?redirectedfrom=MSDN</remark>
    public static bool IsIconic(IntPtr hWnd) {

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsIconic(IntPtr hWnd);

        return IsIconic(hWnd);
    }

    /// <summary>
    /// Determines whether the specified window is maximized (zoomed).
    /// </summary>
    /// <param name="hWnd">A handle to the window of interest.</param>
    /// <returns>True if the window is maximized (zoomed), otherwise false.</returns>
    /// <remark>https://learn.microsoft.com/en-ca/windows/win32/api/winuser/nf-winuser-iszoomed?redirectedfrom=MSDNM</remark>
    public static bool IsZoomed(IntPtr hWnd) {

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsZoomed(IntPtr hWnd);

        return IsZoomed(hWnd);
    }

    /// <summary>
    /// The BitBlt function performs a bit-block transfer of the color data corresponding to a
    /// rectangle of pixels from the specified source device context into a destination device context.
    /// </summary>
    /// <param name="hdc">A handle to the destination device context.</param>
    /// <param name="x">The x-coordinate, in logical units, of the upper-left corner of the destination rectangle.</param>
    /// <param name="y">The y-coordinate, in logical units, of the upper-left corner of the destination rectangle.</param>
    /// <param name="cx">The width, in logical units, of the source and destination rectangles.</param>
    /// <param name="cy">The height, in logical units, of the source and the destination rectangles.</param>
    /// <param name="hdcSrc">A handle to the source device context.</param>
    /// <param name="x1">The x-coordinate, in logical units, of the upper-left corner of the source rectangle.</param>
    /// <param name="y1">The y-coordinate, in logical units, of the upper-left corner of the source rectangle.</param>
    /// <param name="rop">A raster-operation code. These codes define how the color data for the source rectangle is to be
    /// combined with the color data for the destination rectangle to achieve the final color.</param>
    /// <returns>True if the operation succeeeds, else false.</returns>
    /// <remark>https://learn.microsoft.com/en-us/windows/win32/api/wingdi/nf-wingdi-bitblt</remark>
    public static bool BitBlt(IntPtr hdc, int x, int y, int cx, int cy, IntPtr hdcSrc, int x1, int y1, RasterOperation rop) {
        [DllImport("gdi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool BitBlt(IntPtr hdc, int x, int y, int cx, int cy, IntPtr hdcSrc, int x1, int y1, uint rop);

        return BitBlt(hdc, x, y, cx, cy, hdcSrc, x1, y1, (uint)rop);
    }

    /// <summary>
    /// Sends a key down or key up event for the given scan code to the active window.
    /// </summary>
    /// <param name="code">The key to press/release.</param>
    /// <param name="release">If true, a key release event is inserted. Otherwise, a key down event is inserted.</param>
    /// <returns>The number of events inserted into the active window's input queue. This is 1 when no error occurs.</returns>
    /// <remark>https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendinput</remark>
    public static uint SendKeyboardInput(ScanCode code, bool release) {
        [DllImport("user32.dll")]
        static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] Win32Input[] pInputs, int cbSize);

        Win32Input[] inputs = new Win32Input[2];

        Win32Input input = new() {
            type = 1 // 1 = Keyboard Input
        };

        input.U.ki.wScan = code;
        input.U.ki.dwFlags = release ? Win32KeyEventFlags.KEYUP : Win32KeyEventFlags.SCANCODE;
        inputs[0] = input;

        return SendInput((uint)inputs.Length, inputs, Marshal.SizeOf<Win32Input>());
    }

    /// <summary>
    /// Brings a window to the foreground.
    /// </summary>
    /// <param name="hWnd">A handle to the window to bring to the foreground.</param>
    /// <returns>True if the window was brought to the foreground, else false.</returns>
    /// <remark>https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setforegroundwindow</remark>
    public static bool SetForegroundWindow(IntPtr hWnd) {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        return SetForegroundWindow(hWnd);
    }

    /// <summary>
    /// Returns the most recent error from Win32 API calls.
    /// </summary>
    /// <returns>The most recent error from Win32 API calls.</returns>
    public static int GetLastError() {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.U4)]
        static extern uint GetLastError();

        return (int)GetLastError();
    }
}
