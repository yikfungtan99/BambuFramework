using BambuFramework.Audio;
using UnityEngine;

namespace BambuFramework
{
    public class AudioEmitter : MonoBehaviour
    {
        [SerializeField] private AudioReference audioRef;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            audioRef.Play();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
