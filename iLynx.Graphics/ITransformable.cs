#region LICENSE
/*
 * Copyright 2018 Melanie Hjorth
 *
 * Redistribution and use in source and binary forms,
 * with or without modification,
 * are permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice,
 * this list of conditions and the following disclaimer.
 *
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 * this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
 *
 * 3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote
 * products derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
 * OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
 * EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 */
#endregion
using OpenTK;

namespace iLynx.Graphics
{
    /// <summary>
    /// Interface defining various methods and properties for transforming an object.
    /// </summary>
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