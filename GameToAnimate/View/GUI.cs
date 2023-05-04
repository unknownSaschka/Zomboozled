using Model;
using Model.PawnLogic.PlayerLogic;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Zenseless.Geometry;
using Zenseless.OpenGL;

namespace View
{
    public class GUI
    {
        float _blendDelta = 0;
        float _wiggleCounter = 0;
        TextureFont _font;
        int _renderDistance;
        private Vector2 _currentDirectionToChunk;
        
        public GUI(TextureFont font, int renderDistance)
        {
            _font = font;
            _renderDistance = renderDistance;
        }
        
        public void DrawGUI(MainModel model, Player player, TextureManager textureManager)
        {
            //float lifePlayer = (float)player.LifePoints / (float)player.MaxLifePoints;
            //float lifeCar = (float)model.Vehicle.LifePoints / (float)model.Vehicle.MaxLifePoints;

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            /*
            DrawSliderwithName(-0.95f, 0.95f, new System.Numerics.Vector2(9f, 1f), lifeCar, "AUTO");
            DrawSliderwithName(-0.95f, 0.80f, new System.Numerics.Vector2(5f, 1f), lifePlayer, "SPIELER");
            */

            {

            }
            if (player.Chunk != Vector2.Zero && model.WorldManager.ActiveWorldNum != 0 && model.ManagerHolder.PlayerManager.Distance > 15)
            {
                var directionToOrigin = Vector2.Normalize(-(player.Chunk*5 + player.Position-new Vector2(2.5f, 2.5f)) );

                //_currentDirectionToChunk = Vector2.Lerp(_currentDirectionToChunk, directionToOrigin, 0.02f);
                _currentDirectionToChunk = directionToOrigin;
                GL.PushMatrix();

                //GL.Translate(0, 0.9f, 0);
                //GL.Scale(10, 10, 0);
                
                
                GL.Translate(_currentDirectionToChunk.X*0.8f, _currentDirectionToChunk.Y * 0.8f, 0);
                //DrawDistance(model.ManagerHolder.PlayerManager);

                GL.Color3(0, 1f, 0);
                if(_currentDirectionToChunk.Y < 0)
                {
                    _font.Print(0f, 0.175f, 0.0f, 0.05f, model.ManagerHolder.PlayerManager.Distance.ToString("0.0"));
                }
                else
                {
                    _font.Print(0f, -0.175f, 0.0f, 0.05f, model.ManagerHolder.PlayerManager.Distance.ToString("0.0"));
                }
                

                GL.Rotate(Drawing.Vector2ToRotation(_currentDirectionToChunk), 0, 0, 1);
                /*
                GL.LineWidth(3f);
                GL.Begin(PrimitiveType.Lines);

                
                GL.Vertex2(0, 0);
                GL.Vertex2(-0.1f, 0);

                GL.End();
                */
                GL.Color3(1f, 1f, 1f);
                Drawing.DrawTexturedRect(new Box2D(-0.1f, -0.1f, 0.2f, 0.2f), textureManager.ArrowTexture);

                GL.PopMatrix();
            }
            else
            {
                _currentDirectionToChunk = Vector2.Zero;
            }


            if (model.ManagerHolder.PlayerManager.Player.IsInCar)
            {
                DrawSliderwithName(-0.95f, 0.95f, new System.Numerics.Vector2(9f, 1f), model.ManagerHolder.VehicleManager.ActiveVehicle.LifePoints, model.ManagerHolder.VehicleManager.ActiveVehicle.MaxLifePoints, "AUTO");
                DrawSliderwithName(-0.95f, 0.80f, new System.Numerics.Vector2(5f, 1f), player.LifePoints, player.MaxLifePoints, "SPIELER");
            }
            else
            {
                DrawSliderwithName(-0.95f, 0.95f, new System.Numerics.Vector2(5f, 1f), player.LifePoints, player.MaxLifePoints, "SPIELER");
            }

            DrawAmmo(model);
            DrawMoney(model);

            if (model.ManagerHolder.ItemManager.NoMoney)
            {
                _font.Print(-0.5f, -0.5f, 0, 0.1f, "KEIN GELD, BOI");
            }

            if (model.WorldManager.NoWorldChange)
            {
                int time = model.WorldManager.timeWorldChange - model.WorldManager.stopwatch.Elapsed.Seconds;
                if(time >= 0)
                {
                    _font.Print(-0.6f, -0.7f, 0, 0.06f, "KANN NOCH NICHT ZURUECK");
                    _font.Print(-0.5f, -0.8f, 0, 0.06f, "NOCH " + time + " SEKUNDEN");
                }
            }

            if (model.ManagerHolder.PlayerManager.GameEnd)
            {
                _font.Print(-0.6f, -0.7f, 0, 0.06f, "DEIN ZIEL IST ERREICHT");
                _font.Print(-0.7f, -0.8f, 0, 0.06f, "DU BIST AUS DER STADT RAUS");
            }

            if (model.GameOver)
            {
                if (_blendDelta < 1) _blendDelta += 0.01f;
                _wiggleCounter++;
                GL.Color4(0.2f, 0.2f, 0.2f, 0.8f * _blendDelta);
                GL.Begin(PrimitiveType.Quads);
                GL.Vertex2(-1f, -1f);
                GL.Vertex2(1f, -1f);
                GL.Vertex2(1f, 1f);
                GL.Vertex2(-1f, 1f);
                GL.End();


                GL.Color4(1f, 1f, 1f, 1f * _blendDelta);

                var box = new Box2D(-0.8f + (float)Math.Sin(_wiggleCounter) / 700, -0.8f + (float)Math.Cos(_wiggleCounter) / 700, 1.6f, 0.6f);
                Drawing.DrawTexturedRect(box, textureManager.youDiedTexture);

                //Cam.ChangeZoom((float)Math.Sin(blendDelta*0.0000001));

                float renderNew = 0;
                if (player.Chunk.X > player.Chunk.Y)
                {
                    renderNew = player.Chunk.X;
                }
                else
                {
                    renderNew = player.Chunk.Y;
                }

                _font.Print(-0.8f, 0.1f, 0f, 0.07f, "KILLED ENEMIES: " + model.ManagerHolder.PlayerManager.KillCount);
                _font.Print(-0.8f, 0.0f, 0f, 0.07f, "KILLED BOSSES: " + model.ManagerHolder.PlayerManager.KillCountBosses);

                _renderDistance = (int)renderNew;
            }
            else if(_blendDelta>0)
            {
                
                GL.Color4(0.2f, 0.2f, 0.2f, 0.8f * _blendDelta);
                GL.Begin(PrimitiveType.Quads);
                GL.Vertex2(-1f, -1f);
                GL.Vertex2(1f, -1f);
                GL.Vertex2(1f, 1f);
                GL.Vertex2(-1f, 1f);
                GL.End();
                _blendDelta -= 0.01f;
            }
            GL.MatrixMode(MatrixMode.Modelview);
        }

