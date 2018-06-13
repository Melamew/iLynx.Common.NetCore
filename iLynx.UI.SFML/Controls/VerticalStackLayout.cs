using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.SFML.Controls
{
    public class VerticalStackLayout : Panel
    {
        private bool reverse;
        public bool Reverse
        {
            get => reverse;
            set
            {
                if (value == reverse) return;
                var old = reverse;
                reverse = value;
                OnPropertyChanged(old, value);
            }
        }

        //public void Layout()
        //{
        //    var pos = Position;
        //    var dims = Dimensions;
        //    var availableSpace = new FloatRect(pos, dims);
        //    var usedSpace = new FloatRect(pos, new Vector2f(dims.X, 0f));
        //    foreach (var child in Children)
        //    {
        //        var childSpace = child.Measure();
        //        usedSpace.Height += childSpace.Y;
        //        availableSpace.Top += childSpace.Y;
        //        availableSpace.Height -= childSpace.Y;
        //    }
        //    BoundingBox = usedSpace;
        //}

        protected override FloatRect ComputeBoundingBox(FloatRect destinationRect)
        {
            return destinationRect;
            //return base.ComputeBoundingBox(destinationRect);
        }

        protected override void PrepareRender()
        {
            
        }
    }
}