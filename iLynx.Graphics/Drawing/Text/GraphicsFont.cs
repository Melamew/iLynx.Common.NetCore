using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using SixLabors.Fonts;
using SixLabors.Shapes;
using PointF = SixLabors.Primitives.PointF;

namespace iLynx.Graphics.Drawing.Text
{
    public class GraphicsFont
    {
        private readonly float m_fontSize;
        private Font m_font;
        private static readonly FontCollection s_Collection = new FontCollection();
        private static Font s_defaultFont;
        public static Font DefaultFont => s_defaultFont ?? (s_defaultFont = Load(12f, "./fonts/OpenSans-Regular.ttf"));
        private static readonly List<ImageTexture> pages = new List<ImageTexture>();

        public GraphicsFont(FileInfo fontFile, float fontSize)
        {
            m_fontSize = fontSize;
            using (var stream = fontFile.OpenRead())
                m_font = Load(stream, m_fontSize);
        }

        public GraphicsFont(Stream fontFile, float fontSize)
        {
            m_fontSize = fontSize;
            m_font = Load(fontFile, fontSize);
        }

        public GraphicsFont(string fontFamily, float fontSize)
        {
            m_fontSize = fontSize;
            if (!s_Collection.TryFind(fontFamily, out var family)) throw new KeyNotFoundException($"Could not find fontfamily {fontFamily}");
            family.CreateFont(m_fontSize);
        }

        /// <summary>
        /// Gets a font with the specified size from the specified font family.
        /// </summary>
        /// <param name="fontFamily">The font family to load</param>
        /// <param name="size">The character size</param>
        /// <param name="style">The font style</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"><paramref name="fontFamily"/> is not available</exception>
        public static Font GetFont([NotNull]string fontFamily, float size, FontStyle style = FontStyle.Regular)
        {
            if (size < 0f) throw new ArgumentOutOfRangeException(nameof(size));
            if (!s_Collection.TryFind(fontFamily, out var family)) throw new KeyNotFoundException($"Could not find a font family with the specified name ({fontFamily})");
            return s_Collection.CreateFont(family.Name, size, style);
        }

        private static Font Load(float fontSize, string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException(string.Empty, path);
            using (var stream = File.OpenRead(path))
                return Load(stream, fontSize);
        }

        private static Font Load(Stream source, float fontSize)
        {
            var description = FontDescription.LoadDescription(source);
            if (s_Collection.Families.Any(x => x.Name == description.FontFamily)) return s_Collection.CreateFont(description.FontFamily, fontSize);
            source.Seek(0, SeekOrigin.Begin);
            s_Collection.Install(source);
            return s_Collection.CreateFont(description.FontFamily, fontSize);
        }

        public Glyph GetGlyph(char character)
        {
            return new Glyph();
            //TextMeasurer.Measure()
            //Image<Color32> img;
            //img.Mutate(context => context.Fill(Color32.Lime, paths));
            //return new Glyph();
            //var renderer = new TextRenderer();
        }
    }
}
