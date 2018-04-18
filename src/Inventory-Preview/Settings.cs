using PoeHUD.Hud.Settings;
using PoeHUD.Plugins;
using SharpDX;


namespace Inventory_Preview
{
    public class Settings : SettingsBase
    {
        public Settings()
        {
            Enable = true;
            ItemRelated = true;
            UseImages = false;

            CellSize = new RangeNode<int>(30, 1, 100);
            CellPadding = new RangeNode<int>(1, 0, 10);
            PosX = new RangeNode<float>(28.0f, 0.0f, 100.0f);
            PosY = new RangeNode<float>(83.0f, 0.0f, 100.0f);

            CellUsedColor = new Color(255, 0, 0, 255);
            CellFreeColor = new Color(255, 255, 255, 255);
        }

        [Menu("Pos X %")]
        public RangeNode<float> PosX { get; set; }

        [Menu("Pos Y %")]
        public RangeNode<float> PosY { get; set; }

        [Menu("Use Item Related Color (Colors in settings will be ignored)")]
        public ToggleNode ItemRelated { get; set; }

        [Menu("Cell Used Color")]
        public ColorNode CellUsedColor { get; set; }

        [Menu("Cell Free Color")]
        public ColorNode CellFreeColor { get; set; }

        [Menu("Cell Size")]
        public RangeNode<int> CellSize { get; set; }

        [Menu("Cell Padding")]
        public RangeNode<int> CellPadding { get; set; }

        [Menu("Use Images")]
        public ToggleNode UseImages { get; set; }
    }
}
