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
        private readonly Dictionary<string, EventInstance> activeOneShots = new Dictionary<string, EventInstance>();

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
            audioBuses[EAudioChannel.UI] = RuntimeManager.GetBus("bus:/UI");
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

        public static void PlayAudioOneShot(AudioReference audioRef, Vector3 position)
        {
            if (audioRef == null)
            {
                Debug.LogWarning($"Audio Event not found in registry.");
                return;
            }

            string eventKey = audioRef.eventReference.ToString();

            // Check if the event is already playing
            if (Instance.activeOneShots.TryGetValue(eventKey, out EventInstance existingInstance))
            {
                existingInstance.getPlaybackState(out PLAYBACK_STATE playbackState);
                if (playbackState != PLAYBACK_STATE.STOPPED)
                {
                    Debug.Log($"Audio event '{eventKey}' is still playing. Skipping new playback.");
                    return;
                }

                // Remove stopped instances from the active dictionary
                Instance.activeOneShots.Remove(eventKey);
            }

            // Create a new event instance and play it
            EventInstance newInstance = RuntimeManager.CreateInstance(audioRef.eventReference);
            newInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
            newInstance.start();

            // Add to activeOneShots dictionary
            Instance.activeOneShots[eventKey] = newInstance;

            // Release the instance when it's done
            newInstance.release();
        }

        public static EventInstance PlayAudio(AudioReference audioRef, Vector3 position)
        {
            EventInstance e = RuntimeManager.CreateInstance(audioRef.eventReference);

            //// Convert Unity's position to FMOD's 3D attributes
            //FMOD.ATTRIBUTES_3D attributes = RuntimeUtils.To3DAttributes(position);

            //// Set the 3D attributes for the EventInstance
            //e.set3DAttributes(attributes);

            //e.setProperty(FMOD.Studio.EVENT_PROPERTY.MAX, -1);

            e.start();

            return e;
        }

        public static void StopAudio(EventInstance eventInstance, FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
        {
            eventInstance.stop(stopMode);
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
