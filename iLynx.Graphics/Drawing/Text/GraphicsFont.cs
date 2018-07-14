using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using OpenTK;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Text;
using Configuration = SixLabors.ImageSharp.Configuration;
using PointF = SixLabors.Primitives.PointF;

namespace iLynx.Graphics.Drawing.Text
{
    public class FontPage
    {
        private readonly Font m_font;
        private readonly Dictionary<char, Glyph> m_glyphMap = new Dictionary<char, Glyph>(64);
        private PointF m_drawLocation = new PointF(0f, 0f);
        private readonly Vector2 m_scalingFactor;

        public FontPage(Font font)
        {
            m_font = font;
            Image = new Image<Color32>(
                Configuration.Default,
                (int)MathF.Ceiling(font.Size * 32f),
                (int)MathF.Ceiling(font.Size * 32f),
                Color32.Transparent
            );
            m_scalingFactor = new Vector2(1f / Image.Width, 1f / Image.Height);
        }

        //private void SetNextDrawLocation(SizeF charSize)
        //{
        //    m_nextDrawLocation = m_nextDrawLocation + new PointF(charSize.Width, 0f);
        //    if (m_nextDrawLocation.X < m_image.Width) return;
        //    m_nextDrawLocation.X = 0f;
        //    m_nextDrawLocation.Y += m_font.Size;
        //}

        public Glyph GetGlyph(char character, out bool cacheUpdated)
        {
            cacheUpdated = false;
            if (m_glyphMap.TryGetValue(character, out var result)) return result;
            Cache(character);
            cacheUpdated = true;
            return m_glyphMap[character];
        }

        public void Cache(string str)
        {
            foreach (var c in str.Where(x => !m_glyphMap.ContainsKey(x)))
                Cache(c);
#if DEBUG
            Image.Save($"./{m_font.Name}.fontPage.png", new PngEncoder());
#endif
        }

        private void Cache(char character)
        {
            if (char.IsControl(character)) throw new ArgumentOutOfRangeException(nameof(character), "Cannot cache a control character");
            var glyph = new Glyph();
            var sChar = character.ToString();
            var charSize = TextMeasurer.Measure(
                sChar,
                new RendererOptions(m_font)
            );

            if (m_drawLocation.X + charSize.Width >= Image.Width)
            {
                m_drawLocation.X = 0f;
                m_drawLocation.Y += m_font.Size;
            }

            Image.Mutate(i => i.DrawText(sChar, m_font, Color32.White, m_drawLocation));
            glyph.TextureRect = new RectangleF(
                m_drawLocation.X * m_scalingFactor.X,
                m_drawLocation.Y * m_scalingFactor.Y,
                charSize.Width * m_scalingFactor.X,
                charSize.Height * m_scalingFactor.Y
            );
            m_drawLocation.X += charSize.Width;
            m_glyphMap.Add(character, glyph);
        }

        public IEnumerable<Glyph> Glyphs => m_glyphMap.Values;
        public Image<Color32> Image { get; }
    }

    public class GraphicsFont
    {
        private static readonly FontCollection s_Collection = new FontCollection();
        private static Font s_defaultFont;
        private static Font DefaultImageSharpFont => s_defaultFont ?? (s_defaultFont = Load(12f, "./fonts/OpenSans-Regular.ttf", FontStyle.Regular));

        private static readonly Dictionary<Font, FontPage> s_Pages = new Dictionary<Font, FontPage>();
        public static readonly GraphicsFont DefaultFont = new GraphicsFont(DefaultImageSharpFont);

        private FontPage m_fontPage;
        private Font m_font;
        private ImageTexture m_texture;

        public GraphicsFont(Font font)
        {
            Initialize(font);
        }

        public GraphicsFont(GraphicsFont copy, float size, FontStyle style = FontStyle.Regular)
        {
            m_font = new Font(copy.m_font, size, style);
            m_fontPage = new FontPage(m_font);
        }

        public GraphicsFont(FileInfo fontFile, float fontSize, FontStyle style = FontStyle.Regular)
            : this(Load(fontFile.OpenRead(), fontSize, style)) { }

        public GraphicsFont(Stream fontFile, float fontSize, FontStyle style = FontStyle.Regular)
            : this(Load(fontFile, fontSize, style)) { }

        public GraphicsFont(string fontFamily, float fontSize, FontStyle style = FontStyle.Regular)
        {
            if (!s_Collection.TryFind(fontFamily, out var family)) throw new KeyNotFoundException($"Could not find font family {fontFamily}");
            Initialize(family.CreateFont(fontSize, style));
        }

        private static FontPage GetPage(Font font)
        {
            if (s_Pages.TryGetValue(font, out var result)) return result;
            s_Pages.Add(font, result = new FontPage(font));
            return result;
        }

        private void Initialize(Font font)
        {
            m_font = font;
            m_fontPage = GetPage(font);
        }

        public void Cache(string text)
        {
            m_fontPage.Cache(text);
        }

        public static Font GetFont([NotNull]string fontFamily, float size, FontStyle style = FontStyle.Regular)
        {
            if (size < 0f) throw new ArgumentOutOfRangeException(nameof(size));
            if (!s_Collection.TryFind(fontFamily, out var family)) throw new KeyNotFoundException($"Could not find a font family with the specified name ({fontFamily})");
            return s_Collection.CreateFont(family.Name, size, style);
        }

        private static Font Load(float fontSize, string path, FontStyle style)
        {
            if (fontSize < 0) throw new ArgumentOutOfRangeException(nameof(fontSize));
            if (!File.Exists(path)) throw new FileNotFoundException(string.Empty, path);
            using (var stream = File.OpenRead(path))
                return Load(stream, fontSize, style);
        }

        private static Font Load(Stream source, float fontSize, FontStyle style)
        {
            var description = FontDescription.LoadDescription(source);
            if (s_Collection.Families.Any(x => x.Name == description.FontFamily)) return s_Collection.CreateFont(description.FontFamily, fontSize);
            source.Seek(0, SeekOrigin.Begin);
            s_Collection.Install(source);
            return s_Collection.CreateFont(description.FontFamily, fontSize, style);
        }

        public Glyph GetGlyph(char character)
        {
            var result = m_fontPage.GetGlyph(character, out var updated);
            if (updated)
                Texture.Update();
            return result;
        }

        public Image<Color32> CachePage => m_fontPage.Image;

        public ImageTexture Texture => m_texture ?? (m_texture = new ImageTexture(CachePage));

        public float FontSize => m_font.Size;
    }
}
