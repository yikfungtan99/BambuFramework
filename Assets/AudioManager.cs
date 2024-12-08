using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

namespace BambuFramework.Audio
{
    public class AudioManager : SingletonBehaviour<AudioManager>
    {
        public AudioLibrary AudioContainer;

        private readonly Dictionary<EAudioChannel, Bus> audioBuses = new Dictionary<EAudioChannel, Bus>();

        // Dictionary to track event instances by the parent GameObject
        private readonly Dictionary<GameObject, EventInstance> loopingInstances = new Dictionary<GameObject, EventInstance>();

        protected override void Awake()
        {
            base.Awake();
            InitializeAudioBuses();
        }

        /// <summary>
        /// Initializes the FMOD buses corresponding to the audio channels.
        /// </summary>
        private void InitializeAudioBuses()
        {
            audioBuses[EAudioChannel.MASTER] = RuntimeManager.GetBus("bus:/");
            audioBuses[EAudioChannel.SFX] = RuntimeManager.GetBus("bus:/SFX");
            audioBuses[EAudioChannel.MUSIC] = RuntimeManager.GetBus("bus:/MUSIC");
        }

        /// <summary>
        /// Sets the volume of the specified audio channel.
        /// </summary>
        public void SetChannelVolume(EAudioChannel channel, float volume)
        {
            float normalizedVolume = Mathf.Clamp(volume / 100f, 0f, 1f);

            if (audioBuses.TryGetValue(channel, out var bus))
            {
                Bambu.Log($"{channel}: {volume}", Debugging.ELogCategory.AUDIO);
                bus.setVolume(normalizedVolume);
            }
            else
            {
                Debug.LogWarning($"Audio bus for channel '{channel}' not found.");
            }
        }

        /// <summary>
        /// Mutes or unmutes the specified audio channel.
        /// </summary>
        public void SetChannelMute(EAudioChannel channel, bool mute)
        {
            if (audioBuses.TryGetValue(channel, out var bus))
            {
                bus.setMute(mute);
            }
            else
            {
                Debug.LogWarning($"Audio bus for channel '{channel}' not found.");
            }
        }

        public static void PlayAudio(AudioReference audioRef)
        {
            PlayAudio(audioRef, Vector3.zero);
        }

        public static void PlayAudio(AudioReference audioRef, Vector3 position)
        {
            if (audioRef == null)
            {
                Debug.LogWarning($"Audio Event not found in registry.");
                return;
            }

            if (audioRef.isLooping)
            {
                Instance.PlayLoopingAudioInternal(audioRef, position);
            }
            else
            {
                RuntimeManager.PlayOneShot(audioRef.eventReference, position);
            }
        }

        private void PlayLoopingAudioInternal(AudioReference audioRef, Vector3 position)
        {
            if (loopingInstances.ContainsKey(gameObject))
            {
                Debug.LogWarning($"A looping sound is already playing on {gameObject.name}");
                return;
            }

            // Create an emitterInstance as a child of the parentObject
            var emitterInstance = new GameObject($"Emitter_{audioRef.EventName}");
            emitterInstance.transform.SetParent(gameObject.transform);
            emitterInstance.transform.position = position;

            // Create the FMOD instance
            var instance = RuntimeManager.CreateInstance(audioRef.eventReference);
            RuntimeManager.AttachInstanceToGameObject(instance, emitterInstance.transform);
            instance.start();

            // Store the event instance for later management
            loopingInstances[gameObject] = instance;
        }

        /// <summary>
        /// Stops a looping sound attached to the specified game object.
        /// </summary>
        public void StopLoopingAudio(GameObject parentObject)
        {
            if (loopingInstances.TryGetValue(parentObject, out var instance))
            {
                // Stop the sound and release the instance
                instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                instance.release();

                // Destroy the emitterInstance GameObject
                Destroy(parentObject);
                loopingInstances.Remove(parentObject);
            }
            else
            {
                Debug.LogWarning($"No looping sound found for {parentObject.name}");
            }
        }

        /// <summary>
        /// Gets the current volume of the specified audio channel.
        /// </summary>
        public float GetChannelVolume(EAudioChannel channel)
        {
            if (audioBuses.TryGetValue(channel, out var bus))
            {
                bus.getVolume(out float volume);
                return volume;
            }

            Debug.LogWarning($"Audio bus for channel '{channel}' not found.");
            return 0f;
        }

    }
}
