using OpenTK.Graphics.OpenGL;
using System;
using Zenseless.Geometry;
using System.Collections.Generic;
using System.Linq;
using Zenseless.ExampleFramework;
using Zenseless.OpenGL;
using Model.PawnLogic.EnemyLogic;
using Model.WorldLogic.TileLogic;
using Model.WorldLogic;
using Model.ItemLogic;
using Zenseless.HLGL;
using Model.PawnLogic.PlayerLogic;
using Model;
using View.Audio;
using Model.PawnLogic.VehicleLogic;
using Model.PawnLogic.ProjectileLogic;
using Model.StartScreenLogic;
using Model.PawnLogic.BossLogic;

namespace View
{
    internal class MainView : IDisposable
    {
        public ExampleWindow window;
        public Camera Cam;
        private bool _debug = false;
        private TextureFont font;

        private bool _disposed = false;
        private GUI _gui;

        //private int currentBGMusic = 0;

        int renderDistance = 4;
        //private View.CachedSound shotSound;

        private MainModel _mainModel;

        private IShaderProgram _vignetteProgram;
        private IShaderProgram _fogProgram;

        private TextureManager _textureManager;
        private AudioView _audio;

        private Random ran = new Random();

        public bool Debug
        {
            get { return _debug; }
            set { _debug = value; }

        }
        public void PlayAudio(Model.Audio.AudioType audioType, int id)
        {
            switch (audioType)
            {
                case Model.Audio.AudioType.Effect:
                    _audio.PlayAudioEffect(id);
                    break;

                case Model.Audio.AudioType.BGMusic:
                    _audio.Interrupt();
                    break;
            }
        }



        public MainView(MainModel mainModel)
        {
            _mainModel = mainModel;
            _audio = new AudioView();
            _audio.StartMusicThread(mainModel);
            _textureManager = new TextureManager();

        }



        ~MainView()
        {
            Dispose(false);
            //_musicThread.Abort();

        }

        public void LoadFont()
        {
            font = new TextureFont(window.ContentLoader.Load<ITexture2D>("DroidNew.png"), 8, 32, characterSpacing: 0.8f);
        }

        public void Init()
        {
            LoadFont();
            _textureManager.LoadImages(window);
            _gui = new GUI(font, renderDistance);

            try
            {
                _vignetteProgram = window.ContentLoader.LoadPixelShader("vignette");
                _fogProgram = window.ContentLoader.LoadPixelShader("fog");
            }
            catch (Exception e)
            {
                Console.WriteLine("Shader Exception:  " + e);
            }
        }

        private bool CheckNotActive(System.Numerics.Vector2 pos, Player player)
        {
            if (pos.X > player.Position.X / 5 + renderDistance || pos.Y > player.Position.Y / 5 + renderDistance || pos.X < player.Position.X / 5 - renderDistance || pos.Y < player.Position.Y / 5 - renderDistance)
            {

                return true;
            }

            return false;
        }

        public void PauseAudio()
        {
            //loop.Running = false;
        }
        public void ResumeAudio()
        {
            //loop.Running = true;
        }


