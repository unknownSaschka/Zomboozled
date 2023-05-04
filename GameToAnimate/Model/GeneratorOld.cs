using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Zenseless.Geometry;
using static Model.Types;

namespace Model
{
    class GeneratorOld
    {
        private MainModel model;
        

        static private Random ran = new Random();

        public GeneratorOld(MainModel model)
        {
            this.model = model;
        }

        public void Update()
        {

        }

        public void GenerateStartChunks()
        {
            Chunk startChunk = model.world.GenerateChunkWithReturn(0, 0);
            for (int i = 0; i < 10; ++i)
            {
                for (int j = 0; j < 10; ++j)
                {
                    if ((j == 4 || j == 5))    //Zeichnet die Straße derzeit nur bei 0
                    {
                        startChunk.map.Add(new GameObject(ObjectType.Street, i, j));
                    }
                    else     //Zeichnet sonst überall anders Gras
                    {
                        startChunk.map.Add(new GameObject(ObjectType.Grass, i, j));
                    }
                }
            }


            /*
            for (i = -2; i < _anzChunkstoCity; i++)
            {
                for (j = -5; j < 4; j++)
                {
                    model.world.GenerateChunk(i, j);
                }
                _nextXtoGenerate++;
            }
            foreach (KeyValuePair<Vector2, Chunk> chunk in model.world.chunks)
            {

                if ((chunk.Key.Y == 0) && (chunk.Key.X < _anzChunkstoCity))   //Die Straße, die am Anfang gesetzt wird, wird als dieser Typ gesetzt
                { 
                    chunk.Value.chunkType = Chunk.ChunkType.StartStreet;
                }
                else
                {
                    chunk.Value.chunkType = Chunk.ChunkType.Grass;      //Der Rest ist Gras und wird als diesen Typ gesetzt
                }

                for (i = 0; i < 10; ++i)
                {
                    for (j = 0; j < 10; ++j)
                    {

                        if ((j == 4 || j == 5) && (chunk.Key.Y == 0))    //Zeichnet die Straße derzeit nur bei 0
                        {
                            chunk.Value.map.Add(new GameObject(GameObject.Type.Street, i, j));
                        }
                        else if (chunk.Key.Y <= 2)     //Zeichnet sonst überall anders Gras
                        {
                            chunk.Value.map.Add(new GameObject(GameObject.Type.Grass, i, j));
                        }

                    }
                }
                
            }
            */
        }

