using Model.WorldLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Model.Audio.AudioManager;

namespace Model.PawnLogic.BossLogic
{
    public class BossManager
    {

        private MultiValueDictionary<Vector2, Boss> _bosses;
        private Dictionary<Vector2, bool> _spawnedBosses;
        private bool[] killedBosses = new bool[5];

        public BossManager()
        {

            _bosses = new MultiValueDictionary<Vector2, Boss>();
            _spawnedBosses = new Dictionary<Vector2, bool>();
            for (int i = 0; i < 5; i++)
            {
                killedBosses[i] = false;
            }
        }

        public void AddBoss(Vector2 bossChunk, int distance)
        {
            if (_spawnedBosses.ContainsKey(bossChunk))
            {
                return;
            }

            if (distance < 50) return;

            if(distance < 120)
            {
                _bosses.Add(bossChunk, new Malos(new Vector2(2.5f, 2.5f), bossChunk, 1f, 5, 10000));
                _spawnedBosses.Add(bossChunk, true);
            }
            else if(distance < 220)
            {
                _bosses.Add(bossChunk, new Gyorg(new Vector2(2.5f, 2.5f), bossChunk, 1f, 2, 15000));
                _spawnedBosses.Add(bossChunk, true);
            }
            else if(distance < 320)
            {
                _bosses.Add(bossChunk, new Keller(new Vector2(2.5f, 2.5f), bossChunk, 1f, 15, 25000));
                _spawnedBosses.Add(bossChunk, true);
            }
            else if(distance < 420)
            {
                _bosses.Add(bossChunk, new Saitama(new Vector2(2.5f, 2.5f), bossChunk, 1f, 2.7f, 30000));
                _spawnedBosses.Add(bossChunk, true);
            }
            else if(distance < 520)
            {
                _bosses.Add(bossChunk, new Sans(new Vector2(2.5f, 2.5f), bossChunk, 1f, 2.5f, 30000));
                _spawnedBosses.Add(bossChunk, true);
            }

        }

        public void Update(MainModel model, int updateDistance, float deltaT)
        {
            updateDistance = 4;
            var activeChunks = _bosses.Select(i => i).Where(i => i.Key.X <= model.ManagerHolder.PlayerManager.Player.Chunk.X + updateDistance && i.Key.X >= model.ManagerHolder.PlayerManager.Player.Chunk.X - updateDistance && i.Key.Y <= model.ManagerHolder.PlayerManager.Player.Chunk.Y + updateDistance && i.Key.Y >= model.ManagerHolder.PlayerManager.Player.Chunk.Y - updateDistance).ToList();
            Chunk currentChunk = null;

            if(!(model.World.GetChunk(model.ManagerHolder.PlayerManager.Player.Chunk).TileId >= 16 && model.World.GetChunk(model.ManagerHolder.PlayerManager.Player.Chunk).TileId <= 24))
            {
                return;
            }

            foreach (var bossList in activeChunks.ToList())
            {
                currentChunk = model.World.chunks[bossList.Key];
                var tileType = model.ManagerHolder.TileManager.tiles[currentChunk.TileId];
                foreach (var boss in bossList.Value.ToList())
                {
                    if (model.World.chunks.ContainsKey(bossList.Key))
                    {
                        switch (boss.TypeOfBoss)
                        {
                            case Boss.BossType.Malos:
                                Malos malos = (Malos)boss;
                                malos.Update(model);
                                break;

                            case Boss.BossType.Gyorg:
                                Gyorg gyorg = (Gyorg)boss;
                                gyorg.Update(model, deltaT);
                                break;
                            case Boss.BossType.Keller:
                                Keller keller = (Keller)boss;
                                keller.Update(model, deltaT);
                                break;
                            case Boss.BossType.Saitama:
                                Saitama saitama = (Saitama)boss;
                                saitama.Update(model, deltaT);
                                break;
                            case Boss.BossType.Sans:
                                Sans sans = (Sans)boss;
                                sans.Update(model, deltaT);
                                break;
                        }
                        

                        if (boss.LifePoints <= 0)
                        {
                            //Spawne Item beim Tod eines Enemies
                            model.ManagerHolder.PlayerManager.KillCountBosses += 1;
                            model.ManagerHolder.ItemManager.DropItemEnemy(boss);

                            _bosses.Remove(bossList.Key, boss);
                            model.ManagerHolder.AudioManager.PlayEffect(EffectName.Win);
                            //Aufruf für erweitern des Battle Fog
                            if(boss.TypeOfBoss == Boss.BossType.Malos && killedBosses[0] == false)
                            {
                                model.World.FogDistanceStart += 10 * 5;
                                model.World.FogDistanceEnd += 10 * 5;
                                killedBosses[0] = true;
                            }
                            else if(boss.TypeOfBoss == Boss.BossType.Gyorg && killedBosses[1] == false)
                            {
                                model.World.FogDistanceStart += 10 * 5;
                                model.World.FogDistanceEnd += 10 * 5;
                                killedBosses[1] = true;
                            }
                            
                            

                            continue;
                        }


                        if (boss.UpdateCurrentChunk())
                        {
                            _bosses.Remove(bossList.Key, boss);

                            _bosses.Add(boss.Chunk, boss);
                            //Console.WriteLine($"Update in Enemy Manager! From {enemyList.Key} to {enemy.Chunk}");
                        }
                    }
                }

            }
        }

        public IEnumerable<Boss> GetBossOfChunk(Vector2 chunk)
        {
            return _bosses.Where(i => i.Key == chunk).SelectMany(i => i.Value);
        }

        public void ClearAllBosses()
        {
            _bosses.Clear();
            _spawnedBosses.Clear();
            for(int i = 0; i < 5; i++)
            {
                killedBosses[i] = false;
            }
        }
    }
}