        private void DrawDistance(PlayerManager manager)
        {
            GL.PushMatrix();

            //GL.Translate(0.05f, 0.2f, 0f);
            
            
            _font.Print(-1f, 0.5f, 0.0f, 0.1f, manager.Distance.ToString("0.0"));

            GL.PopMatrix();
        }



        private void DrawSlider(float positionX, float positionY, System.Numerics.Vector2 size, float percentage)
        {
            float xSlider = percentage * (size.X - 0.2f);
            //Console.WriteLine(xSlider);

            GL.PushMatrix();
            GL.PushMatrix();

            //Schwarzer Hintergrund des Balkens
            GL.Translate(positionX, positionY, 0);
            GL.Scale(size.X, size.Y, 0);
            GL.Color3(0.3, 0.3, 0.3);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(0.0f, -0.1f);
            GL.Vertex2(0.1f, -0.1f);
            GL.Vertex2(0.1f, 0f);
            GL.Vertex2(0.0f, 0f);
            GL.End();

            GL.PopMatrix();

            //Grüner Vordergrund des Balkens
            GL.PushMatrix();
            //Matrix4 bar = Matrix4.CreateTranslation(positionX, positionY, 0);
            //bar = Matrix4.Mult(bar, Matrix4.CreateTranslation(+0.5f, -0.5f, 0));
            //GL.LoadMatrix(ref bar);

            GL.Translate(positionX + 0.01f, positionY - 0.01f, 0);
            GL.Scale(xSlider, size.Y - 0.2f, 0);

            var color = LerpColorRBG(new Vector3(1f, 0f, 0f), new Vector3(0f, 1f, 0f), percentage);

            GL.Color3(color.X, color.Y, color.Z);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(0.0f, 0.0f);
            GL.Vertex2(0.0f, -0.1f);
            GL.Vertex2(0.1f, -0.1f);
            GL.Vertex2(0.1f, 0.0f);
            GL.End();

            GL.PopMatrix();
            GL.PopMatrix();
        }

