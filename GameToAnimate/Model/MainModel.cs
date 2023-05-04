using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Model.WorldLogic.TileLogic;
using Model.PawnLogic.WeaponLogic;
using Model.WorldLogic.PathfindingLogic;
using Model.WorldLogic;
using Model.PawnLogic;
using Model.InterfaceCollection;
using Extension;
using Model.ItemLogic;
using Model.Audio;
using Model.PawnLogic.EnemyLogic;
using Model.PawnLogic.ProjectileLogic;
using Model.StartScreenLogic;
using DiscordRPC.Logging;

namespace Model
{
    public enum GameState { Menu, Running, End }
    public class MainModel
    {

        
        public StartScreen StartScreen { get; internal set; }
        private float _aspectRatio;
        private bool _gameOver = false;
        private GameState gameState;
        public GameState GameState {
            get { return gameState; }
            set
            {
                switch (value)
                {
                    case GameState.Running:
                        ManagerHolder.DiscordRPC.SetPresence(new DiscordRPC.RichPresence()
                        {
                            Details = "Killt Zombies",
                            State = "0 Zombies gekillt",
                            Assets = new DiscordRPC.Assets()
                            {
                                LargeImageKey = "tootiitoot",
                                LargeImageText = "Zoggd",
                                SmallImageText = "tootiitoot"
                            }
                        });
                        break;
                }
                gameState = value;
            }
        }
        public ManagerHolder ManagerHolder { get; }
        public WorldManager WorldManager { get; }

        public World World { get { return WorldManager.ActiveWorld; } }
        public float AspectRatio => _aspectRatio;
        
        public Generator Generator
        {
            get { return WorldManager.ActiveGenerator; }
        }
        //private Vehicle _vehicle;
        //public Vehicle Vehicle => _vehicle;

        IMovementLogic _movementLogic;
        public IMovementLogic MovementLogic { get { return _movementLogic; } set { _movementLogic = value; } }

        public WeaponManager WeaponManager { get; }

        public void AspectRatioChanged(float aspect)
        {
            _aspectRatio = aspect;
        }
        public bool WorldDebug { get; set; } = false;

        public MainModel()
        {
            GameState = GameState.Menu;
            ManagerHolder = new ManagerHolder(this);

            //Initialisierung für Discord RPC
            ManagerHolder.DiscordRPC.Logger = new ConsoleLogger() { Level = LogLevel.Warning };
            ManagerHolder.DiscordRPC.Initialize();
            ManagerHolder.DiscordRPC.SetPresence(new DiscordRPC.RichPresence()
            {
                State = "Hauptmenü",
                Assets = new DiscordRPC.Assets()
                {
                    LargeImageKey = "tootiitoot",
                    LargeImageText = "Zoggd",
                    SmallImageText = "tootiitoot"
                }
            });

            WorldManager = new WorldManager(ManagerHolder);
            _movementLogic = new MovementLogicFoot(ManagerHolder.PlayerManager.Player);

            WeaponManager = new WeaponManager(ManagerHolder.ProjectileManager);

            StartScreen = new StartScreen();
            //_vehicle = new Vehicle(new Vector2(3, 3), new Vector2(0, 0), 0.3f, 9, 400);
        }

        ~MainModel()
        {
            Console.WriteLine("Main beendet");
        }

        public bool GameOver
        {
            set
            {
                //AudioEvent?.Invoke( Audio.AudioType.BGMusic, 0);
                if (value)
                {
                    ManagerHolder.AudioManager.PlaySong(BackgroundSongName.Dead);
                    
                    System.Timers.Timer timer = new System.Timers.Timer(1000 * 10);
                    //timer.Elapsed += (sender, e) => { Environment.Exit(1); };

                    timer.Elapsed += (sender, e) =>
                    {
                        //WorldManager.ChangeWorld(0);
                        WorldManager.ChangeWorld();
                        ManagerHolder.PlayerManager.Player.LifePoints = ManagerHolder.PlayerManager.Player.MaxLifePoints;
                        ManagerHolder.PlayerManager.Player.ResetPosition();

                        ManagerHolder.PlayerManager.Player.Money = ManagerHolder.PlayerManager.Player.Money/2;

                        ManagerHolder.AudioManager.PlaySong(BackgroundSongName.Leftheria);

                        WeaponManager.ResetAmmoCount();
                        _gameOver = false;
                        timer.Stop();
                    };
                    timer.Start();
                    _gameOver = true;
                }
                
            }
            get { return _gameOver; }
        }

        

        public void Update(InputData inputData, float deltaT)
        {
            
            var itemManager = ManagerHolder.ItemManager;
            var player = ManagerHolder.PlayerManager.Player;
            var tileManager = ManagerHolder.TileManager;
            /*
            itemManager.Update();
            */
            _movementLogic.Move(inputData, player, deltaT);

            if (tileManager.tiles[World.chunks[player.Chunk].TileId].chunkType != (Types.ChunkType.Park))
            { 

                WeaponManager.Update(inputData, player, _aspectRatio, ManagerHolder.AudioManager.AudioEvent, deltaT);
            }
            /*
            ManagerHolder.ProjectileManager.Update(deltaT);
            ManagerHolder.ProjectileManager.CollisonTest(World, ManagerHolder.TileManager,ManagerHolder.EnemyManager);
            */
        }

        
    }
}

