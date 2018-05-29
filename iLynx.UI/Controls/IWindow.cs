using System;
using System.Collections.Generic;
using System.Text;

namespace iLynx.UI.Controls
{
    public interface IWindow : IUIElement
    {
        /// <summary>
        /// Shows the window on screen
        /// </summary>
        void Show();
    }
}