        public void DrawSliderwithName(float positionX, float positionY, System.Numerics.Vector2 size, float percentage, String name)
        {
            DrawSlider(positionX, positionY, size, percentage);

            GL.PushMatrix();

            //Matrix4 text = Matrix4.CreateScale(0.6f, 1f, 0f);
            //GL.LoadMatrix(ref text);

            GL.Enable(EnableCap.Texture2D); //TODO: only for non shader pipeline relevant -> remove at some point
            GL.Translate(positionX + size.X / 10, positionY, 0);
            GL.Scale(0.6f, 1f, 1f);
            //GL.Translate(-1f, 0f, 0);
            GL.Color4(1.0, 1.0, 1.0, 1.0);
            _font.Print(0f, -0.11f, 0f, 0.1f, name);

            GL.PopMatrix();
        }

        public void DrawSliderwithName(float positionX, float positionY, System.Numerics.Vector2 size, int currentAmount, int maxAmount, String name)
        {

            float percentage = (float)currentAmount / (float)maxAmount;
            DrawSlider(positionX, positionY, size, percentage);

            GL.PushMatrix();

            //Matrix4 text = Matrix4.CreateScale(0.6f, 1f, 0f);
            //GL.LoadMatrix(ref text);
            GL.Enable(EnableCap.Texture2D); //TODO: only for non shader pipeline relevant -> remove at some point
            GL.Translate(positionX + size.X / 10, positionY, 0);
            GL.Scale(0.6f, 1f, 1f);
            //GL.Translate(-1f, 0f, 0);
            GL.Color4(1.0, 1.0, 1.0, 1.0);
            _font.Print(0f, -0.11f, 0f, 0.1f, name);

            GL.PopMatrix();


            GL.PushMatrix();

            GL.Translate(positionX + size.X / 50, positionY - 0.105f, 0.5f);
            GL.Scale(0.6f, 0.9f, 1f);
            string amount = currentAmount.ToString() + "/" + maxAmount.ToString();
            _font.Print(0, 0, 0.0f, 0.1f, amount);
            GL.PopMatrix();
        }

        public void DrawAmmo(Model.MainModel model)
        {
            GL.PushMatrix();

            GL.Translate(0.55f, 0.85f, 0);
            GL.Color4(1.0, 1.0, 1.0, 1.0);
            string score = model.WeaponManager.CurrentMagazineAmmo.ToString() + "/" + model.WeaponManager.CurrentMagazineAmmoMax.ToString();
            _font.Print(0, 0, 0.0f, 0.1f, score);
            score = model.WeaponManager.CurrentAmmoOfActiveWeapon.ToString();
            _font.Print(0, -0.1f, 0.0f, 0.1f, score);

            GL.PopMatrix();
            GL.PushMatrix();

            GL.Translate(0.2f, 0.85f, 0);
            GL.Color4(1.0, 1.0, 1.0, 1.0);
            string maxAmmo = "AMMO";
            _font.Print(0, 0, 0.0f, 0.1f, maxAmmo);


            GL.PopMatrix();
            GL.PushMatrix();

            GL.Translate(0.5f, 0.65f, 0);
            GL.Scale(0.7f, 1f, 1f);
            GL.Color4(1.0, 1.0, 1.0, 1.0);
            string currentWeaponname = model.WeaponManager.GetCurerntWeaponName();
            _font.Print(0, 0, 0.0f, 0.1f, currentWeaponname);

            GL.PopMatrix();
        }

        public void DrawMoney(MainModel model)
        {
            GL.PushMatrix();

            GL.Translate(0.7f, 0.55f, 0);
            GL.Scale(0.7f, 1f, 1f);
            GL.Color4(1.0, 1.0, 1.0, 1.0);
            string score = model.ManagerHolder.PlayerManager.Player.Money.ToString();
            _font.Print(0, 0, 0.0f, 0.1f, score);

            GL.PopMatrix();

            GL.PushMatrix();

            GL.Translate(0.4f, 0.55f, 0);
            GL.Scale(0.7f, 1f, 1f);
            GL.Color4(1.0, 1.0, 1.0, 1.0);
            string name = "MONEY";
            _font.Print(0, 0, 0.0f, 0.1f, name);

            GL.PopMatrix();
        }

        static Vector3 LerpColorRBG(Vector3 a, Vector3 b, float t)
        {
            return Vector3.Lerp(a, b, t);
        }

    }


}
