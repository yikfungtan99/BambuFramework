using Eflatun.SceneReference;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace BambuFramework.SceneManagement
{
    public class SceneManager : SingletonBehaviour<SceneManager>
    {
        [SerializeField] private SceneReference[] initialPermanentScenes;
        [SerializeField] private SceneReference[] initialSwappableScenes;

        private List<SceneReference> permanentSceneLayer = new List<SceneReference>();
        private List<SceneReference> swappableSceneLayer = new List<SceneReference>();

        private void Start()
        {
            AddPermanentScenes(initialPermanentScenes);
            SwapScenes(initialSwappableScenes);
        }

        public void AddPermanentScene(SceneReference sceneRef)
        {
            LoadAddressableScene(sceneRef, LoadSceneMode.Additive);
            permanentSceneLayer.Add(sceneRef);
        }

        public void AddPermanentScenes(SceneReference[] sceneRef)
        {
            for (int i = 0; i < sceneRef.Length; i++)
            {
                AddPermanentScene(sceneRef[i]);
            }
        }

        public async void SwapScene(SceneReference sceneRef)
        {
            await UnloadAllSwappable();

            AddSceneToSwappable(sceneRef);
        }

        public async void SwapScenes(SceneReference[] sceneRef)
        {
            await UnloadAllSwappable();

            for (int i = 0; i < sceneRef.Length; i++)
            {
                AddSceneToSwappable(sceneRef[i]);
            }
        }

        private void AddSceneToSwappable(SceneReference sceneRef)
        {
            LoadAddressableScene(sceneRef, LoadSceneMode.Additive);
            swappableSceneLayer.Add(sceneRef);
        }
        private async Task UnloadAllSwappable()
        {
            for (int i = 0; i < swappableSceneLayer.Count; i++)
            {
                await UnloadScene(swappableSceneLayer[i]);
            }
        }

        public void LoadAddressableScene(SceneReference sceneRef, LoadSceneMode sceneMode)
        {
            StartCoroutine(LoadAddressableInternal(sceneRef, sceneMode));
        }

        private IEnumerator LoadAddressableInternal(SceneReference sceneRef, LoadSceneMode sceneMode)
        {
            string path = sceneRef.Path;

            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(path, sceneMode);
            yield return handle;
        }

        public async Task UnloadScene(SceneReference sceneRef)
        {
            AsyncOperation unloadOperation = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneRef.Name);
            await WaitForAsyncOperation(unloadOperation);
        }

        // Helper method to await an AsyncOperation
        private Task WaitForAsyncOperation(AsyncOperation operation)
        {
            var tcs = new TaskCompletionSource<bool>();
            operation.completed += _ => tcs.SetResult(true);
            return tcs.Task;
        }
    }
}
