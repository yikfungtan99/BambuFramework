using BambuFramework.Audio;
using BambuFramework.UI;
using UnityEngine;

namespace BambuFramework
{
    public class AudioUIManager : SingletonBehaviour<AudioUIManager>
    {
        private UIManager uiManager;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            uiManager = UIManager.Instance;

            uiManager.OnSelect += OnSelect;
            uiManager.OnSubmit += OnSubmit;
            uiManager.OnCancel += OnCancel;
        }

        private void OnDestroy()
        {
            if (uiManager == null) return;

            uiManager.OnSelect -= OnSelect;
            uiManager.OnSubmit -= OnSubmit;
            uiManager.OnCancel -= OnCancel;
        }

        private void OnSelect()
        {
            Debug.Log("Play Select");
            AudioLibrary.Instance.UISelect.Play();
        }

        private void OnSubmit()
        {
            Debug.Log("Play Submit");
            AudioLibrary.Instance.UISubmit.Play();
        }

        private void OnCancel()
        {
            Debug.Log("Play Cancel");
            AudioLibrary.Instance.UICancel.Play();
        }
    }
}
