using System;
using System.Collections.Generic;
using Zenseless.Geometry;
using System.IO;
using static Model.WorldLogic.TileLogic.Types;

namespace Model.WorldLogic.TileLogic
{

    public class TileManager
    {
        private static string fileName = "tiles.bin";
        public Dictionary<int, Tile> tiles;
        //public List<TileSave> saveTiles = new List<TileSave>();     //Zum abspeichern der erstellten Tiles

        public void LoadTiles()
        {
            //List<TileSave> tileSave = new List<TileSave>();
            TileHolder tileHolder = new TileHolder();
            decimal version = tileHolder.Version;
            var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            Console.WriteLine("Load Tiles");

            try
            {
                using (Stream stream = File.Open(fileName, FileMode.Open))
                {
                    tileHolder = (TileHolder)bformatter.Deserialize(stream);
                    //stream.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("File error, creating a new File");
                Console.WriteLine(e.InnerException);
                TileSaver tSaver = new TileSaver();
                tSaver.CreateTiles();
                tSaver.SaveTiles();
                LoadTiles();
                return;
            }


            

            if (tileHolder.Version != version)
            {
                TileSaver tSaver = new TileSaver();
                tSaver.CreateTiles();
                tSaver.SaveTiles();
                LoadTiles();
                return;
            }


            tiles = new Dictionary<int, Tile>();
            //Tile temp;
            //int loadedTiles = 0;
            foreach (TileSave save in tileHolder.SaveTiles)
            {
                //Console.WriteLine(save.TileID);
                tiles.Add(save.TileID, new Tile(save.gameObjects, save.collisionBoxes, save.streetOut, save.chunkType));
                //Console.WriteLine("Tile geladen");
                //loadedTiles++;
            }
            //Console.WriteLine(tiles[1].chunkType + " " + tiles[1].collisionBoxes + " " + tiles[1].streetOut);
            //Console.WriteLine("LoadedTiles: " + loadedTiles);
            Console.WriteLine("Vorhandene Tiles: " + tiles.Count);
        }

    }

   


}
