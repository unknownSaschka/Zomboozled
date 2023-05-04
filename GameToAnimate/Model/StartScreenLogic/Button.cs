using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Zenseless.Geometry;

namespace Model.StartScreenLogic
{
    public class Button
    {
        public String ButtonText { get; internal set; }
        public Box2D Dimensions { get; internal set; }
        public Vector2 TextOffset { get; internal set; }
        public bool Selected { get; set; } = false;
        public Button(String text, Box2D dimensions)
        {
            ButtonText = text;
            Dimensions = dimensions;
            TextOffset = new Vector2(0.01f, 0.05f);
        }
    }
}
