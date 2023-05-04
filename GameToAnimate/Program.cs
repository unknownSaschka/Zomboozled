using OpenTK.Input;
using System;
using OpenTK.Graphics;

namespace Game
{
    internal class Program
    {

        [STAThread]
        private static void Main(string[] args)
        {

            Controller.Logic GameLogic = new Controller.Logic();

            GameLogic.Run();
            Console.WriteLine("Spiel beendet!");
            Console.Read();
        }

    }

}
