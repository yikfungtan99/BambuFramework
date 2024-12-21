using UnityEngine;
using UnityEngine.UIElements;

namespace BambuFramework.UI
{
    [CreateAssetMenu(fileName = "UI Templates Container", menuName = "BAMBU/UI/UI Templates Container")]
    public class UITemplatesContainer : SingletonSerializedScriptableObject<UITemplatesContainer>
    {
        public VisualTreeAsset PopupWindow;
        public VisualTreeAsset PopupOptionButton;
    }
}
