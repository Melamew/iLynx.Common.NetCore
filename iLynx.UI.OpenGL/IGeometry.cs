using System.Collections.Generic;

namespace iLynx.UI.OpenGL
{
    public interface IGeometry
    {
        IEnumerable<Vertex> Vertices { get; }
    }
}