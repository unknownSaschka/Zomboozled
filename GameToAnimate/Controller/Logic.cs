using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.PawnLogic.WeaponLogic;
using OpenTK;
using Vector2 = System.Numerics.Vector2;
using Zenseless.ExampleFramework;
using OpenTK.Input;
using System.Xml.Serialization;
using Extension;
using Model.WorldLogic.TileLogic;
using Model.XMLParser;
using Model.PawnLogic;
using Model.WorldLogic;
using Zenseless.HLGL;
using Zenseless.Geometry;
using Model.PawnLogic.PlayerLogic;
using Model.ItemLogic;
using Zenseless.Base;
using Model.StartScreenLogic;

namespace Controller
{
    public class Logic
    {
        private const string LoadedMessage = "Loaded Settings!";

        //private readonly Random _ran = new Random();
        //private readonly int _updateDistance = 4;

        private KeyboardInput _keyboardInput;
        private GameSettings _settings;
        private Model.MainModel _model;
        private View.MainView _view;
        private GameTime _time;
        private InputData _inputData;
        //private Generator _generator;
        private Collisions _collisions;
        //private IPawn pawn;
        //private bool backgroundAudio = true;



        //private Collisions _collisions;
        //private IPawn pawn;





        private void LoadSettingsFromXML()
        {
            _settings = new GameSettings();

            XMLManager<GameSettings> settingsReader = new XMLManager<GameSettings>();
            const string Filename = "Settings.xml";
            settingsReader.DeserealizeObj(Filename, ref _settings);
            //Console.WriteLine(LoadedMessage);
            //Console.WriteLine(_settings.ScreenWidth + " " + _settings.ScreenHeigth);
        }

        private void InitView()
        {
            using (_view = new View.MainView(_model))
            {

                _view.window = new ExampleWindow(_settings.ScreenWidth, _settings.ScreenHeigth, 60);
                _view.Cam = new View.Camera(_settings.ScreenWidth, _settings.ScreenHeigth);


                _view.Init();

                _inputData.xAxis = 0;
                _inputData.yAxis = 0;

                _time = new GameTime();
                _keyboardInput = new KeyboardInput(_view.window);
                _model.ManagerHolder.AudioManager.AudioEvent += (Model.Audio.AudioType audioType, int id) => _view.PlayAudio(audioType, id);
                _view.window.GameWindow.Resize += (sender, eventArgs) => Logic.Window_Resize(_view.Cam, _view.window, _model);
                _view.window.Update += (deltaT) => Window_Update(ref _inputData, this, deltaT, _view.Cam, _view.window, _settings);
                _view.window.Render += () => _view.Draw(_model, _view.Cam, _model.ManagerHolder, _time.AbsoluteTime);

                _view.window.Run();
            }

        }
        public Logic()
        {
            LoadSettingsFromXML();

            _inputData = new InputData
            {
                xAxis = 0,
                yAxis = 0
            };

            _model = new MainModel();


            _collisions = new Collisions(_model.ManagerHolder, _model);

            _model.ManagerHolder.PlayerManager.PlayerChunkChangedEvent += () => _model.ManagerHolder.PlayerManager.CheckPlayerChunk(_model.ManagerHolder, _model.World, _model.Generator, _model);
        }

        public void Run()
        {
            InitView();
            //_view.StopAudio();
        }


        public void Update(InputData keyData, float deltaT)
        {
            if (_model.GameState == GameState.Running)
            {
                if (!_model.GameOver)
                    UpdateGame(keyData, deltaT);
            }
            else
            {
                MenuLogic(keyData);
            }
        }

        private void UpdateGame(InputData keyData, float deltaT)
        {
            Vector2 currentChunk = _model.ManagerHolder.PlayerManager.Player.Chunk;
            if (_model.ManagerHolder.PlayerManager.Player.UpdateCurrentChunk())
            {
                _model.World.chunks[currentChunk].Direction = Chunk.DirectionToPlayer.NoWay;
                _model.ManagerHolder.PlayerManager.PlayerChunkChangedEvent?.Invoke();
            }

            _model.Update(keyData, deltaT);

            var itemManager = _model.ManagerHolder.ItemManager;
            var player = _model.ManagerHolder.PlayerManager.Player;
            //var tileManager = _model.ManagerHolder.TileManager;
            itemManager.Update();
            UpdatePawns(deltaT);

            if (PullItems(player, new Vector2(0, 0), 10, deltaT) ||
                PullItems(player, new Vector2(0, 1), 10, deltaT) ||
                PullItems(player, new Vector2(1, 0), 10, deltaT) ||
                PullItems(player, new Vector2(0, -1), 10, deltaT) ||
                PullItems(player, new Vector2(-1, 0), 10, deltaT))
            {
                PullItemsLogic(player);

            }

            _collisions.CheckAllCollisions(keyData);
        }

