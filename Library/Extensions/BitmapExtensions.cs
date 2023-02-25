using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Pokepanion.Library.Extensions;

public static class BitmapExtensions {

    public static void Show(this Bitmap tex) {

        // Save image to temp folder
        string filePath = Path.GetTempFileName() + ".png";
        tex.Save(filePath, ImageFormat.Png);

        // Open saved file in default viewer
        Process photoViewer = new();
        photoViewer.StartInfo.FileName = "explorer";
        photoViewer.StartInfo.Arguments = $"\"{filePath}\"";
        photoViewer.Start();
    }

    public static bool IsApproximatelyEqual(this Bitmap first, Bitmap second, int sampleSpacing = 4) {

        int minWidth = Math.Min(first.Width, second.Width);
        int minHeight = Math.Min(first.Height, second.Height);

        for (int y = 0; y < minHeight; y += sampleSpacing) {
            for (int x = 0; x < minWidth; x += sampleSpacing) {
                if (!first.GetPixel(x, y).Equals(second.GetPixel(x, y))) {
                    return false;
                }
            }
        }

        return true;
    }
}
