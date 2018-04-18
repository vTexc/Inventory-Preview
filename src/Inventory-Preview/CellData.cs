using PoeHUD.Models.Enums;
using PoeHUD.Poe.Components;
using PoeHUD.Poe.RemoteMemoryObjects;

namespace Inventory_Preview
{
    public enum ItemType {
        NORMAL,
        MAGIC,
        RARE,
        UNIQUE,
        CURRENCY,
        QUEST,
        GEM,
        NONE
    }

    public struct CellData
    {
        public bool IsUsable;
        public bool IsQuestItem;
        public bool IsStackable;
        public bool IsModable;
        public bool IsSkillGem;

        public int CurrentStackSize;
        public int MaxStackSize;
        public ItemRarity ItemRarity;
        public int ItemPosX;
        public int ItemPosY;
        public int ItemCellWidth;
        public int ItemCellHeight;
        public ItemType Type;
        public int ItemCellSizeX { get { return ItemPosX + ItemCellWidth; } }
        public int ItemCellSizeY { get { return ItemPosY + ItemCellHeight; } }
        public string MetaData;

        public CellData(ServerInventory.InventSlotItem item)
        {
            var itemBase = item.Item.GetComponent<Base>();
            var itemStack = item.Item.GetComponent<Stack>();
            var itemRender = item.Item.GetComponent<RenderItem>();
            var itemMods = item.Item.GetComponent<Mods>();

            long x = 1234;
            IsUsable = item.Item.GetComponents().TryGetValue("Usable", out x);
            IsQuestItem = item.Item.GetComponents().TryGetValue("Quest", out x);
            IsSkillGem = item.Item.GetComponents().TryGetValue("SkillGem", out x);

            IsStackable = (itemStack != null && itemStack.Info != null);
            IsModable = (itemMods != null && itemMods.ItemRarity != null);

            ItemRarity = IsModable ? itemMods.ItemRarity : ItemRarity.Normal;
            CurrentStackSize = IsStackable ? itemStack.Size : 0;
            MaxStackSize = IsStackable ? itemStack.Info.MaxStackSize : 0;
            ItemPosX = item.PosX;
            ItemPosY = item.PosY;
            ItemCellWidth = itemBase.ItemCellsSizeX <= 0 ? 1 : itemBase.ItemCellsSizeX;
            ItemCellHeight = itemBase.ItemCellsSizeY <= 0 ? 1 : itemBase.ItemCellsSizeY; ;
            MetaData = itemRender.ResourcePath;

            Type = ItemType.NONE;
            if (IsQuestItem)
            {
                Type = ItemType.QUEST;
            }
            else if(IsStackable)
            {
                Type = ItemType.CURRENCY;
            }
            else if (IsSkillGem)
            {
                Type = ItemType.GEM;
            }
            else if(IsModable)
            {
                switch(ItemRarity)
                {
                    case ItemRarity.Normal:
                        Type = ItemType.NORMAL;
                        break;
                    case ItemRarity.Magic:
                        Type = ItemType.MAGIC;
                        break;
                    case ItemRarity.Rare:
                        Type = ItemType.RARE;
                        break;
                    case ItemRarity.Unique:
                        Type = ItemType.UNIQUE;
                        break;
                    default: break;
                }
            }
        }
    }
}