        internal void Draw(Model.MainModel model, Camera cam, IManagerHolder manager, float absoluteTime)
        {
            var renderState2 = window.RenderContext.RenderState;
            renderState2.Set(BlendStates.AlphaBlend);
            GL.Enable(EnableCap.Texture2D);
            GL.Clear(ClearBufferMask.ColorBufferBit);


            cam.ApplyScreenSize();

            if (model.GameState == GameState.Running)
            {


                DrawGameWorld(model, manager, absoluteTime);
            }
            else
            {
                cam.zoom = 0.5f;

                GL.PushMatrix();
                GL.Translate(-0.5f, 0f, 0f);
                GL.Translate(0f, -0.5f, 0f);
                /*
                GL.Color3(0.5f, 0.5f, 0.5f);
                GL.Begin(PrimitiveType.Quads);
                GL.Vertex2(0f, 0f);
                GL.Vertex2(1f, 0f);
                GL.Vertex2(1f, 1f);
                GL.Vertex2(0f, 1f);
                GL.End();
                */
                GL.Color3(1f, 1f, 1f);

                GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcColor);
                Drawing.DrawTexturedRect(new Box2D(0f, 0f, 1f, 1f), _textureManager.TootTexture);

                var col = (float)Math.Sin(absoluteTime);
                if (col < 0) col = 0;
                GL.Color3(col, col, col);


                Drawing.DrawTexturedRect(new Box2D(0f, 0f, 1f, 1f), _textureManager.TootIntense);


                //GL.Scale(1.2f, 0, 0);
                //GL.Scale(2f, 1f, 0);

                GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

                GL.Color3(1f, 1f, 1f);

                Drawing.DrawTexturedRect(new Box2D(0.1f, 0.8f, .8f, .3f), _textureManager.LogoTexture);
                foreach (Button but in model.StartScreen.Buttons)
                {

                    GL.Color3(1f, 1f, 1f);
                    var Rect = but.Dimensions;

                    /*
                    GL.Vertex2(-0.5f, 0f);
                    GL.Vertex2(0.5f, 0f);
                    GL.Vertex2(.5f, .5f);
                    GL.Vertex2(-.5f, .5f);
                    GL.End();
                    GL.Vertex2(but.Dimensions.CenterX - but.Dimensions.SizeX/2, but.Dimensions.CenterY + but.Dimensions.SizeY / 2);
                    GL.Vertex2(but.Dimensions.CenterX + but.Dimensions.SizeX / 2, but.Dimensions.CenterY + but.Dimensions.SizeY / 2);
                    GL.Vertex2(but.Dimensions.CenterX + but.Dimensions.SizeX / 2, but.Dimensions.CenterY - but.Dimensions.SizeY / 2);
                    GL.Vertex2(but.Dimensions.CenterX - but.Dimensions.SizeX / 2, but.Dimensions.CenterY - +but.Dimensions.SizeY / 2);
                    GL.End();
                    */

                    _textureManager.ButtonTexture.Activate();
                    GL.Begin(PrimitiveType.Quads);
                    GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(Rect.MinX, 1 - Rect.MaxY);
                    GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(Rect.MaxX, 1 - Rect.MaxY);
                    GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(Rect.MaxX, 1 - Rect.MinY);
                    GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(Rect.MinX, 1 - Rect.MinY);
                    GL.End();
                    _textureManager.ButtonTexture.Deactivate();

                    /*
                    GL.Begin(PrimitiveType.Quads);
                    GL.Vertex2(-0.5f, 0f);
                    GL.Vertex2(0.5f, 0f);
                    GL.Vertex2(.5f, .5f);
                    GL.Vertex2(-.5f, .5f);
                    GL.End();
                    GL.Color3(0f, 1f, 0f);
                    */

                    GL.Color3(ran.NextDouble(), ran.NextDouble(), ran.NextDouble());
                    font.Print(but.Dimensions.MinX + but.TextOffset.X, 1 - but.Dimensions.MaxY + but.TextOffset.Y, 0f, 0.19f, but.ButtonText);

                    /*
                    GL.Begin(PrimitiveType.LineLoop);
                    GL.Vertex2(0f, 0f);
                    GL.Vertex2(1f, 0f);
                    GL.Vertex2(1f, 1f);
                    GL.Vertex2(0f, 1f);
                    GL.End();
                    */



                }
                GL.PopMatrix();

            }

        }

