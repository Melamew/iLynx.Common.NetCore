using OpenTK;

namespace iLynx.Graphics
{
    public interface ITransformable
    {
        /// <summary>
        /// Gets or Sets the scale of this object
        /// </summary>
        Vector3 Scale { get; set; }
        /// <summary>
        /// Gets or Sets the translation (Position) of this object
        /// </summary>
        Vector3 Translation { get; set; }
        /// <summary>
        /// Gets or Sets the rotation of this object
        /// </summary>
        Quaternion Rotation { get; set; }
        /// <summary>
        /// Gets or Sets the origin point of this object
        /// </summary>
        Vector3 Origin { get; set; }
        /// <summary>
        /// Gets the combined matrix produced by applying all transformations
        /// </summary>
        Matrix4 Transform { get; }

        /// <summary>
        /// Offsets this object by the specified vector
        /// </summary>
        /// <param name="direction">The <see cref="Vector3"/> to offset by - Values greater or less than 1 and -1 are allow - This vector is not normalized</param>
        void Translate(Vector3 direction);

        /// <summary>
        /// Offsets this object by the specified amounts
        /// </summary>
        /// <param name="x">The distance to move in the X direction</param>
        /// <param name="y">The distance to move in the Y direction</param>
        /// <param name="z">The distance to move in the Z direction</param>
        void Translate(float x, float y, float z);

        /// <summary>
        /// Rotates this object around the specified axis by the specified angle
        /// </summary>
        /// <param name="axis">The (object local) axis to rotate around</param>
        /// <param name="angle">The angle in radians to rotate by</param>
        void RotateAround(Vector3 axis, float angle);

        /// <summary>
        /// Rotates this object around the specified axis by the specified angle
        /// </summary>
        /// <param name="axis">The global axis to rotate around</param>
        /// <param name="angle">The angle in radians to rotate by</param>
        void RotateAroundGlobal(Vector3 axis, float angle);
    }
}