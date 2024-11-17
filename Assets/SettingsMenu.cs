using BambuFramework.UI;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace BambuFramework
{
    public class SettingsMenu : MenuScreen
    {
        protected override Button firstButton => null;

        private TabView tabView;
        private int tabCount;

        private void OnEnable()
        {
            tabView = Root.Q<TabView>("TabView"); // Make sure your TabView has the "TabView" name or update this accordingly

            if (tabView == null)
            {
                Bambu.Log("TabView not found in UI Document!");
                return;
            }

            tabCount = tabView.childCount;

            // Initialize the first tab to be selected
            tabView.selectedTabIndex = 0;
            SetFocusOnTab(tabView.selectedTabIndex);
            Hide();
        }

        protected override void UpdateMenu()
        {
            if (tabView == null) return;

            // Keyboard navigation
            if (Keyboard.current.eKey.wasPressedThisFrame) // E for next tab
            {
                NavigateTabs(1);
            }
            else if (Keyboard.current.qKey.wasPressedThisFrame) // Q for previous tab
            {
                NavigateTabs(-1);
            }

            // Gamepad navigation
            if (Gamepad.current != null)
            {
                if (Gamepad.current.dpad.right.wasPressedThisFrame) // D-Pad right for next tab
                {
                    NavigateTabs(1);
                }
                else if (Gamepad.current.dpad.left.wasPressedThisFrame) // D-Pad left for previous tab
                {
                    NavigateTabs(-1);
                }
            }
        }

        private void NavigateTabs(int direction)
        {
            // Update selected index and wrap around if necessary
            tabView.selectedTabIndex = (tabView.selectedTabIndex + direction + tabCount) % tabCount;
            SetFocusOnTab(tabView.selectedTabIndex);
        }

        private void SetFocusOnTab(int index)
        {
            // Highlight the currently selected tab header
            var selectedTab = tabView.Children().ElementAt(index) as VisualElement;
            selectedTab?.Focus();
        }
    }
}
