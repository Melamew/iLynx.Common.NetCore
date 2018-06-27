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
using System.Collections.Generic;
using iLynx.Common;
using OpenTK.Input;
using OpenTK;

namespace iLynx.UI.OpenGL.Input
{
    public static class InputHandler
    {
        private delegate void InputEventMapper(IInputElement sender, InputEventArgs e);
        private static IInputElement focusElement;
        private static MouseButton lastDownButton;
        private static readonly Stack<IInputElement> MouseOverStack = new Stack<IInputElement>();

        private static readonly Dictionary<Type, List<InputEventMapper>> InputMappers =
            new Dictionary<Type, List<InputEventMapper>>();

        static InputHandler()
        {
            //EventDispatcher.AddHandler(EventType.MouseMoved, OnMouseMoved);
            //EventDispatcher.AddHandler(EventType.MouseButtonPressed, OnMouseButtonDown);
            //EventDispatcher.AddHandler(EventType.MouseButtonReleased, OnMouseButtonUp);
            //EventDispatcher.AddHandler(EventType.KeyPressed, OnKeyDown);
            //EventDispatcher.AddHandler(EventType.KeyReleased, OnKeyUp);
            //EventDispatcher.AddHandler(EventType.TextEntered, OnTextEntered);
            //EventDispatcher.AddHandler(EventType.MouseWheelScrolled, OnMouseScroll);
            //EventDispatcher.AddHandler(EventType.MouseLeft, OnMouseLeft);
        }

        public static void Register<TElement, TArgs>(InputCallback<TElement, TArgs> handler) where TElement : IInputElement where TArgs : InputEventArgs
        {
            InputMappers.AddOrUpdateMany(typeof(TArgs), (element, args) => handler((TElement)element, (TArgs)args));
        }

        public static bool RequestFocus(IInputElement element)
        {
            if (null == element) throw new ArgumentNullException(nameof(element));
            if (!element.IsFocusable) return false;
            Raise(focusElement, new LostFocusEventArgs());
            focusElement = element;
            Raise(focusElement, new GotFocusEventArgs());
            return true;
        }

        private static void Raise(IInputElement target, InputEventArgs e)
        {
            if (InputMappers.TryGetValue(e.GetType(), out var mappers))
                mappers.ForEach(mapper => mapper?.Invoke(target, e));
        }

        public static bool InputHitTest(PointF position, Window w, out IInputElement element)
        {
            element = null;
            if (!(w.RootElement is IInputElement inputElement) || !inputElement.HitTest(position, out var e))
                return false;
            element = e;
            return true;
        }

        //private static void OnMouseLeft(Window source, Event e)
        //{
        //    while (MouseOverStack.TryPop(out var element))
        //        Raise(element, new MouseLeaveEventArgs((Vector2)Mouse.GetPosition(source)));
        //}

        //private static void OnTextEntered(Window source, Event e)
        //{
        //    var f = focusElement;
        //    if (null == f) return;
        //    var utf32 = unchecked((int)e.Text.Unicode);
        //    Raise(f, new TextInputEventArgs(char.ConvertFromUtf32(utf32)));
        //}

        //private static void OnMouseScroll(Window source, Event e)
        //{
        //    var position = new Vector2(e.MouseWheelScroll.X, e.MouseWheelScroll.Y);
        //    if (InputHitTest(position, source, out var element))
        //        Raise(element, new MouseScrollEventArgs(position, e.MouseWheelScroll.Delta, e.MouseWheelScroll.Wheel));
        //}

        //private static ModifierKeys GetModifiers(KeyEvent e)
        //{
        //    var result = ModifierKeys.None;
        //    result |= e.Alt > 0 ? ModifierKeys.Alt : ModifierKeys.None;
        //    result |= e.Control > 0 ? ModifierKeys.Control : ModifierKeys.None;
        //    result |= e.System > 0 ? ModifierKeys.System : ModifierKeys.None;
        //    result |= e.Shift > 0 ? ModifierKeys.Shift : ModifierKeys.None;
        //    return result;
        //}

        //private static void OnKeyUp(Window source, Event e)
        //{
        //    var f = focusElement;
        //    if (null != f)
        //        Raise(f, new KeyUpEventArgs(e.Key.Code, GetModifiers(e.Key)));
        //}

        //private static void OnKeyDown(Window source, Event e)
        //{
        //    var f = focusElement;
        //    if (null != f)
        //        Raise(f, new KeyDownEventArgs(e.Key.Code, GetModifiers(e.Key)));
        //}

        //private static void OnMouseButtonUp(Window source, Event e)
        //{
        //    var position = new Vector2(e.MouseButton.X, e.MouseButton.Y);
        //    if (!InputHitTest(position, source, out var element)) return;
        //    if (focusElement == element && e.MouseButton.Button == lastDownButton)
        //        Raise(element, new MouseButtonInputEventArgs(position, e.MouseButton.Button));
        //    Raise(element, new MouseUpEventArgs(position, e.MouseButton.Button));
        //}

        //private static void OnMouseButtonDown(Window window, Event e)
        //{
        //    var position = new Vector2(e.MouseButton.X, e.MouseButton.Y);
        //    if (!InputHitTest(position, window, out var element)) return;
        //    if (element.IsFocusable && focusElement != element)
        //        RequestFocus(element);
        //    lastDownButton = e.MouseButton.Button;
        //    Raise(element, new MouseDownEventArgs(position, e.MouseButton.Button));
        //}

        //private static void OnMouseMoved(Window window, Event e)
        //{
        //    var position = new Vector2(e.MouseMove.X, e.MouseMove.Y);
        //    if (!InputHitTest(position, window, out var element)) return;
        //    var localPosition = element.ToLocalCoords(position);
        //    Raise(element, new MouseEventArgs(localPosition));
        //    if (MouseOverStack.Count == 0)
        //    {
        //        MouseOverStack.Push(element);
        //        Raise(element, new MouseEnterEventArgs(localPosition));
        //        return;
        //    }

        //    while (MouseOverStack.TryPop(out var parent))
        //    {
        //        if (!element.IsChildOf(parent) && element != parent)
        //            Raise(parent, new MouseLeaveEventArgs(parent.ToLocalCoords(position)));
        //        else
        //        {
        //            MouseOverStack.Push(parent);
        //            if (element != parent)
        //            {
        //                MouseOverStack.Push(element);
        //                Raise(element, new MouseEnterEventArgs(localPosition));
        //            }

        //            break;
        //        }
        //    }
        //}

        private static bool IsChildOf(this IRenderElement potentialChild, IRenderElement potentialParent)
        {
            while (null != potentialChild)
            {
                potentialChild = potentialChild.Parent;
                if (potentialChild == potentialParent) return true;
            }
            return false;
        }
    }
}