        private void UpdatePawns(float deltaT)
        {
            _model.ManagerHolder.ProjectileManager.Update(deltaT);
            _model.ManagerHolder.ProjectileManager.CollisonTest(_model.World, _model.ManagerHolder.TileManager, _model.ManagerHolder.EnemyManager, _model.ManagerHolder.BossManager, _model.ManagerHolder.PlayerManager, _model.ManagerHolder.AudioManager, _model.ManagerHolder.PlayerManager.Player.BaseDamage);

            _model.WorldManager.Update();

            _model.ManagerHolder.EnemyManager.Update(_model, 2, deltaT);
            _model.ManagerHolder.BossManager.Update(_model, 2, deltaT);
            _model.ManagerHolder.VehicleManager.Update();

            _model.ManagerHolder.PlayerManager.Update(_model);
        }

        private void PullItemsLogic(Player player)
        {
            var updateDistance = 1;
            var linq = _model.ManagerHolder.ItemManager.ItemList.Select(i => i).Where(i => i.Key.X <= player.Chunk.X + updateDistance && i.Key.X >= player.Chunk.X - updateDistance && i.Key.Y <= player.Chunk.Y + updateDistance && i.Key.Y >= player.Chunk.Y - updateDistance);


            foreach (var itemList in linq.ToList())
            {
                foreach (var item in itemList.Value.ToList())
                {
                    if (item.UpdateCurrentChunk())
                    {
                        _model.ManagerHolder.ItemManager.ItemList.Remove(itemList.Key, item);
                        _model.ManagerHolder.ItemManager.ItemList.Add(item.Chunk, item);

                    }
                }

            }
        }

        private void MenuLogic(InputData keyData)
        {
            if (keyData.shoot)
            {
                //Console.WriteLine(keyData.MousePos);

                foreach (Button but in _model.StartScreen.Buttons)
                {

                    if (keyData.MousePos.X > but.Dimensions.MinX && keyData.MousePos.X < but.Dimensions.MaxX && keyData.MousePos.Y < but.Dimensions.MaxY && keyData.MousePos.Y > but.Dimensions.MinY)
                    {
                        //Console.WriteLine("PRESS! " + but.Dimensions.MinX + " " + but.Dimensions.MaxX + " " + but.Dimensions.MaxY + " " + but.Dimensions.MinY);
                        //Console.WriteLine("PRESS " + but.Dimensions + " " + keyData.MousePos);

                        _view.Cam.zoom = 2;

                        _model.GameState = GameState.Running;
                        _model.ManagerHolder.AudioManager.PlaySong(Model.Audio.BackgroundSongName.Leftheria);
                    }
                }



            }
        }

        private bool PullItems(Player player, Vector2 chunkDir, float pickupRadius, float deltaT)
        {
            var colliderPlayer = new Circle(player.Position.X - chunkDir.X * 5, player.Position.Y - chunkDir.Y * 5, player.Radius * pickupRadius);
            var items = _model.ManagerHolder.ItemManager.GetItemsOfChunk(_model.ManagerHolder.PlayerManager.Player.Chunk + chunkDir);
            bool changed = false;
            //Console.WriteLine("GOT " + items.Count<Item>()  + "ITEMS");
            foreach (var item in items)
            {
                if (colliderPlayer.Intersects(item.Collider))
                {

                    item.MoveTorwards(colliderPlayer.Center, deltaT);
                    changed = true;
                }
            }
            return changed;
        }

        private static void Window_Resize(View.Camera cam, ExampleWindow window, Model.MainModel _model)
        {
            cam.height = window.GameWindow.Height;
            cam.width = window.GameWindow.Width;
            _model.AspectRatioChanged((float)window.GameWindow.Width / window.GameWindow.Height);
        }

        private void Window_Update(ref InputData input, Logic logic, float deltaT, View.Camera cam, ExampleWindow window, GameSettings settings)
        {


            input.shoot = false;
            _keyboardInput.ReadKeyboard();

            if (_keyboardInput.HasChanged())
            {
                input.xAxis = _keyboardInput.Axis(settings.PrimaryKeyBindings.Left, settings.PrimaryKeyBindings.Right);
                input.yAxis = _keyboardInput.Axis(settings.PrimaryKeyBindings.Backwards, settings.PrimaryKeyBindings.Forwards);


                if (input.xAxis == 0)
                {
                    input.xAxis = _keyboardInput.Axis(settings.SecondaryKeyBindings.Left, settings.SecondaryKeyBindings.Right);
                }
                if (input.yAxis == 0)
                {
                    input.yAxis = _keyboardInput.Axis(settings.SecondaryKeyBindings.Backwards, settings.SecondaryKeyBindings.Forwards);
                }

                DebugMessages(logic);

                if (_keyboardInput.KeyPress(Key.E))
                {

                    _model.ManagerHolder.VehicleManager.InteractWithCar(false);
                }


                //input.shoot = _keyboardInput.KeyPress(Key.Space);

                ChangeWeapon();

                //if (_keyboardInput.KeyPress(Key.F10))
                //{
                //    if (backgroundAudio)
                //    {
                //        _view.PauseAudio();
                //        backgroundAudio = false;
                //    }
                //    else
                //    {
                //        _view.ResumeAudio();
                //        backgroundAudio = true;
                //    }
                //}

                DebugSpeed();

                _keyboardInput.SaveOldKeyboard();
            }

            var mouseWheel = MouseInput.GetMouseDelta();
            if (mouseWheel != 0 && _model.GameState == GameState.Running)
            {
                cam.ChangeZoom(mouseWheel);
                //Console.WriteLine(mouseWheel);
            }

            if (MouseInput.LeftPressed())
            {

                Vector2 mousePos = MouseInput.GetMousePosition();


                input.MousePos = ScreenToGame(mousePos, window);


                //Console.WriteLine(pos);

                input.shoot = true;

            }

            if (MouseInput.LeftDown())
            {
                Vector2 mousePos = MouseInput.GetMousePosition();
                input.MousePos = ScreenToGame(mousePos, window);

                input.shootDown = true;
            }
            else
            {
                input.shootDown = false;
            }

            logic.Update(input, deltaT);

        }

