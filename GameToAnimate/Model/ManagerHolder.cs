using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.ItemLogic;
using Model.PawnLogic.EnemyLogic;
using Model.PawnLogic.ProjectileLogic;
using Model.WorldLogic.PathfindingLogic;
using Model.WorldLogic.TileLogic;
using Model.PawnLogic.PlayerLogic;
using Model.PawnLogic.VehicleLogic;
using Model.PawnLogic.BossLogic;
using DiscordRPC;

namespace Model
{
    public class ManagerHolder : IManagerHolder
    {
        public DiscordRpcClient DiscordRPC { get; set; }
        public ItemManager ItemManager { get; }

        public EnemyManager EnemyManager { get; }

        public Pathfinding Pathfinding { get; }

        public TileManager TileManager { get; }

        public ProjectileManager ProjectileManager { get; }

        public PlayerManager PlayerManager { get; }

        public VehicleManager VehicleManager { get; }

        public BossManager BossManager { get; }

        public Audio.AudioManager AudioManager { get; }

        public ManagerHolder(MainModel model)
        {
            DiscordRPC = new DiscordRpcClient("526373305830080512");

            PlayerManager = new PlayerManager(DiscordRPC);
            
            ItemManager = new ItemManager(PlayerManager.Player, model);

            TileManager = new TileManager();

            Pathfinding = new Pathfinding(4);

            TileManager.LoadTiles();

            AudioManager = new Audio.AudioManager();

            VehicleManager = new VehicleManager(this, model);

            ProjectileManager = new ProjectileManager(model);
            EnemyManager = new EnemyManager(PlayerManager, ItemManager, TileManager);
            BossManager = new BossManager();
            
        }
    }
}
