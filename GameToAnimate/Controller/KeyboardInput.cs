using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenseless.ExampleFramework;

namespace Controller
{
    public class KeyboardInput
    {
        private KeyboardState keyboardState, lastKeyboardState;
        private ExampleWindow _window;
        public KeyboardInput(ExampleWindow window)
        {
            _window = window;
        }


        public bool ReadKeyboard()
        {
            if (_window.GameWindow.Focused)
            {

                keyboardState = Keyboard.GetState();
                
            }
            return true;
        }

        public void SaveOldKeyboard()
        {
            lastKeyboardState = keyboardState;
        }

        public bool KeyPress(Key key)
        {
            return (keyboardState[key] && (keyboardState[key] != lastKeyboardState[key]));
        }

        public bool HasChanged()
        {
            return !(keyboardState == lastKeyboardState);
        }

        /// <summary>
        /// Mapped zwei Keys zu einer Achse
        /// </summary>
        /// <param name="minKey"></param> Der Key für den unteren Wert der Achse
        /// <param name="maxKey"></param> Der Key für den oberen Wert der Achse
        /// <returns></returns>
        public float Axis(OpenTK.Input.Key minKey, OpenTK.Input.Key maxKey)
        {
            if (keyboardState[minKey] != keyboardState[maxKey])
            {
                return keyboardState[minKey] ? -1.0f : keyboardState[maxKey] ? 1.0f : 0.0f;
            }
            return 0.0f;
        }

        public float AxisTwoPairs(Key minKeyOne, Key maxKeyOne, Key minKeyTwo, Key maxKeyTwo)
        {
            if (keyboardState[minKeyOne] != keyboardState[maxKeyOne])
            {
                return keyboardState[minKeyOne] ? -1.0f : keyboardState[maxKeyOne] ? 1.0f : 0.0f;
            }
            if (keyboardState[minKeyTwo] != keyboardState[maxKeyTwo])
            {
                return keyboardState[minKeyTwo] ? -1.0f : keyboardState[maxKeyTwo] ? 1.0f : 0.0f;
            }
            return 0.0f;
        }

        /// <summary>
        /// Funktion die zurückgibt ob ein einzelner Key gedrückt wurde
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static bool Key(OpenTK.Input.Key keyName)
        {
            return Keyboard.GetState()[keyName];
        }
    }
}