        public void GenerateCityChunk()
        {
            Vector2 currentChunkOfPlayer = model.player.Chunk;
            Chunk chunk;
            double randomZeug;
            //Chunk.StreetOut streetOut;
            int i = 0, j = 0;

            for(i = -2; i < 3; i++)     //Prüft ein 3x3 Feld um den Spieler herrum ab. Wenn es für Koords keine Chunks gibt, sollen diese generiert werden
            {
                for(j = -2; j < 3; j++)
                {
                    if(model.world.chunks.ContainsKey(new Vector2(currentChunkOfPlayer.X + i, currentChunkOfPlayer.Y + j)))
                    {
                        continue;
                    }
                    randomZeug = ran.NextDouble();

                   
                    if (currentChunkOfPlayer.X+i < (currentChunkOfPlayer.Y+j)/2 || currentChunkOfPlayer.X + i < -(currentChunkOfPlayer.Y + j) / 2)
                    {
                        chunk = model.world.GenerateChunkWithReturn((int)currentChunkOfPlayer.X + i, (int)currentChunkOfPlayer.Y + j);
                        DrawStartChunks(chunk);
                    }
                    //chunk = model.world.GenerateChunkWithReturn((int)currentChunkOfPlayer.X + i, (int)currentChunkOfPlayer.Y + j);


                    else if((model.player.Chunk.X+1)%16 == 14)
                    {
                        //Console.WriteLine("Vor ArenaChunk generate");
                        chunk = model.world.GenerateChunkWithReturn((int)currentChunkOfPlayer.X + i, (int)currentChunkOfPlayer.Y + j);
                        chunk.chunkType = Chunk.ChunkType.CityStreet;
                        RandomStreets(chunk);
                        PlaceStreets(chunk, new Vector2((int)currentChunkOfPlayer.X + i, (int)currentChunkOfPlayer.Y + j));
                        FillCorners(chunk, new Vector2((int)currentChunkOfPlayer.X + i, (int)currentChunkOfPlayer.Y + j));

                        if (model.world.chunks.ContainsKey(new Vector2((int)model.player.Chunk.X + 2, 0)) == false)
                        {
                            chunk = model.world.GenerateChunkWithReturn((int)model.player.Chunk.X + 2, 0);
                            GenerateBossChunks(chunk);
                        }
                    }
                    else if (randomZeug < 0.008d && model.player.Chunk.X > 5 && (model.player.Chunk.Y > 3 || model.player.Chunk.Y < -3))
                    {
                        Console.WriteLine("Arena");
                        chunk = model.world.GenerateChunkWithReturn((int)currentChunkOfPlayer.X + i, (int)currentChunkOfPlayer.Y + j);
                        Vector2[,] vector2 = new Vector2[2, 2];
                        vector2 = Check4X4Chunk(chunk);
                        if(vector2 == null)     //Kann für den Chunk doch keine Arena setzen, also normale Straße generiern und weiter
                        {
                            //Console.WriteLine("vector2NULL");
                            chunk.chunkType = Chunk.ChunkType.CityStreet;
                            RandomStreets(chunk);
                            PlaceStreets(chunk, new Vector2((int)currentChunkOfPlayer.X + i, (int)currentChunkOfPlayer.Y + j));
                            FillCorners(chunk, new Vector2((int)currentChunkOfPlayer.X + i, (int)currentChunkOfPlayer.Y + j));
                            continue;
                        }

                        GenerateArena(vector2);
                        DrawArenaChunk(chunk);      //Weil ein Chunk in der Ecke immer schwarz bleibt, wird dieser so gezeichnet. Jaaa, ich weis, is billig aber prototyp
                    }
                    else if (randomZeug < 0.02d && currentChunkOfPlayer.Y + j != 0 && (currentChunkOfPlayer.X + i) > 6)     //Generierung der Parks
                    {
                        chunk = model.world.GenerateChunkWithReturn((int)currentChunkOfPlayer.X + i, (int)currentChunkOfPlayer.Y + j);
                        chunk.chunkType = Chunk.ChunkType.Park;
                        DrawPark(chunk);
                    }
                    else
                    {
                        chunk = model.world.GenerateChunkWithReturn((int)currentChunkOfPlayer.X + i, (int)currentChunkOfPlayer.Y + j);
                        chunk.chunkType = Chunk.ChunkType.CityStreet;
                        RandomStreets(chunk);
                        PlaceStreets(chunk, new Vector2((int)currentChunkOfPlayer.X + i, (int)currentChunkOfPlayer.Y + j));
                        FillCorners(chunk, new Vector2((int)currentChunkOfPlayer.X + i, (int)currentChunkOfPlayer.Y + j));
                    }
                }
            }
        }

        

        public void RandomStreets(Chunk chunk)
        {
            Chunk.StreetOut outStreets = new Chunk.StreetOut();
            bool street = false;
            Chunk chunkyChunk;

            if(model.world.chunks.ContainsKey(new System.Numerics.Vector2(chunk.chunkX - 1, chunk.chunkY)))
            {
                chunkyChunk = model.world.GetChunk(chunk.chunkX - 1, chunk.chunkY);
                if (chunkyChunk.streetOut.right == true) street = true;
            }

            double str = ran.NextDouble();
            if ((str > 0.5d) || street != false || (chunk.chunkY%4 == 0))
            {
                outStreets.left = true;
            }
            else
            {
                outStreets.left = false;
            }

            street = false;
            if (model.world.chunks.ContainsKey(new System.Numerics.Vector2(chunk.chunkX + 1, chunk.chunkY)))
            {
                chunkyChunk = model.world.GetChunk(chunk.chunkX + 1, chunk.chunkY);
                if (chunkyChunk.streetOut.left == true) street = true;
            }
            
            str = ran.NextDouble();
            if ((str > 0.5d) || street != false || (chunk.chunkY%4 == 0))
            {
                outStreets.right = true;
            }
            else
            {
                outStreets.right = false;
            }

            street = false;
            if (model.world.chunks.ContainsKey(new System.Numerics.Vector2(chunk.chunkX, chunk.chunkY + 1)))
            {
                chunkyChunk = model.world.GetChunk(chunk.chunkX, chunk.chunkY + 1);
                if (chunkyChunk.streetOut.down == true) street = true;
            }
            
            str = ran.NextDouble();
            if ((str > 0.3d) || street != false || (chunk.chunkX%4 == 1))
            {
                outStreets.up = true;
            }
            else
            {
                outStreets.up = false;
            }

            street = false;
            if (model.world.chunks.ContainsKey(new System.Numerics.Vector2(chunk.chunkX, chunk.chunkY - 1)))
            {
                chunkyChunk = model.world.GetChunk(chunk.chunkX, chunk.chunkY - 1);
                if (chunkyChunk.streetOut.up == true) street = true;
            }
            
            str = ran.NextDouble();
            if ((str > 0.3d) || street != false || (chunk.chunkX%4 == 1))
            {
                outStreets.down = true;
            }
            else
            {
                outStreets.down = false;
            }

            chunk.streetOut = outStreets;
        }

        

