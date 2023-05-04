using Model.WorldLogic;
using Model.WorldLogic.TileLogic;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenseless.Geometry;
using Zenseless.OpenGL;

namespace View
{
    public class DebugView
    {

        public static void DebugDrawing(Model.MainModel model, int magicNumber, KeyValuePair<System.Numerics.Vector2, Chunk> chunk, TextureFont font)
        {
            GL.PushMatrix();

            GL.Translate(chunk.Key.X * magicNumber, chunk.Key.Y * magicNumber, 0);

            string value = chunk.Value.Distance.ToString();
            //font.Print(-0.5f * font.Width(value, 0.1f), 0f, 0.0f, 0.3f, value);

            
            GL.Enable(EnableCap.Texture2D); //TODO: only for non shader pipeline relevant -> remove at some point
            GL.Color4(1.0, 1.0, 1.0, 1.0);
            string score = chunk.Value.Distance.ToString();
            font.Print(0, 0, 0.0f, 0.5f, score);
            

            if (chunk.Value.Direction == Chunk.DirectionToPlayer.Left)
            {
                GL.Color3(0.000, 0.000, (float)0.25);
                //DrawTools.DrawCircle(2.5f, 2.5f, 1, 8);
                GL.LineWidth(4);
                GL.Begin(PrimitiveType.LineLoop);

                GL.Vertex2(0.0f, 2.5f);
                GL.Vertex2(2.0f, 2.5f);

                GL.End();
            }
            else if (chunk.Value.Direction == Chunk.DirectionToPlayer.Right)
            {
                GL.Color3(0.000, 0.000, (float)0.25);
                //DrawTools.DrawCircle(2.5f, 2.5f, 1, 8);
                GL.LineWidth(4);
                GL.Begin(PrimitiveType.LineLoop);

                GL.Vertex2(2.0f, 2.5f);
                GL.Vertex2(5.0f, 2.5f);


                GL.End();
            }
            else if (chunk.Value.Direction == Chunk.DirectionToPlayer.Top)
            {
                GL.Color3(0.000, 0.000, (float)0.25);
                //DrawTools.DrawCircle(2.5f, 2.5f, 1, 8);
                GL.LineWidth(4);
                GL.Begin(PrimitiveType.LineLoop);

                GL.Vertex2(2.5f, 5f);
                GL.Vertex2(2.5f, 2f);

                GL.End();
            }
            else if (chunk.Value.Direction == Chunk.DirectionToPlayer.Down)
            {
                GL.Color3(0.000, 0.000, (float)0.25);
                //DrawTools.DrawCircle(2.5f, 2.5f, 1, 8);
                GL.LineWidth(4);
                GL.Begin(PrimitiveType.LineLoop);

                GL.Vertex2(2.5f, 2f);
                GL.Vertex2(2.5f, 0f);


                GL.End();
            }
            else if (chunk.Value.Direction == Chunk.DirectionToPlayer.ThisChunk)
            {
                GL.Color3(1.000, 0.000, 0.000);
                //DrawTools.DrawCircle(2.5f, 2.5f, 1, 8);
                GL.LineWidth(4);
                GL.Begin(PrimitiveType.LineLoop);

                GL.Vertex2(0.0f, 0.0f);
                GL.Vertex2(5f, 0.0f);

                GL.Vertex2(5f, 5f);

                GL.Vertex2(0.0f, 5f);


                GL.End();
            }
            else if (chunk.Value.Direction == Chunk.DirectionToPlayer.FreeEDown)
            {
                GL.Color3(0.000, 0.000, (float)0.25);
                //DrawTools.DrawCircle(2.5f, 2.5f, 1, 8);
                GL.LineWidth(4);
                GL.Begin(PrimitiveType.LineLoop);

                GL.Vertex2(2.5f, 2f);
                GL.Vertex2(2.5f, 0f);


                GL.End();
                DrawTools.DrawCircle(0, 0, 1, 4);


            }
            else if (chunk.Value.Direction == Chunk.DirectionToPlayer.NoWay)
            {
                GL.Color3(0.000, 0.025f, (float)0.25);
                DrawTools.DrawCircle(2.5f, 2.5f, 0.4f, 4);

            }




            GL.PopMatrix();
        }


        public static void DrawChunkCollisions(TileManager tileManager, int tileID)
        {
            foreach (Box2D col in tileManager.tiles[tileID].collisionBoxes)
            {
                GL.PushMatrix();
                GL.Translate(col.CenterX, col.CenterY, 0);

                GL.LineWidth(3);
                GL.Color4(0, 0, 1f, 0.5f);
                GL.Begin(PrimitiveType.LineLoop);

                GL.Vertex2(-col.SizeX / 2, -col.SizeY / 2);
                GL.Vertex2(-col.SizeX / 2, +col.SizeY / 2);
                GL.Vertex2(+col.SizeX / 2, col.SizeY / 2);
                GL.Vertex2(+col.SizeX / 2, -col.SizeY / 2);
                GL.End();
                GL.PopMatrix();
            }
        }
    }
}
