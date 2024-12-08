using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace BambuFramework.Audio
{
    [Serializable]
    public class AudioEvent
    {
        [ShowInInspector, ReadOnly, LabelText("Event Name")]

        [InlineProperty, LabelWidth(100)]

        [TextArea, LabelWidth(100)]
        public string description;
    }
}
