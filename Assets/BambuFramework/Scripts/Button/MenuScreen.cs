using BambuFramework.Debug;
using UnityEngine;
using UnityEngine.UIElements;

namespace BambuFramework.UI
{
    public abstract class MenuScreen : MonoBehaviour
    {
        protected abstract Button firstButton { get; }
        protected Button lastFocusedButton;
        private VisualElement root;
        protected VisualElement Root
        {
            get
            {
                if (root == null)
                {
                    root = GetComponent<UIDocument>().rootVisualElement;
                }

                return root;
            }
        }

        protected bool isVisible => Root.visible;

        public void Show()
        {
            BambuLogger.Log($"Showing: {nameof(gameObject)}", ELogCategory.UI);
            Root.visible = true;
            Root.schedule.Execute(() => firstButton.Focus()).StartingIn(1);
        }

        public void Hide()
        {
            BambuLogger.Log($"Hiding: {nameof(gameObject)}", ELogCategory.UI);
            Root.visible = false;
        }
    }
}
