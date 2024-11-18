using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace BambuFramework.UI
{
    [CreateAssetMenu(fileName = "Input Prompts Container", menuName = "BAMBU/UI/Input Prompts Container")]
    public class InputPromptsContainer : SerializedScriptableObject
    {
        public Dictionary<int, InputPromptData[]> InputPrompts = new Dictionary<int, InputPromptData[]>();
    }
}
