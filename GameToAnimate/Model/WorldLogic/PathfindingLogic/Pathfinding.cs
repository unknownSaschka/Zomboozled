using Model.WorldLogic.TileLogic;
using System.Linq;
using System.Numerics;
using Model.PawnLogic;

namespace Model.WorldLogic.PathfindingLogic
{
    public class Pathfinding
    {
        private uint _updateDistance;
        private uint _updateId;

        public Pathfinding(uint updateDistance)
        {
            _updateDistance = updateDistance;
            _updateId = 0;
        }



        public void UpdateEnemyMovements(World world, Pawn target, TileManager tileManager)
        {


            Chunk targetChunk = world.chunks[target.Chunk];
            world.chunks[target.Chunk].Direction = Chunk.DirectionToPlayer.ThisChunk;


            //A park disables the enemy Pathfinding in a certain radius
            if (tileManager.tiles[targetChunk.TileId].chunkType == Types.ChunkType.Park || tileManager.tiles[targetChunk.TileId].chunkType == Types.ChunkType.Start)
            {
                var linq = world.chunks.Select(i => i).Where(i => i.Key.X <= target.Chunk.X + _updateDistance * 3 && i.Key.X >= target.Chunk.X - _updateDistance * 3 && i.Key.Y <= target.Chunk.Y + _updateDistance * 3 && i.Key.Y >= target.Chunk.Y - _updateDistance * 3);
                foreach (var chunk in linq)
                {
                    //chunk.Value.DirectionToPlayer = Chunk.DirectionToPlayer.NoWay;
                    //World.chunks[target.Chunk].UpdatePathInformation(99, Chunk.DirectionToPlayer.NoWay, 1);
                    chunk.Value.Direction = Chunk.DirectionToPlayer.NoWay;
                }
                world.chunks[target.Chunk].Direction = Chunk.DirectionToPlayer.ThisChunk;
                return;
            }
            if (tileManager.tiles[targetChunk.TileId].chunkType == Types.ChunkType.Boss)
            {



            }
            //World.chunks[target.Chunk].Direction = Chunk.DirectionToPlayer.ThisChunk;
            UpdateEnemyPathfindingStreet(target.Chunk, 1, Chunk.DirectionToPlayer.ThisChunk, _updateId++, world, target, tileManager);
        }

        public void UpdateEnemyPathfindingStreet(Vector2 chunk, uint counter, Chunk.DirectionToPlayer originDir, uint updateId, World world, Pawn target, TileManager tileManager)
        {
            Vector2 newChunk = chunk;
            //Console.WriteLine($"Updated Chunk: {chunk} coming from {originDir.ToString()}");

            world.chunks[chunk].UpdateId = updateId;
            if (counter > _updateDistance) return;


            newChunk.X += 1;
            int tileIDoriginalChunk = world.chunks[chunk].TileId;

            var orgChunkType = tileManager.tiles[tileIDoriginalChunk].chunkType;
            Chunk newChunkValue;

            var originalChunk = tileManager.tiles[tileIDoriginalChunk];
            Tile targetTile;

            if (originDir != Chunk.DirectionToPlayer.Left && (originalChunk.streetOut.free || originalChunk.streetOut.right == true))
            {
                if (world.chunks.TryGetValue(newChunk, out newChunkValue))
                {
                    targetTile = tileManager.tiles[newChunkValue.TileId];
                    if(targetTile.streetOut.left == true || targetTile.streetOut.free)
                    {
                        if (world.chunks[newChunk].UpdatePathInformation(counter, Chunk.DirectionToPlayer.Left, updateId))
                        {
                            UpdateEnemyPathfindingStreet(newChunk, counter + 1, Chunk.DirectionToPlayer.Right, updateId, world, target, tileManager);
                        }
                    }

                }
            }


            newChunk.X -= 2;
            if (originDir != Chunk.DirectionToPlayer.Right && (originalChunk.streetOut.free || originalChunk.streetOut.left == true))
            {
                if (world.chunks.TryGetValue(newChunk, out newChunkValue))
                {
                    targetTile = tileManager.tiles[newChunkValue.TileId];
                    if (targetTile.streetOut.right == true || targetTile.streetOut.free)
                    {
                        if (world.chunks[newChunk].UpdatePathInformation(counter, Chunk.DirectionToPlayer.Right, updateId))
                        {
                            UpdateEnemyPathfindingStreet(newChunk, counter + 1, Chunk.DirectionToPlayer.Left, updateId, world, target, tileManager);
                        }
                    }
                }
            }



            newChunk.X += 1;
            newChunk.Y += 1;
            if (originDir != Chunk.DirectionToPlayer.Down && (originalChunk.streetOut.free || originalChunk.streetOut.up == true))
            {
                if (world.chunks.TryGetValue(newChunk, out newChunkValue))
                {

                    targetTile = tileManager.tiles[newChunkValue.TileId];
                    if (targetTile.streetOut.down == true || targetTile.streetOut.free)
                    {
                        if (world.chunks[newChunk].UpdatePathInformation(counter, Chunk.DirectionToPlayer.Down, updateId))
                        {
                            UpdateEnemyPathfindingStreet(newChunk, counter + 1, Chunk.DirectionToPlayer.Top, updateId, world, target, tileManager);
                        }
                    }
                }
            }


            newChunk.Y -= 2;
            if (originDir != Chunk.DirectionToPlayer.Top && (originalChunk.streetOut.free || originalChunk.streetOut.down == true))
            {
                if (world.chunks.TryGetValue(newChunk, out newChunkValue))
                {
                    targetTile = tileManager.tiles[newChunkValue.TileId];
                    if (targetTile.streetOut.up == true || targetTile.streetOut.free)
                    {
                        if (world.chunks[newChunk].UpdatePathInformation(counter, Chunk.DirectionToPlayer.Top, updateId))
                        {
                            UpdateEnemyPathfindingStreet(newChunk, counter + 1, Chunk.DirectionToPlayer.Down, updateId, world, target, tileManager);
                        }
                    }
                }
            }

        }
    }
}

