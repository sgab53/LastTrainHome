using Cysharp.Threading.Tasks;
using LTH.Core.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LTH.Core
{
    [DisallowMultipleComponent]
    public class GameBootstrapper : MonoBehaviour
    {
        [SerializeField] private LoadingScreenUIController _loadingScreen;
        [SerializeField] private SceneAsset[] _startupScenes;

        private AsyncOperation[] _operations;
        private int _sceneIndex;

        private void Start()
        {
            _operations = new AsyncOperation[_startupScenes.Length];
            _sceneIndex = SceneManager.GetActiveScene().buildIndex;

            _loadingScreen.ShowEvent += StartLoadingScenes;
            _loadingScreen.HideEvent += StartActivatingScenes;

            ShowLoadingScreen().Forget();
        }

        private void StartLoadingScenes()
        {
            LoadScenes().Forget();
        }

        private async UniTaskVoid ShowLoadingScreen()
        {
            await UniTask.Delay(500, true);
            _loadingScreen.Show();
        }

        private async UniTaskVoid LoadScenes()
        {
            await UniTask.NextFrame();

            for (var i = 0; i < _startupScenes.Length; ++i)
            {
                _operations[i] =
                    SceneManager.LoadSceneAsync(_startupScenes[i].name, LoadSceneMode.Additive);
            }

            var loading = 0f;

            _loadingScreen.UpdateLoading(0);
            await UniTask.NextFrame();

            while (loading < 0.9f)
            {
                var progress = 0f;
                foreach (var operation in _operations)
                    progress += operation.progress;

                loading = progress / _operations.Length;

                _loadingScreen.UpdateLoading(loading);
                await UniTask.Yield();
            }

            loading = 1f;

            _loadingScreen.UpdateLoading(loading);
            await UniTask.Yield();
            _loadingScreen.Hide();
        }

        private void StartActivatingScenes()
        {
            ActivateScenes().Forget();
        }

        private async UniTaskVoid ActivateScenes()
        {
            foreach (var operation in _operations)
                operation.allowSceneActivation = true;

            await UniTask.Delay(500, true);

            await SceneManager.UnloadSceneAsync(_sceneIndex);
        }
    }
}
