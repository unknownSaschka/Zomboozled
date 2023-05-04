using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Model.WorldLogic.TileLogic;
using Model.WorldLogic;
using Model.InterfaceCollection;

namespace Model.WorldLogic
{
    public class Generator : IGenerator
    {
        private World _world;
        private int _shopGenerateNumber = 0;
        private IManagerHolder _manager;
        private int generatorDistance = 4;
        static private Random random = new Random();


        //private List<float> _placedBossChunks = new List<float>();

        public Generator(World world, IManagerHolder manager)
        {

            _manager = manager;
            _world = world;

            GenerateBossChunks(20);
            GenerateBossChunks(40);
            GenerateBossChunks(60);
            GenerateBossChunks(80);
            GenerateBossChunks(100);
        }

        public void Generate()
        {
            Vector2 currentChunkOfPlayer = _manager.PlayerManager.Player.Chunk;
            Chunk chunk;
            double randomNum;
            int x = 0, y = 0;


            

            for(y = -generatorDistance + 1; y < generatorDistance; y++)
            {
                for(x = -generatorDistance + 1; x < generatorDistance; x++)
                {
                    if (_world.chunks.ContainsKey(new Vector2(currentChunkOfPlayer.X + x, currentChunkOfPlayer.Y + y)))
                    {
                        continue;
                    }
                    randomNum = random.NextDouble();

                    //Ende
                    if(Vector2.Distance(Vector2.Zero, new Vector2(currentChunkOfPlayer.X + x, currentChunkOfPlayer.Y + y)) >= 103)
                    {
                        chunk = _world.GenerateChunkWithReturn((int)currentChunkOfPlayer.X + x, (int)currentChunkOfPlayer.Y + y);
                        chunk.TileId = 31;
                    }
                    
                    else if(randomNum < 0.02d && currentChunkOfPlayer.Y + y != 0 && (currentChunkOfPlayer.X + x) > 6)
                    {   //Park
                        chunk = _world.GenerateChunkWithReturn((int)currentChunkOfPlayer.X + x, (int)currentChunkOfPlayer.Y + y);
                        chunk.TileId = 29;
                        //_manager.EnemyManager.SpanwEnemy(chunk);
                    }
                    else
                    {
                        chunk = _world.GenerateChunkWithReturn((int)currentChunkOfPlayer.X + x, (int)currentChunkOfPlayer.Y + y);
                        GenerateCity(chunk);
                        randomNum = random.NextDouble();
                        if(randomNum > 0.70d)
                        {
                            if((_manager.TileManager.tiles[chunk.TileId].streetOut.left == true && _manager.TileManager.tiles[chunk.TileId].streetOut.right == true) && random.NextDouble() > 0.90d)
                            {
                                //_manager.VehicleManager.AddCar(new Vector2(1.5f, 3.0f), new Vector2(chunk.chunkX, chunk.chunkY), 0.3f, 7f, 400, PawnLogic.VehicleLogic.VehicleManager.CarType.Car);
                                if(random.NextDouble() > 0.98d)
                                {
                                    _manager.VehicleManager.AddCar(new Vector2(1.5f, 3.0f), new Vector2(chunk.chunkX, chunk.chunkY), 0.5f, 7f, 400, PawnLogic.VehicleLogic.CarType.Tank);
                                } else
                                {
                                    _manager.VehicleManager.AddCar(new Vector2(1.5f, 3.0f), new Vector2(chunk.chunkX, chunk.chunkY), 0.2f, 7f, 400, PawnLogic.VehicleLogic.CarType.Car);
                                }
                                
                            }
                        }
                    }
                }
            }
        }

        public void GenerateStart()
        {
            /*
            Chunk startChunk = _world.GenerateChunkWithReturn(0, 0);
            startChunk.TileId = 29;     //TileId für Straße, die nach links/rechts geht
            startChunk = _world.GenerateChunkWithReturn(-1, 0);
            startChunk.TileId = 29;
            */
            //Console.WriteLine("Drawt StartChunks");
            Chunk chunk;
            for(int y = -1; y < 2; y++)
            {
                for(int x = -1; x < 2; x++)
                {
                    chunk = _world.GenerateChunkWithReturn(x, y);
                    if (x == 0 && y == 0)
                    {
                        chunk.TileId = 50;
                       
                    }
                    else chunk.TileId = 20;
                }
            }
            //Console.WriteLine("StartChunks hinzugefügt");
        }

