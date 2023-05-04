using System.Collections.Generic;
using System.Numerics;
using Model;
using Zenseless.Geometry;
using Model.PawnLogic.EnemyLogic;
using Model.WorldLogic;
using Model.WorldLogic.TileLogic;
using System;
using Extension;
using Model.PawnLogic.BossLogic;
using Model.PawnLogic.PlayerLogic;
using System.Diagnostics;
using Model.Audio;

namespace Model.PawnLogic.ProjectileLogic
{
    public class ProjectileManager
    {
        private List<Projectile> _activeProjectiles;
        private Stack<Projectile> _poolProjectiles;
        private List<Grenade> _grenades;
        private List<Explosion> _explosion;
        private List<Thomas> _thomases;

        public List<DamageNumber> damageNumbers;

        private Stopwatch stopwatch = new Stopwatch();
        private MainModel _model;

        public ProjectileManager(MainModel model){
            _model = model;
            _activeProjectiles = new List<Projectile>();
            _poolProjectiles = new Stack<Projectile>();
            _grenades = new List<Grenade>();
            _explosion = new List<Explosion>();
            _thomases = new List<Thomas>();

            damageNumbers = new List<DamageNumber>();
            }

        public void Add(Vector2 originPos, Vector2 originChunk, Vector2 direction, float damage, float size, bool destination)   //bool destination False = soll spielerschaden machen, true = soll gegner schaden machen
        {
            if (_poolProjectiles.Count > 0)
            {
                var projectile = _poolProjectiles.Pop();
                projectile.Reset(originPos, direction, originChunk, 10, damage, size, destination);
                _activeProjectiles.Add(projectile);
            }
            else
            {
                //_enemyDic.Add(chunkPos, new Enemy(enemyPos, chunkPos, 0.3f, 1));
                _activeProjectiles.Add(new Projectile(originPos, direction, originChunk, 10, 1, damage, size, destination));
            }
        }

        public void AddGrenade(Vector2 originPos, Vector2 originChunk, Vector2 direction, float damage, float speed, float explosionRadius, float time, bool playerDamage)
        {
            _grenades.Add(new Grenade(originPos, direction, originChunk, speed, time, damage, 1.5f, playerDamage));
            //_grenades.Add(new Grenade(originPos, direction, originChunk, 5, 0.5, 1000, 1.5f));

        }

        public void AddThomas(Vector2 originPos, Vector2 originChunk, Vector2 direction, float damage, float speed, float time)
        {
            _thomases.Add(new Thomas(originPos, direction, originChunk, speed, 10, damage));
        }

        public void ClearAllProjectiles()
        {
            _activeProjectiles.Clear();
            _poolProjectiles.Clear();
            _grenades.Clear();
            _explosion.Clear();
            _thomases.Clear();

        }

        public void Update(float deltaT)
        {
            foreach(Projectile proj in _activeProjectiles.ToArray())
            {
                
                if(proj.Lifetime < 0)
                {
                    //System.Console.WriteLine("Removed projectile");
                    _activeProjectiles.Remove(proj);
                    _poolProjectiles.Push(proj);
                    continue;
                }
                proj.Update(deltaT);
                proj.UpdateCurrentChunk();
            }

            foreach(Grenade grenade in _grenades.ToArray())
            {
                if(grenade.LifeTime < 0)
                {
                    _explosion.Add(grenade.CreateExplosion());
                    _grenades.Remove(grenade);
                    continue;
                }
                grenade.Update(deltaT);
                grenade.UpdateCurrentChunk();
            }

            foreach (Thomas thomas in _thomases.ToArray())
            {
                if (thomas.Lifetime < 0)
                {
                    _thomases.Remove(thomas);
                    continue;
                }
                thomas.Update(deltaT);
                thomas.UpdateCurrentChunk();
            }

            foreach (Explosion exp in _explosion.ToArray())
            {
                
                if (exp.LifeTime < 0)
                {
                    //Console.WriteLine("Remove Explosion");
                    _explosion.Remove(exp);
                    
                    continue;
                }
                exp.Update(deltaT);

            }

            foreach (DamageNumber number in damageNumbers.ToArray())
            {
                number.Timer--;
                if (number.Timer == 0)
                {
                    damageNumbers.Remove(number);
                }
                else
                {
                    Vector2 pos = number.Position;
                    number.Position = new Vector2(pos.X + (float)Math.Sin(number.Timer)/100, pos.Y+ (float)number.Timer/1000);
                }
                
            }
        }

        public IEnumerable<Projectile> Projectile
        {
            get { return _activeProjectiles; }
        }
        public IEnumerable<Grenade> Grenades
        {
            get { return _grenades; }
        }
        public IEnumerable<Explosion> Explosions
        {
            get { return _explosion; }
        }

        public IEnumerable<Thomas> Thomases
        {
            get { return _thomases; }
        }

