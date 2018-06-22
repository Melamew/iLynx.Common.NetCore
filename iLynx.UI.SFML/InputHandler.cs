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
using System;
using SFML.System;
using SFML.Window;

namespace iLynx.UI.Sfml
{
    public static class InputHandler
    {
        private static IInputElement focusElement;
        private static bool haveHooked = false;

        public static void HookEvents()
        {
            if (haveHooked) return;
            haveHooked = true;
            EventManager.AddHandler(EventType.MouseMoved, OnMouseMoved);
            EventManager.AddHandler(EventType.MouseButtonPressed, OnMouseButtonDown);
        }

        public static bool InputHitTest(Vector2f position, Window w, out IInputElement element)
        {
            element = null;
            if (!(w.RootElement is IInputElement inputElement) || !inputElement.HitTest(position, out var e))
                return false;
            element = e;
            return true;
        }

        private static void OnMouseButtonDown(Window window, Event e)
        {
            var position = new Vector2f(e.MouseButton.X, e.MouseButton.Y);
            if (!InputHitTest(position, window, out var element)) return;
            if (element.Focusable && focusElement != element)
            {
                focusElement?.OnLostFocus();
                (focusElement = element).OnReceivedFocus();
            }
            element.OnMouseButtonDown(new MouseButtonArgs(position, e.MouseButton.Button));
        }

        private static void OnMouseMoved(Window window, Event e)
        {
            var position = new Vector2f(e.MouseMove.X, e.MouseMove.Y);
            if (InputHitTest(position, window, out var element))
                element.OnMouseOver(new MouseArgs(element.ToLocalCoords(position)));
        }

        //public static void AddMouseMoveHandler(IUIElement element, Action<Vector2f> callback)
        //{

        //}
    }
}