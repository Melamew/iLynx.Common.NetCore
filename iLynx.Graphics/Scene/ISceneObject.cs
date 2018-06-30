using System;
using System.Collections.Generic;
using System.Text;
using iLynx.Graphics.Rendering;

namespace iLynx.Graphics.Scene
{
    /// <summary>
    /// The base interface for an object to be rendered in a scene
    /// </summary>
    public interface ISceneObject : IDisposable
    {
        /// <summary>
        /// Updates this scene object (Perform transform computations etc. here)
        /// </summary>
        void Update();

        /// <summary>
        /// Render this object to the specified <see cref="IRenderContext"/>
        /// </summary>
        /// <param name="renderContext"></param>
        void Display(IRenderContext renderContext);

        /// <summary>
        /// Gets a read-only collection of this <see cref="ISceneObject"/>'s children
        /// </summary>
        IReadOnlyCollection<ISceneObject> Children { get; }

        /// <summary>
        /// Gets the parent of this <see cref="ISceneObject"/>
        /// </summary>
        ISceneObject Parent { get; }
    }

    //public interface ITransform
    //{
    //    /// <summary>
    //    /// Applies this transform to the specified <see cref="ISceneObject"/>
    //    /// </summary>
    //    /// <param name="obj"></param>
    //    void Apply(ISceneObject obj);
    //}
}