        public void ShowOutStreet()
        {
            /*
            Console.WriteLine("Oben" + _manager.TileManager.tiles[(_world.GetChunk(_model.Player.Chunk.X, _model.Player.Chunk.Y + 1).TileId)].streetOut.down);
            Console.WriteLine("Unten" + _manager.TileManager.tiles[(_world.GetChunk(_model.Player.Chunk.X, _model.Player.Chunk.Y - 1).TileId)].streetOut.up);
            Console.WriteLine("Links" + _manager.TileManager.tiles[(_world.GetChunk(_model.Player.Chunk.X - 1, _model.Player.Chunk.Y).TileId)].streetOut.right);
            Console.WriteLine("Rechts" + _manager.TileManager.tiles[(_world.GetChunk(_model.Player.Chunk.X + 1, _model.Player.Chunk.Y).TileId)].streetOut.left);
            */
        }

        private void GenerateCity(Chunk chunk)
        {
            _shopGenerateNumber++;
            int temp;
            double randomNum;
            bool up = false, down = false, left = false, right = false;
            
            //Sichergehen, dass es in jede Richtung 4 Straßen nach draußen gibt
            if((chunk.chunkX == 2 || chunk.chunkX == -2)  && chunk.chunkY == 0)
            {
                left = true;
                right = true;
            }
            if(chunk.chunkX == 0 && (chunk.chunkY == 2 || chunk.chunkY == -2))
            {
                up = true;
                down = true;
            }

            randomNum = random.NextDouble();
            if (_world.chunks.ContainsKey(new Vector2(chunk.chunkX, chunk.chunkY - 1)))
            {
                temp = _world.GetChunk(chunk.chunkX, chunk.chunkY - 1).TileId;
                if (_manager.TileManager.tiles[temp].streetOut.up == true || temp == 29)
                {
                    down = true;
                }
                else if(temp > 15 && temp < 25 && randomNum < 0.5d)
                {
                    down = true;
                }
            }
            else if (randomNum < 0.5d) down = true;

            randomNum = random.NextDouble();
            if(_world.chunks.ContainsKey(new Vector2(chunk.chunkX - 1, chunk.chunkY)))
            {
                temp = _world.GetChunk(chunk.chunkX - 1, chunk.chunkY).TileId;
                if (_manager.TileManager.tiles[temp].streetOut.right == true || temp == 29)
                {
                    left = true;
                }
                else if (temp > 15 && temp < 25 && randomNum < 0.4d)
                {
                    left = true;
                }
            }
            else if(randomNum < 0.5d) left = true;

            randomNum = random.NextDouble();
            if (_world.chunks.ContainsKey(new Vector2(chunk.chunkX + 1, chunk.chunkY)))
            {
                temp = _world.GetChunk(chunk.chunkX + 1, chunk.chunkY).TileId;
                if (_manager.TileManager.tiles[temp].streetOut.left == true || temp == 29)
                {
                    right = true;
                }
                else if (temp > 15 && temp < 25 && randomNum < 0.4d)
                {
                    right = true;
                }
            } else if (randomNum < 0.6d) right = true;

            randomNum = random.NextDouble();
            if (_world.chunks.ContainsKey(new Vector2(chunk.chunkX, chunk.chunkY + 1)))
            {
                temp = _world.GetChunk(chunk.chunkX, chunk.chunkY + 1).TileId;
                if (_manager.TileManager.tiles[temp].streetOut.down == true || temp == 29)
                {
                    up = true;
                }
                else if (temp > 15 && temp < 25 && randomNum < 0.5d)
                {
                    up = true;
                }
            }
            else if (randomNum < 0.7d) up = true;


            //Setzen der TileIDs


            if (up && down && left && right)    //Kreuzung
            {
                chunk.TileId = 11;
            }
            if(!up && down && left && right)    //links/unten/rechts
            {
                chunk.TileId = 9;
            }
            if (up && !down && left && right)   //links/oben/rechts
            {
                chunk.TileId = 7;
            }
            if (up && down && !left && right)   //unten/rechts/oben
            {
                chunk.TileId = 10;
            }
            if (up && down && left && !right)   //oben/links/unten
            {
                chunk.TileId = 8;
            }
            if (!up && !down && left && right)  //links/rechts
            {
                chunk.TileId = 1;
            }
            if (up && down && !left && !right)  //oben/unten
            {
                chunk.TileId = 2;
            }
            if (up && !down && left && !right)  //oben/links
            {
                chunk.TileId = 3;
            }
            if (up && !down && !left && right)  //rechts/oben
            {
                chunk.TileId = 6;
            }
            if (!up && down && !left && right)  //unten/rechts
            {
                chunk.TileId = 5;
            }
            if (!up && down && left && !right)  //links/unten
            {
                chunk.TileId = 4;
            }
            if (up && !down && !left && !right)  //oben
            {
                chunk.TileId = 14;
            }
            if (!up && down && !left && !right)  //unten
            {
                chunk.TileId = 15;
            }
            if (!up && !down && left && !right)  //links
            {
                chunk.TileId = 12;
            }
            if (!up && !down && !left && right)  //rechts
            {
                chunk.TileId = 13;
            }
            if (!up && !down && !left && !right)  //Nur Haus
            {
                chunk.TileId = 30;
            }
            
            _manager.EnemyManager.SpanwEnemy(chunk);

        }

