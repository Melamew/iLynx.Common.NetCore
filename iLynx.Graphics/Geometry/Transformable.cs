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

namespace iLynx.Graphics.Geometry
{
    public abstract class Transformable
    {
        private Vector3 scale = new Vector3(1f);
        private Vector3 translation = new Vector3(0f);
        private Quaternion rotation = Quaternion.Identity;
        private Vector3 origin;
        private Matrix4 transform = Matrix4.Identity;

        public Vector3 Scale
        {
            get => scale;
            set
            {
                if (value == scale) return;
                scale = value;
                Update();
            }
        }

        protected Transformable()
        {
            Update();
        }

        public void Translate(Vector3 direction)
        {
            transform *= Matrix4.CreateTranslation(direction);
        }

        public void RotateAround(Vector3 axis, float angle)
        {
            transform *= Matrix4.CreateFromAxisAngle(axis, angle);
        }

        private void Update()
        {
            transform =
                Matrix4.CreateTranslation(-origin) *
                Matrix4.CreateFromQuaternion(rotation) *
                Matrix4.CreateScale(scale);
            transform *= Matrix4.CreateTranslation(translation);
        }

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

        public Matrix4 Transform
        {
            get => transform;
            set
            {
                if (value == transform) return;
                transform = value;
                rotation = transform.ExtractRotation();
                scale = transform.ExtractScale();
                translation = transform.ExtractTranslation();
            }
        }
    }
}