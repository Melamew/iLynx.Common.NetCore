using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Text;
using Configuration = SixLabors.ImageSharp.Configuration;
using PointF = SixLabors.Primitives.PointF;

namespace iLynx.Graphics.Drawing.Text
{
    public class FontPage
    {
        private readonly Font m_font;
        private readonly Image<Color32> m_image;
        private readonly Dictionary<char, Glyph> m_glyphMap = new Dictionary<char, Glyph>(64);
        private PointF m_nextDrawLocation = new PointF(0f, 0f);
        private readonly Texture m_texture;

        public FontPage(Font font)
        {
            m_font = font;
            m_image = new Image<Color32>(Configuration.Default, (int)font.Size * 28, font.LineHeight * 4, Color32.Transparent);
            m_texture = new ImageTexture(m_image, horizontalWrapMode: TextureWrapMode.ClampToEdge, verticalWrapMode: TextureWrapMode.ClampToEdge);
        }

        private void SetNextDrawLocation()
        {
            m_nextDrawLocation = m_nextDrawLocation + new PointF(m_font.Size, 0f);
            if (m_nextDrawLocation.X < m_image.Width) return;
            m_nextDrawLocation.X = 0f;
            m_nextDrawLocation.Y += m_font.LineHeight;
        }

        public Glyph GetGlyph(char character)
        {
            if (m_glyphMap.TryGetValue(character, out var result)) return result;
            result = new Glyph();
            var sChar = character.ToString();
            var charSize = TextMeasurer.Measure(
                sChar,
                new RendererOptions(m_font)
            );
            m_image.Mutate(i => i.DrawText(sChar, m_font, Color32.White, m_nextDrawLocation));
            result.Texture = m_texture;
            result.TextureRect = new RectangleF(m_nextDrawLocation.X, m_nextDrawLocation.Y, charSize.Width, charSize.Height);
            SetNextDrawLocation();
            m_glyphMap.Add(character, result);
            return result;
        }


        public IEnumerable<Glyph> Glyphs => m_glyphMap.Values;
    }
    public class GraphicsFont
    {
        private readonly float m_fontSize;
        private Font m_font;
        private static readonly FontCollection s_Collection = new FontCollection();
        private static Font s_defaultFont;
        public static Font DefaultFont => s_defaultFont ?? (s_defaultFont = Load(12f, "./fonts/OpenSans-Regular.ttf"));
        private static readonly Dictionary<(string FontFamily, float FontSize), ImageTexture> s_Pages = new Dictionary<(string, float), ImageTexture>();

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

        /// <summary>
        /// Loads a font with the specified size from the specified file
        /// </summary>
        /// <param name="fontSize">The size of the font</param>
        /// <param name="path">The path to the file of the font</param>
        /// <returns>A font object with the font contained in the specified file</returns>
        /// <exception cref="FileNotFoundException">If path does not exist</exception>
        /// <exception cref="ArgumentOutOfRangeException">The font size is invalid</exception>
        private static Font Load(float fontSize, string path)
        {
            if (fontSize < 0) throw new ArgumentOutOfRangeException(nameof(fontSize));
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

        //        private readonly Image<Color32> GetPage()
        //        {
        //            
        //        }

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
