using Sirenix.OdinInspector;
using UnityEngine;

namespace BambuFramework.Audio
{
    [CreateAssetMenu(fileName = "Audio Library", menuName = "BAMBU/AUDIO/Audio Library")]
    public class AudioLibrary : SingletonSerializedScriptableObject<AudioLibrary>
    {
        // Header for Main Menu Music
        [FoldoutGroup("Music")]
        [ListDrawerSettings(ListElementLabelName = "EventName")]
        public AudioReference[] MainMenuMusic;

        // Header for UI Sounds
        [FoldoutGroup("UI")]
        public AudioReference UISelect;
        [FoldoutGroup("UI")]
        public AudioReference UISubmit;
        [FoldoutGroup("UI")]
        public AudioReference UICancel;
    }
}
