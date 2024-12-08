using FMODUnity;
using System;
using UnityEngine;

namespace BambuFramework.Audio
{
    [Serializable]
    public class AudioReference
    {
        [Tooltip("Select an audio event from the registry.")]
        public string EventName
        {
            get
            {
                if (eventReference.IsNull)
                {
                    return "Unnamed Event";
                }

                // Remove the "event:/" prefix if it exists
                return eventReference.Path.StartsWith("event:/")
                    ? eventReference.Path.Substring("event:/".Length)
                    : eventReference.Path;
            }
        }

        public EventReference eventReference;

        [Tooltip("Toggle if this audio should loop.")]
        public bool isLooping;

        public void Play()
        {
            Play(Vector3.zero);
        }

        /// <summary>
        /// Plays the selected audio at the object's position.
        /// </summary>
        public void Play(Transform transform)
        {
            Play(transform.position);
        }

        private void Play(Vector3 position)
        {
            AudioManager.PlayAudio(this, position);
        }
    }
}
