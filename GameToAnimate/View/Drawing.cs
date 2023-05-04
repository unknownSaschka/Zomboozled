using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenseless.Geometry;
using Zenseless.HLGL;

namespace View
{
    public class Drawing
    {
        public static void DrawTexturedRect(IReadOnlyBox2D Rect, ITexture texture)
        {
            //the texture has to be enabled before use
            texture.Activate();
            GL.Begin(PrimitiveType.Quads);
            //when using textures we have to set a texture coordinate for each vertex
            //by using the TexCoord command BEFORE the Vertex command
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(Rect.MinX, Rect.MinY);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(Rect.MaxX, Rect.MinY);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(Rect.MaxX, Rect.MaxY);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(Rect.MinX, Rect.MaxY);
            GL.End();
            texture.Deactivate();
        }

        public static float Vector2ToRotation(System.Numerics.Vector2 inputVector)
        {
            return (float)Math.Atan2(inputVector.Y, inputVector.X) * (float)(180.0 / Math.PI);
        }
    }
}
