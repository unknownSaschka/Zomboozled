using OpenTK.Graphics.OpenGL;
using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Zenseless.Geometry;
using System.Collections.Generic;
using System.Linq;

using Zenseless.ExampleFramework;
using Model;

namespace View {
    internal class View {
        public ExampleWindow window;
        public Camera Cam;
        //private TileManager _Tmanager;
        int renderDistance = 3;
        OpenTK.Vector3d Rotate = new OpenTK.Vector3d(1.0, 0.5, 0.0);
        public View() {
            
        }

        private bool CheckNotActive(System.Numerics.Vector2 pos, Model.Player player)
        {
            if (pos.X > player.Position.X / 5 + renderDistance|| pos.Y > player.Position.Y / 5 + renderDistance || pos.X < player.Position.X / 5 - renderDistance || pos.Y < player.Position.Y / 5 - renderDistance)
            {

                return true;
            }

            return false;
        }




        internal void Draw(rMainModel model, Camera cam, TileManager _Tmanager) {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            var player = model.Player;
            cam.ApplyScreenSize();
            

            GL.PushMatrix();

            GL.LoadIdentity();

            GL.Scale(.5f, .5f, 1f);

            GL.Translate(-player.currentChunk.X*5, -player.currentChunk.Y*5, 0);
            GL.Translate(-player.position.X, -player.position.Y, 0f);


 

            var linq = model.world.chunks.Select(i => i).Where(i => i.Key.X <= player.currentChunk.X+renderDistance && i.Key.X >= player.currentChunk.X - renderDistance && i.Key.Y <= player.currentChunk.Y + renderDistance && i.Key.Y >= player.currentChunk.Y - renderDistance);

            int magicNumber = 5;
            int tileID;
            foreach (KeyValuePair < System.Numerics.Vector2, Model.Chunk > chunk in linq) {
                tileID = chunk.Value.TileID;
                //Console.WriteLine("TileID: " + tileID);
                GL.PushMatrix();
                
                GL.Translate(chunk.Key.X*magicNumber, chunk.Key.Y*magicNumber, 0);


                //Alle gameObjects aus dem TileManager
                foreach (Model.GameObject obj in _Tmanager.tiles[tileID].gameObjects)
                {
                    GL.PushMatrix();

                    GL.Translate(obj.position.X / 2, obj.position.Y / 2, 0);
                    //GL.Scale(2, 2, 0);
                    GL.Begin(PrimitiveType.Quads);

                    GL.Color3(obj.r, obj.g, obj.b);
                    GL.Vertex2(0.0f, 0.0f);
                    GL.Vertex2(0.5f, 0.0f);

                    GL.Vertex2(0.5f, 0.5f);

                    GL.Vertex2(0.0f, 0.5f);
                    GL.Color3(1, 1, 1);

                    GL.End();



                    GL.PopMatrix();
                }

                //Alle GameObjechts (Gegner/ Spieler usw.) aus der ChunkKlasse selber
                foreach (Model.GameObject obj in chunk.Value.map) {
                    GL.PushMatrix();

                    GL.Translate(obj.position.X/2, obj.position.Y/2, 0);
                    //GL.Scale(2, 2, 0);
                    GL.Begin(PrimitiveType.Quads);

                    GL.Color3(obj.r, obj.g, obj.b);
                    GL.Vertex2(0.0f, 0.0f);
                    GL.Vertex2(0.5f, 0.0f);
   
                    GL.Vertex2(0.5f, 0.5f);

                    GL.Vertex2(0.0f, 0.5f);
                    GL.Color3(1,1,1);

                    GL.End();



                    GL.PopMatrix();
                }
                foreach (Model.GameObject obj in chunk.Value.gameObjects) {


                }



                foreach (Box2D col in _Tmanager.tiles[tileID].collisionBoxes)
                {
                    GL.PushMatrix();
                    GL.Translate(col.CenterX, col.CenterY, 0);

                    GL.LineWidth(3);
                    GL.Color4(0, 0, 1f, 0.5f);
                    GL.Begin(PrimitiveType.LineLoop);

                    GL.Vertex2(-col.SizeX/2, -col.SizeY / 2);
                    GL.Vertex2(-col.SizeX / 2, +col.SizeY / 2);
                    GL.Vertex2(+col.SizeX / 2, col.SizeY / 2);
                    GL.Vertex2(+col.SizeX / 2, -col.SizeY / 2);
                    GL.End();
                    GL.PopMatrix();
                }


                //End Chunk Space
                GL.PopMatrix();

            }


       
            foreach (KeyValuePair<System.Numerics.Vector2, Model.Chunk> chunk in linq)
            {


                GL.PushMatrix();

                GL.Translate(chunk.Key.X * magicNumber, chunk.Key.Y * magicNumber, 0);

                foreach (var projectile in chunk.Value.Projectiles)
                {
                    GL.PushMatrix();

                    GL.Translate(projectile.position.X, projectile.position.Y, 1);


                    GL.Begin(PrimitiveType.Quads);
                    GL.Color3(1.000, 1.000, 0.000);
                    GL.Vertex2(0.0f, 0.0f);
                    GL.Vertex2(0.05f, 0.0f);
                    GL.Vertex2(0.05f, 0.05f);
                    GL.Vertex2(0.0f, 0.05f);
                    GL.Color3(1, 1, 1);
                    GL.End();

                    GL.PopMatrix();


                }


                foreach (Model.Enemy obj in chunk.Value.enemies)
                {
                    GL.PushMatrix();

                    GL.Translate(obj.Position.X, obj.Position.Y, 1);
                    
                    
                    GL.Begin(PrimitiveType.Quads);
                    GL.Color3(0, 0, 1);
                    GL.Vertex2(0.0f, 0.0f);
                    GL.Vertex2(0.1f, 0.0f);
                    GL.Vertex2(0.1f, 0.1f);
                    GL.Vertex2(0.0f, 0.1f);
                    GL.Color3(1, 1, 1);
                    GL.End();
                    
                    GL.PopMatrix();

                }

                GL.PopMatrix();
            }
            






            //Player Drawing
            GL.PushMatrix();
            
            GL.Translate(model.player.currentChunk.X*5, model.player.currentChunk.Y*5, 0);
            GL.Translate(model.player.position.X, model.player.position.Y, 1);
            
            GL.Begin(PrimitiveType.Quads);

            GL.Color3(1.0, 1.0, 1.0);
            GL.Vertex2(0.0f, 0.0f);
            GL.Vertex2(0.1f, 0.0f);
            GL.Vertex2(0.1f, 0.1f);

            GL.Vertex2(0.0f, 0.1f);


            GL.End();




            GL.PopMatrix();
            GL.PopMatrix();




            //GUI SPACE
            

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            GL.PushMatrix();
            GL.Translate(-0.96, 0.85, 0);
            //GL.Scale(1.1, 1.1, 0);
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(0.3, 0.3, 0.3);
            GL.Vertex2(0.0f, 0.0f);
            GL.Vertex2(0.5f, 0.0f);

            GL.Vertex2(0.5f, 0.1f);

            GL.Vertex2(0.0f, 0.1f);
            GL.End();
            GL.Scale(0.96, 0.8, 0);
            GL.Translate(0.01, 0.01, 0);
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(0, 1.0, 0);
            GL.Vertex2(0.0f, 0.0f);
            GL.Vertex2(0.5f, 0.0f);

            GL.Vertex2(0.5f, 0.1f);

            GL.Vertex2(0.0f, 0.1f);
            GL.End();



            
            GL.PopMatrix();

            



            GL.MatrixMode(MatrixMode.Modelview);
        }
       
    }
}