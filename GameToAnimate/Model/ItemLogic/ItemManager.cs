using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenseless.Geometry;
using System.Numerics;
using Model.PawnLogic.EnemyLogic;
using Model.PawnLogic.PlayerLogic;
using System.Drawing;
using System.Diagnostics;
using Model.WorldLogic;
using Model.PawnLogic.ProjectileLogic;
using Model.PawnLogic;

namespace Model.ItemLogic
{
    public class ItemManager
    {
        public enum ItemType { Health, Waffe, Speed, Geld, Ammo}
        public enum ShopItemType { Heart, Ammo, AmmoMax, Resistance, DamageUp, Length}
        public bool NoMoney { get; internal set; }
        private MultiValueDictionary<Vector2, Item> _items = new MultiValueDictionary<Vector2, Item>();
        private MultiValueDictionary<Vector2, ShopItem> _shopItems = new MultiValueDictionary<Vector2, ShopItem>();
        public MultiValueDictionary<Vector2, Item> ItemList { get { return _items; } }
        public MultiValueDictionary<Vector2, ShopItem> ShopItems { get { return _shopItems; } }
        private Stopwatch timer = new Stopwatch();
        private Random ran = new Random();
        private Player _player;
        private MainModel _model;
        //private ProjectileManager _projectileManager;
        private bool collectedShopItem = false;
        private Stopwatch itemCollectTimer = new Stopwatch();

        private int[] upgradeCount = new int[(int) ShopItemType.Length];


        public ItemManager(Player player, MainModel model)
        {
            _player = player;
            _model = model;
            //_projectileManager = projectileManager;

            for(int i = 0; i < (int)ShopItemType.Length; i++)
            {
                upgradeCount[i] = 1;
            }
        }

        public void Update()
        {
            if(timer.Elapsed.TotalSeconds > 5)
            {
                _player.Speed = 5;
                timer.Stop();
                timer.Reset();
            }

            if(itemCollectTimer.Elapsed.Seconds > 2)
            {
                itemCollectTimer.Stop();
                collectedShopItem = false;
            }
        }

        public void DropItemEnemy(Pawn enemy)
        {
            //Console.WriteLine($"Bei DropItem: {enemy.Chunk}");
            double randomDrop = ran.NextDouble();

            Vector2 randomPosition = new Vector2(enemy.Collider.CenterX - 0.1f * (float)randomDrop, enemy.Collider.CenterY + 0.1f * (float)randomDrop);

            _items.Add(enemy.Chunk, new Item(ItemType.Geld, randomPosition, enemy.Chunk));

            randomDrop = ran.NextDouble();
            randomPosition = new Vector2(enemy.Collider.CenterX + 0.1f * (float)randomDrop, enemy.Collider.CenterY - 0.1f * (float)randomDrop);
            if (randomDrop > 0.7d)
            {
                _items.Add(enemy.Chunk, new Item(ItemType.Health, randomPosition, enemy.Chunk));
            }
            else if(randomDrop > 0.1f)
            {
                _items.Add(enemy.Chunk, new Item(ItemType.Speed, randomPosition, enemy.Chunk));
            }

            randomDrop = ran.NextDouble();
            if(randomDrop > 0.6f)
            {
                _items.Add(enemy.Chunk, new Item(ItemType.Ammo, randomPosition, enemy.Chunk));
            }

        }

        public void ShopItem()
        {
                _shopItems.Add(new Vector2(1, -1), new ShopItem(ShopItemType.DamageUp, new Vector2(2.5f, 2.5f), new Vector2(1, -1)));
                _shopItems.Add(new Vector2(-1, -1), new ShopItem(ShopItemType.Heart, new Vector2(2.5f, 2.5f), new Vector2(-1, -1)));
                _shopItems.Add(new Vector2(-1, 1), new ShopItem(ShopItemType.Ammo, new Vector2(2.5f, 2.5f), new Vector2(-1, 1)));
                _shopItems.Add(new Vector2(0, 1), new ShopItem(ShopItemType.AmmoMax, new Vector2(1f, 2.5f), new Vector2(0, 1)));
                _shopItems.Add(new Vector2(1, 1), new ShopItem(ShopItemType.Resistance, new Vector2(2.5f, 2.5f), new Vector2(1, 1)));
        }

