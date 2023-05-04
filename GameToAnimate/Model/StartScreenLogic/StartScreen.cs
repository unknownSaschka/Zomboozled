using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Zenseless.Geometry;

namespace Model.StartScreenLogic
{
    public class StartScreen
    {
        public List<Button> Buttons { get; internal set; }


        public StartScreen()
        {
            Buttons = new List<Button>();
            Buttons.Add(new Button("START", new Box2D(0.10f, 0.7f, 0.80f, 0.25f)));
        }
    }




}
