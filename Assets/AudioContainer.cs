using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BambuFramework.Audio
{
    [Serializable]
    public class AudioEvent
    {
        public string eventName;
        public EAudioChannel channel;
        public EventReference eventReference;

        [TextArea]
        public string description;
    }

    [CreateAssetMenu(fileName = "Audio Container", menuName = "BAMBU/AUDIO/Audio Container")]
    public class AudioContainer : SingletonSerializedScriptableObject<AudioContainer>
    {
        [Tooltip("List of all audio events in the project")]
        public List<AudioEvent> audioEvents = new List<AudioEvent>();

        /// <summary>
        /// Finds an audio event by name.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        /// <returns>The associated EventReference, or null if not found.</returns>
        public EventReference? GetEventReference(string name)
        {
            var audioEvent = audioEvents.Find(e => e.eventName == name);
            return audioEvent.eventReference;
        }

        public AudioEvent GetAudioEvent(string eventName)
        {
            return audioEvents.Find(e => e.eventName == eventName);
        }
    }
}