        private void DrawGameWorld(MainModel model, IManagerHolder manager, float absoluteTime)
        {
            var player = manager.PlayerManager.Player;
            var tileManager = manager.TileManager;
            var itemManager = manager.ItemManager;

            GL.PushMatrix();

            GL.LoadIdentity();

            GL.Scale(.5f, .5f, 1f);

            GL.Translate(-player.Chunk.X * 5, -player.Chunk.Y * 5, 0);
            GL.Translate(-player.Position.X, -player.Position.Y, 0f);





            var linq = model.World.chunks.Select(i => i).Where(i => i.Key.X <= player.Chunk.X + renderDistance && i.Key.X >= player.Chunk.X - renderDistance && i.Key.Y <= player.Chunk.Y + renderDistance && i.Key.Y >= player.Chunk.Y - renderDistance);





            int magicNumber = 5;
            DrawWorldChunks(tileManager, linq, magicNumber);
            //Zeichnen der Items

            DrawItems(itemManager, linq, magicNumber, absoluteTime);

            System.Numerics.Vector3 color;

            //Zeichnen der ShopItems
            foreach (KeyValuePair<System.Numerics.Vector2, Chunk> chunk in linq)
            {
                GL.PushMatrix();
                GL.Translate(chunk.Key.X * magicNumber, chunk.Key.Y * magicNumber, 0);

                foreach (ShopItem shopItem in itemManager.GetShopItemsOfChunk(chunk.Key))
                {
                    color = itemManager.GetColorOfShopItem(shopItem.ShopItemType);
                    //GL.Color3(color.X, color.Y, color.Z);
                    GL.Color3(1f, 1f, 1f);

                    switch (shopItem.ShopItemType)
                    {
                        case ItemManager.ShopItemType.Heart:
                            Drawing.DrawTexturedRect(new Box2D(shopItem.Position.X - shopItem.Radius, shopItem.Position.Y - shopItem.Radius, shopItem.Radius * 2, shopItem.Radius * 2), _textureManager.heartTexture);
                            break;
                        case ItemManager.ShopItemType.Resistance:
                            Drawing.DrawTexturedRect(new Box2D(shopItem.Position.X - shopItem.Radius, shopItem.Position.Y - shopItem.Radius, shopItem.Radius * 2, shopItem.Radius * 2), _textureManager.resistanceTexture);
                            break;
                        case ItemManager.ShopItemType.Ammo:
                            Drawing.DrawTexturedRect(new Box2D(shopItem.Position.X - shopItem.Radius, shopItem.Position.Y - shopItem.Radius, shopItem.Radius * 2, shopItem.Radius * 2), _textureManager.ammoTexture);
                            break;
                        case ItemManager.ShopItemType.AmmoMax:
                            Drawing.DrawTexturedRect(new Box2D(shopItem.Position.X - shopItem.Radius, shopItem.Position.Y - shopItem.Radius, shopItem.Radius * 2, shopItem.Radius * 2), _textureManager.ammoMaxTexture);
                            break;
                        case ItemManager.ShopItemType.DamageUp:
                            Drawing.DrawTexturedRect(new Box2D(shopItem.Position.X - shopItem.Radius, shopItem.Position.Y - shopItem.Radius, shopItem.Radius * 2, shopItem.Radius * 2), _textureManager.damageUpTexture);
                            break;
                    }

                    DrawShopItemText(shopItem);
                }

                GL.PopMatrix();

            }

            DrawEnemies(manager, linq, magicNumber);
            DrawBosses(manager, linq, magicNumber);

            foreach (var projectile in manager.ProjectileManager.Projectile.ToArray())
            {
                GL.Color3(1.000, 1.000, 1.000);
                //DrawTools.DrawCircle(projectile.Position.X + projectile.Chunk.X * 5, projectile.Position.Y + projectile.Chunk.Y * 5, projectile.Radius, 8);
                GL.PushMatrix();
                GL.Color3(1.000, 1.000, 0.000);
                GL.Translate(projectile.Collider.CenterX + projectile.Chunk.X * 5, projectile.Collider.CenterY + projectile.Chunk.Y * 5, 0);
                GL.Rotate(Drawing.Vector2ToRotation(projectile.Direction), 0, 0, 1);
                var projRec = new Box2D(-projectile.Radius * 2, -projectile.Radius * 2, projectile.Radius * 4, projectile.Radius * 4);
                //DrawTools.DrawCircle(projectile.Position.X + projectile.Chunk.X * 5, projectile.Position.Y + projectile.Chunk.Y * 5, projectile.Radius, 8);
                Drawing.DrawTexturedRect(projRec, _textureManager.BulletTexture);
                GL.PopMatrix();
            }

            foreach (var projectile in manager.ProjectileManager.Grenades.ToArray())
            {
                GL.PushMatrix();
                GL.Color3(1.000, 1.000, 0.000);
                GL.Translate(projectile.Collider.CenterX + projectile.Chunk.X * 5, projectile.Collider.CenterY + projectile.Chunk.Y * 5, 0);

                //var projRec = new Box2D(-projectile.Radius / 2, -projectile.Radius / 2, projectile.Radius * 2, projectile.Radius);
                //DrawTools.DrawCircle(projectile.Position.X + projectile.Chunk.X * 5, projectile.Position.Y + projectile.Chunk.Y * 5, projectile.Radius, 8);
                //Drawing.DrawTexturedRect(projRec, );
                _textureManager.granate.Draw(new Box2D(-projectile.Radius, -projectile.Radius, projectile.Radius * 2, projectile.Radius * 2), (float)absoluteTime);
                GL.PopMatrix();
            }

            foreach (var projectile in manager.ProjectileManager.Explosions.ToArray())
            {
                //GL.Color3(1.0f, 0.2f + 0.60f * projectile.LifeTime, 0.2f + 0.50f * projectile.LifeTime);
                //DrawTools.DrawCircle(projectile.Position.X + projectile.Chunk.X * 5, projectile.Position.Y + projectile.Chunk.Y * 5, projectile.Radius, 8);
                GL.Color3(1f, 1f, 1f);
                _textureManager.explosion.Draw(new Box2D(projectile.Position.X - projectile.Radius + projectile.Chunk.X * 5, projectile.Position.Y - projectile.Radius + projectile.Chunk.Y * 5, projectile.Radius * 2, projectile.Radius * 2), (float)(projectile.LifeTimeMax - projectile.LifeTime));
            }

            foreach (var projectile in manager.ProjectileManager.Thomases.ToArray())
            {
                GL.PushMatrix();
                GL.Color3(1f, 1f, 1f);
                GL.Translate(projectile.Collider.CenterX + projectile.Chunk.X * 5, projectile.Collider.CenterY + projectile.Chunk.Y * 5, 0);
                GL.Rotate(Drawing.Vector2ToRotation(projectile.Direction), 0, 0, 1);
                var thomasRect = new Box2D(-projectile.Radius, -projectile.Radius / 2, projectile.Radius * 2, projectile.Radius);
                Drawing.DrawTexturedRect(thomasRect, _textureManager.thomasTexture);

                GL.PopMatrix();

            }

            if (Debug)
            {
                foreach (KeyValuePair<System.Numerics.Vector2, Chunk> chunk in linq)
                    DebugView.DebugDrawing(model, magicNumber, chunk, font);
            }


            DrawPlayer(player);

            foreach (KeyValuePair<System.Numerics.Vector2, Chunk> chunk in linq)
            {
                foreach (Vehicle vehicle in _mainModel.ManagerHolder.VehicleManager.GetVehicleOfChunk(chunk.Key))
                {
                    DrawVehicle(vehicle);
                }
            }

            
            foreach (KeyValuePair<System.Numerics.Vector2, Chunk> chunk in linq)
            {
                foreach (DamageNumber number in _mainModel.ManagerHolder.ProjectileManager.damageNumbers)
                {
                    GL.PushMatrix();

                    GL.Translate(number.Chunk.X * 5 + number.Position.X, number.Chunk.Y * 5 + number.Position.Y, 0);
                    GL.Color4(0.8f, 0f, 0f, 0.3f);
                    font.Print(0, 0, 0, 0.2f, number.Amount.ToString());

                    GL.PopMatrix();
                }
            }

            if (_mainModel.ManagerHolder.VehicleManager.ActiveVehicle != null)
            {
                DrawVehicle(_mainModel.ManagerHolder.VehicleManager.ActiveVehicle);
            }


            GL.PopMatrix();

            if (model.WorldManager.ActiveWorldNum != 0)
            {
                _textureManager.FogTexture.Activate();

                _fogProgram.Activate();

                //_fogProgram.Uniform("matrix", new System.Numerics.Matrix4x4(matrix.M11, matrix.M12, matrix.M13, matrix.M14, matrix.M21, matrix.M22, matrix.M23, matrix.M24, matrix.M31, matrix.M32, matrix.M33, matrix.M34, matrix.M41, matrix.M42, matrix.M43, matrix.M44));
                //SetShaderParameter("effectScale", 0.1f);
                //_fogProgram.Uniform("effectScale", 0.6f);
                _fogProgram.Uniform("cameraZoom", (float)Cam.zoom);
                _fogProgram.Uniform("time", (float)absoluteTime);
                _fogProgram.Uniform("centerLocation", new System.Numerics.Vector2(2.5f+model.World.FogCenterChunk.X*5, 2.5f + model.World.FogCenterChunk.Y * 5));
                //_fogProgram.Uniform("centerLocation",  player.Position);
                _fogProgram.Uniform("aspectRatio", new System.Numerics.Vector2(Cam.width, Cam.height));
                _fogProgram.Uniform("startFogEndFog", new System.Numerics.Vector2(_mainModel.World.FogDistanceStart, _mainModel.World.FogDistanceEnd));
                // _fogProgram.Uniform("playerPosition", player.Position+player.Chunk*5);
                _fogProgram.Uniform("playerPosition", player.Chunk * 5 + player.Position);
                //_fogProgram.Uniform("circleInformation", new System.Numerics.Vector3(player.Chunk.X * 5 + player.Position.X, player.Chunk.Y * 5 + player.Position.Y, (float)Cam.zoom));
                //_vignetteProgram.Uniform("windowSize", new System.Numerics.Vector2(window.GameWindow.ClientRectangle.Width, window.GameWindow.ClientRectangle.Height));
                //Console.WriteLine(window.GameWindow.ClientRectangle.Width + " " + window.GameWindow.ClientRectangle.Height);

                GL.Color3(1f, 1f, 1f);
                GL.Begin(PrimitiveType.Quads);

                GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(0f, 0f);
                GL.TexCoord2(0.0f, 4f); GL.Vertex2(1f, 0f);
                GL.TexCoord2(4f, 4f); GL.Vertex2(1f, 1f);
                GL.TexCoord2(4f, 0.0f); GL.Vertex2(0f, 1f);
                GL.End();

                _fogProgram.Deactivate();
                _textureManager.FogTexture.Deactivate();


            }
            //renderState2.Set(BlendStates.Opaque);
            if (0.5f > (float)player.LifePoints / player.MaxLifePoints)
            {
                _vignetteProgram.Activate();
                //SetShaderParameter("effectScale", 0.1f);
                _vignetteProgram.Uniform("effectScale", 0.6f);
                _vignetteProgram.Uniform("circleInformation", new System.Numerics.Vector3(1f, (float)player.LifePoints / player.MaxLifePoints, (float)Math.Abs(Math.Sin(absoluteTime) * 0.1f)));
                //_vignetteProgram.Uniform("windowSize", new System.Numerics.Vector2(window.GameWindow.ClientRectangle.Width, window.GameWindow.ClientRectangle.Height));
                //Console.WriteLine(window.GameWindow.ClientRectangle.Width + " " + window.GameWindow.ClientRectangle.Height);
                GL.Color3(1f, 1f, 1f);
                GL.Begin(PrimitiveType.Quads);

                GL.Vertex2(0f, 0f);
                GL.Vertex2(1f, 0f);
                GL.Vertex2(1f, 1f);
                GL.Vertex2(0f, 1f);
                GL.End();

                _vignetteProgram.Deactivate();
            }
            //renderState2.Set(BlendStates.AlphaBlend);
            //GUI SPACE
            _gui.DrawGUI(model, player, _textureManager);

            if (_mainModel.WorldDebug)
            {
                GL.PushMatrix();

                font.Print(-1f, -1f, 0f, 0.1f, "DEBUG MODE");

                GL.PopMatrix();
            }

        }

