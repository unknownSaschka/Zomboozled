using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Model.WorldLogic;
using Model.ItemLogic;
using Model.PawnLogic.PlayerLogic;
using Model.WorldLogic.TileLogic;

namespace Model.PawnLogic.EnemyLogic
{
    public class EnemyManager
    {
        //private MainModel _model;
        private MultiValueDictionary<Vector2, Enemy> _enemyDic;
        private Stack<Enemy> _enemyPool;
        private Random ran = new Random();
        private float _enemySize = 0.1f;
        private int _enemyLifePoints = 100;
        private PlayerManager _playerManager;
        private TileManager _tileManager;
        private ItemManager _itemManager;

        public EnemyManager(PlayerManager playerManager, ItemLogic.ItemManager itemManager, TileManager tileManager)
        {
            //Console.WriteLine($"Test {Vector2.Zero}");
            _enemyDic = new MultiValueDictionary<Vector2, Enemy>();
            _enemyPool = new Stack<Enemy>();

            _playerManager = playerManager;
            _tileManager = tileManager;
            _itemManager = itemManager;
            for (int i = 0; i < 10; i++)
            {
                _enemyPool.Push(new Enemy(new Vector2(0, 0), new Vector2(0, 0), _enemySize, 1, _enemyLifePoints, 20));
            }
        }



        //public IEnumerable<object> Objects { get { return obs.Select(o => o); } }

        public IEnumerable<Enemy> GetEnemiesOfChunk(Vector2 chunkPos)
        {
            return _enemyDic.Where(i => i.Key == chunkPos).SelectMany(i => i.Value);
        }


        public void AddEnemy(Vector2 chunkPos, Vector2 enemyPos)
        {
            float distance = Vector2.Distance(Vector2.Zero, chunkPos);

            if (_enemyPool.Count > 0)
            {
                var enemy = _enemyPool.Pop();
                enemy.Reset(chunkPos, enemyPos);
                _enemyDic.Add(chunkPos, enemy);
            }
            else
            {
                var randomValue = ran.NextDouble();
                if (randomValue < 0.005)
                {
                    _enemyDic.Add(chunkPos, new FlyingEnemy(enemyPos, chunkPos, _enemySize * 2, 1 * 3, (_enemyLifePoints + +(int)distance) / 3, 10));
                    
                }else if(randomValue < 0.075)
                {
                    _enemyDic.Add(chunkPos, new Enemy(enemyPos, chunkPos, _enemySize*3, 0.3f, _enemyLifePoints*20 + +(int)distance * 2, 30));
                }
                else
                {
                    _enemyDic.Add(chunkPos, new Enemy(enemyPos, chunkPos, _enemySize, 1, _enemyLifePoints + (int)distance, 20));
                }
                
            }
        }

        public void Update(MainModel model, int updateDistance, float deltaT)
        {
            if (model.WorldDebug)
            {
                return;
            }
            var activeChunks = _enemyDic.Select(i => i).Where(i => i.Key.X <= _playerManager.Player.Chunk.X + updateDistance && i.Key.X >= _playerManager.Player.Chunk.X - updateDistance && i.Key.Y <= _playerManager.Player.Chunk.Y + updateDistance && i.Key.Y >= _playerManager.Player.Chunk.Y - updateDistance).ToList();
            Chunk currentChunk = null;
            
            
            foreach (var enemyList in activeChunks.ToList())
            {
                currentChunk = model.World.chunks[enemyList.Key];
                var tileType =_tileManager.tiles[currentChunk.TileId];
                foreach (var enemy in enemyList.Value.ToList())
                {
                    if (model.World.chunks.ContainsKey(enemyList.Key))
                    {
                        if(enemy.Type == Enemy.EnemyType.Bird)
                        {
                            var fly = (FlyingEnemy)enemy;
                            fly.Update(_playerManager.Player, currentChunk, tileType, deltaT);
                        }
                        else
                        {
                            enemy.Update(_playerManager.Player, currentChunk, tileType, deltaT);
                        }

                        if (enemy.Health <= 0)
                        {
                            //Spawne Item beim Tod eines Enemies
                            _playerManager.KillCount += 1;
                             _itemManager.DropItemEnemy(enemy);
                            _enemyDic.Remove(enemyList.Key, enemy);
                            _enemyPool.Push(enemy);
                            continue;
                        }

                        if (enemy.UpdateCurrentChunk())
                        {
                            _enemyDic.Remove(enemyList.Key, enemy);
                            _enemyDic.Add(enemy.Chunk, enemy);
                            //Console.WriteLine($"Update in Enemy Manager! From {enemyList.Key} to {enemy.Chunk}");
                        }
                    }
                }

            }
        }

        public void ClearAllEnemies()
        {
            _enemyDic = new MultiValueDictionary<Vector2, Enemy>();
            _enemyPool = new Stack<Enemy>();
        }

        public void SpanwEnemy(Chunk chunk)
        {
            
            //AddEnemy(new Vector2(chunk.chunkX, chunk.chunkY), new Vector2(2.5f, 2.5f));
            int randomNum, i, TileId = chunk.TileId;
            
            if(TileId == 0)
            {
                //Console.WriteLine("EnemyManager: TileId gibts nicht!");
                return;
            }

            if(_tileManager.tiles[TileId].streetOut.left == true)
            {
                randomNum = (int) (ran.NextDouble() * 3);

                for (i = 0; i < randomNum; i++)
                {
                    AddEnemy(new Vector2(chunk.chunkX, chunk.chunkY), new Vector2(0.5f, 2.5f));
                }
            }

            if (_tileManager.tiles[TileId].streetOut.right == true)
            {
                randomNum = (int)(ran.NextDouble() * 3);

                for (i = 0; i < randomNum; i++)
                {
                    AddEnemy(new Vector2(chunk.chunkX, chunk.chunkY), new Vector2(4.5f, 2.5f));
                }
            }

            if (_tileManager.tiles[TileId].streetOut.up == true)
            {
                randomNum = (int)(ran.NextDouble() * 3);

                for (i = 0; i < randomNum; i++)
                {
                    AddEnemy(new Vector2(chunk.chunkX, chunk.chunkY), new Vector2(2.5f, 4.5f));
                }
            }

            if (_tileManager.tiles[TileId].streetOut.down == true)
            {
                randomNum = (int)(ran.NextDouble() * 3);

                for (i = 0; i < randomNum; i++)
                {
                    AddEnemy(new Vector2(chunk.chunkX, chunk.chunkY), new Vector2(2.5f, 0.5f));
                }
            }

            if(TileId == 20)
            {
                //Console.WriteLine("EnemySpawns BossChunk");
                //randomNum = (int)(ran.NextDouble() * 10);
                for (i = 0; i < 15; i++)
                {
                    AddEnemy(new Vector2(chunk.chunkX, chunk.chunkY), new Vector2((float)ran.NextDouble() * 5, (float)ran.NextDouble() * 5));
                }
            }

            if(TileId < 29 && TileId > 24)
            {
                //Console.WriteLine("EnemySpawns Arena");
                randomNum = (int)(ran.NextDouble() * 10);
                for (i = 0; i < randomNum; i++)
                {
                    AddEnemy(new Vector2(chunk.chunkX, chunk.chunkY), new Vector2((float)ran.NextDouble() * 5, (float)ran.NextDouble() * 5));
                }
            }
        }

        

    }
}
