using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Model.InterfaceCollection
{
    public interface IWorldObject
    {
        Vector2 Chunk { get; }
        Vector2 Position { get; }



    }
}