        private void DrawVehicle(Vehicle vehicle)
        {
            GL.PushMatrix();

            GL.Translate(vehicle.Chunk.X * 5 + vehicle.Position.X, vehicle.Chunk.Y * 5 + vehicle.Position.Y, 0);
            GL.Rotate(Drawing.Vector2ToRotation(vehicle.CurrentDirection), 0, 0, 1);
            //DrawTools.DrawCircle(vehicle.Radius / 2, 0, vehicle.Radius / 2, 5);
            //DrawTools.DrawCircle(-vehicle.Radius / 2, 0, vehicle.Radius / 2, 5);
            var carBox = new Box2D(-vehicle.Radius, -vehicle.Radius / 2, vehicle.Radius * 2, vehicle.Radius);
            switch (vehicle.CarType)
            {
                case CarType.Tank:
                    carBox = new Box2D(-vehicle.Radius, -(vehicle.Radius + 0.2f) / 2, vehicle.Radius * 2, vehicle.Radius + 0.2f);
                    Drawing.DrawTexturedRect(carBox, _textureManager.tank1Texture);
                    break;
                default:
                    Drawing.DrawTexturedRect(carBox, _textureManager.car1Texture);
                    break;
            }

            GL.PopMatrix();
        }


        private void DrawWorldChunks(TileManager tileManager, IEnumerable<KeyValuePair<System.Numerics.Vector2, Chunk>> linq, int magicNumber)
        {
            int tileID;
            foreach (KeyValuePair<System.Numerics.Vector2, Chunk> chunk in linq)
            {
                tileID = chunk.Value.TileId;
                //Console.WriteLine("TileId: " + tileID);
                GL.PushMatrix();

                GL.Translate(chunk.Key.X * magicNumber, chunk.Key.Y * magicNumber, 0);


                //Alle gameObjects aus dem TileManager
                foreach (GameObject obj in tileManager.tiles[tileID].gameObjects)
                {

                    if (obj.type == Types.ObjectType.Street)
                    {
                        GL.Color3(obj.r + 0.2f, obj.g + 0.2f, obj.b + 0.2f);

                        Drawing.DrawTexturedRect(new Box2D(obj.position.X / 2, obj.position.Y / 2, 0.5f, 0.5f), _textureManager.streetTexture);
                        //GL.Translate(obj.position.X / 2, obj.position.Y / 2, 0);

                        continue;
                    }
                    if (obj.type == Types.ObjectType.Sidewalk)
                    {
                        GL.Color3(0.95f, 0.95f, 0.95f);
                        Drawing.DrawTexturedRect(new Box2D(obj.position.X / 2, obj.position.Y / 2, 0.5f, 0.5f), _textureManager.streetTexture);
                        /*
                        uint dir = 4;
                        var tile = tileManager.tiles[tileID];
                        if (tile.streetOut.down || tile.streetOut.up)
                        {
                            if (obj.position.X == 3) dir = 3;
                            else if (obj.position.X == 6) dir = 5;
                        }
                        if (tile.streetOut.left || tile.streetOut.right)
                        {
                            if (obj.position.Y == 3) dir = 7;
                            if (obj.position.Y == 6) dir = 1;
                            if (tile.streetOut.down)
                            {
                                if (obj.position.X == 3 && obj.position.Y == 3) dir = 10;
                                if (obj.position.X == 6 && obj.position.Y == 3) dir = 9;
                            }
                        }

                        if(tile.streetOut.down && tile.)


                        if (obj.position.Y == 4 || obj.position.Y == 5)
                        {
                            if (!tile.streetOut.right)
                            {
                                if (obj.position.X == 6) dir = 5;
                            }
                            if (!tile.streetOut.left)
                            {
                                if (obj.position.X == 3) dir = 3;
                            }
                        }
                        var Rect = _textureManager._streetConnectedSheet.CalcSpriteTexCoords(dir);
                        var RectPos = new Box2D(obj.position.X / 2, obj.position.Y / 2, 0.5f, 0.5f);
                        _textureManager._streetConnectedSheet.Activate();
                        GL.Begin(PrimitiveType.Quads);


                        //when using textures we have to set a texture coordinate for each vertex
                        //by using the TexCoord command BEFORE the Vertex command
                        GL.TexCoord2(Rect.MinX, Rect.MinY); GL.Vertex2(RectPos.MinX, RectPos.MinY);
                        GL.TexCoord2(Rect.MaxX, Rect.MinY); GL.Vertex2(RectPos.MaxX, RectPos.MinY);
                        GL.TexCoord2(Rect.MaxX, Rect.MaxY); GL.Vertex2(RectPos.MaxX, RectPos.MaxY);
                        GL.TexCoord2(Rect.MinX, Rect.MaxY); GL.Vertex2(RectPos.MinX, RectPos.MaxY);
                        GL.End();
                        _textureManager._streetConnectedSheet.Deactivate();

                        //DrawTexturedRect(new Box2D(obj.position.X / 2, obj.position.Y / 2, 0.5f, 0.5f), );
                        //GL.Translate(obj.position.X / 2, obj.position.Y / 2, 0);
                        */
                        continue;
                    }
                    if (obj.type == Types.ObjectType.Building)
                    {
                        GL.Color3(obj.r, obj.g, obj.b);

                        Drawing.DrawTexturedRect(new Box2D(obj.position.X / 2, obj.position.Y / 2, 0.5f, 0.5f), _textureManager.building1Texture);
                        //GL.Translate(obj.position.X / 2, obj.position.Y / 2, 0);

                        continue;
                    }
                    if (obj.type == Types.ObjectType.Wall)
                    {
                        GL.Color3(obj.r, obj.g, obj.b);

                        Drawing.DrawTexturedRect(new Box2D(obj.position.X / 2, obj.position.Y / 2, 0.5f, 0.5f), _textureManager.wallTexture);
                        //GL.Translate(obj.position.X / 2, obj.position.Y / 2, 0);

                        continue;
                    }
                    if (obj.type == Types.ObjectType.Grass)
                    {
                        GL.Color3(obj.r, obj.g, obj.b);

                        Drawing.DrawTexturedRect(new Box2D(obj.position.X / 2, obj.position.Y / 2, 0.5f, 0.5f), _textureManager.grassTexture);
                        //GL.Translate(obj.position.X / 2, obj.position.Y / 2, 0);

                        continue;
                    }
                    if (obj.type == Types.ObjectType.ShopGround)
                    {
                        GL.Color3(1f, 1f, 1f);

                        Drawing.DrawTexturedRect(new Box2D(obj.position.X / 2, obj.position.Y / 2, 0.5f, 0.5f), _textureManager.shopGroundTexture);
                        //GL.Translate(obj.position.X / 2, obj.position.Y / 2, 0);

                        continue;
                    }

                    GL.PushMatrix();

                    GL.Translate(obj.position.X / 2, obj.position.Y / 2, 0);
                    GL.Color3(obj.r, obj.g, obj.b);
                    GL.Begin(PrimitiveType.Quads);


                    GL.Vertex2(0.0f, 0.0f);
                    GL.Vertex2(0.5f, 0.0f);

                    GL.Vertex2(0.5f, 0.5f);

                    GL.Vertex2(0.0f, 0.5f);
                    GL.Color3(1, 1, 1);

                    GL.End();

                    GL.PopMatrix();

                }
                if (_debug)
                {
                    DebugView.DrawChunkCollisions(tileManager, tileID);
                }



                //End Chunk Space
                GL.PopMatrix();

            }
        }

