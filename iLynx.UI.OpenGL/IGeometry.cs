using System.Collections.Generic;

namespace iLynx.UI.OpenGL
{
    public interface IGeometry
    {
        void GenerateVertexArrays();

        void DeleteVertexArrays();
    }
}