        public void PlaceStreets(Chunk cityChunk, Vector2 chunkPos)
        {
            int i = 0, j = 0;
            bool streetMiddle = false;
            Chunk.StreetOut outStreets = cityChunk.streetOut;
            if (outStreets.down == true)
            {
                
                streetMiddle = true;
                for (i = 4; i < 6; i++)     //
                {
                    for (j = 0; j < 4; j++)
                    {
                        cityChunk.map.Add(new GameObject(ObjectType.Street, i, j));
                    }
                }
            }
            else
            {
                for (i = 4; i < 6; i++)
                {
                    for (j = 0; j < 4; j++)
                    {
                        cityChunk.map.Add(new GameObject(ObjectType.Building, i, j));
                        
                    }
                }

                cityChunk.staticBoundingBoxes.Add(new Box2D( 2f, 0f, 1f, 2f)); //Straße Unten
            }

            if (outStreets.left == true)
            {
                streetMiddle = true;
                for (i = 0; i < 4; i++)
                {
                    for (j = 4; j < 6; j++)
                    {
                        cityChunk.map.Add(new GameObject(ObjectType.Street, i, j));
                    }
                }
            }
            else
            {
                for (i = 0; i < 4; i++)
                {
                    for (j = 4; j < 6; j++)
                    {
                        cityChunk.map.Add(new GameObject(ObjectType.Building, i, j));
                        
                    }
                }
                
                cityChunk.staticBoundingBoxes.Add(new Box2D( 0f, 2f, 2f, 1f)); //Straße Links
            }

            if (outStreets.right == true)
            {
                streetMiddle = true;
                for (i = 6; i < 10; i++)
                {
                    for (j = 4; j < 6; j++)
                    {
                        cityChunk.map.Add(new GameObject(ObjectType.Street, i, j));
                    }
                }
            }
            else
            {
                for (i = 6; i < 10; i++)
                {
                    for (j = 4; j < 6; j++)
                    {
                        cityChunk.map.Add(new GameObject(ObjectType.Building, i, j));
                        
                    }
                }
                
                cityChunk.staticBoundingBoxes.Add(new Box2D( 3f, 2f, 2f, 1f));  //Straße Rechts
            }

            if (outStreets.up == true)
            {
                streetMiddle = true;
                for (i = 4; i < 6; i++)
                {
                    for (j = 6; j < 10; j++)
                    {
                        cityChunk.map.Add(new GameObject(ObjectType.Street, i, j));
                    }
                }
            }
            else
            {
                for (i = 4; i < 6; i++)
                {
                    for (j = 6; j < 10; j++)
                    {
                        cityChunk.map.Add(new GameObject(ObjectType.Building, i, j));
                        
                    }
                }
                
                cityChunk.staticBoundingBoxes.Add(new Box2D( 2f, 3f, 1f, 2f));  //Straße Oben
            }

            if(streetMiddle == true)
            {
                for (i = 4; i < 6; i++)      //Generieren der Straße in der Mitte
                {
                    for (j = 4; j < 6; j++)
                    {
                        cityChunk.map.Add(new GameObject(ObjectType.Street, i, j));
                    }
                }
            }
            else
            {
                for (i = 4; i < 6; i++)      //Generieren der Straße in der Mitte
                {
                    for (j = 4; j < 6; j++)
                    {
                        cityChunk.map.Add(new GameObject(ObjectType.Building, i, j));
                    }
                }
            }
        }