        private void DrawItems(ItemManager itemManager, IEnumerable<KeyValuePair<System.Numerics.Vector2, Chunk>> linq, int magicNumber, float deltaT)
        {
            System.Numerics.Vector3 color;
            var col = (float)Math.Abs(Math.Sin(deltaT * 20));
            GL.Color4(1f, 1f, 1f, col);

            foreach (KeyValuePair<System.Numerics.Vector2, Chunk> chunk in linq)
            {
                GL.PushMatrix();
                GL.Translate(chunk.Key.X * magicNumber, chunk.Key.Y * magicNumber, 0);


                foreach (Item item in itemManager.GetItemsOfChunk(chunk.Key))
                {
                    switch (item.ItemType)
                    {
                        case ItemManager.ItemType.Geld:

                            Drawing.DrawTexturedRect(new Box2D(item.Position.X - item.Radius, item.Position.Y - item.Radius, item.Radius * 2, item.Radius * 2), _textureManager.moneyTexture);
                            break;
                        case ItemManager.ItemType.Health:

                            Drawing.DrawTexturedRect(new Box2D(item.Position.X - item.Radius, item.Position.Y - item.Radius, item.Radius * 2, item.Radius * 2), _textureManager.healthItemTexture);
                            break;
                        case ItemManager.ItemType.Speed:

                            Drawing.DrawTexturedRect(new Box2D(item.Position.X - item.Radius, item.Position.Y - item.Radius, item.Radius * 2, item.Radius * 2), _textureManager.speedTexture);
                            break;
                        case ItemManager.ItemType.Ammo:

                            Drawing.DrawTexturedRect(new Box2D(item.Position.X - item.Radius, item.Position.Y - item.Radius, 0.1f + item.Radius * 2, 0.05f + item.Radius * 2), _textureManager.ammoItemTexture);
                            break;
                        default:
                            color = itemManager.GetColorOfItem(item.ItemType);
                            GL.Color3(color.X, color.Y, color.Z);
                            //DrawTools.DrawCircle(item.Position.X, item.Position.Y, item.Radius, 8);

                            Drawing.DrawTexturedRect(new Box2D(item.Position.X - item.Radius, item.Position.Y - item.Radius, item.Radius * 2, item.Radius * 2), _textureManager.healthItemTexture);
                            break;
                    }
                }

                GL.PopMatrix();
            }
        }





