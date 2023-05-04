using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using Zenseless.OpenGL;

namespace View
{
    class Camera
    {
        public int width, height;
        public double zoom = 5;
        public Camera(int screenWidth, int screenHeight)
        {
            height = screenHeight;
            width = screenWidth;
        }
        static double minZoom = 0.5;
        static double maxZoom = 30.0;
        static double zoomStep = 0.3;
        public void ApplyScreenSize()
        {

            //GL.Viewport(0, 0, width, height);
            GL.Viewport(0, 0, width, height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            //GL.Ortho(0, (double) width/height*zoom, 0, 1.0*zoom, -1, 1);
            var ratio = (double)width / height;

            GL.Ortho(-ratio * zoom, (double)width / height * zoom, -1 * zoom, 1 * zoom, -1, 7);
            //GL.Ortho(0.0, width, height, 1.0, -10.0, 20.0);
            //GL.Ortho(0.0, 1)
            GL.MatrixMode(MatrixMode.Modelview);
        }

        public void ChangeZoom(int zoomFactor)
        {
            if (zoomFactor < 0 && zoom > minZoom)
            {
                zoom -= zoomStep;
            }else if (zoomFactor > 0 && zoom < maxZoom){
                zoom += zoomStep;
            }
        }

        public void ChangeZoom(float zoomFactor)
        {
            if (zoomFactor < 0 && zoom > minZoom)
            {
                zoom -= zoomStep;
            }
            else if (zoomFactor > 0 && zoom < maxZoom)
            {
                zoom += zoomStep;
            }
        }
    }
}