        private void FillCorners(Chunk cityChunk, Vector2 chunkPos)
        {
            //Für Zeichnen
            int i = 0, j = 0;
            for (i = 0; i < 4; i++) //Links unten
            {
                for (j = 0; j < 4; j++)
                {
                    cityChunk.map.Add(new GameObject(ObjectType.Building, i, j));
                }
            }
            for (i = 6; i < 10; i++) //Rechts unten
            {
                for (j = 0; j < 4; j++)
                {
                    cityChunk.map.Add(new GameObject(ObjectType.Building, i, j));
                }
            }
            for (i = 0; i < 4; i++) //Links oben
            {
                for (j = 6; j < 10; j++)
                {
                    cityChunk.map.Add(new GameObject(ObjectType.Building, i, j));
                }
            }
            for (i = 6; i < 10; i++) //Rechts oben
            {
                for (j = 6; j < 10; j++)
                {
                    cityChunk.map.Add(new GameObject(ObjectType.Building, i, j));
                }
            }

            //Für Kollisionen
            /*
            cityChunk.staticBoundingBoxes.Add(new Zenseless.Geometry.Box2D(cityChunk.chunkX, cityChunk.chunkY, 0.5f * 4, 0.5f * 4));                        //Unten Links
            cityChunk.staticBoundingBoxes.Add(new Zenseless.Geometry.Box2D(cityChunk.chunkX+0.5f*6, cityChunk.chunkY, 0.5f * 4, 0.5f * 4));                 //Unten Rechts
            cityChunk.staticBoundingBoxes.Add(new Zenseless.Geometry.Box2D(cityChunk.chunkX, cityChunk.chunkY + 0.5f*6, 0.5f * 4, 0.5f * 4));               //Oben Links
            cityChunk.staticBoundingBoxes.Add(new Zenseless.Geometry.Box2D(cityChunk.chunkX + 0.5f * 6, cityChunk.chunkY + 0.5f * 6, 0.5f * 4, 0.5f * 4));  //Oben Rechts
            */
            
            cityChunk.staticBoundingBoxes.Add(new Box2D(0f, 0f, 2f, 2f));                       //Unten Links
            cityChunk.staticBoundingBoxes.Add(new Box2D(3f, 0f, 2f, 2f));                //Unten Rechts
            cityChunk.staticBoundingBoxes.Add(new Box2D(0f, 3f, 2f, 2f));               //Oben Links
            cityChunk.staticBoundingBoxes.Add(new Box2D(3f, 3f, 2f, 2f));        //Oben Rechts
            
        }

