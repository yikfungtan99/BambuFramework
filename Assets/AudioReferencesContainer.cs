using System.Collections.Generic;
using UnityEngine;

namespace BambuFramework.Audio
{
    [CreateAssetMenu(fileName = "Audio Reference Container", menuName = "BAMBU/AUDIO/Audio Reference Container")]
    public class AudioReferencesContainer : ScriptableObject
    {
        public List<AudioReference> audioReferences = new List<AudioReference>();
    }
}
