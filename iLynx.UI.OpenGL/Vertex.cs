using System.Numerics;

namespace iLynx.UI.OpenGL
{
    /// <summary>
    /// The base structure for each and every vertex
    /// All vertex shaders will be based upon this.
    /// </summary>
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
    }
}