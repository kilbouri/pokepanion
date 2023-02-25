using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using ImageMagick;
using Pokepanion.Library.Extensions;

namespace Pokepanion.Library.GameProcessing;

public class WindowInspector {

    private readonly struct InspectedRegion {
        public string RegionName { get; }
        public Rectangle Region { get; }

        public InspectedRegion(string regionName, Rectangle region)
            => (RegionName, Region) = (regionName, region);
    }

    private readonly List<InspectedRegion> regions = new();
    private readonly GameWindow targetWindow;

    public WindowInspector(GameWindow targetWindow) => this.targetWindow = targetWindow;

    public WindowInspector InspectRegion(string regionName, Rectangle region) {
        regions.Add(new InspectedRegion(regionName, region));
        return this;
    }

    public bool TryInspect(out Dictionary<string, string?> result, bool replaceEmptyPagesWithNull = true) {
        try {
            result = Inspect(replaceEmptyPagesWithNull);
            return true;
        } catch (Exception) {
            result = new();
            return false;
        }
    }

    public Dictionary<string, string?> Inspect(bool replaceEmptyPagesWithNull = true) {
        using Bitmap windowCapture = targetWindow.CaptureScreen();
        using Bitmap processedCapture = ProcessCapture(windowCapture);
        using TextExtraction extractor = new(TextExtraction.Language.ENGLISH_PKMN_GBA_DS);

        Dictionary<string, string?> output = new();

        foreach (var region in regions) {
            string? regionContents = extractor.ExtractRegion(processedCapture, region.Region);

            if (replaceEmptyPagesWithNull && regionContents.Equals("Empty page!!")) {
                regionContents = null;
            }

            if (string.IsNullOrEmpty(regionContents)) {
                regionContents = null;
            }

            output.Add(region.RegionName, regionContents);
        }

        return output;
    }

    public void ShowInspectedRegions() {
        using Bitmap windowCapture = targetWindow.CaptureScreen();
        using Bitmap processedTexture = ProcessCapture(windowCapture);

        using Graphics procTextGraphics = Graphics.FromImage(processedTexture);

        foreach (var region in regions) {
            procTextGraphics.DrawRectangle(new Pen(Color.DarkRed, 5), region.Region);
        }

        procTextGraphics.Flush();
        processedTexture.Show();
    }

    private static Bitmap ProcessCapture(Bitmap capture) {
        var image = new MagickFactory().Image.Create(capture);

        // Remove alpha channel and convert to grayscale
        image.Grayscale();

        // Threshold the image so that anything that isn't text is discarded
        // (note: this works for now because the text I care about is white, idk if that will
        // hold in the future...)
        byte range = 40;

        byte floor = (byte)(byte.MaxValue - range);
        byte ceiling = byte.MaxValue;

        var minForWhite = MagickColor.FromRgb(floor, floor, floor);
        var maxForWhite = MagickColor.FromRgb(ceiling, ceiling, ceiling);

        image.ColorThreshold(minForWhite, maxForWhite);

        return image.ToBitmap(ImageFormat.MemoryBmp);
    }

}
