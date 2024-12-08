using BambuFramework.Debugging;
using System.Collections.Generic;
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

        private List<Button> buttons = new List<Button>();

        public void Init(UIManager uiManager)
        {
            this.uiManager = uiManager;

            buttons = Root.Query<Button>().ToList();

            foreach (Button b in buttons)
            {
                b.RegisterCallback<FocusEvent>(Focus);
                b.RegisterCallback<BlurEvent>(Blur);
                b.clicked += Click;
            }
        }

        private void Update()
        {
            if (!isActivated) return;

            UpdateMenu();
        }

        protected abstract void UpdateMenu();

        public virtual void Show(Player player, bool sortingOrder = true)
        {
            BambuLogger.Log($"Showing: {gameObject.name}", ELogCategory.UI);

            Root.visible = true;

            if (sortingOrder) uiDocument.sortingOrder = 1;

            initiatedPlayer = player;
        }

        public virtual void Hide(bool sortingOrder = true)
        {
            BambuLogger.Log($"Hiding: {gameObject.name}", ELogCategory.UI);

            Root.visible = false;
            if (sortingOrder) uiDocument.sortingOrder = -1;
        }

        protected virtual void Cancel()
        {

        }

        protected virtual void Blur(BlurEvent evt)
        {
        }

        protected virtual void Focus(FocusEvent evt)
        {
            uiManager.Select();
        }

        private void Click()
        {
            uiManager.Submit();
        }
    }
}
