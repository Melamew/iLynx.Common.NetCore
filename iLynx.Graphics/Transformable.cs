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
    public abstract class Transformable : ITransformable
    {
        private Vector3 origin;
        private Quaternion rotation = Quaternion.Identity;
        private Vector3 size = new Vector3(1f);
        private Vector3 translation = new Vector3(0f);

        protected Transformable()
        {
            Update();
        }

        /// <inheritdoc/>
        public Vector3 Scale
        {
            get => size;
            set
            {
                if (value == size) return;
                size = value;
                Update();
            }
        }

        /// <inheritdoc/>
        public void Translate(Vector3 direction)
        {
            translation += direction;
            Update();
        }

        /// <inheritdoc/>
        public void Translate(float x, float y, float z)
        {
            Translate(new Vector3(x, y, z));
        }

        /// <inheritdoc/>
        public void RotateAround(Vector3 axis, float angle)
        {
            rotation *= Quaternion.FromAxisAngle(axis, angle);
            Update();
        }

        /// <inheritdoc/>
        public Vector3 Translation
        {
            get => translation;
            set
            {
                if (value == translation) return;
                translation = value;
                Update();
            }
        }

        /// <inheritdoc/>
        public Quaternion Rotation
        {
            get => rotation;
            set
            {
                if (value == rotation) return;
                rotation = value;
                Update();
            }
        }

        /// <inheritdoc/>
        public Vector3 Origin
        {
            get => origin;
            set
            {
                if (value == origin) return;
                origin = value;
                Update();
            }
        }

        /// <inheritdoc/>
        public Matrix4 Transform { get; private set; } = Matrix4.Identity;


        /// <inheritdoc/>
        public void RotateAroundGlobal(Vector3 axis, float angle)
        {
            rotation = Quaternion.FromAxisAngle(axis, angle) * rotation;
            Update();
        }

        private void Update()
        {
            Transform = Matrix4.Identity;
            Transform *= Matrix4.CreateTranslation(-origin);
            Transform *= Matrix4.CreateFromQuaternion(rotation);
            Transform *= Matrix4.CreateScale(size);
            Transform *= Matrix4.CreateTranslation(translation);
            OnTransformChanged();
        }

        /// <summary>
        /// Called whenever the final transform of the object has changed (<see cref="Transform"/>)
        /// </summary>
        protected virtual void OnTransformChanged()
        {
        }
    }
}