using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.PawnLogic.BossLogic;
using Model.WorldLogic.PathfindingLogic;
using Model.WorldLogic.TileLogic;

namespace Model
{
    public interface IManagerHolder
    {
        ItemLogic.ItemManager ItemManager { get; }
        PawnLogic.EnemyLogic.EnemyManager EnemyManager { get; }
        Pathfinding Pathfinding { get; }
        TileManager TileManager { get; }
        PawnLogic.ProjectileLogic.ProjectileManager ProjectileManager { get; }
        PawnLogic.VehicleLogic.VehicleManager VehicleManager { get; }

        PawnLogic.PlayerLogic.PlayerManager PlayerManager { get; }

        Audio.AudioManager AudioManager { get; }
        PawnLogic.BossLogic.BossManager BossManager{ get;}
    }
}
