using Model.WorldLogic.TileLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.WorldLogic.TileLogic
{
    [Serializable]
    public class TileHolder
    {
        public decimal Version = 0.000061m;
        public List<TileSave> SaveTiles = new List<TileSave>();
    }
}
