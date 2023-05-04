using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Model.PawnLogic;
using static Model.ItemLogic.ItemManager;
using Model.WorldLogic;

namespace Model.ItemLogic
{
    public class Item : Pawn
    {
        private ItemType _itemtype;
        public ItemType ItemType { get { return _itemtype; } }
        public Item(ItemType itemType, Vector2 position, Vector2 chunk) : base(position, chunk, 0.05f, 2f, 1)
        {
            //Console.WriteLine($"Bei NewItem: {pawn.Chunk}");
            _itemtype = itemType;
        }
    }

    public class ShopItem : Pawn
    {
        private ShopItemType _shopItemType;
        public ShopItemType ShopItemType{ get { return _shopItemType;} }
        public ShopItem(ShopItemType shopItemType, Vector2 position, Vector2 chunk) : base(position, chunk, 0.4f, 0, 1)
        {
            //Console.WriteLine($"Bei NewItem: {pawn.Chunk}");
            _shopItemType = shopItemType;
        }
    }
}
