using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller{
    class Logic {


        Model.Model model;

        public Logic(ref Model.Model model) => this.model = model;
        int anzChunkstoCity = 2;    //Ab wann die Stadt beginnen soll. Später aber als indikator, ab wo die Stadt weiter generiert werden soll
        int nextXtoGenerate = -1;   //Beginnt bei -1, da Anfang der Genertion vom Start auch bei -1 beginnt
        private static Random ran = new Random();
        public void MakeDebugGame() {


            model.Chunks.Add(new Model.Chunk(0, 0));
            model.Chunks.Add(new Model.Chunk(1, 1));
            model.Chunks.Add(new Model.Chunk(0, 1));
            model.Chunks.Add(new Model.Chunk(0, 2));
            model.Player = new Model.Player(0, 1, model.Chunks[0]);

            /*
            model.Chunks[0].map.Add(new Model.GameObject(Model.GameObject.Type.Ground, 0, 0));
            model.Chunks[0].map.Add(new Model.GameObject(Model.GameObject.Type.Ground, 0, 1));
            model.Chunks[0].map.Add(new Model.GameObject(Model.GameObject.Type.Ground, 1, 1));
            model.Chunks[0].map.Add(new Model.GameObject(Model.GameObject.Type.Ground, 2, 2));
            model.Chunks[0].map.Add(new Model.GameObject(Model.GameObject.Type.Ground, 2, 1));
            model.Chunks[0].map.Add(new Model.GameObject(Model.GameObject.Type.Ground, 2, 0));
            model.Chunks[0].map.Add(new Model.GameObject(Model.GameObject.Type.Ground, 3, 3));
            model.Chunks[0].map.Add(new Model.GameObject(Model.GameObject.Type.Ground, 3, 1));
            model.Chunks[0].map.Add(new Model.GameObject(Model.GameObject.Type.Ground, 3, 0));
            */

            for (int i = 0; i < 10; ++i)
            {
                for (int j = 0; j < 10; ++j)
                {
                    //model.Chunks[0].map.Add(new Model.GameObject(Model.GameObject.Type.Ground, i, j));
                    //model.Chunks[1].map.Add(new Model.GameObject(Model.GameObject.Type.Ground, i, j));
                    if (j == 4 || j == 5)
                    {
                        model.Chunks[0].map.Add(new Model.GameObject(Model.GameObject.Type.Street, i, j));
                    } else
                    {
                        model.Chunks[0].map.Add(new Model.GameObject(Model.GameObject.Type.Grass, i, j));
                    }

                }
            }
        }

        public void LoadGame() {



        }

        public void InitGame() {
            generateStartChunks();      //generiert eine einfache Straße mit Gras herrum
            generateCity();
        }

        private void generateCity()         //geplant ist, dass diese Funktion jedes aufgerufen werden soll, wenn der Spieler weiter läuft und es ein bis 2 chunks vor ihm nichts geben würde
        {
            generateCityChunk();
        }

        /// <summary>
        /// Generiert die ersten Chunks bevor die Stadt anfängt
        /// </summary>
        private void generateStartChunks()
        {

            int i = 0, j = 0, chunkNr = 0;

            for (i = -1; i < anzChunkstoCity; i++)
            {
                for (j = -5; j < 5; j++)
                {
                    model.Chunks.Add(new Model.Chunk(i, j));

                }
                nextXtoGenerate++;
            }

            model.Player = new Model.Player(0, 1, model.Chunks[1]);
            chunkNr = 0;
            foreach (Model.Chunk chunk in model.Chunks) {

                if ((chunk.chunkY == 0) && (chunk.chunkX < anzChunkstoCity))   //Die Straße, die am Anfang gesetzt wird, wird als dieser Typ gesetzt
                {
                    chunk.chunkType = Model.Chunk.ChunkType.StartStreet;
                }
                else
                {
                    chunk.chunkType = Model.Chunk.ChunkType.Grass;      //Der Rest ist Gras und wird als diesen Typ gesetzt
                }

                for (i = 0; i < 10; ++i)
                {
                    for (j = 0; j < 10; ++j)
                    {

                        if ((j == 4 || j == 5) && (chunk.chunkY == 0))    //Zeichnet die Straße derzeit nur bei 0
                        {
                            model.Chunks[chunkNr].map.Add(new Model.GameObject(Model.GameObject.Type.Street, i, j));
                        }
                        else if (chunk.chunkY <= 2)     //Zeichnet sonst überall anders Gras
                        {
                            model.Chunks[chunkNr].map.Add(new Model.GameObject(Model.GameObject.Type.Grass, i, j));
                        }

                    }
                }
                chunkNr++;

            }
        }

        public Model.Chunk.StreetOut randomStreets(Model.Chunk.StreetOut outStreets)
        {
            
            double str = ran.NextDouble();
            
            if(str > 0.2d)
            {
                outStreets.left = true;
            }
            else
            {
                outStreets.left = false;
            }
            
            str = ran.NextDouble();
            if (str > 0.3d)
            {
                outStreets.right = true;
            }
            else
            {
                outStreets.right = false;
            }
            
            str = ran.NextDouble();
            if (str > 0.4d)
            {
                outStreets.up = true;
            }
            else
            {
                outStreets.up = false;
            }
           
            str = ran.NextDouble();
            if (str > 0.4d)
            {
                outStreets.down = true;
            }
            else
            {
                outStreets.down = false;
            }
            //Console.WriteLine(outStreets.up);
            //Console.WriteLine(outStreets.down);
            //Console.WriteLine(outStreets.left);
            //Console.WriteLine(outStreets.right);

            return outStreets;
        }

        public void generateCityChunk()
        {
            int i = nextXtoGenerate, j = 0;
            Model.Chunk.StreetOut outStreets = new Model.Chunk.StreetOut();
            

                for (j = -5; j <= 5; j++)
                {
                    outStreets = new Model.Chunk.StreetOut();

                    if (j == 0)     //Damit es immer eine durchgehende Straße durch die Stadt gibt
                    {
                        outStreets = randomStreets(outStreets);
                        outStreets.left = true;
                        outStreets.right = true;
                        if ((i % 3) == 0)
                        {
                            outStreets.up = true;
                            outStreets.down = true;
                        }
                    }
                    else if ((i%3)==0)
                    {
                        //Console.WriteLine("elseif");
                        outStreets = randomStreets(outStreets);
                        outStreets.up = true;
                        outStreets.down = true;
                    }
                    else
                    {
                         outStreets = randomStreets(outStreets);
                    }
                    model.Chunks.Add(new Model.Chunk(i, j, Model.Chunk.ChunkType.CityStreet, outStreets));
                    //Console.WriteLine("Neue CityChunks");
                    
                }
                nextXtoGenerate++;


            foreach (Model.Chunk cityChunk in model.Chunks)
            {
                //Console.WriteLine("foreach schleife");
                if (cityChunk.chunkType != Model.Chunk.ChunkType.CityStreet)
                {
                    continue;
                }

                if (cityChunk.chunkX >= anzChunkstoCity)
                {
                    //Console.WriteLine("Test");
                    for (i = 4; i < 6; i++)      //Generieren der Straße in der Mitte
                    {
                        for (j = 4; j < 6; j++)
                        {
                            cityChunk.map.Add(new Model.GameObject(Model.GameObject.Type.Street, i, j));
                        }
                    }

                    placeStreets(cityChunk);
                    fillCorners(cityChunk);

                }

            }

        }



        public void placeStreets(Model.Chunk cityChunk)
        {
            int i = 0, j = 0;
            Model.Chunk.StreetOut outStreets = cityChunk.streetOut;
            if (outStreets.down == true)
            {
                //Console.WriteLine("Down True");
                for (i = 4; i < 6; i++)
                {
                    for (j = 0; j < 4; j++)
                    {
                        cityChunk.map.Add(new Model.GameObject(Model.GameObject.Type.Street, i, j));
                    }
                }
            }
            else
            {
                //Console.WriteLine("Down False");
                for (i = 4; i < 6; i++)
                {
                    for (j = 0; j < 4; j++)
                    {
                        cityChunk.map.Add(new Model.GameObject(Model.GameObject.Type.Building, i, j));
                    }
                }
            }

            if (outStreets.left == true)
            {
                for (i = 0; i < 4; i++)
                {
                    for (j = 4; j < 6; j++)
                    {
                        cityChunk.map.Add(new Model.GameObject(Model.GameObject.Type.Street, i, j));
                    }
                }
            }
            else
            {
                for (i = 0; i < 4; i++)
                {
                    for (j = 4; j < 6; j++)
                    {
                        cityChunk.map.Add(new Model.GameObject(Model.GameObject.Type.Building, i, j));
                    }
                }
            }

            if (outStreets.right == true)
            {
                for (i = 6; i < 10; i++)
                {
                    for (j = 4; j < 6; j++)
                    {
                        cityChunk.map.Add(new Model.GameObject(Model.GameObject.Type.Street, i, j));
                    }
                }
            }
            else
            {
                for (i = 6; i < 10; i++)
                {
                    for (j = 4; j < 6; j++)
                    {
                        cityChunk.map.Add(new Model.GameObject(Model.GameObject.Type.Building, i, j));
                    }
                }
            }

            if (outStreets.up == true)
            {
                for (i = 4; i < 6; i++)
                {
                    for (j = 6; j < 10; j++)
                    {
                        cityChunk.map.Add(new Model.GameObject(Model.GameObject.Type.Street, i, j));
                    }
                }
            }
            else
            {
                for (i = 4; i < 6; i++)
                {
                    for (j = 6; j < 10; j++)
                    {
                        cityChunk.map.Add(new Model.GameObject(Model.GameObject.Type.Building, i, j));
                    }
                }
            }
        }

        private void fillCorners(Model.Chunk cityChunk)
        {
            int i = 0, j = 0;
            for (i = 0; i < 4; i++) //Links unten
            {
                for (j = 0; j < 4; j++)
                {
                    cityChunk.map.Add(new Model.GameObject(Model.GameObject.Type.Building, i, j));
                }
            }
            for (i = 6; i < 10; i++) //Rechts unten
            {
                for (j = 0; j < 4; j++)
                {
                    cityChunk.map.Add(new Model.GameObject(Model.GameObject.Type.Building, i, j));
                }
            }
            for (i = 0; i < 4; i++) //Links oben
            {
                for (j = 6; j < 10; j++)
                {
                    cityChunk.map.Add(new Model.GameObject(Model.GameObject.Type.Building, i, j));
                }
            }
            for (i = 6; i < 10; i++) //Rechts oben
            {
                for (j = 6; j < 10; j++)
                {
                    cityChunk.map.Add(new Model.GameObject(Model.GameObject.Type.Building, i, j));
                }
            }
        }
        

        public void Update(ref InputData keyData, float deltaT) {
            model.Player.Move(keyData.xAxis* deltaT, keyData.yAxis*deltaT);


        }
    }

    struct InputData {
        public float xAxis;
        public float yAxis;

        public Boolean shoot;



    }
}