        private void DrawPlayer(Player player)
        {
            //Player Drawing
            GL.PushMatrix();
            {

                GL.Color3(1.000, 1.000, 1.000);
                //DrawTools.DrawCircle(0, 0, player.Radius, 16);

                GL.Translate(player.Chunk.X * 5 + player.Position.X, player.Chunk.Y * 5 + player.Position.Y, 0);

                GL.Rotate(Drawing.Vector2ToRotation(player.ViewDirection), 0, 0, 1);

                var playerBox = new Box2D(-player.Radius, -player.Radius, player.Radius * 2, player.Radius * 2);
                Drawing.DrawTexturedRect(playerBox, _textureManager.playerTexture);



            }
            GL.PopMatrix();
        }


        public void DrawShopItemText(ShopItem item)
        {
            GL.PushMatrix();
            switch (item.ShopItemType)
            {
                case ItemManager.ShopItemType.Heart:
                    GL.Translate(new OpenTK.Vector3(1f, 0f, 0f));
                    GL.Scale(0.5f, 1f, 1f);
                    font.Print(item.Collider.CenterX - 0.5f, item.Collider.CenterY + 0.6f, 1, 0.4f, "MAX LIFE");
                    font.Print(item.Collider.CenterX - 0.6f, item.Collider.CenterY + 0.3f, 1, 0.4f, "LIFE +20");
                    font.Print(item.Collider.CenterX, item.Collider.CenterY - 0.75f, 1, 0.4f, _mainModel.ManagerHolder.ItemManager.GetCurrentShopItemPrice(ItemManager.ShopItemType.Heart).ToString());
                    break;
                case ItemManager.ShopItemType.Resistance:
                    GL.Translate(new OpenTK.Vector3(1f, 0f, 0f));
                    GL.Scale(0.5f, 1f, 1f);
                    font.Print(item.Collider.CenterX - 0.5f, item.Collider.CenterY + 0.6f, 1, 0.4f, "MORE RESI");
                    font.Print(item.Collider.CenterX - 0.6f, item.Collider.CenterY + 0.3f, 1, 0.4f, "ARMOR +" + _mainModel.ManagerHolder.ItemManager.GetCurrentShopItemValue(ItemManager.ShopItemType.Resistance));
                    font.Print(item.Collider.CenterX, item.Collider.CenterY - 0.75f, 1, 0.4f, _mainModel.ManagerHolder.ItemManager.GetCurrentShopItemPrice(ItemManager.ShopItemType.Resistance).ToString());
                    break;
                case ItemManager.ShopItemType.Ammo:
                    GL.Translate(new OpenTK.Vector3(1f, 0f, 0f));
                    GL.Scale(0.5f, 1f, 1f);
                    font.Print(item.Collider.CenterX - 0.5f, item.Collider.CenterY + 0.6f, 1, 0.4f, "FULL AMMO");
                    //font.Print(item.Collider.CenterX - 0.6f, item.Collider.CenterY + 0.3f, 1, 0.4f, "ARMOR +" + _mainModel.ManagerHolder.ItemManager.GetCurrentShopItemValue(ItemManager.ShopItemType.Resistance));
                    font.Print(item.Collider.CenterX, item.Collider.CenterY - 0.75f, 1, 0.4f, _mainModel.ManagerHolder.ItemManager.GetCurrentShopItemPrice(ItemManager.ShopItemType.Ammo).ToString());
                    break;
                case ItemManager.ShopItemType.AmmoMax:
                    GL.Translate(new OpenTK.Vector3(1f, 0f, 0f));
                    GL.Scale(0.5f, 1f, 1f);
                    font.Print(item.Collider.CenterX - 0.5f, item.Collider.CenterY + 0.6f, 1, 0.4f, "INCREASE MAX AMMO");
                    //font.Print(item.Collider.CenterX - 0.6f, item.Collider.CenterY + 0.3f, 1, 0.4f, "ARMOR +" + _mainModel.ManagerHolder.ItemManager.GetCurrentShopItemValue(ItemManager.ShopItemType.Resistance));
                    font.Print(item.Collider.CenterX, item.Collider.CenterY - 0.75f, 1, 0.4f, _mainModel.ManagerHolder.ItemManager.GetCurrentShopItemPrice(ItemManager.ShopItemType.AmmoMax).ToString());
                    break;
                case ItemManager.ShopItemType.DamageUp:
                    GL.Translate(new OpenTK.Vector3(1f, 0f, 0f));
                    GL.Scale(0.5f, 1f, 1f);
                    font.Print(item.Collider.CenterX - 1.3f, item.Collider.CenterY + 0.6f, 1, 0.4f, "INCREASE DAMAGE");
                    font.Print(item.Collider.CenterX - 0.8f, item.Collider.CenterY + 0.3f, 1, 0.4f, "DAMAGE +" + _mainModel.ManagerHolder.ItemManager.GetCurrentShopItemValue(ItemManager.ShopItemType.DamageUp));
                    font.Print(item.Collider.CenterX, item.Collider.CenterY - 0.75f, 1, 0.4f, _mainModel.ManagerHolder.ItemManager.GetCurrentShopItemPrice(ItemManager.ShopItemType.DamageUp).ToString());
                    break;
            }

            GL.PopMatrix();
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                //streetTexture.Dispose();
                //wallTexture.Dispose();
                //youDiedTexture.Dispose();
                font.Dispose();
            }

