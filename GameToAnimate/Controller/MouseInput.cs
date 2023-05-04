using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using OpenTK.Input;


namespace Controller
{
    public static class MouseInput
    {
        private static int oldMouseState;
        private static OpenTK.Input.MouseState _oldMouseState;
        public static int GetMouseDelta()
        {
            //int newMouseState = oldMouseState - OpenTK.Input.Mouse.GetState().Wheel;
            int newMouseState = OpenTK.Input.Mouse.GetState().Wheel;

            int returnState = oldMouseState - newMouseState;
            oldMouseState = newMouseState;
            return returnState;
        }

        public static Vector2 GetMousePosition()
        {
            Vector2 position = new Vector2(OpenTK.Input.Mouse.GetCursorState().X, OpenTK.Input.Mouse.GetCursorState().Y);


            return position;
        }

        public static Boolean LeftDown()
        {

            return OpenTK.Input.Mouse.GetState().IsButtonDown(MouseButton.Left);
        }

        public static Boolean LeftPressed()
        {
            MouseState newState = OpenTK.Input.Mouse.GetState();
            if (newState.LeftButton != _oldMouseState.LeftButton)
            {
                if (newState.LeftButton == ButtonState.Pressed)
                {
                    _oldMouseState = newState;
                    return true;
                }
                _oldMouseState = newState;


            }

            return false;
        }

    }
}
