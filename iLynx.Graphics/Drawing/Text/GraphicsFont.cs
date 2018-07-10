using System.Collections.Generic;
using System.IO;
using System.Linq;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Drawing;
using SixLabors.ImageSharp.Processing.Drawing.Brushes;
using SixLabors.ImageSharp.Processing.Drawing.Pens;
using SixLabors.Shapes;
using PointF = SixLabors.Primitives.PointF;
using RectangleF = SixLabors.Primitives.RectangleF;

namespace iLynx.Graphics.Drawing.Text
{
    //public class GraphicsFont
    //{
    //    private readonly float m_fontSize;
    //    private Font m_font;
    //    private static readonly FontCollection s_Collection = new FontCollection();

    //    private GraphicsFont(float fontSize)
    //    {
    //        m_fontSize = fontSize;
    //    }

    //    public GraphicsFont(FileInfo fontFile, float fontSize)
    //        : this(fontSize)
    //    {
    //        using (var stream = fontFile.OpenRead())
    //        m_font = Load(stream);
    //    }

    //    public GraphicsFont(Stream fontFile, float fontSize)
    //        : this(fontSize)
    //    {
    //        m_font = Load(fontFile);
    //    }

    //    public GraphicsFont(string fontFamily, float fontSize)
    //        : this(fontSize)
    //    {
    //        if (!s_Collection.TryFind(fontFamily, out var family)) throw new KeyNotFoundException($"Could not find fontfamily {fontFamily}");
    //        family.CreateFont(m_fontSize);
    //    }

    //    private Font Load(Stream source)
    //    {
    //        var description = FontDescription.LoadDescription(source);
    //        if (s_Collection.Families.Any(x => x.Name == description.FontFamily)) return s_Collection.CreateFont(description.FontFamily, m_fontSize);
    //        source.Seek(0, SeekOrigin.Begin);
    //        s_Collection.Install(source);
    //        return s_Collection.CreateFont(description.FontFamily, m_fontSize);
    //    }

    //    public Glyph GetGlyph(char character)
    //    {
    //        var paths = TextBuilder.GenerateGlyphs("Some text", PointF.Empty, new RendererOptions(m_font));
    //        return new Glyph();
    //        //TextMeasurer.Measure()
    //        //Image<Color32> img;
    //        //img.Mutate(context => context.Fill(Color32.Lime, paths));
    //        //return new Glyph();
    //        //var renderer = new TextRenderer();
    //    }
    //}
}
