﻿using FMOD.Studio;
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
        private EventInstance currentEventInstance;

        public void Play()
        {
            Play(Vector3.zero);
        }

        public void Play(Transform transform)
        {
            Play(transform.position);
        }

        /// <summary>
        /// Plays the selected audio at the object's position.
        /// </summary>
        public void Play(Vector3 position)
        {
            AudioManager.PlayAudioOneShot(this, position);
        }
        public void PlayAsInstance()
        {
            currentEventInstance = AudioManager.PlayAudio(this, Vector3.zero);
        }

        public void PlayAsInstance(Vector3 position)
        {
            currentEventInstance = AudioManager.PlayAudio(this, position);
        }

        public void Stop()
        {
            AudioManager.StopAudio(currentEventInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }
}
