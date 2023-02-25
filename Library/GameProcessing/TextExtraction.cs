using System;
using System.Collections.Generic;
using System.Drawing;
using Tesseract;

namespace Pokepanion.Library.GameProcessing;

public class TextExtraction : IDisposable {

    private readonly TesseractEngine engine;

    /// <summary>
    /// Enum of valid language packs to use for Optical Character Recognition
    /// </summary>
    public enum Language {
        ENGLISH = 0,
        ENGLISH_PKMN_GBA_DS = 1
    }

    private static readonly Dictionary<Language, string> languageMap = new() {
        { Language.ENGLISH, "eng" },
        { Language.ENGLISH_PKMN_GBA_DS, "pkmngba_en" }
    };

    public TextExtraction(Language language) {
        if (Environment.GetEnvironmentVariable("TESSDATA_PREFIX") == null) {
            throw new OcrDataMissingException("TESSDATA_PREFIX must be set");
        }

        engine = new(null, languageMap[language]);
    }

    /// <summary>
    /// Extracts text from the specified region of the provided image.
    /// </summary>
    /// <param name="bmp">The image to extract text from</param>
    /// <param name="region">The region of the image to extract text from</param>
    /// <returns>The text in the region of the image, or "Empty page!!" if no text exists.</returns>
    public string ExtractRegion(Bitmap bmp, Rectangle region) {
        Rect toProcess = new(region.X, region.Y, region.Width, region.Height);
        using var page = engine.Process(bmp, toProcess);

        return page.GetText().Trim();
    }

    #region IDisposable
    ~TextExtraction() {
        Dispose(false);
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
        if (disposing) {
            // managed/child IDisposable cleanup
            engine.Dispose();
        }

        // unmanaged cleanup (nothing)
    }
    #endregion

    private class OcrDataMissingException : Exception {
        public OcrDataMissingException(string message) :
            base($"Unable to initialize Text Extraction engine: {message}") { }
    }
}
