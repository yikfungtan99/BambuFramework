using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BambuFramework.UI
{
    public class InputHints : MenuScreen
    {
        [SerializeField] private InputPromptsContainer inputHintPrompts;
        [SerializeField] private VisualTreeAsset inputHint;

        protected override Button firstButton => null;

        protected int currentHintsIndex = -1;

        private List<TemplateContainer> existingInputHintPromptList = new List<TemplateContainer>();

        private void Start()
        {
            Show();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public override void Show(Player player = null, bool sortingOrder = true)
        {
            if (inputHint == null) return;
            if (Root == null) return;

            Show(0);
        }

        public void Show(int index)
        {
            if (currentHintsIndex == index) return;

            currentHintsIndex = index;

            if (currentHintsIndex == -1)
            {
                Bambu.Log($"return");
                Hide();
                return;
            }

            // Get the list of prompts for the given index
            var prompts = inputHintPrompts.InputPrompts[index];

            if (prompts == null)
            {
                Bambu.Log($"return");
                Hide();
                return;
            }

            // Ensure enough TemplateContainers exist in the pool
            for (int i = existingInputHintPromptList.Count; i < prompts.Length; i++)
            {
                // Create new TemplateContainers as needed and add to the pool
                TemplateContainer newTemplate = inputHint.Instantiate();
                existingInputHintPromptList.Add(newTemplate);

                // Add the new TemplateContainer to the UI hierarchy
                Root[0].Add(newTemplate);
            }

            // Update existing TemplateContainers with prompt data
            for (int i = 0; i < prompts.Length; i++)
            {
                TemplateContainer container = existingInputHintPromptList[i];
                InputPromptData prompt = prompts[i];

                container.Q<Label>().text = prompt.inputTitle;
                container.Q<VisualElement>("imgIcon").style.backgroundImage = prompt.inputTexture;
                container.style.display = DisplayStyle.Flex; // Make it visible
            }

            // Hide unused TemplateContainers
            for (int i = prompts.Length; i < existingInputHintPromptList.Count; i++)
            {
                existingInputHintPromptList[i].style.display = DisplayStyle.None; // Hide the excess containers
            }

            Root.visible = true;
        }

        protected override void UpdateMenu()
        {

        }

        public override void Hide(bool sortingOrder = false)
        {
            base.Hide(sortingOrder);
            currentHintsIndex = -1;
        }
    }
}
