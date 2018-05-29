
using System.Runtime.InteropServices;
using OpenTK;

namespace iLynx.UI.OpenGL
{
    /// <summary>
    /// The base structure for each and every vertex
    /// All vertex shaders will be based upon this.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vertex
    {
        /// <summary>
        /// The position of the vertex in 3space
        /// </summary>
        public Vector3 Position;
        
        /// <summary>
        /// The normal ("facing") of this vertex
        /// </summary>
        public Vector3 Normal;

        public Vertex(Vector3 position)
         : this(position, default(Vector3))
        {
            
        }

        public Vertex(Vector3 position, Vector3 normal)
        {
            Position = position;
            Normal = normal;
        }
    }
}