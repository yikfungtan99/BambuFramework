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
        [ValidateInput(nameof(IsValidEventName), "The selected event name is not valid.")]
        [Tooltip("Select an audio event from the registry.")]
        public string eventName;

        [Tooltip("Toggle if this audio should loop.")]
        public bool isLooping;

        [TextArea]
        [Tooltip("Optional description for this audio.")]
        public string description;

        public void Play()
        {
            Play(Vector3.zero);
        }

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

            Play(transform.position);
        }

        private void Play(Vector3 position)
        {
            if (isLooping)
            {
                AudioManager.Instance.PlayLoopingAudio(eventName);
            }
            else
            {
                AudioManager.Instance.PlayAudio(eventName, position);
            }
        }

        /// <summary>
        /// Populates the dropDown list with audio event names from the registry.
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

        /// <summary>
        /// Validates whether the selected event name exists in the AudioContainer.
        /// </summary>
        private bool IsValidEventName(string name)
        {
            var registry = AudioContainer.Instance;

            if (registry == null)
            {
                Debug.LogError("AudioRegistry not found in Resources.");
                return false;
            }

            return registry.audioEvents.Any(e => e.eventName == name);
        }
    }
}
