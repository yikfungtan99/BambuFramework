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

        public void Init(UIManager uiManager)
        {
            this.uiManager = uiManager;
        }

        private void Update()
        {
            UpdateMenu();
        }

        protected abstract void UpdateMenu();

        public virtual void Show(bool sortingOrder = true)
        {
            BambuLogger.Log($"Showing: {gameObject.name}", ELogCategory.UI);

            Root.visible = true;
            Root.schedule.Execute(() => firstButton?.Focus()).StartingIn(1);

            if (sortingOrder) uiDocument.sortingOrder = 1;
        }

        public virtual void Hide(bool sortingOrder = true)
        {
            BambuLogger.Log($"Hiding: {gameObject.name}", ELogCategory.UI);

            Root.visible = false;
            if (sortingOrder) uiDocument.sortingOrder = -1;
        }
    }
}