            //loop.Dispose();

            //Console.WriteLine("ABORTION!");
            _audio.Abort();
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }



        private void DrawEnemies(IManagerHolder manager, IEnumerable<KeyValuePair<System.Numerics.Vector2, Chunk>> linq, int magicNumber)
        {
            foreach (KeyValuePair<System.Numerics.Vector2, Chunk> chunk in linq)
            {
                GL.PushMatrix();
                GL.Color3(1.000, 1.000, 1.000);
                GL.Translate(chunk.Key.X * magicNumber, chunk.Key.Y * magicNumber, 0);
                foreach (Enemy obj in manager.EnemyManager.GetEnemiesOfChunk(chunk.Key))
                {



                    GL.PushMatrix();
                    GL.Translate(obj.Position.X, obj.Position.Y, 0);
                    GL.Rotate(Drawing.Vector2ToRotation(obj.CurrentDirection), 0, 0, 1);
                    if (obj.Type == Enemy.EnemyType.Bird)
                    {
                        Drawing.DrawTexturedRect(new Box2D(-obj.Radius, -obj.Radius, obj.Radius * 2, obj.Radius * 2), _textureManager.birdTexture);
                    }
                    else
                    {
                        Drawing.DrawTexturedRect(new Box2D(-obj.Radius, -obj.Radius, obj.Radius * 2, obj.Radius * 2), _textureManager.enemyTexture1);
                    }



                    GL.PopMatrix();

                    if (Debug)
                    {
                        DrawTools.DrawCircle(obj.Position.X + obj.CurrentDirection.X / 3, obj.Position.Y + obj.CurrentDirection.Y / 3, obj.Radius, 3);
                        GL.Color3(1.000, 1.000, 0.000);
                        DrawTools.DrawCircle(obj.Position.X + obj.DestinationDirection.X / 3, obj.Position.Y + obj.DestinationDirection.Y / 3, obj.Radius / 2, 6);
                    }

                }
                GL.PopMatrix();
            }
        }

        private void DrawBosses(IManagerHolder manager, IEnumerable<KeyValuePair<System.Numerics.Vector2, Chunk>> linq, int magicNumber)
        {
            foreach (KeyValuePair<System.Numerics.Vector2, Chunk> chunk in linq)
            {
                GL.PushMatrix();
                GL.Color3(1.000, 1.000, 1.000);
                GL.Translate(chunk.Key.X * magicNumber, chunk.Key.Y * magicNumber, 0);

                foreach (Boss boss in manager.BossManager.GetBossOfChunk(chunk.Key))
                {
                    GL.PushMatrix();
                    GL.Translate(boss.Position.X, boss.Position.Y, 0);
                    GL.Rotate(Drawing.Vector2ToRotation(boss.CurrentDirection), 0, 0, 1);
                    switch (boss.TypeOfBoss)
                    {
                        case Boss.BossType.Malos:
                            Drawing.DrawTexturedRect(new Box2D(-boss.Radius, -boss.Radius, boss.Radius * 2, boss.Radius * 2), _textureManager.theSomethingTexture);
                            break;
                        case Boss.BossType.Gyorg:
                            Drawing.DrawTexturedRect(new Box2D(-boss.Radius, -boss.Radius, boss.Radius * 2, boss.Radius * 2), _textureManager.pepsiManTexture);
                            break;
                        case Boss.BossType.Keller:
                            Drawing.DrawTexturedRect(new Box2D(-boss.Radius, -boss.Radius, boss.Radius * 2, boss.Radius * 2), _textureManager.sanicTexture);
                            break;
                        case Boss.BossType.Saitama:
                            Drawing.DrawTexturedRect(new Box2D(-boss.Radius, -boss.Radius, boss.Radius * 2, boss.Radius * 2), _textureManager.kellerZeldaCDITexture);
                            break;
                        case Boss.BossType.Sans:
                            Drawing.DrawTexturedRect(new Box2D(-boss.Radius, -boss.Radius, boss.Radius * 2, boss.Radius * 2), _textureManager.sansTexture);
                            break;
                        default:
                            Drawing.DrawTexturedRect(new Box2D(-boss.Radius, -boss.Radius, boss.Radius * 2, boss.Radius * 2), _textureManager.sansTexture);
                            break;
                    }
                    
                    GL.PopMatrix();
                }

                GL.PopMatrix();
            }
        }
    }
}