        private void DebugMessages(Logic logic)
        {
            if (_keyboardInput.KeyPress(Key.P) == true)
            {
                //Console.WriteLine("PlayerPosition: " + _model.ManagerHolder.PlayerManager.Player.Collider.CenterX + " , " + _model.ManagerHolder.PlayerManager.Player.Collider.CenterY);
            }
            if (_keyboardInput.KeyPress(Key.O) == true)
            {
                const string ChunkPosString = "ChunkPos: ";
                //Console.WriteLine(ChunkPosString + logic._model.ManagerHolder.PlayerManager.Player.Chunk.X + " , " + logic._model.ManagerHolder.PlayerManager.Player.Chunk.Y);
            }
            if (_keyboardInput.KeyPress(Key.F3) == true)
            {
                _view.Debug = !_view.Debug;
            }

            if (_keyboardInput.KeyPress(Key.L) == true)
            {
                if (_model.WorldManager.ActiveWorldNum == 0) return;
                _model.Generator.ShowOutStreet();
            }
            if (_keyboardInput.KeyPress(Key.Q) == true)
            {
                Console.WriteLine("LookWhere it goes! " + _model.World.chunks[_model.ManagerHolder.PlayerManager.Player.Chunk].Direction);
            }
            if (_keyboardInput.KeyPress(Key.Q) == true)
            {
                Console.WriteLine("LookWhere it goes! " + _model.World.chunks[_model.ManagerHolder.PlayerManager.Player.Chunk].Direction);
            }
            if (_keyboardInput.KeyPress(Key.N) == true)
            {
                if (_model.ManagerHolder.VehicleManager.ActiveVehicle != null)
                {
                    Console.WriteLine($"Car: {_model.ManagerHolder.VehicleManager.ActiveVehicle.Chunk} Player: {_model.ManagerHolder.PlayerManager.Player.Chunk}");
                }
            }
            if (_keyboardInput.KeyPress(Key.B) == true)
            {
                _model.WorldDebug = !_model.WorldDebug;
            }
            if (_keyboardInput.KeyPress(Key.V) == true)
            {
                _model.World.FogDistanceStart += 10 * 5;
                _model.World.FogDistanceEnd += 10 * 5;
            }


        }

        private void DebugSpeed()
        {
            if (_keyboardInput.KeyPress(Key.KeypadPlus))
            {
                _model.ManagerHolder.PlayerManager.Player.Speed += 5;
            }
            if (_keyboardInput.KeyPress(Key.KeypadMinus))
            {
                _model.ManagerHolder.PlayerManager.Player.Speed -= 5;
            }
        }

        private void ChangeWeapon()
        {
            if (_keyboardInput.KeyPress(Key.Number1))
            {
                _model.WeaponManager.SwitchToWeapon(1);
            }
            if (_keyboardInput.KeyPress(Key.Number2))
            {
                _model.WeaponManager.SwitchToWeapon(2);
            }
            if (_keyboardInput.KeyPress(Key.Number3))
            {
                _model.WeaponManager.SwitchToWeapon(3);
            }
            if (_keyboardInput.KeyPress(Key.Number4))
            {
                _model.WeaponManager.SwitchToWeapon(4);
            }
            if (_keyboardInput.KeyPress(Key.Number5))
            {
                _model.WeaponManager.SwitchToWeapon(5);
            }
        }

        public static Vector2 ScreenToGame(Vector2 mousePos, ExampleWindow window)
        {
            System.Drawing.Point pos = window.GameWindow.PointToClient(new System.Drawing.Point((int)mousePos.X, (int)mousePos.Y));
            mousePos.X = (float)pos.X / window.GameWindow.Width;
            mousePos.Y = (float)pos.Y / window.GameWindow.Height;
            return mousePos;
        }


    }




}
