using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenseless.Geometry;
using System.Numerics;
using Model.WorldLogic.TileLogic;
using System.Diagnostics;
using System.Windows.Forms;
using Model.PawnLogic.EnemyLogic;
using Model.PawnLogic;
using Model.ItemLogic;
using Model.WorldLogic;
using Model.PawnLogic.VehicleLogic;
using Model.PawnLogic.BossLogic;

namespace Model
{
    public class Collisions
    {
        private MainModel _model;

        //private float playerCollisionOffset = 0.05f;
        private Vector2 currentChunkPlayer;
        private List<Box2D> _collisionBoxes;
        //private GameTime time = new GameTime();
        private Stopwatch timer = new Stopwatch();
        //private Vector2 _oldPositionofPlayer;
        private Vector2 _newPositionofPlayer;
        private IManagerHolder _manager;

        //private Vector2 _deltaPlayer;

        public Collisions(IManagerHolder manager, MainModel model)
        {
            //_oldPositionofPlayer = new Vector2(0, 0);
            _newPositionofPlayer = new Vector2(0, 0);

            _manager = manager;
            _model = model;
            //_deltaPlayer = new Vector2(0, 0);
            timer.Start();
        }

        public Collisions(MainModel model)
        {
            _model = model;
        }

        public void CheckAllCollisions(InputData inputData)
        {
            //Console.WriteLine($"Life of Player: {_manager.PlayerManager.Player.LifePoints}");
            _newPositionofPlayer = _manager.PlayerManager.Player.Position;
            //_deltaPlayer = _newPositionofPlayer - _oldPositionofPlayer;
            currentChunkPlayer = _manager.PlayerManager.Player.Chunk;
            _collisionBoxes = new List<Box2D>();     //Koordinaten der Boxen in WorldKoordianten
            AddCollisionBoxes(_collisionBoxes, currentChunkPlayer);
            CheckShopItemCollisions();

            if (_manager.PlayerManager.Player.IsInCar)
            {
                CheckCarCollisions(_collisionBoxes, ref inputData);
            }
            else
            {
                if (!_model.WorldDebug)
                {
                    CheckPlayerWithBuildingCollisions(_manager.PlayerManager.Player, _collisionBoxes, ref inputData);
                }
            }

            if((currentChunkPlayer.X >= 2) && (_model.WorldManager.ActiveWorldNum == 0))
            {
                _model.WorldManager.ChangeWorld();
            }

            //Collisions für WeltChange
            if(currentChunkPlayer.Equals(new Vector2(0, 0)) && _model.WorldManager.ActiveWorldNum != 0)
            {
                if (!_model.WorldManager.ChangeWorld())
                {
                    
                }
                
            }

            if (CheckPlayerWithEnemyCollisions() || CheckPlayerWithBossCollisions())
            {
                _model.ManagerHolder.AudioManager.PlayEffect(Audio.AudioManager.EffectName.Damage);
                //Console.WriteLine("BOI");

                //Console.WriteLine($"CarLife: {_model.vehicle.LifePoints}");

                if (_manager.PlayerManager.Player.LifePoints <= 0)
                {
                    _manager.PlayerManager.Player.LifePoints = 0;
                    //MessageBox.Show("LOL Verloren. Du bist schlecht");
                    //Environment.Exit(1);
                    _model.GameOver = true;

                }

                if(_manager.VehicleManager.ActiveVehicle != null)
                {
                    if (_manager.VehicleManager.ActiveVehicle.LifePoints <= 0)
                    {

                        //_manager.VehicleManager.RemoveCar(_manager.VehicleManager.ActiveVehicle);
                        _manager.VehicleManager.InteractWithCar(true);
                        /*
                        MessageBox.Show("Auto verloren. Derzeit keine Ahnung, was zu tun ist. Game schließt sich :P");
                        Environment.Exit(1);
                        */

                        //_manager.PlayerManager.Player.IsInCar = false;
                        /*
                        _model.InteractWithCar();
                        _model.Vehicle.MoveToPosition(new Vector2(0,0), new Vector2(0,0));
                        _model.Vehicle.currentDir = new Vector2(1, 1);
                        _model.Vehicle.LifePoints = 400;
                        */
                    }
                }

                
            }
            CheckPlayerWithItemCollision();
            CheckEnemyOnBuildingCollisions(_collisionBoxes);
            
            
        }

