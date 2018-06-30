using iLynx.Graphics.Rendering;

namespace iLynx.Graphics.Scene
{
    public class Scene : IScene
    {
        public void Dispose()
        {
            Root?.Dispose();
        }

        public void Display(IRenderContext context)
        {
            Root?.Display(context);
        }

        public void Update()
        {
            Root?.Update();
        }

        public ISceneObject Root { get; set; }
    }
}