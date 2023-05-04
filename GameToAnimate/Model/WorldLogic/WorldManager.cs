using Model.InterfaceCollection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenseless.Geometry;

namespace Model.WorldLogic
{
    public class WorldManager
    {
        public World[] World { get; } = new World[3];
        //public World StartWorld { get; }
        public Generator[] Generator { get; } = new Generator[3];
        public StartGenerator StartGenerator { get; }
        private IManagerHolder _manager;
        public int ActiveWorldNum { get; set; }
        public World ActiveWorld { get { return World[ActiveWorldNum]; } }
        public Generator ActiveGenerator
        {
            get
            {
                if (ActiveWorldNum == 0)
                {
                    return null;
                }
                else
                {
                    return Generator[ActiveWorldNum];
                }
            }
        }

        private int prevMainWorld;
        public bool NoWorldChange { get; internal set; }
        public bool NoWorldChanceCol { get; internal set; }
        public int timeWorldChange { get; internal set; } = 30;
        public Stopwatch stopwatch = new Stopwatch();
        public Stopwatch anzeige = new Stopwatch();

        public WorldManager(IManagerHolder manager)
        {
            _manager = manager;
            ActiveWorldNum = 0;
            World[0] = new World(manager);
            World[1] = new World(manager);
            World[2] = new World(manager);

            StartGenerator = new StartGenerator(World[0], manager);
            Generator[1] = new Generator(World[1], manager);
            Generator[2] = new Generator(World[2], manager);

            World[1].FogDistanceStart = 11 * 5;
            World[1].FogDistanceEnd = 12 * 5;
            World[2].FogDistanceStart = 11 * 5;
            World[2].FogDistanceEnd = 12 * 5;

            StartGenerator.GenerateStart();
            Generator[1].GenerateStart();
            Generator[2].GenerateStart();

            _manager.ItemManager.ShopItem();
        }

        public void NewWorld(int worldNumber)
        {
            if (worldNumber == 0)
            {
                Console.WriteLine("Fehler bei WorldNum. StartWorld kann nicht neu erstellt werden!");
                return;
            }
            else if (worldNumber == 1) ActiveWorldNum = 2;
            else ActiveWorldNum = 1;
            prevMainWorld = 2;

            _manager.ItemManager.DeleteShopItems();
            _manager.ItemManager.DeleteItems();
            _manager.VehicleManager.RemoveVehicles();
            _manager.ProjectileManager.ClearAllProjectiles();
            World[worldNumber] = new World(_manager);
            Generator[worldNumber] = new Generator(World[worldNumber], _manager);
            
        }

        public void Update()
        {
            if (anzeige.IsRunning)
            {
                if(anzeige.Elapsed.Seconds > 3)
                {
                    NoWorldChange = false;
                    anzeige.Stop();
                }
            }

            if(stopwatch.Elapsed.Seconds >= timeWorldChange)
            {
                NoWorldChanceCol = false;
            }

            if(stopwatch.IsRunning && stopwatch.Elapsed.Seconds > timeWorldChange + 5)
            {
                stopwatch.Stop();
            }
        }

        public void ChangeWorld(int WorldNum)
        {
            int oldWorldNum = ActiveWorldNum;
            ActiveWorldNum = WorldNum;

            if(WorldNum == 0)
            {
                _manager.ItemManager.DeleteShopItems();
                _manager.ItemManager.DeleteItems();
                _manager.VehicleManager.RemoveVehicles();
                _manager.ProjectileManager.ClearAllProjectiles();
                World[oldWorldNum] = new World(_manager);
                Generator[oldWorldNum] = new Generator(World[oldWorldNum], _manager);
            }
        }

        public bool ChangeWorld()
        {
            if(stopwatch.Elapsed.Seconds < timeWorldChange && stopwatch.IsRunning)
            {
                NoWorldChange = true;
                anzeige.Restart();

                Circle player = _manager.PlayerManager.Player.Collider;
                

                return false;
            }
            else
            {
                NoWorldChange = false;
                stopwatch.Stop();
            }

            if(ActiveWorldNum == 0)
            {
                if (prevMainWorld == 1) ActiveWorldNum = 2;
                else ActiveWorldNum = 1;

                _manager.PlayerManager.Player.ChangePlayerChunkToZero();
                _manager.ItemManager.DeleteShopItems();
                ActiveGenerator.Generate();
                _manager.VehicleManager.AddCar(new System.Numerics.Vector2(0, 0), new System.Numerics.Vector2(1, 0), 0.3f, 10, 400, PawnLogic.VehicleLogic.CarType.Car);
                _manager.AudioManager.ChangeBackgroundMusic(TileLogic.Types.ChunkType.Boss, (int)_manager.PlayerManager.Distance);
                stopwatch.Restart();
                NoWorldChanceCol = true;
            }
            else
            {
                prevMainWorld = ActiveWorldNum;
                ActiveWorldNum = 0;
                _manager.EnemyManager.ClearAllEnemies();
                _manager.ItemManager.DeleteShopItems();
                _manager.ItemManager.DeleteItems();
                _manager.VehicleManager.RemoveVehicles();
                _manager.ProjectileManager.ClearAllProjectiles();
                _manager.BossManager.ClearAllBosses();

                if (_manager.PlayerManager.Player.IsInCar)
                {
                    _manager.VehicleManager.InteractWithCar(true);
                }

                _manager.ItemManager.ShopItem();

                World[prevMainWorld] = new World(_manager);
                Generator[prevMainWorld] = new Generator(World[prevMainWorld], _manager);
                World[prevMainWorld].FogDistanceStart = 11 * 5;
                World[prevMainWorld].FogDistanceEnd = 12 * 5;

                Generator[prevMainWorld].GenerateStart();
                _manager.PlayerManager.Player.ChangePlayerChunkToZero();
            }
            return true;
        }
    }
}