        private void ShopItem(ShopItemType shopItemType)
        {
            switch (shopItemType)
            {
                case ShopItemType.DamageUp:
                    _shopItems.Add(new Vector2(1, -1), new ShopItem(ShopItemType.DamageUp, new Vector2(2.5f, 2.5f), new Vector2(1, -1)));
                    break;
                case ShopItemType.Heart:
                    _shopItems.Add(new Vector2(-1, -1), new ShopItem(ShopItemType.Heart, new Vector2(2.5f, 2.5f), new Vector2(-1, -1)));
                    break;
                case ShopItemType.Ammo:
                    _shopItems.Add(new Vector2(-1, 1), new ShopItem(ShopItemType.Ammo, new Vector2(2.5f, 2.5f), new Vector2(-1, 1)));
                    break;
                case ShopItemType.AmmoMax:
                    _shopItems.Add(new Vector2(0, 1), new ShopItem(ShopItemType.AmmoMax, new Vector2(1f, 2.5f), new Vector2(0, 1)));
                    break;
                case ShopItemType.Resistance:
                    _shopItems.Add(new Vector2(1, 1), new ShopItem(ShopItemType.Resistance, new Vector2(2.5f, 2.5f), new Vector2(1, 1)));
                    break;
            }
        }

        public void DeleteShopItems()
        {
            _shopItems.Clear();
        }

        public void DeleteItems()
        {
            _items.Clear();
        }

        public IEnumerable<Item> GetItemsOfChunk(Vector2 chunkPos)
        {
            return _items.Where(i => i.Key == chunkPos).SelectMany(i => i.Value);
        }

        public IEnumerable<ShopItem> GetShopItemsOfChunk(Vector2 chunkPos)
        {
            return _shopItems.Where(i => i.Key == chunkPos).SelectMany(i => i.Value);
        }

        public void ApplyItemEffect(ItemType itemType)
        {
            
            switch (itemType)
            {
                case ItemType.Health:
                    //Console.WriteLine("Health Plus");
                    if (_player.LifePoints <= _player.MaxLifePoints - 10)
                    {
                        _player.LifePoints += 10;
                    }
                    else
                    {
                        _player.LifePoints = _player.MaxLifePoints;
                    }
                    break;
                case ItemType.Speed:
                    //Console.WriteLine("Speed Plus");
                    if (_player.Speed < 8)
                        _player.Speed += 3;
                    timer.Restart();
                    break;
                case ItemType.Geld:
                    _player.Money += 10;
                    //Console.WriteLine($"Money: {_player.Money}");
                    break;
                case ItemType.Ammo:
                    uint ammoCount = (uint)(ran.NextDouble() * 30) + 10;
                    _model.WeaponManager.AddAmmo();
                    break;
                default:
                    return;
            }
        }

