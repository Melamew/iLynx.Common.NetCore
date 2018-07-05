using OpenTK;

namespace iLynx.Graphics.Geometry
{
    public interface IGeometry : IDrawable, ITransformable
    {
        Color FillColor { get; set; }
    }
}