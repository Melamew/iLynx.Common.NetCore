using System;
using System.Collections.Generic;
using System.Text;

namespace iLynx.UI.Controls
{
    public interface IContentControl : IControl
    {
        IControl Content { get; set; }
    }
}
