using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Model.Audio;
using Model.WorldLogic;
using Model.PawnLogic;
using Model.InterfaceCollection;
using DiscordRPC;

namespace Model.PawnLogic.PlayerLogic
{
    public delegate void PlayerChunkChanged();
    public class PlayerManager
    {
        public Player Player { get; }
        public float PlayerSpeed { get; } = 5;
        public PlayerChunkChanged PlayerChunkChangedEvent;
        public float Distance { get { return Vector2.Distance(Vector2.Zero, Player.Position + Player.Chunk * 5 + new Vector2(-2.5f, -2.5f)); } }
        private DiscordRpcClient RPCClient;
        public int killCount;
        public int KillCount {
            get { return killCount; }
            set
            {
                RPCClient.SetPresence(new DiscordRPC.RichPresence()
                {
                    Details = "Killt Zombies",
                    State = value + " Zombies gekillt",
                    Assets = new DiscordRPC.Assets()
                    {
                        LargeImageKey = "tootiitoot",
                        LargeImageText = "Zoggd",
                        SmallImageText = "tootiitoot"
                    }
                });
                killCount = value;
            }
        }
        public int KillCountBosses { get; set; } = 0;
        public bool GameEnd { get; internal set; }

        public PlayerManager(DiscordRpcClient discordRPC)
        {
            RPCClient = discordRPC;
            Player = new Player(new Vector2(2.5f, 2.5f), new Vector2(0, 0), 0.1f, PlayerSpeed, 100); 

        }
        public void CheckPlayerChunk(IManagerHolder managerHolder, World world, Generator generator, MainModel model)
        {
            var tileManager = managerHolder.TileManager;
            var player = managerHolder.PlayerManager.Player;
            Vector2 currentChunk = Player.Chunk;


            Vector2 newChunk = Player.Chunk;
            //Vector2 chunkDifference = newChunk - currentChunk;
            //Console.WriteLine($"Neuer Chunk: {_player.Chunk}");
            if (!player.IsInCar)
            {
                managerHolder.AudioManager.ChangeBackgroundMusic((tileManager.tiles[world.chunks[player.Chunk].TileId].chunkType), (int)Distance);
            }
            
            if(model.WorldManager.ActiveWorldNum != 0)
            {
                generator.Generate();
            }

            if(model.ManagerHolder.TileManager.tiles[((model.World.GetChunk(Player.Chunk.X, Player.Chunk.Y))).TileId].chunkType == WorldLogic.TileLogic.Types.ChunkType.Boss)
            {
                for(int y = -1; y < 2; y++)
                {
                    for (int x = -1; x < 2; x++)
                    {
                        if(((model.World.GetChunk(Player.Chunk.X + x, Player.Chunk.Y + y))).TileId == 20)
                        {
                            model.ManagerHolder.BossManager.AddBoss(new Vector2(Player.Chunk.X + x, Player.Chunk.Y + y), (int)Distance);
                        }
                    }
                }
            }

            if(model.ManagerHolder.TileManager.tiles[((model.World.GetChunk(Player.Chunk.X, Player.Chunk.Y))).TileId].chunkType == WorldLogic.TileLogic.Types.ChunkType.EndGrass)
            {
                GameEnd = true;
            }

            //Distance = (int)Vector2.Distance(Vector2.Zero, Player.Position + Player.Chunk * 5);
            
            

            managerHolder.Pathfinding.UpdateEnemyMovements(world, player, tileManager);
            world.chunks[newChunk].UpdatePathInformation(0, Chunk.DirectionToPlayer.ThisChunk, 0);



            //_world.chunks[newChunk].Direction = Chunk.DirectionToPlayer.ThisChunk;
        }

        public void Update(MainModel model)
        {
            if (Player.LifePoints <= 0)
            {
                Player.LifePoints = 0;
                model.GameOver = true;
            }
        }

        public delegate void MoneyChange(int amount);


    }
}