        /// <summary>
        /// Generiert anhand der gegebenen Distanz vom Bunker Standorte, an denen die Bosschunks gespawnt werden können
        /// </summary>
        /// <param name="distance">Distanz vom Mittelpunkt</param>
        private void GenerateBossChunks(int distance)
        {
            int anzBossChunks = (int)(distance * 0.3f);
            //Console.WriteLine($"Anzahl der errechneten BossChunks {anzBossChunks}");
            //float winkellDiff = 10 / distance;
            float winkelDiff = 0.05f;
            //Console.WriteLine("WinkelDiff" + winkelDiff);
            List<float> placedWinkel = new List<float>();
            int versuche = 0;
            float randomWinkel;
            bool passed = true;

            //Winkel generieren und prüfen, ob sie weit genug auseinanderliegen
            for (int i = 0; i < anzBossChunks; i++)
            {
                //Console.WriteLine("WeiterenBosschunk");
                versuche = 0;
                while (versuche < 10)
                {
                    randomWinkel = (float)random.NextDouble();
                    passed = true;
                    foreach (float winkel in placedWinkel)
                    {
                        //Console.WriteLine($"winkel > randomWinkel + winkellDiff {winkel > randomWinkel + winkellDiff}");
                        //Console.WriteLine($"winkel < randomWinkel + winkellDiff {winkel < randomWinkel - winkellDiff}");
                        //Console.WriteLine($"randomWinkel > 0.05f {randomWinkel > 0.05f} randomWinkel < 0.05f {randomWinkel < 0.95f}");

                        if ((winkel >= randomWinkel + winkelDiff || winkel <= randomWinkel - winkelDiff) && randomWinkel > 0.05f && randomWinkel < 0.95f)
                        {
                            
                        }
                        else
                        {
                            passed = false;
                            versuche++;
                            //Console.WriteLine("Break");
                            break;
                        }
                    }
                    if (passed)
                    {
                        placedWinkel.Add(randomWinkel);
                        break;
                    }
                }

                //if (versuche >= 10) break;
            }
            //Console.WriteLine($"Zu setzende BossChunks: {placedWinkel.Count}");
            float winkelPi;

            //Umrechnen von Gradzahl (0-2*Pi) in Vector2
            foreach(float winkel in placedWinkel)
            {
                winkelPi = winkel * (float)(2 * Math.PI);

                int x = (int)(distance * Math.Cos(winkelPi));
                int y = (int)(distance * Math.Sin(winkelPi));

                SetBossChunk(new Vector2(x, y));
            }



        }

        private void SetBossChunk(Vector2 bossChunkCoord)
        {
            Chunk chunk;
            //Console.WriteLine($"Setze BossChunk bei {bossChunkCoord}");
            
            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    if (_world.chunks.ContainsKey(new Vector2(bossChunkCoord.X + x, bossChunkCoord.Y + y)))
                    {
                        chunk = _world.GetChunk(bossChunkCoord.X + x, bossChunkCoord.Y + y);
                    }
                    else
                    {
                        chunk = _world.GenerateChunkWithReturn((int)(bossChunkCoord.X + x), (int)(bossChunkCoord.Y + y));
                    }
                    if (x == -1 && y == -1) chunk.TileId = 22;
                    if (x == 0 && y == -1) chunk.TileId = 23;
                    if (x == 1 && y == -1) chunk.TileId = 24;
                    if (x == -1 && y == 0) chunk.TileId = 19;
                    if (x == 0 && y == 0) chunk.TileId = 20;
                    if (x == 1 && y == 0) chunk.TileId = 21;
                    if (x == -1 && y == 1) chunk.TileId = 16;
                    if (x == 0 && y == 1) chunk.TileId = 17;
                    if (x == 1 && y == 1) chunk.TileId = 18;
                }
            }
            
            
        }

        public void DebugGenerate(float x, float y, int TileID)
        {
            Chunk chunk;

            if (x < (y) / 2 || x < -(y) / 2)
            {
                chunk = _world.GenerateChunkWithReturn((int)x, (int)y);
                if ((y) == 0) chunk.TileId = 56;
                else chunk.TileId = 55;
            }
            else
            {
                chunk = _world.GenerateChunkWithReturn((int)x, (int)y);
                chunk.TileId = TileID;
            }
        }
    }
}
