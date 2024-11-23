using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;

namespace BambuFramework.Audio
{
    [Serializable]
    public class AudioReference
    {
        [ValueDropdown(nameof(GetEventNames))]
        [Tooltip("Select an audio event from the registry.")]
        public string eventName;

        [Tooltip("Toggle if this audio should loop.")]
        public bool isLooping;

        [TextArea]
        [Tooltip("Optional description for this audio.")]
        public string description;

        /// <summary>
        /// Plays the selected audio at the object's position.
        /// </summary>
        public void Play(Transform transform)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                Debug.LogWarning("No audio event selected.");
                return;
            }

            Vector3 position = transform.position;

            if (isLooping)
            {
                AudioManager.Instance.PlayLoopingAudio(eventName, transform.gameObject);
            }
            else
            {
                AudioManager.Instance.PlayAudio(eventName, position);
            }
        }

        /// <summary>
        /// Populates the dropdown list with audio event names from the registry.
        /// </summary>
        private ValueDropdownList<string> GetEventNames()
        {
            var registry = AudioContainer.Instance;

            if (registry == null)
            {
                Debug.LogError("AudioRegistry not found in Resources.");
                return new ValueDropdownList<string>();
            }

            var eventNames = registry.audioEvents.Select(e => e.eventName).ToList();
            var dropdownList = new ValueDropdownList<string>();

            foreach (var name in eventNames)
            {
                dropdownList.Add(name, name);
            }

            return dropdownList;
        }
    }
}