        public void CheckPlayerWithBuildingCollisions(Pawn pawn, List<Box2D> collisionBoxes, ref InputData keyData)
        {
            //Vector2 zwischen = pawn.Position;
            int capacity = 0;
            foreach (Box2D colBox in collisionBoxes)
            {
                if (UndoOverlap(pawn.Collider, colBox, ref keyData))
                {
                    capacity++;
                }
            }
            if (capacity > 1)
            {
                for (int i = collisionBoxes.Count - 1; i > 0; --i)
                {
                    UndoOverlap(pawn.Collider, collisionBoxes[i], ref keyData);
                }
            }

            pawn.MoveToPosition(pawn.Position);
              
        }

        public void CheckPlayerWithItemCollision()
        {
            //MultiValueDictionary<Vector2, Item> itemList = _model.ItemManager.ItemList.ToList();

            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    var itemInChunks = _manager.ItemManager.GetItemsOfChunk(new Vector2(currentChunkPlayer.X + x * 5, currentChunkPlayer.Y + y * 5));
                    foreach (Item item in itemInChunks.ToList())
                    {
                        if (item.Collider.Intersects(_manager.PlayerManager.Player.Collider))
                        {
                            _manager.ItemManager.ApplyItemEffect(item.ItemType);
                            _manager.ItemManager.ItemList.Remove(new Vector2(currentChunkPlayer.X + x, currentChunkPlayer.Y + y), item);
                        }
                    }
                }
            }
        }

        public bool CheckPlayerWithEnemyCollisions()
        {
            if (_model.WorldDebug)
            {
                return false;
            }
            double timeCheck;
            Circle zwischen = new Circle(_manager.PlayerManager.Player.Collider.CenterX, _manager.PlayerManager.Player.Collider.CenterY, _manager.PlayerManager.Player.Radius);
            bool intersected = false;
            if (_manager.PlayerManager.Player.IsInCar)
            {
                timeCheck = 1d;
            }
            else
            {
                timeCheck = 1d;
            }
            
            foreach (Enemy enemy in _manager.EnemyManager.GetEnemiesOfChunk(currentChunkPlayer))
            {
                if (zwischen.Intersects(enemy.Collider) && _manager.VehicleManager.ActiveVehicle == null)
                {
                    
                    enemy.Collider.UndoOverlap(_manager.PlayerManager.Player.Collider);
                    //enemy.Collider.UndoOverlap(_manager.VehicleManager.ActiveVehicle.Collider);
                    if (timer.Elapsed.TotalSeconds > timeCheck)
                    {
                        //Console.WriteLine("intersected = true");
                        intersected = true;
                        timer.Restart();
                        /*
                        if (_manager.PlayerManager.Player.IsInCar)
                        {
                            _manager.VehicleManager.ActiveVehicle.AddDamage(enemy.BaseDamage, 0);
                            //Hier DamageSound einfügen

                        }
                        else 
                        */_manager.PlayerManager.Player.AddDamage(enemy.BaseDamage, 0);
                        intersected = true;
                    }
                    
                }

                if(_manager.VehicleManager.ActiveVehicle != null)
                {
                    if (_manager.VehicleManager.ActiveVehicle.Collider.Intersects(enemy.Collider))
                    {
                        
                            if (timer.Elapsed.TotalSeconds > timeCheck)
                            {
                                //_manager.VehicleManager.ActiveVehicle.AddDamage(enemy.BaseDamage, 0);
                                //Console.WriteLine("intersected = true");
                                intersected = true;
                            
                                _manager.VehicleManager.ActiveVehicle.AddDamage(enemy.BaseDamage, 0);
                                timer.Restart();
                            }

                            
                            enemy.Collider.UndoOverlap(_manager.VehicleManager.ActiveVehicle.Collider);
                        if (_model.MovementLogic.IsMoving())
                        {
                            enemy.GetDamage(100);

                        }
                    }
                }
            }
            
            foreach (Vehicle vehicle in _manager.VehicleManager.GetNearbyVehicles(_manager.PlayerManager.Player.Chunk, 2))
            {
                //Console.WriteLine(vehicle.Chunk);
                foreach (Enemy enemy in _manager.EnemyManager.GetEnemiesOfChunk(vehicle.Chunk))
                {
                    
                    if (vehicle.Collider.Intersects(enemy.Collider))
                    {
                        enemy.Collider.UndoOverlap(vehicle.Collider);
                       
                        if (_model.ManagerHolder.VehicleManager.ActiveVehicle != null)
                        {
                            

                            if (vehicle == _model.ManagerHolder.VehicleManager.ActiveVehicle)
                            {
                                enemy.GetDamage(100);
                                if (timer.Elapsed.TotalSeconds > timeCheck)
                                {
                                    //_manager.VehicleManager.ActiveVehicle.AddDamage(enemy.BaseDamage, 0);
                                    //Console.WriteLine("intersected = true");
                                    intersected = true;

                                    timer.Restart();
                                }
                            }
                        }
                    }
                }
            }
            
            
             
            return intersected;
            
        }

        private bool CheckPlayerWithBossCollisions()
        {
            bool intersected = false;
            foreach(Boss boss in _manager.BossManager.GetBossOfChunk(_manager.PlayerManager.Player.Chunk))
            {
                if (boss.Collider.Intersects(_manager.PlayerManager.Player.Collider))
                {
                    switch (boss.TypeOfBoss)
                    {
                        case Boss.BossType.Malos:
                            if (timer.Elapsed.Seconds >= 1)
                            {
                                _manager.PlayerManager.Player.AddDamage(40, 0);
                                intersected = true;
                                timer.Restart();
                            }
                            break;
                        case Boss.BossType.Gyorg:
                            if (timer.Elapsed.Seconds >= 1)
                            {
                                _manager.PlayerManager.Player.AddDamage(40, 0);
                                intersected = true;
                                timer.Restart();
                            }
                            break;
                        case Boss.BossType.Keller:
                            if (timer.Elapsed.Seconds >= 1)
                            {
                                _manager.PlayerManager.Player.AddDamage(30, 0);
                                intersected = true;
                                timer.Restart();
                            }
                            break;
                        case Boss.BossType.Saitama:
                            if (timer.Elapsed.Seconds >= 1)
                            {
                                _manager.PlayerManager.Player.AddDamage(50, 0);
                                intersected = true;
                                timer.Restart();
                            }
                            break;
                    }
                }
            }
            return intersected;
        }
        
        public void CheckEnemyOnBuildingCollisions(List<Box2D> collisionBoxes)
        {
            //EnemyListe geben, die derzeit Aktiv sind oder sowas
            foreach(Enemy enemy in _manager.EnemyManager.GetEnemiesOfChunk(currentChunkPlayer))
            {
                if (enemy.Type == Enemy.EnemyType.Bird) continue;
                foreach(Box2D colBox in collisionBoxes)
                {
                    UndoOverlap(enemy.Collider, colBox);
                }
            }
        }

        private void CheckCarCollisions(List<Box2D> collisionBoxes, ref InputData keyData)
        {
            //Console.WriteLine("In CarCollisions");
            Circle zwischen = _manager.VehicleManager.ActiveVehicle.Collider;
            foreach (Box2D colBox in collisionBoxes)
            {
                UndoOverlap(zwischen, colBox, ref keyData);
            }

            _manager.VehicleManager.ActiveVehicle.Position = new Vector2(zwischen.CenterX, zwischen.CenterY);
        }

        private void CheckShopItemCollisions()
        {
            _manager.ItemManager.NoMoney = false;
            Circle player = _manager.PlayerManager.Player.Collider;
            ShopItem itemToDelete = null;

            foreach (ShopItem shopItem in _manager.ItemManager.GetShopItemsOfChunk(_manager.PlayerManager.Player.Chunk))
            {
                if (shopItem.Collider.Intersects(_manager.PlayerManager.Player.Collider))
                {
                    //Console.WriteLine("Intersects " + shopItem.ShopItemType);
                    itemToDelete = shopItem;
                }
            }

            if (itemToDelete == null) return;
            
            if (_manager.ItemManager.ApplyShopItemEffect(itemToDelete))
            {
                _manager.ItemManager.ShopItems.Remove(itemToDelete.Chunk, itemToDelete);
                //_manager.ItemManager.DeleteShopItems();
            }
        }

        /// <summary>
        /// Fügt alle CollisionBoxes von Gebäuden aus den umliegenden Chunks in die Liste ein
        /// </summary>
        /// <param name="collisionBoxes"></param>
        /// <param name="currentChunkPlayer"></param>
        private void AddCollisionBoxes(List<Box2D> collisionBoxes, Vector2 currentChunkOfPlayer)
        {
            int chunk;
            List<Box2D> collisions;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (!_model.World.chunks.ContainsKey(new Vector2(currentChunkOfPlayer.X + i, currentChunkOfPlayer.Y + j)))
                    {
                        //Console.WriteLine("Collisions: Chunk nicht vorhanden");
                        continue;
                    }
                    chunk = _model.World.GetChunk(currentChunkOfPlayer.X + i, currentChunkOfPlayer.Y + j).TileId;
                    collisions = _manager.TileManager.tiles[chunk].collisionBoxes;
                    if (i == 0 && j == 0)
                    {
                        foreach (Box2D col in collisions)
                        {
                            collisionBoxes.Add(col);
                        }
                    }

                    if (i < 0)
                    {
                        foreach (Box2D col in collisions)
                        {
                            if (col.MaxX > 4)
                            {
                                collisionBoxes.Add(new Box2D(col.MinX + i * 5, col.MinY + j * 5, col.SizeX, col.SizeY));
                            }
                        }
                    }
                    if (i > 0)
                    {
                        foreach (Box2D col in collisions)
                        {
                            if (col.MinX < 1)
                            {
                                collisionBoxes.Add(new Box2D(col.MinX + i * 5, col.MinY + j * 5, col.SizeX, col.SizeY));
                            }
                        }
                    }
                    if (j < 0)
                    {
                        foreach (Box2D col in collisions)
                        {
                            if (col.MaxY > 4)
                            {
                                collisionBoxes.Add(new Box2D(col.MinX + i * 5, col.MinY + j * 5, col.SizeX, col.SizeY));
                            }
                        }
                    }
                    if (j > 0)
                    {
                        foreach (Box2D col in collisions)
                        {
                            if (col.MinY < 1)
                            {
                                collisionBoxes.Add(new Box2D(col.MinX + i * 5, col.MinY + j * 5, col.SizeX, col.SizeY));
                            }
                        }
                    }

                    if (_model.WorldManager.NoWorldChanceCol && chunk == 50)
                    {
                        collisionBoxes.Add(new Box2D(4f + i * 5, 2f + j * 5, 0.5f, 1f));
                    }
                }
            }
        }

        private bool UndoOverlap(Circle player, Box2D colBox, ref InputData keyData)
        {
            if (colBox.Intersects(player) == false)
            {
                //Console.WriteLine("Not Intersects");
                return false;
            }
            //Console.WriteLine("Intersects");
            
            

            int dirX = -1, dirY = -1;
            bool changedX = false, changedY = false;
            AxisDeltaDelta(colBox.MinX, colBox.MaxX, player.CenterX, ref dirX);
            AxisDeltaDelta(colBox.MinY, colBox.MaxY, player.CenterY, ref dirY);
            //Console.WriteLine($"dirX:{dirX} dirY:{dirY}");
            //Console.WriteLine(changedX + " " + changedY);
            //Console.WriteLine($"CollisionBox {colBox} Player {player} dirX: {dirX} dirY: {dirY}");


            if(dirX == 0 && changedY != true)   //Befindet sich links
            {
                //Console.WriteLine("changedY");
                //player.CenterX = colBox.MinX - _manager.PlayerManager.Player.Radius;
                player.CenterX = colBox.MinX - player.Radius;
                changedX = true;
            }
            if(dirX == 1 && changedY != true)   //Befindet sich rechts
            {
                //Console.WriteLine("changedY");
                player.CenterX = colBox.MaxX + player.Radius;
                changedX = true;
            }
            if(dirY == 0 && changedX != true)   //Befindet sich unterhalb
            {
                //Console.WriteLine("changedX");

                player.CenterY = colBox.MinY - player.Radius;
                changedY = true;
            }
            if(dirY == 1 && changedX != true)   //Befindet sich oberhalb
            {
                //Console.WriteLine("changedX");
                player.CenterY = colBox.MaxY + player.Radius;
                changedY = true;
            }
            if (dirX != -1 && dirY != -1)    //Befindet sich an einer Ecke
            {
                //Console.WriteLine("Ausnahme");
            }
            if(changedX == false && changedY == false)
            {
                if(keyData.yAxis < 0 && keyData.xAxis == 0) //kommt von oben
                {
                    player.CenterY = colBox.MaxY + player.Radius;
                }
                else if(keyData.yAxis > 0 && keyData.xAxis == 0) //kommt von unten
                {
                    player.CenterY = colBox.MinY - player.Radius;
                }
                else if(keyData.xAxis < 0 && keyData.yAxis == 0)   //kommt von rechts
                {
                    player.CenterX = colBox.MaxX + player.Radius;
                }
                else if(keyData.xAxis > 0 && keyData.yAxis == 0)   //kommt von links
                {
                    player.CenterX = colBox.MinX - player.Radius;
                }
                else
                {
                    LeUndoOverlapV2(player, colBox);

                    /*
                    if(colBox.MinX - _oldPositionofPlayer.X > 0 && _newPositionofPlayer.X - colBox.CenterX > 0) //Kommt von links
                    {
                        Console.WriteLine("Kommt von links");
                        player.CenterX = colBox.MinX - _manager.PlayerManager.Player.Radius;
                        return true;
                    }
                    if(_oldPositionofPlayer.X - colBox.MaxX > 0 && colBox.MaxX - _newPositionofPlayer.X > 0)    //Kommt von rechts
                    {
                        Console.WriteLine("Kommt von rechts");
                        player.CenterX = colBox.MaxX + _manager.PlayerManager.Player.Radius;
                        return true;
                    }
                    if(colBox.MinY - _oldPositionofPlayer.Y > 0 && _newPositionofPlayer.X - colBox.MinY > 0)    //kommt von unten
                    {
                        Console.WriteLine("Kommt von unten");
                        player.CenterY = colBox.MinY - _manager.PlayerManager.Player.Radius;
                        return true;
                    }
                    if(_oldPositionofPlayer.Y - colBox.MaxY > 0 && colBox.MaxY - _newPositionofPlayer.Y > 0)    //Kommt von oben
                    {
                        Console.WriteLine("Kommt von oben");
                        player.CenterY = colBox.MaxY + _manager.PlayerManager.Player.Radius;
                    }
                    */

                }

            }
            
            
            return true;
        }

        public void UndoOverlap(Circle enemy, Box2D colBox)
        {
            if (colBox.Intersects(enemy) == false)
            {
                //Console.WriteLine("Not Intersects");
                return;
            }
            //Console.WriteLine("Intersects");
            int dirX = -1, dirY = -1;
            bool changedX = false, changedY = false;
            AxisDeltaDelta(colBox.MinX, colBox.MaxX, enemy.CenterX, ref dirX);
            AxisDeltaDelta(colBox.MinY, colBox.MaxY, enemy.CenterY, ref dirY);
            //Console.WriteLine($"dirX:{dirX} dirY:{dirY}");
            //Console.WriteLine(changedX + " " + changedY);
            //Console.WriteLine($"CollisionBox {colBox} Player {player} dirX: {dirX} dirY: {dirY}");


            if (dirX == 0 && changedY != true)   //Befindet sich links
            {
                //Console.WriteLine("changedY");
                enemy.CenterX = colBox.MinX - _manager.PlayerManager.Player.Radius;
                changedX = true;
            }
            if (dirX == 1 && changedY != true)   //Befindet sich rechts
            {
                //Console.WriteLine("changedY");
                enemy.CenterX = colBox.MaxX + _manager.PlayerManager.Player.Radius;
                changedX = true;
            }
            if (dirY == 0 && changedX != true)   //Befindet sich unterhalb
            {
                //Console.WriteLine("changedX");

                enemy.CenterY = colBox.MinY - _manager.PlayerManager.Player.Radius;
                changedY = true;
            }
            if (dirY == 1 && changedX != true)   //Befindet sich oberhalb
            {
                //Console.WriteLine("changedX");
                enemy.CenterY = colBox.MaxY + _manager.PlayerManager.Player.Radius;
                changedY = true;
            }
        }

        private void AxisDeltaDelta(float min, float max, float center, ref int dir)
        {
            //Console.WriteLine($"min: {min} max: {max} player: {center}");
            var diffMin = min - center;
            //Console.WriteLine("difMin: " + diffMin);
            if (0 < diffMin) //left / lower case
            {
                dir = 0;
                return;
            }
            else
            {
                var diffMax = center - max;
                //Console.WriteLine("difMax: " + diffMax);
                if (0 < diffMax) //right / upper case
                {
                    dir = 1;
                    return;
                }
            }
            return;
        }

        private void LeUndoOverlapV2(Circle player, Box2D colBox)
        {
            if (!colBox.Intersects(player)) return;

            Vector2[] directions = new Vector2[]
            {
                new Vector2(colBox.MaxX - player.CenterX + player.Radius, 0), // push distance A in positive X-direction
				new Vector2(colBox.MinX - player.CenterX - player.Radius, 0), // push distance A in negative X-direction
				new Vector2(0, colBox.MaxY - player.CenterY + player.Radius), // push distance A in positive Y-direction
				new Vector2(0, colBox.MinY - player.CenterY - player.Radius)  // push distance A in negative Y-direction
            };

            float[] pushDistSqrd = new float[4];
            for (int i = 0; i < 4; ++i)
            {
                pushDistSqrd[i] = directions[i].LengthSquared();
            }
            //find minimal positive overlap amount
            int minId = 0;
            for (int i = 1; i < 4; ++i)
            {
                minId = pushDistSqrd[i] < pushDistSqrd[minId] ? i : minId;
            }

            player.CenterX += directions[minId].X;
            player.CenterY += directions[minId].Y;
        }


        public bool RaycastOnCircle(Vector2 originPoint, Vector2 endPoint, Circle circle)
        {
            var d = endPoint - originPoint;
            var f = originPoint - circle.Center;

            return false;
        }

        public bool RaycastOnLine(Vector2 rayStart, Vector2 rayEnd, Vector2 lineStart, Vector2 lineEnd)
        {
            Vector2 rayDelta = rayEnd - rayStart;
            Vector2 lineDelta = lineEnd - lineStart;

            



            return false;
        }
    }
}