        public bool ApplyShopItemEffect(ShopItem shopItem)
        {
            NoMoney = false;

            if (collectedShopItem)
            {
                return false;
            }

            switch (shopItem.ShopItemType)
            {
                case ShopItemType.Heart:
                    if (_player.Money >= Math.Pow(2, upgradeCount[(int)ShopItemType.Heart]) * 100)
                    {
                        _player.Money -= (int)Math.Pow(2, upgradeCount[(int)ShopItemType.Heart]) * 100;
                        _player.MaxLifePoints += 20;
                        _player.LifePoints = _player.MaxLifePoints;
                        upgradeCount[(int)ShopItemType.Heart]++;
                        ShopItem(ShopItemType.Heart);
                        collectedShopItem = true;
                        itemCollectTimer.Restart();
                        return true;
                    }
                    else
                    {
                        NoMoney = true;
                        return false;
                    }
                case ShopItemType.Resistance:
                    if (_player.Money >= Math.Pow(2, upgradeCount[(int)ShopItemType.Resistance]) * 100)
                    {
                        _player.Armor += 5;
                        _player.Money -= (int)Math.Pow(2, upgradeCount[(int)ShopItemType.Resistance]) * 100;
                        upgradeCount[(int)ShopItemType.Resistance]++;
                        ShopItem(ShopItemType.Resistance);
                        collectedShopItem = true;
                        itemCollectTimer.Restart();
                        return true;
                    }
                    NoMoney = true;
                    return false;
                case ShopItemType.Ammo:
                    if (_player.Money >= Math.Pow(2, upgradeCount[(int)ShopItemType.Ammo]) * 100)
                    {
                        _model.WeaponManager.ResetAmmoCount();
                        _player.Money -= (int)Math.Pow(2, upgradeCount[(int)ShopItemType.Ammo]) * 100;
                        upgradeCount[(int)ShopItemType.Ammo]++;
                        ShopItem(ShopItemType.Ammo);
                        collectedShopItem = true;
                        itemCollectTimer.Restart();
                        return true;
                    }
                    NoMoney = true;
                    return false;
                case ShopItemType.AmmoMax:
                    if (_player.Money >= Math.Pow(2, upgradeCount[(int)ShopItemType.AmmoMax]) * 100)
                    {
                        _model.WeaponManager.IncreaseAmmoCount();
                        _player.Money -= (int)Math.Pow(2, upgradeCount[(int)ShopItemType.AmmoMax]) * 100;
                        upgradeCount[(int)ShopItemType.AmmoMax]++;
                        ShopItem(ShopItemType.AmmoMax);
                        collectedShopItem = true;
                        itemCollectTimer.Restart();
                        return true;
                    }
                    NoMoney = true;
                    return false;
                case ShopItemType.DamageUp:
                    if (_player.Money >= Math.Pow(2, upgradeCount[(int)ShopItemType.DamageUp]) * 100)
                    {
                        _player.BaseDamage += 20;
                        _player.Money -= (int)Math.Pow(2, upgradeCount[(int)ShopItemType.DamageUp]) * 100;
                        upgradeCount[(int)ShopItemType.DamageUp]++;
                        ShopItem(ShopItemType.DamageUp);
                        collectedShopItem = true;
                        itemCollectTimer.Restart();
                        return true;
                    }
                    NoMoney = true;
                    return false;
                default:
                    Console.WriteLine("Fehler, LEL");
                    return false;
            }
        }

        public int GetCurrentShopItemPrice(ShopItemType itemType)
        {
            return (int)Math.Pow(2, upgradeCount[(int)itemType]) * 100;
        }

        public int GetCurrentShopItemValue(ShopItemType itemType)
        {
            switch (itemType)
            {
                case ShopItemType.Heart:
                    return 20 * upgradeCount[(int)ShopItemType.Heart];
                case ShopItemType.Resistance:
                    return 5;
                case ShopItemType.DamageUp:
                    return 20;
            }
            return 0;
        }

        public Vector3 GetColorOfItem(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.Health:
                    return new Vector3(0f, 1f, 0f);
                case ItemType.Speed:
                    return new Vector3(0f, 0f, 1f);
                case ItemType.Geld:
                    return new Vector3(1f, 1f, 0f);
                default:
                    return new Vector3(0f, 0f, 0f);
            }
        }

        public Vector3 GetColorOfShopItem(ShopItemType itemType)
        {
            switch (itemType)
            {
                case ShopItemType.Heart:
                    return new Vector3(1f, 1f, 1f);
                default:
                    return new Vector3(0f, 0f, 0f);
            }
        }
    }
}
