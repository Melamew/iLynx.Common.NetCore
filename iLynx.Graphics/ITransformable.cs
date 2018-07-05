using OpenTK;

namespace iLynx.Graphics
{
    public interface ITransformable
    {
        Vector3 Scale { get; set; }
        Vector3 Translation { get; set; }
        Quaternion Rotation { get; set; }
        Vector3 Origin { get; set; }
        Matrix4 Transform { get; }
        void Translate(Vector3 direction);
        void RotateAround(Vector3 axis, float angle);
    }
}