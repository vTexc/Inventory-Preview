using PoeHUD.Controllers;
using PoeHUD.Poe.RemoteMemoryObjects;
using SharpDX;
using System.Linq;

namespace Inventory_Preview
{

    public struct Info
    {
        /**
         * Plugin
         */
        public const string PluginName = "Inventory Preview";
        public static string pluginDirectory { get { return PoeHUD.Plugins.BasePlugin.API.PluginsDebug.First(plugin => plugin.PluginName.Equals(PluginName)).PluginDirectory; } }

        /**
         * Window
         */
        public static RectangleF windowRect { get { return GameController.Instance.Window.GetWindowRectangle(); } }

        /**
         * Player
         */
        public static ServerInventory playerMainInventory { get { return GameController.Instance.Game.IngameState.ServerData.GetPlayerInventoryByType(InventoryTypeE.Main); } }

        /**
         * UI
         */
        public static bool isLeftPanelVisible { get { return GameController.Instance.Game.IngameState.IngameUi.OpenLeftPanel.IsVisible; } }
        public static bool isRightPanelVisible { get { return GameController.Instance.Game.IngameState.IngameUi.OpenRightPanel.IsVisible; } }

        public static bool isInventoryVisible { get { return GameController.Instance.Game.IngameState.IngameUi.InventoryPanel.IsVisible; } }

        public static bool isTreePanelVisible { get { return GameController.Instance.Game.IngameState.IngameUi.TreePanel.IsVisible; } }
        public static bool isAtlasPanelVisible { get { return GameController.Instance.Game.IngameState.IngameUi.AtlasPanel.IsVisible; } }

        public static bool isAnyFullScreenPanelVisible { get { return isTreePanelVisible || isAtlasPanelVisible; } }
        public static bool isAnySidePanelVisible { get { return isLeftPanelVisible || isRightPanelVisible; } }
    }
}