        public void CollisonWithBuilding(MainModel model)
        {
            foreach (Projectile proj in _activeProjectiles.ToArray())
            {
                //var item;
                //var tileID = model.World.chunks.TryGetValue(proj.Chunk, out item).TileId;

                if (!model.World.chunks.TryGetValue(proj.Chunk, out Chunk item))
                {
                    proj.Lifetime = -1;
                    continue;
                }

                foreach (Box2D box in model.ManagerHolder.TileManager.tiles[item.TileId].collisionBoxes)
                {
                    if (box.Intersects(proj.Collider))
                    {
                        proj.Lifetime = -1;
                        break;
                    }
                }

            }
        }
        public void CollisonTest(World world, TileManager tileManager, EnemyManager enemyManager, BossManager bossManager, PlayerManager playerManager, AudioManager audioManager, int playerBaseDamage)
        {
            if(stopwatch.Elapsed.Seconds > 3)
            {
                stopwatch.Stop();
            }

            bool hitSomething = false;
            foreach (Projectile proj in _activeProjectiles.ToArray())
            {
                hitSomething = false;
                //var item;
                //var tileID = model.World.chunks.TryGetValue(proj.Chunk, out item).TileId;

                //Prevents the prjectile from leaving the currently generated World
                if (!world.chunks.TryGetValue(proj.Chunk, out Chunk item))
                {
                    proj.Lifetime = -1;
                    continue;
                }
                //Checks collision with the chunk collision boxes
                foreach (Box2D box in tileManager.tiles[item.TileId].collisionBoxes)
                {
                    if (box.Intersects(proj.Collider))
                    {
                        proj.Lifetime = -1;
                        hitSomething = true;
                        break;
                    }
                }
                if (hitSomething) continue;

                //Checks collision with the enemies in the chunk itself
                foreach (Enemy enemy in enemyManager.GetEnemiesOfChunk(proj.Chunk))
                {
                    if(enemy.Collider.Intersects(proj.Collider)){

                        enemy.GetDamage(proj.Damage + playerBaseDamage);
                        proj.Lifetime = -1;

                        AddDamageNumber(enemy, (int)proj.Damage + playerBaseDamage);

                        break;
                    }

                }

                foreach(Boss boss in bossManager.GetBossOfChunk(proj.Chunk))
                {
                    if (boss.Collider.Intersects(proj.Collider) && (world.GetChunk(playerManager.Player.Chunk).TileId >= 16 && world.GetChunk(playerManager.Player.Chunk).TileId <= 24))
                    {
                        if (proj.Destination)
                        {
                            boss.GetDamage((int)proj.Damage + playerBaseDamage);
                            proj.Lifetime = -1;
                            AddDamageNumber(boss, (int)proj.Damage + playerBaseDamage);
                        }
                        break;
                    }
                }

                if(proj.Collider.Intersects(playerManager.Player.Collider) && !proj.Destination && proj.Chunk.Equals(playerManager.Player.Chunk) && !stopwatch.IsRunning)
                {
                    playerManager.Player.LifePoints -= (int)proj.Damage;
                    proj.Lifetime = -1;
                    AddDamageNumber(playerManager.Player, (int)proj.Damage);
                    audioManager.PlayEffect(Audio.AudioManager.EffectName.Damage);
                    stopwatch.Restart();
                }

            }
            foreach (Grenade grenade in _grenades.ToArray())
            {
                if (!world.chunks.TryGetValue(grenade.Chunk, out Chunk item))
                {
                    
                    continue;
                }
                foreach (Box2D box in tileManager.tiles[item.TileId].collisionBoxes)
                {       
                   switch(box.GetCollisionBoxNormal(new Box2D(grenade.Position.X - grenade.Radius, grenade.Position.Y - grenade.Radius, grenade.Radius * 2, grenade.Radius * 2)))
                        {
                            case 1:
                                grenade.BounceOfWall(Grenade.WallToBounceOffDirection.YAxis);
                                //links bounce
                                Console.WriteLine("Bounce 1");
                                break;
                            case 2:
                                //rechts bounce
                                //grenade.BounceOfWall(Grenade.WallToBounceOffDirection.YAxis);
                                Console.WriteLine("Bounce 2");
                                break;
                            case 3:
                                //grenade.BounceOfWall(Grenade.WallToBounceOffDirection.XAxis);
                                //Fall untere Wand
                                Console.WriteLine("Bounce 3");
                                break;
                            case 4:
                                //grenade.BounceOfWall(Grenade.WallToBounceOffDirection.XAxis);
                                Console.WriteLine("Bounce 4");
                                break;
                        case 0:

                            break;
                    }
                }
            }
            foreach (Explosion expl in _explosion.ToArray())
            {
                if (expl.Exploded == true) continue;
                expl.Exploded = true;
                if (!world.chunks.TryGetValue(expl.Chunk, out Chunk item))
                {
                    Console.WriteLine("NO CHUNK!");
                    continue;
                }
                
                if(expl.PlayerDamage == true)
                {
                    if (playerManager.Player.Collider.Intersects(expl.Collider) && playerManager.Player.Chunk.Equals(expl.Chunk) && !stopwatch.IsRunning)
                    {
                        Console.WriteLine($"PlayerDamage {expl.Damage} Schaden");
                        playerManager.Player.LifePoints -= (int)expl.Damage;
                        stopwatch.Restart();
                    }
                }

                foreach(Enemy enemy in enemyManager.GetEnemiesOfChunk(expl.Chunk))
                {
                    if(enemy.Collider.Intersects(expl.Collider)){
                        enemy.GetDamage((int)expl.Damage + playerBaseDamage);
                        AddDamageNumber(enemy, (int)expl.Damage + playerBaseDamage);
                    }
                }

                foreach (Boss boss in bossManager.GetBossOfChunk(expl.Chunk))
                {
                    if (boss.Collider.Intersects(expl.Collider))
                    {
                        boss.GetDamage((int)expl.Damage + playerBaseDamage);
                        AddDamageNumber(boss, (int)expl.Damage + playerBaseDamage);
                    }
                }

                Vector2 checkChunk = expl.Chunk;
                if (expl.Position.X < expl.Radius)
                {

                    checkChunk.X--;
                    foreach (Enemy enemy in enemyManager.GetEnemiesOfChunk(checkChunk))
                    {
                        if (enemy.Collider.Intersects(expl.Collider))
                        {
                            enemy.GetDamage((int)expl.Damage + playerBaseDamage);
                        }
                    }

                    foreach (Boss boss in bossManager.GetBossOfChunk(checkChunk))
                    {
                        if (boss.Collider.Intersects(expl.Collider))
                        {
                            boss.GetDamage((int)expl.Damage + playerBaseDamage);
                        }
                    }
                }

                if (expl.Position.X > 5 - expl.Radius)
                {
                    checkChunk = expl.Chunk;
                    checkChunk.X++;
                    foreach (Enemy enemy in enemyManager.GetEnemiesOfChunk(checkChunk))
                    {
                        if (enemy.Collider.Intersects(expl.Collider))
                        {
                            enemy.GetDamage((int)expl.Damage + playerBaseDamage);
                        }

                    }

                    foreach (Boss boss in bossManager.GetBossOfChunk(checkChunk))
                    {
                        if (boss.Collider.Intersects(expl.Collider))
                        {
                            boss.GetDamage((int)expl.Damage + playerBaseDamage);
                        }
                    }
                }

                if (expl.Position.Y < expl.Radius)
                {
                    checkChunk = expl.Chunk;
                    checkChunk.Y--;
                    foreach (Enemy enemy in enemyManager.GetEnemiesOfChunk(checkChunk))
                    {
                        if (enemy.Collider.Intersects(expl.Collider))
                        {
                            enemy.GetDamage((int)expl.Damage + playerBaseDamage);
                        }
                    }

                    foreach (Boss boss in bossManager.GetBossOfChunk(checkChunk))
                    {
                        if (boss.Collider.Intersects(expl.Collider))
                        {
                            boss.GetDamage((int)expl.Damage + playerBaseDamage);
                        }
                    }
                }

                if (expl.Position.Y > 5 - expl.Radius)
                {
                    checkChunk = expl.Chunk;
                    checkChunk.Y++;
                    foreach (Enemy enemy in enemyManager.GetEnemiesOfChunk(checkChunk))
                    {
                        if (enemy.Collider.Intersects(expl.Collider))
                        {
                            enemy.GetDamage((int)expl.Damage + playerBaseDamage);
                        }
                    }

                    foreach (Boss boss in bossManager.GetBossOfChunk(checkChunk))
                    {
                        if (boss.Collider.Intersects(expl.Collider))
                        {
                            boss.GetDamage((int)expl.Damage + playerBaseDamage);
                        }
                    }
                }
            }

            foreach(Thomas thomas in _thomases.ToArray())
            {
                
                for(int y = -1; y < 2; y++)
                {
                    for (int x = -1; x < 2; x++)
                    {
                        Vector2 chunkOffset = new Vector2(x, y);
                        foreach (Enemy enemy in enemyManager.GetEnemiesOfChunk(thomas.Chunk + chunkOffset))
                        {
                            Circle currentThomas = new Circle(thomas.Collider.CenterX, thomas.Collider.CenterY, thomas.Radius);
                            currentThomas.CenterX += x * 5;
                            currentThomas.CenterY += y * 5;
                            //Console.WriteLine(currentThomas.Chunk + new Vector2(x, y));
                            if (enemy.Collider.Intersects(currentThomas))
                            {
                                enemy.GetDamage((int)thomas.Damage + playerBaseDamage);
                            }
                        }
                    }
                }
            }
        }

        private void AddDamageNumber(Pawn pawn, int amount)
        {
            damageNumbers.Add(new DamageNumber(pawn.Chunk, pawn.Position, amount));
        }
    }

    public class DamageNumber
    {
        public Vector2 Chunk { get; set; }
        public Vector2 Position { get; set; }
        public int Amount { get; set; }
        public int Timer { get; set; } = 40;

        public DamageNumber(Vector2 chunk, Vector2 position, int amount)
        {
            Chunk = chunk;
            Position = position;
            Amount = amount;
        }
    }
}
