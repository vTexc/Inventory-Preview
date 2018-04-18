using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PoeHUD.Plugins;
using SharpDX;
using PoeHUD.Framework.Helpers;

namespace Inventory_Preview
{
    public class Core : BaseSettingsPlugin<Settings>
    {
        public Core()
        {
            PluginName = Info.PluginName;
        }

        private const int CELLS_GRID_X = 12;
        private const int CELLS_GRID_Y = 5;

        private int[] cellsInfo;
        private SharpDX.Size2F defaultCellSize { get { return GetDrawableCellSize(1, 1); } }
        private Dictionary<ItemType, Color> itemsColors = new Dictionary<ItemType, Color>
        {
            { ItemType.NORMAL, Color.White },
            { ItemType.MAGIC, new Color(136, 136, 255, 255) },
            { ItemType.RARE, new Color(255, 255, 119, 255) },
            { ItemType.UNIQUE, new Color(175, 96, 37, 255) },
            { ItemType.CURRENCY, new Color(242, 196, 98, 255) },
            { ItemType.QUEST, new Color(74, 230, 58, 255) },
            { ItemType.GEM, new Color(26, 162, 155, 255) },
            { ItemType.NONE, Color.White }
        };
        
        private Size2F drawRect { get { return new Size2F(
                                                          (Settings.CellSize * CELLS_GRID_X - Settings.CellPadding * (CELLS_GRID_X - 1)),
                                                          (Settings.CellSize * CELLS_GRID_Y - Settings.CellPadding * (CELLS_GRID_Y - 1))); } }

        private Vector2 drawPoint { get { return new Vector2(
                                                            (Info.windowRect.Width - drawRect.Width) * Settings.PosX * .01f,
                                                            (Info.windowRect.Height - drawRect.Height) * Settings.PosY * .01f); } }

        public override void Initialise()
        {
            cellsInfo = new int[CELLS_GRID_X * CELLS_GRID_Y];
        }

        public override void OnClose()
        {
            base.OnClose();
        }

        public override void OnPluginDestroyForHotReload()
        {
            base.OnPluginDestroyForHotReload();
        }

        public override void Render()
        {
            if (Info.isAnyFullScreenPanelVisible || Info.isAnySidePanelVisible) return;

            Array.Clear(cellsInfo, 0, cellsInfo.Length);
            DrawNonEmptyCells();
            DrawEmptyCells();
        }

        private void DrawNonEmptyCells()
        {
            foreach(var item in Info.playerMainInventory.InventorySlotItems)
            {
                DrawCell(new CellData(item));
            }
        }

        private void DrawEmptyCells()
        {
            RectangleF rect = new RectangleF(0, 0, defaultCellSize.Width, defaultCellSize.Height);
            for (int i = 0; i < cellsInfo.Length; i++)
            {
                if (cellsInfo[i] <= 0)
                {
                    var x = i % CELLS_GRID_X;
                    var y = i / CELLS_GRID_X;

                    Vector2 pos = drawPoint.Translate(Settings.CellSize * x, Settings.CellSize * y);
                    rect.X = pos.X;
                    rect.Y = pos.Y;
                    DrawDefaultCell(rect, Settings.ItemRelated ? SharpDX.Color.White : Settings.CellFreeColor.Value);
                }
            }
        }

        private void DrawCell(CellData cell)
        {
            Vector2 cellPos = drawPoint.Translate(
                                                  Settings.CellSize * cell.ItemPosX,
                                                  Settings.CellSize * cell.ItemPosY);
            Size2F cellSize = GetDrawableCellSize(cell.ItemCellWidth, cell.ItemCellHeight);

            RectangleF cellRect = new RectangleF(
                                                  cellPos.X,
                                                  cellPos.Y,
                                                  cellSize.Width,
                                                  cellSize.Height);

            DrawDefaultCell(cellRect, Settings.ItemRelated ? itemsColors[cell.Type] : Settings.CellUsedColor.Value);

            if(Settings.UseImages)
            {
                if (!string.IsNullOrEmpty(cell.MetaData))
                {
                    var getImg = ImageCache.GetImage(cell.MetaData);

                    if (getImg.bIsDownloaded)
                    {
                        Graphics.DrawPluginImage(getImg.FilePath, cellRect);
                    }
                }
            } else if(!cell.IsStackable)
            {
                int textSize = (int)Math.Min(cellSize.Width, cellSize.Height);
                Graphics.DrawText(cell.Type.ToString()[0].ToString(), textSize, cellRect.Center - new Vector2(0, textSize/2), Color.White, SharpDX.Direct3D9.FontDrawFlags.Center);
            }

            if(cell.IsStackable)
            {
                int textSize = (int)(Settings.CellSize * 0.6f);
                var textPos = cellRect.TopLeft;
                var textColor = cell.CurrentStackSize == cell.MaxStackSize ? new Color(0, 186, 154) : Color.White;

                Graphics.DrawText(cell.CurrentStackSize.ToString(), textSize, textPos, textColor, SharpDX.Direct3D9.FontDrawFlags.Left);
            }

            for(int i = cell.ItemPosX; i < cell.ItemCellSizeX ;  i++)
            {
                for (int j = cell.ItemPosY; j < cell.ItemCellSizeY; j++)
                {
                    cellsInfo.SetValue(1, (j * CELLS_GRID_X) + i);
                }
            }
        }

        private Size2F GetDrawableCellSize(int x, int y)
        {
            return new Size2F(
                              (Settings.CellSize * x) - Settings.CellPadding,
                              (Settings.CellSize * y) - Settings.CellPadding);
        }

        private void DrawDefaultCell(RectangleF rect, Color color)
        {
            Graphics.DrawImage("cell.png", rect, color);
        }

        private int GetPaddedValue(int value)
        {
            int padding = Settings.CellPadding;
            return (4 - (value * sizeof(int)) % 4) % 4;
        }
    }
}
