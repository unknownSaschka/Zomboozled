using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;
using Zenseless.Geometry;
using static Model.WorldLogic.TileLogic.Types;

namespace Model.WorldLogic.TileLogic
{

    public class TileSaver
    {
        private static string fileName = "tiles.bin";
        //private TileManager _manager;
        
        public TileHolder tileHolder = new TileHolder();
        
        public TileSaver()
        {
            //_manager = manager;
        }
        public void SaveTiles()
        {
            try
            {
                using (Stream stream = File.Create(fileName))
                {

                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    bformatter.Serialize(stream, tileHolder);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in save tiles: " + e);
            }
        }

        public void CreateTiles()
        {
            TileSave tileSave = new TileSave();
            int i, j;

            //ID 1 Straße links/rechts
            tileSave.TileID = 1;
            StraßeLinks(tileSave, true, true);
            StraßeRechts(tileSave, true, true);
            StraßeOben(tileSave, false, false);
            StraßeUnten(tileSave, false, false);
            ChunkEckenmitHaus(tileSave);
            StraßeMitte(tileSave);
            for(i = 0; i < 10; i++)     //Hinzufügen der restlichen Sidewalks
            {
                for(j = 0; j < 10; j++)
                {
                    if(j == 3 || j == 6)
                    {
                        tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, i, j));
                    }
                }
            }
            tileSave.collisionBoxes.Add(new Box2D(0f, 0f, 5f, 1.5f));
            tileSave.collisionBoxes.Add(new Box2D(0f, 3.5f, 5f, 1.5f));
            tileSave.chunkType = ChunkType.CityStreet;
            tileSave.description = "Stadtstraße mit Ausgängen links/rechts und sidewalks";
            for (int x = 0; x < 10; ++x)
            {
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 7));
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 8));
            }
            tileHolder.SaveTiles.Add(tileSave);



            tileSave = new TileSave();

            //ID 2 Straße oben/unten
            tileSave.TileID = 2;
            StraßeOben(tileSave, true, true);
            StraßeUnten(tileSave, true, true);
            StraßeLinks(tileSave, false, false);
            StraßeRechts(tileSave, false, false);
            ChunkEckenmitHaus(tileSave);
            StraßeMitte(tileSave);
            for (i = 0; i < 10; i++)     //Hinzufügen der restlichen Sidewalks
            {
                for (j = 0; j < 10; j++)
                {
                    if (i == 3 || i == 6)
                    {
                        tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, i, j));
                    }
                }
            }

            tileSave.collisionBoxes.Add(new Box2D(0f, 0f, 1.5f, 5f));
            tileSave.collisionBoxes.Add(new Box2D(3.5f, 0f, 1.5f, 5f));
            tileSave.chunkType = ChunkType.CityStreet;
            tileSave.description = "Stadtstraße mit Ausgängen oben/unten und sidewalks";
            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //ID 3 Straße oben/links
            tileSave.TileID = 3;
            StraßeOben(tileSave, true, true);
            StraßeLinks(tileSave, true, true);
            StraßeRechts(tileSave, false, false);
            StraßeUnten(tileSave, false, false);
            ChunkEckenmitHaus(tileSave);
            StraßeMitte(tileSave);
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 3, 6));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 3, 3));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 4, 3));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 5, 3));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 6, 3));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 6, 4));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 6, 5));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 6, 6));
            tileSave.collisionBoxes.Add(new Box2D(0f, 3.5f, 1.5f, 1.5f));
            tileSave.collisionBoxes.Add(new Box2D(0f, 0f, 5f, 1.5f));
            tileSave.collisionBoxes.Add(new Box2D(3.5f, 1.5f, 1.5f, 3.5f));
            tileSave.chunkType = ChunkType.CityStreet;
            tileSave.description = "Stadtstraße mit Ausgängen oben/links und sidewalks";

            for (int x = 0; x < 3; ++x)
            {
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 7));
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 8));
            }

            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //ID 4 Straße links/unten
            tileSave.TileID = 4;
            StraßeLinks(tileSave, true, true);
            StraßeUnten(tileSave, true, true);
            StraßeOben(tileSave, false, false);
            StraßeRechts(tileSave, false, false);
            ChunkEckenmitHaus(tileSave);
            StraßeMitte(tileSave);
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 3, 3));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 6, 3));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 6, 4));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 6, 5));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 6, 6));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 5, 6));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 4, 6));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 3, 6));
            tileSave.collisionBoxes.Add(new Box2D(0f, 0f, 1.5f, 1.5f));
            tileSave.collisionBoxes.Add(new Box2D(0f, 3.5f, 5f, 1.5f));
            tileSave.collisionBoxes.Add(new Box2D(3.5f, 0f, 1.5f, 5f));
            tileSave.chunkType = ChunkType.CityStreet;
            tileSave.description = "Stadtstraße mit Ausgängen unten/links und sidewalks";

            for (int x = 0; x < 7; ++x)
            {
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 7));
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 8));
            }

            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //ID 5 Straße unten/rechts
            tileSave.TileID = 5;
            StraßeUnten(tileSave, true, true);
            StraßeRechts(tileSave, true, true);
            StraßeOben(tileSave, false, false);
            StraßeLinks(tileSave, false, false);
            ChunkEckenmitHaus(tileSave);
            StraßeMitte(tileSave);
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 6, 3));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 3, 3));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 3, 4));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 3, 5));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 3, 6));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 4, 6));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 5, 6));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 6, 6));
            tileSave.collisionBoxes.Add(new Box2D(3.5f, 0f, 1.5f, 1.5f));
            tileSave.collisionBoxes.Add(new Box2D(0f, 0f, 1.5f, 5f));
            tileSave.collisionBoxes.Add(new Box2D(1.5f, 3.5f, 3.5f, 1.5f));
            tileSave.chunkType = ChunkType.CityStreet;
            tileSave.description = "Stadtstraße mit Ausgängen unten/rechts und sidewalks";

            for (int x = 3; x < 10; ++x)
            {
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 7));
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 8));
            }


            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //Id 6 Straße rechts/oben
            tileSave.TileID = 6;
            StraßeRechts(tileSave, true, true);
            StraßeOben(tileSave, true, true);
            StraßeLinks(tileSave, false, false);
            StraßeUnten(tileSave, false, false);
            ChunkEckenmitHaus(tileSave);
            StraßeMitte(tileSave);
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 3, 3));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 3, 4));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 3, 5));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 3, 6));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 4, 3));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 5, 3));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 6, 3));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 6, 6));
            tileSave.collisionBoxes.Add(new Box2D(0f, 1.5f, 1.5f, 3.5f));
            tileSave.collisionBoxes.Add(new Box2D(0f, 0f, 5f, 1.5f));
            tileSave.collisionBoxes.Add(new Box2D(3.5f, 3.5f, 1.5f, 1.5f));
            tileSave.chunkType = ChunkType.CityStreet;
            tileSave.description = "Stadtstraße mit Ausgängen oben/rechts und sidewalks";

            for (int x = 7; x < 10; ++x)
            {
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 7));
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 8));
            }


            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //ID 7 Straße links/oben/rechts
            tileSave.TileID = 7;
            StraßeLinks(tileSave, true, true);
            StraßeOben(tileSave, true, true);
            StraßeRechts(tileSave, true, true);
            StraßeUnten(tileSave, false, false);
            ChunkEckenmitHaus(tileSave);
            StraßeMitte(tileSave);
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 3, 6));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 6, 6));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 3, 3));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 4, 3));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 5, 3));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 6, 3));
            tileSave.collisionBoxes.Add(new Box2D(0f, 0f, 5f, 1.5f));
            tileSave.collisionBoxes.Add(new Box2D(0f, 3.5f, 1.5f, 1.5f));
            tileSave.collisionBoxes.Add(new Box2D(3.5f, 3.5f, 1.5f, 1.5f));
            tileSave.chunkType = ChunkType.CityStreet;
            tileSave.description = "Stadtstraße mit Ausgängen links/oben/rechts und sidewalks";

            for (int x = 0; x < 3; ++x)
            {
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 7));
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 8));
            }
            for (int x = 7; x < 10; ++x)
            {
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 7));
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 8));
            }
            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //ID 8 Straße oben/links/unten
            tileSave.TileID = 8;
            StraßeLinks(tileSave, true, true);
            StraßeOben(tileSave, true, true);
            StraßeUnten(tileSave, true, true);
            StraßeRechts(tileSave, false, false);
            ChunkEckenmitHaus(tileSave);
            StraßeMitte(tileSave);
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 3, 3));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 3, 6));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 6, 3));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 6, 4));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 6, 5));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 6, 6));
            tileSave.collisionBoxes.Add(new Box2D(0f, 0f, 1.5f, 1.5f));
            tileSave.collisionBoxes.Add(new Box2D(0f, 3.5f, 1.5f, 1.5f));
            tileSave.collisionBoxes.Add(new Box2D(3.5f, 0f, 1.5f, 5f));
            tileSave.chunkType = ChunkType.CityStreet;
            tileSave.description = "Stadtstraße mit Ausgängen oben/links/unten und sidewalks";

            for (int x = 0; x < 3; ++x)
            {
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 7));
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 8));
            }

            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //ID 9 Straße links/unten/rechts
            tileSave.TileID = 9;
            StraßeLinks(tileSave, true, true);
            StraßeUnten(tileSave, true, true);
            StraßeRechts(tileSave, true, true);
            StraßeOben(tileSave, false, false);
            ChunkEckenmitHaus(tileSave);
            StraßeMitte(tileSave);
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 3, 3));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 6, 3));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 3, 6));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 4, 6));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 5, 6));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 6, 6));
            tileSave.collisionBoxes.Add(new Box2D(0f, 0f, 1.5f, 1.5f));
            tileSave.collisionBoxes.Add(new Box2D(3.5f, 0f, 1.5f, 1.5f));
            tileSave.collisionBoxes.Add(new Box2D(0f, 3.5f, 5f, 1.5f));
            tileSave.chunkType = ChunkType.CityStreet;
            tileSave.description = "Stadtstraße mit Ausgängen links/unten/rechts und sidewalks";

            for (int x = 0; x < 10; ++x)
            {
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 7));
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 8));
            }

            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //ID 10 Straße unten/rechts/oben
            tileSave.TileID = 10;
            StraßeUnten(tileSave, true, true);
            StraßeRechts(tileSave, true, true);
            StraßeOben(tileSave, true, true);
            StraßeLinks(tileSave, false, false);
            ChunkEckenmitHaus(tileSave);
            StraßeMitte(tileSave);
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 6, 6));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 6, 3));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 3, 3));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 3, 4));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 3, 5));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 3, 6));
            tileSave.collisionBoxes.Add(new Box2D(0f, 0f, 1.5f, 5f));
            tileSave.collisionBoxes.Add(new Box2D(3.5f, 0f, 1.5f, 1.5f));
            tileSave.collisionBoxes.Add(new Box2D(3.5f, 3.5f, 1.5f, 1.5f));
            tileSave.chunkType = ChunkType.CityStreet;
            tileSave.description = "Stadtstraße mit Ausgängen unten/rechts/oben und sidewalks";

            for (int x = 7; x < 10; ++x)
            {
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 7));
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 8));
            }
            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //ID 11 Straße Kreuzung
            tileSave.TileID = 11;
            StraßeUnten(tileSave, true, true);
            StraßeRechts(tileSave, true, true);
            StraßeOben(tileSave, true, true);
            StraßeLinks(tileSave, true, true);
            ChunkEckenmitHaus(tileSave);
            StraßeMitte(tileSave);
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 6, 6));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 6, 3));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 3, 3));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 3, 6));
            tileSave.collisionBoxes.Add(new Box2D(0f, 0f, 1.5f, 1.5f));
            tileSave.collisionBoxes.Add(new Box2D(3.5f, 0f, 1.5f, 1.5f));
            tileSave.collisionBoxes.Add(new Box2D(0f, 3.5f, 1.5f, 1.5f));
            tileSave.collisionBoxes.Add(new Box2D(3.5f, 3.5f, 1.5f, 1.5f));
            tileSave.chunkType = ChunkType.CityStreet;
            tileSave.description = "Stadtstraße Kreuzung und sidewalks";
            for (int x = 0; x < 3; ++x)
            {
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 7));
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 8));
            }
            for (int x = 7; x < 10; ++x)
            {
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 7));
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 8));
            }
            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //ID 12 links
            tileSave.TileID = 12;
            StraßeUnten(tileSave, false, false);
            StraßeRechts(tileSave, false, false);
            StraßeOben(tileSave, false, false);
            StraßeLinks(tileSave, true, true);
            ChunkEckenmitHaus(tileSave);
            StraßeMitte(tileSave);
            for (i = 0; i < 10; i++)     //Hinzufügen der restlichen Sidewalks
            {
                for (j = 0; j < 10; j++)
                {
                    if ((j == 3 || j == 6) && i < 7)
                    {
                        tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, i, j));
                    }
                }
            }
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 6, 4));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 6, 5));
            tileSave.collisionBoxes.Add(new Box2D(0f, 0f, 5f, 1.5f));
            tileSave.collisionBoxes.Add(new Box2D(0f, 3.5f, 5f, 1.5f));
            tileSave.collisionBoxes.Add(new Box2D(3.5f, 1.5f, 1.5f, 2f));
            tileSave.chunkType = ChunkType.CityStreet;
            tileSave.description = "Stadtstraße nur links";

            for (int x = 0; x < 7; ++x)
            {
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 7));
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 8));
            }
            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //ID 13 rechts
            tileSave.TileID = 13;
            StraßeUnten(tileSave, false, false);
            StraßeRechts(tileSave, true, true);
            StraßeOben(tileSave, false, false);
            StraßeLinks(tileSave, false, false);
            ChunkEckenmitHaus(tileSave);
            StraßeMitte(tileSave);
            for (i = 0; i < 10; i++)     //Hinzufügen der restlichen Sidewalks
            {
                for (j = 0; j < 10; j++)
                {
                    if ((j == 3 || j == 6) && i > 2)
                    {
                        tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, i, j));
                    }
                }
            }
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 3, 4));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 3, 5));
            tileSave.collisionBoxes.Add(new Box2D(0f, 0f, 5f, 1.5f));
            tileSave.collisionBoxes.Add(new Box2D(0f, 3.5f, 5f, 1.5f));
            tileSave.collisionBoxes.Add(new Box2D(0f, 1.5f, 1.5f, 2f));
            tileSave.chunkType = ChunkType.CityStreet;
            tileSave.description = "Stadtstraße nur rechts";

            for (int x = 3; x < 10; ++x)
            {
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 7));
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 8));
            }

            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //ID 14 oben
            tileSave.TileID = 14;
            StraßeUnten(tileSave, false, false);
            StraßeRechts(tileSave, false, false);
            StraßeOben(tileSave, true, true);
            StraßeLinks(tileSave, false, false);
            ChunkEckenmitHaus(tileSave);
            StraßeMitte(tileSave);
            for (i = 0; i < 10; i++)     //Hinzufügen der restlichen Sidewalks
            {
                for (j = 0; j < 10; j++)
                {
                    if ((i == 3 || i == 6) && j > 2)
                    {
                        tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, i, j));
                    }
                }
            }
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 4, 3));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 5, 3));
            tileSave.collisionBoxes.Add(new Box2D(0f, 0f, 1.5f, 5f));
            tileSave.collisionBoxes.Add(new Box2D(3.5f, 0f, 1.5f, 5f));
            tileSave.collisionBoxes.Add(new Box2D(1.5f, 0f, 2f, 1.5f));
            tileSave.chunkType = ChunkType.CityStreet;
            tileSave.description = "Stadtstraße nur oben";
            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //ID 15 unten
            tileSave.TileID = 15;
            StraßeUnten(tileSave, true, true);
            StraßeRechts(tileSave, false, false);
            StraßeOben(tileSave, false, false);
            StraßeLinks(tileSave, false, false);
            ChunkEckenmitHaus(tileSave);
            StraßeMitte(tileSave);
            for (i = 0; i < 10; i++)     //Hinzufügen der restlichen Sidewalks
            {
                for (j = 0; j < 10; j++)
                {
                    if ((i == 3 || i == 6) && j < 7)
                    {
                        tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, i, j));
                    }
                }
            }
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 4, 6));
            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, 5, 6));
            tileSave.collisionBoxes.Add(new Box2D(0f, 0f, 1.5f, 5f));
            tileSave.collisionBoxes.Add(new Box2D(3.5f, 0f, 1.5f, 5f));
            tileSave.collisionBoxes.Add(new Box2D(1.5f, 3.5f, 2f, 1.5f));
            tileSave.chunkType = ChunkType.CityStreet;
            tileSave.description = "Stadtstraße nur unten";

            for (int x = 3; x < 7; ++x)
            {
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 7));
                tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Wall, x, 8));
            }

            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //ID 16 BossChunk oben links
            tileSave.TileID = 16;
            for (i = 0; i < 10; i++)
            {
                for (j = 0; j < 10; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Street, i, j));
                }
            }
            tileSave.chunkType = ChunkType.Boss;
            tileSave.streetOut.free = true;
            tileSave.description = "Bosschunk oben links";
            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //ID 17 BossChunk oben mitte
            tileSave.TileID = 17;
            for (i = 0; i < 10; i++)
            {
                for (j = 0; j < 10; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Street, i, j));
                }
            }
            tileSave.chunkType = ChunkType.Boss;
            tileSave.streetOut.free = true;
            tileSave.description = "Bosschunk oben mitte";
            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //ID 18 BossChunk oben rechts
            tileSave.TileID = 18;
            for (i = 0; i < 10; i++)
            {
                for (j = 0; j < 10; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Street, i, j));
                }
            }
            tileSave.chunkType = ChunkType.Boss;
            tileSave.streetOut.free = true;
            tileSave.description = "Bosschunk oben rechts";
            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //ID 19 BossChunk mitte links
            tileSave.TileID = 19;
            for (i = 0; i < 10; i++)
            {
                for (j = 0; j < 10; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Street, i, j));
                }
            }
            tileSave.chunkType = ChunkType.Boss;
            tileSave.streetOut.free = true;
            tileSave.description = "Bosschunk mitte links";
            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //ID 20 BossChunk mitte mitte
            tileSave.TileID = 20;
            for (i = 0; i < 10; i++)
            {
                for (j = 0; j < 10; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Street, i, j));
                }
            }
            tileSave.chunkType = ChunkType.Boss;
            tileSave.streetOut.free = true;
            tileSave.description = "Bosschunk mitte mitte";
            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //ID 21 BossChunk mitte rechts
            tileSave.TileID = 21;
            for (i = 0; i < 10; i++)
            {
                for (j = 0; j < 10; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Street, i, j));
                }
            }
            tileSave.chunkType = ChunkType.Boss;
            tileSave.streetOut.free = true;
            tileSave.description = "Bosschunk mitte rechts";
            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //ID 22 BossChunk unten links
            tileSave.TileID = 22;
            for (i = 0; i < 10; i++)
            {
                for (j = 0; j < 10; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Street, i, j));
                }
            }
            tileSave.chunkType = ChunkType.Boss;
            tileSave.streetOut.free = true;
            tileSave.description = "Bosschunk mitte rechts";
            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //ID 23 BossChunk unten mitte
            tileSave.TileID = 23;
            for (i = 0; i < 10; i++)     //Hinzufügen der restlichen Sidewalks
            {
                for (j = 0; j < 10; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Street, i, j));
                }
            }
            tileSave.chunkType = ChunkType.Boss;
            tileSave.streetOut.free = true;
            tileSave.description = "Bosschunk mitte rechts";
            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //ID 24 BossChunk unten rechts
            tileSave.TileID = 24;
            for (i = 0; i < 10; i++)     //Hinzufügen der restlichen Sidewalks
            {
                for (j = 0; j < 10; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Street, i, j));
                }
            }
            tileSave.chunkType = ChunkType.Boss;
            tileSave.streetOut.free = true;
            tileSave.description = "Bosschunk mitte rechts";
            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //ID 25 Arena links oben
            tileSave.TileID = 25;
            for (i = 0; i < 10; i++)     //Hinzufügen der restlichen Sidewalks
            {
                for (j = 0; j < 10; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Street, i, j));
                }
            }
            tileSave.chunkType = ChunkType.Arena;
            tileSave.streetOut.free = true;
            tileSave.description = "Arena links oben";
            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //ID 26 Arena rechts oben
            tileSave.TileID = 26;
            for (i = 0; i < 10; i++)     //Hinzufügen der restlichen Sidewalks
            {
                for (j = 0; j < 10; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Street, i, j));
                }
            }
            tileSave.chunkType = ChunkType.Arena;
            tileSave.streetOut.free = true;
            tileSave.description = "Arena rechts oben";
            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //ID 27 Arena links unten
            tileSave.TileID = 27;
            for (i = 0; i < 10; i++)     //Hinzufügen der restlichen Sidewalks
            {
                for (j = 0; j < 10; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Street, i, j));
                }
            }
            tileSave.chunkType = ChunkType.Arena;
            tileSave.streetOut.free = true;
            tileSave.description = "Arena links unten";
            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //ID 28 Arena rechts unten
            tileSave.TileID = 28;
            for (i = 0; i < 10; i++)     //Hinzufügen der restlichen Sidewalks
            {
                for (j = 0; j < 10; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Street, i, j));
                }
            }
            tileSave.chunkType = ChunkType.Arena;
            tileSave.streetOut.free = true;
            tileSave.description = "Arena rechts unten";
            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //ID 29 Park klein
            tileSave.TileID = 29;
            for (i = 0; i < 10; ++i)
            {
                for (j = 0; j < 10; ++j)
                {
                    if (i == 0 || i == 9 || j == 0 || j == 9)
                    {
                        tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, i, j));
                    }
                    else
                    {
                        tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Grass, i, j));
                    }
                }
            }
            tileSave.chunkType = ChunkType.Park;
            tileSave.description = "kleiner Park";
            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //ID 30 pur Gebäude
            tileSave.TileID = 30;
            for (i = 0; i < 10; ++i)
            {
                for (j = 0; j < 10; ++j)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Building, i, j));
                }
            }
            tileSave.collisionBoxes.Add(new Box2D(0f, 0f, 5f, 5f));
            tileSave.chunkType = ChunkType.CityStreet;
            tileSave.description = "Gebäude";
            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //Nur Graß (Ende des Spiels)
            tileSave.TileID = 31;
            for (i = 0; i < 10; i++)
            {
                for (j = 0; j < 10; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Grass, i, j));
                }
            }
            tileSave.chunkType = ChunkType.EndGrass;
            tileSave.description = "Ende des Spiels";
            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();

            //ID 50 StadtBunker
            tileSave.TileID = 50;
            tileSave.chunkType = ChunkType.CityStreet;
            for(i = 0; i < 9; i++)
            {
                for(j = 0; j < 10; j++)
                {
                    if(i > 0 && j > 0 && j < 9) tileSave.gameObjects.Add(new GameObjectSave(ObjectType.ShopGround, i, j));
                    else tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Building, i, j));
                }
            }
            for (i = 9; i < 10; i++)
            {
                for (j = 0; j < 10; j++)
                {
                    if (j > 3 && j < 6) tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Street, i, j));
                    else tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Building, i, j));
                }
            }
            tileSave.collisionBoxes.Add(new Box2D(0f, 0f, 0.5f, 5f));
            tileSave.collisionBoxes.Add(new Box2D(0.5f, 0f, 4.5f, 0.5f));
            tileSave.collisionBoxes.Add(new Box2D(4.5f, 0.5f, 0.5f, 1.5f));
            tileSave.collisionBoxes.Add(new Box2D(4.5f, 3f, 0.5f, 2f));
            tileSave.collisionBoxes.Add(new Box2D(0.5f, 4.5f, 4f, 0.5f));
            tileHolder.SaveTiles.Add(tileSave);


            tileSave = new TileSave();

            //ID 100 StartBunker links unten
            tileSave.TileID = 100;
            tileSave.chunkType = ChunkType.Park;
            for (i = 0; i < 10; i++)
            {
                for (j = 0; j < 3; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Building, i, j));
                }
            }
            for (i = 0; i < 3; i++)
            {
                for (j = 3; j < 10; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Building, i, j));
                }
            }
            for (i = 3; i < 10; i++)
            {
                for (j = 3; j < 10; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.ShopGround, i, j));
                }
            }
            tileSave.collisionBoxes.Add(new Box2D(0f, 0f, 5f, 1.5f));
            tileSave.collisionBoxes.Add(new Box2D(0f, 1.5f, 1.5f, 3.5f));

            tileHolder.SaveTiles.Add(tileSave);
            tileSave = new TileSave();

            //ID 101 StartBunker unten mitte
            tileSave.TileID = 101;
            tileSave.chunkType = ChunkType.Park;
            for (i = 0; i < 10; i++)
            {
                for (j = 0; j < 3; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Building, i, j));
                }
            }
            for (i = 0; i < 10; i++)
            {
                for (j = 3; j < 10; j++)
                {
                    if(i == 4 || i == 5)
                    {
                        tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Building, i, j));
                    }
                    else
                    {
                        tileSave.gameObjects.Add(new GameObjectSave(ObjectType.ShopGround, i, j));
                    }
                   
                }
            }
            tileSave.collisionBoxes.Add(new Box2D(0f, 0f, 5f, 1.5f));
            tileSave.collisionBoxes.Add(new Box2D(2f, 1.5f, 1f, 3.5f));

            tileHolder.SaveTiles.Add(tileSave);
            tileSave = new TileSave();

            //ID 102 StartBunker unten rechts
            tileSave.TileID = 102;
            tileSave.chunkType = ChunkType.Park;
            for (i = 0; i < 10; i++)
            {
                for (j = 0; j < 3; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Building, i, j));
                }
            }
            for (i = 7; i < 10; i++)
            {
                for (j = 3; j < 10; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Building, i, j));
                }
            }
            for (i = 0; i < 7; i++)
            {
                for (j = 3; j < 10; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.ShopGround, i, j));
                }
            }
            tileSave.collisionBoxes.Add(new Box2D(0f, 0f, 5f, 1.5f));
            tileSave.collisionBoxes.Add(new Box2D(3.5f, 1.5f, 1.5f, 3.5f));
            tileHolder.SaveTiles.Add(tileSave);
            tileSave = new TileSave();

            //ID 103 StartBunker links mitte
            tileSave.TileID = 103;
            tileSave.chunkType = ChunkType.Park;
            for (i = 0; i < 3; i++)
            {
                for (j = 0; j < 10; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Building, i, j));
                }
            }
            for (i = 3; i < 10; i++)
            {
                for (j = 0; j < 10; j++)
                {
                    if ((j == 2 || j == 7) &&  (i < 7))
                    {
                        tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Building, i, j));
                    }
                    else
                    {
                        tileSave.gameObjects.Add(new GameObjectSave(ObjectType.ShopGround, i, j));
                    }
                }
            }
            tileSave.collisionBoxes.Add(new Box2D(0f, 0f, 1.5f, 5f));
            tileSave.collisionBoxes.Add(new Box2D(1.5f, 1f, 2f, 0.5f));
            tileSave.collisionBoxes.Add(new Box2D(1.5f, 3.5f, 2f, 0.5f));
            tileHolder.SaveTiles.Add(tileSave);
            tileSave = new TileSave();

            //ID 104 StartBunker mitte mitte
            tileSave.TileID = 104;
            tileSave.chunkType = ChunkType.Park;
            for (i = 0; i < 10; i++)
            {
                for (j = 0; j < 10; j++)
                {
                    if(j == 2 || j == 7 || (i == 4 || i == 5) && (j > 7 || j < 2))
                    {
                        tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Building, i, j));
                    }
                    else
                    {
                        tileSave.gameObjects.Add(new GameObjectSave(ObjectType.ShopGround, i, j));
                    }
                    
                }
            }
            tileSave.collisionBoxes.Add(new Box2D(2f, 0f, 1f, 1f));
            tileSave.collisionBoxes.Add(new Box2D(0f, 1f, 5f, 0.5f));
            tileSave.collisionBoxes.Add(new Box2D(0f, 3.5f, 5f, 0.5f));
            tileSave.collisionBoxes.Add(new Box2D(2f, 4f, 1f, 1f));
            tileHolder.SaveTiles.Add(tileSave);
            tileSave = new TileSave();

            //ID 105 StartBunker rechts mitte
            tileSave.TileID = 105;
            tileSave.chunkType = ChunkType.Park;
            for (i = 7; i < 10; i++)
            {
                for (j = 0; j < 10; j++)
                {
                    if(j < 4 || j > 5)
                    {
                        tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Building, i, j));
                    }
                    else
                    {
                        tileSave.gameObjects.Add(new GameObjectSave(ObjectType.ShopGround, i, j));
                    }
                }
            }
            for (i = 0; i < 7; i++)
            {
                for (j = 0; j < 10; j++)
                {
                    if ((j == 2 || j == 7) && (i > 2))
                    {
                        tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Building, i, j));
                    }
                    else
                    {
                        tileSave.gameObjects.Add(new GameObjectSave(ObjectType.ShopGround, i, j));
                    }
                }
            }
            tileSave.collisionBoxes.Add(new Box2D(3.5f, 0f, 1.5f, 2f));
            tileSave.collisionBoxes.Add(new Box2D(3.5f, 3f, 1.5f, 2f));
            tileSave.collisionBoxes.Add(new Box2D(1.5f, 1f, 2f, 0.5f));
            tileSave.collisionBoxes.Add(new Box2D(1.5f, 3.5f, 2f, 0.5f));
            tileHolder.SaveTiles.Add(tileSave);
            tileSave = new TileSave();

            //ID 106 StartBunker links oben
            tileSave.TileID = 106;
            tileSave.chunkType = ChunkType.Park;
            for (i = 0; i < 3; i++)
            {
                for (j = 0; j < 10; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Building, i, j));
                }
            }
            for (i = 3; i < 10; i++)
            {
                for (j = 7; j < 10; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Building, i, j));
                }
            }
            for (i = 3; i < 10; i++)
            {
                for (j = 0; j < 7; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.ShopGround, i, j));
                }
            }
            tileSave.collisionBoxes.Add(new Box2D(0f, 0f, 1.5f, 5f));
            tileSave.collisionBoxes.Add(new Box2D(1.5f, 3.5f, 3.5f, 1.5f));
            tileHolder.SaveTiles.Add(tileSave);
            tileSave = new TileSave();

            //ID 107 StartBunker oben mitte
            tileSave.TileID = 107;
            tileSave.chunkType = ChunkType.Park;
            for (i = 0; i < 10; i++)
            {
                for (j = 7; j < 10; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Building, i, j));
                }
            }
            for (i = 0; i < 10; i++)
            {
                for (j = 0; j < 7; j++)
                {
                    if (i == 4 || i == 5)
                    {
                        tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Building, i, j));
                    }
                    else
                    {
                        tileSave.gameObjects.Add(new GameObjectSave(ObjectType.ShopGround, i, j));
                    }
                }
            }
            tileSave.collisionBoxes.Add(new Box2D(0f, 3.5f, 5f, 1.5f));
            tileSave.collisionBoxes.Add(new Box2D(2f, 0f, 1f, 3.5f));
            tileHolder.SaveTiles.Add(tileSave);
            tileSave = new TileSave();

            //ID 108 StartBunker oben rechts
            tileSave.TileID = 108;
            tileSave.chunkType = ChunkType.Park;
            for (i = 0; i < 10; i++)
            {
                for (j = 7; j < 10; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Building, i, j));
                }
            }
            for (i = 7; i < 10; i++)
            {
                for (j = 0; j < 7; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Building, i, j));
                }
            }
            for (i = 0; i < 7; i++)
            {
                for (j = 0; j < 7; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.ShopGround, i, j));
                }
            }
            tileSave.collisionBoxes.Add(new Box2D(0f, 3.5f, 5f, 1.5f));
            tileSave.collisionBoxes.Add(new Box2D(3.5f, 0f, 1.5f, 3.5f));
            tileHolder.SaveTiles.Add(tileSave);

            tileSave = new TileSave();
            //ID 109 Schwarze Void um den Bunker
            tileSave.TileID = 109;
            tileSave.chunkType = ChunkType.Park;
            tileHolder.SaveTiles.Add(tileSave);


            Console.WriteLine("Alle Tiles gespeichert");
        }



        //Zum erstellen von Straßen an bestimmten Stellen
        //Aufruf der Straßen, erster Parameter: TileSave, zweiter Parameter, ob es sich an der Stelle um Straße oder Haus handelt, dritter Parameter: Ob es Sidewalks dazu gibt
        private void StraßeLinks(TileSave tileSave, bool street, bool sidewalks)
        {
            if (street == true)
            {
                tileSave.streetOut.left = true;
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 3; j < 7; j++)
                    {
                        if(sidewalks == true && (j == 3 || j == 6) && i != 3)
                        {
                            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, i, j));
                            continue;
                        }
                        if (j > 3 && j < 6)
                        {
                            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Street, i, j));
                        }
                    }
                }
            }
            else
            {
                tileSave.streetOut.left = false;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 3; j < 7; j++)
                    {
                        tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Building, i, j));
                    }
                }
            }
        }

        private void StraßeRechts(TileSave tileSave, bool street, bool sidewalks)
        {
            if(street == true)
            {
                tileSave.streetOut.right = true;
                for (int i = 6; i < 10; i++)
                {
                    for (int j = 3; j < 7; j++)
                    {
                        if (sidewalks == true && (j == 3 || j == 6) && i != 6) {
                            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, i, j));
                            continue;
                        }
                        if (j > 3 && j < 6)
                        {
                            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Street, i, j));
                        }
                    }
                }
            }
            else
            {
                tileSave.streetOut.right = false;
                for (int i = 7; i < 10; i++)
                {
                    
                    for (int j = 3; j < 7; j++)
                    {
                        tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Building, i, j));
                    }
                    
                }
            }
        }

        private void StraßeOben(TileSave tileSave, bool street, bool sidewalks)
        {
            if(street == true)
            {
                tileSave.streetOut.up = true;
                for (int i = 3; i < 7; i++)
                {
                    for (int j = 6; j < 10; j++)
                    {
                        if(sidewalks == true && (i == 3 || i == 6) && j != 6)
                        {
                            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, i, j));
                            continue;
                        }
                        if (i > 3 && i < 6)
                        {
                            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Street, i, j));
                        }
                    }
                }
            }
            else
            {
                tileSave.streetOut.up = false;
                for (int i = 3; i < 7; i++)
                {
                    for (int j = 7; j < 10; j++)
                    {
                        tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Building, i, j));
                    }
                }
            }
        }

        private void StraßeUnten(TileSave tileSave, bool street, bool sidewalks)
        {
            if (street == true)
            {
                tileSave.streetOut.down = true;
                for (int i = 3; i < 7; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if(sidewalks == true && (i == 3 || i == 6) && j != 3)
                        {
                            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Sidewalk, i, j));
                            continue;
                        }
                        if (i > 3 && i < 6)
                        {
                            tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Street, i, j));
                        }
                    }
                }
            }
            else
            {
                tileSave.streetOut.down = false;
                for (int i = 3; i < 7; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Building, i, j));
                    }
                }
            }
        }

        //Sidewalks müssen so generiert werden
        private void StraßeMitte(TileSave tileSave)
        {
            for (int i = 4; i < 6; i++)      //Generieren der Straße in der Mitte
            {
                for (int j = 4; j < 6; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Street, i, j));
                }
            }
        }

        private void ChunkEckenmitHaus(TileSave tileSave)
        {
            int i = 0, j = 0;
            for (i = 0; i < 3; i++) //Links unten
            {
                for (j = 0; j < 3; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Building, i, j));
                }
            }
            for (i = 7; i < 10; i++) //Rechts unten
            {

                for (j = 0; j < 3; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Building, i, j));
                }
            }
            for (i = 0; i < 3; i++) //Links oben
            {
                for (j = 7; j < 10; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Building, i, j));
                }           
            }
            for (i = 7; i < 10; i++) //Rechts oben
            {
                for (j = 7; j < 10; j++)
                {
                    tileSave.gameObjects.Add(new GameObjectSave(ObjectType.Building, i, j));
                }
            }
        }

        public void CreateSomeTiles()
        {
            TileSave testTile = new TileSave();
            testTile.TileID = 1;
            for(int x = 0; x < 10; x++)
            {
                for(int y = 4; y < 6; y++)
                {
                    testTile.gameObjects.Add(new GameObjectSave(Types.ObjectType.Street, x, y));
                }
            }
            testTile.collisionBoxes.Add(new Box2D(0f, 0f, 10f, 4f));
            testTile.collisionBoxes.Add(new Box2D(0f, 6f, 10f, 4f));
            testTile.streetOut.left = true;
            testTile.streetOut.right = true;
            testTile.description = "Straße, die links und rechts einen Ausgang hat";
            testTile.chunkType = Types.ChunkType.CityStreet;
            tileHolder.SaveTiles.Add(testTile);
        }
        

    }
}