        private void DrawPark(Chunk chunk)
        {
            Console.WriteLine("Park");
            int i, j;
            for (i = 0; i < 10; ++i)
            {
                for (j = 0; j < 10; ++j)
                {
                    if (i == 0 || i == 9 || j == 0 || j == 9)
                    {
                        chunk.map.Add(new GameObject(ObjectType.Street, i, j));
                    }
                    else
                    {
                        chunk.map.Add(new GameObject(ObjectType.Grass, i, j));
                    }
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "0#")]
        private void GenerateArena(Vector2[,] chunks)
        {
            int i, j, chX, chY;
            for(i = 0; i < 2; i++)
            {
                for (j = 0; j < 2; j++)
                {
                    if(model.world.chunks.ContainsKey(chunks[i, j]) == false)
                    {
                        model.world.GenerateChunk((int)chunks[i, j].X, (int)chunks[i, j].Y);
                        //Console.WriteLine("ZeichneChunkBei:" + chunks[i, j].X);
                        //Console.WriteLine("ZeichneChunkBei:" + chunks[i, j].Y);
                    }
                }
            }

            for(i = 0; i < 2; i++)
            {
                for(j = 0; j < 2; j++)
                {
                    for (chX = 0; chX < 10; chX++)
                    {
                        for(chY = 0; chY < 10; chY++)
                        {
                            if (chunks[i, j].X != 0 && chunks[i, j].X != 0)
                            {
                                //model.world.GetChunk(chunks[i, j].X, chunks[i, j].Y).map.Add(new GameObject(ObjectType.Street, chX, chY));
                                //Console.WriteLine("ZeichneChunkBei:" + chunks[i, j].X);
                                //Console.WriteLine("ZeichneChunkBei:" + chunks[i, j].Y);
                            }
                        }
                    }

                    //Console.WriteLine("Zeichne ArenaChunk");
                }
            }
        }

        private void DrawArenaChunk(Chunk chunk)
        {
            //Console.WriteLine("Hat Chunk mit den Koordinaten" + chunk.chunkX + "" + chunk.chunkY + "bekommen");
            for (int chX = 0; chX < 10; chX++)
            {
                for (int chY = 0; chY < 10; chY++)
                {
                    chunk.map.Add(new GameObject(ObjectType.Street, chX, chY));
                    //chunk.chunkType = Chunk.ChunkType.Arena00;
                }
            }
        }

        private Vector2[,] Check4X4Chunk(Chunk chunk)
        {
            Vector2[,] possibleMatchChunk = new Vector2[3,3];
            Vector2[,] matched = new Vector2[2,2];
            bool[,] possibleMatchBool = new bool[3, 3];
            int i, j;

            for(i = -1; i < 2; i++)
            {
                for(j = -1; j < 2; j++)
                {
                    if(model.world.chunks.ContainsKey(new Vector2(chunk.chunkX + i, chunk.chunkY + j)) == false)
                    {
                        possibleMatchChunk[i + 1, j + 1] = new Vector2(chunk.chunkX + i, chunk.chunkY + j);
                        possibleMatchBool[i + 1, j + 1] = true;
                        Console.WriteLine(possibleMatchChunk);
                    }
                    else
                    {
                        possibleMatchBool[i + 1, j + 1] = false;
                    }
                }
            }
            
            if(possibleMatchBool[0,0] && possibleMatchBool[0,1] && possibleMatchBool[1,0])
            {
                matched[0, 0] = possibleMatchChunk[0, 0];
                matched[0, 1] = possibleMatchChunk[0, 1];
                matched[1, 0] = possibleMatchChunk[1, 0];
                matched[1, 1] = possibleMatchChunk[1, 1];
                return matched;
            }

            if(possibleMatchBool[1,0] && possibleMatchBool[2,0] && possibleMatchBool[2, 1])
            {
                matched[0, 0] = possibleMatchChunk[1, 0];
                matched[0, 1] = possibleMatchChunk[2, 0];
                matched[1, 0] = possibleMatchChunk[1, 1];
                matched[1, 1] = possibleMatchChunk[2, 1];
                return matched;
            }

            if(possibleMatchBool[0,1] && possibleMatchBool[0,2] && possibleMatchBool[1, 2])
            {
                matched[0, 0] = possibleMatchChunk[0, 1];
                matched[1, 0] = possibleMatchChunk[1, 1];
                matched[0, 1] = possibleMatchChunk[2, 0];
                matched[1, 1] = possibleMatchChunk[1, 2];
                return matched;
            }

            if(possibleMatchBool[1,2] && possibleMatchBool[2,1] && possibleMatchBool[2,2])
            {
                matched[0, 0] = possibleMatchChunk[1, 1];
                matched[1, 0] = possibleMatchChunk[2, 1];
                matched[0, 1] = possibleMatchChunk[1, 2];
                matched[1, 1] = possibleMatchChunk[2, 2];
                return matched;
            }
            //Console.WriteLine("RETURN NULL second false");
            return null;
        }

        private void GenerateBossChunks(Chunk chunk)
        {
            int i, j;
            Chunk bossChunk;
            for(i = 0; i < 3; i++)
            {
                for(j = -1; j < 2; j++)
                {
                    if(model.world.chunks.ContainsKey(new Vector2(chunk.chunkX + i, chunk.chunkY + j)) == false)
                    {
                        //Console.WriteLine("Generiere BossChunk");
                        bossChunk = model.world.GenerateChunkWithReturn(chunk.chunkX + i, chunk.chunkY + j);
                        if(i == 1 && j == 0)
                        {
                            bossChunk.chunkType = Chunk.ChunkType.BossCenter;
                        }
                        else
                        {
                            bossChunk.chunkType = Chunk.ChunkType.Boss;
                        }
                        
                        DrawArenaChunk(bossChunk);
                    }
                    else
                    {
                        chunk.chunkType = Chunk.ChunkType.Boss;
                        DrawArenaChunk(chunk);
                    }
                }
            }
        }

        private void DrawStartChunks(Chunk grassChunks)
        {
            if ((grassChunks.chunkY == 0))   //Die Straße, die am Anfang gesetzt wird, wird als dieser Typ gesetzt
            {
                grassChunks.chunkType = Chunk.ChunkType.StartStreet;
            }
            else
            {
                grassChunks.chunkType = Chunk.ChunkType.Grass;      //Der Rest ist Gras und wird als diesen Typ gesetzt
            }

            for (int i = 0; i < 10; ++i)
            {
                for (int j = 0; j < 10; ++j)
                {

                    if ((j == 4 || j == 5) && (grassChunks.chunkY == 0))    //Zeichnet die Straße derzeit nur bei 0
                    {
                        grassChunks.map.Add(new GameObject(ObjectType.Street, i, j));
                    }
                    else     //Zeichnet sonst überall anders Gras
                    {
                        grassChunks.map.Add(new GameObject(ObjectType.Grass, i, j));
                    }

                }
            }
        }
    }
}
