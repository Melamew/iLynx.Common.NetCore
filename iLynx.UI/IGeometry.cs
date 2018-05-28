using System.Collections.Generic;

namespace iLynx.UI
{
    public interface IGeometry
    {
        IEnumerable<Vertex> Vertices { get; }
    }
}