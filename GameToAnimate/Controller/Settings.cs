using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

namespace Controller
{
    public class KeyBindings
    {
        public OpenTK.Input.Key Forwards = Key.W;
        public OpenTK.Input.Key Backwards = Key.S;
        public OpenTK.Input.Key Left = Key.A;
        public OpenTK.Input.Key Right = Key.D;
    }

    public class GameSettings
    {
        public int ScreenWidth = 512;
        public int ScreenHeigth = 512;
        public Boolean Fullscreen = false;
        public KeyBindings PrimaryKeyBindings, SecondaryKeyBindings;

        

        public GameSettings()
        {
            PrimaryKeyBindings = new KeyBindings();
            SecondaryKeyBindings = new KeyBindings
            {
                Backwards = Key.Down,
                Forwards = Key.Up,
                Right = Key.Right,
                Left = Key.Left
            };
        }
    }
}
