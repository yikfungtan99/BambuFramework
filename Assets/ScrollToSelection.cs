using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace BambuFramework
{
    public class ScrollToSelection : MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollRect;  // Reference to the ScrollRect component
        [SerializeField] private RectTransform content;  // The content holder of the ScrollView
        [SerializeField] private RectTransform selectedItem;  // The RectTransform of the selected item

        [Button]
        public void ScrollToSelectedItem()
        {
            // Get the position of the selected item relative to the content area
            Vector2 targetPosition = selectedItem.localPosition;

            // Scroll to the target position
            // Convert local position to normalized position (0 to 1) in ScrollRect space
            Vector2 contentSize = content.rect.size;
            Vector2 scrollViewSize = scrollRect.GetComponent<RectTransform>().sizeDelta;
            Vector2 normalizedPosition = new Vector2(
                Mathf.Clamp01(targetPosition.x / (contentSize.x - scrollViewSize.x)),
                Mathf.Clamp01(targetPosition.y / (contentSize.y - scrollViewSize.y))
            );

            // Use Lerp for smooth scrolling or set the normalized position directly
            scrollRect.horizontalNormalizedPosition = normalizedPosition.x;
            scrollRect.verticalNormalizedPosition = normalizedPosition.y;
        }
    }
}
