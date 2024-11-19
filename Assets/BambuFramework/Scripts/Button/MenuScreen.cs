using BambuFramework.Debug;
using UnityEngine;
using UnityEngine.UIElements;

namespace BambuFramework.UI
{
    [RequireComponent(typeof(UIDocument))]
    public abstract class MenuScreen : MonoBehaviour
    {
        private UIDocument uiDocument;
        protected UIDocument UiDocument
        {
            get
            {
                if (uiDocument == null)
                {
                    uiDocument = GetComponent<UIDocument>();
                }

                return uiDocument;
            }
        }

        protected UIManager uiManager;

        public GameObject GameObject { get => uiDocument.gameObject; }

        protected abstract Button firstButton { get; }
        protected Button lastFocusedButton;
        private VisualElement root;
        protected VisualElement Root
        {
            get
            {
                if (root == null)
                {
                    root = UiDocument.rootVisualElement;
                }

                return root;
            }
        }

        protected bool isVisible => Root.visible;
        protected bool isActivated => isVisible;

        protected Player initiatedPlayer;

        public void Init(UIManager uiManager)
        {
            this.uiManager = uiManager;
        }

        private void Update()
        {
            if (!isActivated) return;

            UpdateMenu();
        }

        protected abstract void UpdateMenu();

        public virtual void Show(Player player = null, bool sortingOrder = true)
        {
            BambuLogger.Log($"Showing: {gameObject.name}", ELogCategory.UI);

            Root.visible = true;
            Root.schedule.Execute(() => firstButton?.Focus()).StartingIn(1);

            if (sortingOrder) uiDocument.sortingOrder = 1;

            initiatedPlayer = player;
        }

        public virtual void Hide(bool sortingOrder = true)
        {
            BambuLogger.Log($"Hiding: {gameObject.name}", ELogCategory.UI);

            Root.visible = false;
            if (sortingOrder) uiDocument.sortingOrder = -1;
        }
    }
}
