using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.WorldLogic.TileLogic
{
    [Serializable]
    public static class Types
    {
        public enum ChunkType { Start, CityStreet, Park, Arena, Boss, EndGrass };
        public enum ArenaType { };
        public enum BossType { };
        public enum ObjectType { Street, Grass, Sidewalk, Building, Wall, Ground, Structure, Object, Item, Enemy, Player, TypeCount, Projectile, StartHouse, ShopGround };
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
        [Serializable]
        public struct StreetOut { public bool up; public bool down; public bool left; public bool right; public bool free; };
    }

}
