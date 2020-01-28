using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using OpenTK;

namespace iLynx.Graphics.Maths
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LineSegment3D
    {
        public readonly Vector3 P1;
        public readonly Vector3 P2;
    }
}
