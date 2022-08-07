using SunsetSystems.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using SunsetSystems.Utils.Threading;

namespace SunsetSystems.Bootstraper
{
    public class SunsetBootstraper : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField]
        private List<SceneAsset> bootstrapScenes = new();

        protected async void Start()
        {
            List<string> bootstrapScenePaths = new();
            bootstrapScenes.ForEach(sc => bootstrapScenePaths.Add(AssetDatabase.GetAssetOrScenePath(sc)));
            await Task.WhenAll(LoadScenesByPathAsync(bootstrapScenePaths));
        }

        private List<Task> LoadScenesByPathAsync(List<string> paths)
        {
            List<Task> tasks = new();
            LoadSceneParameters parameters = new();
            parameters.loadSceneMode = LoadSceneMode.Additive;
            parameters.localPhysicsMode = LocalPhysicsMode.Physics2D;
            Dispatcher dispatcher = this.FindFirstComponentWithTag<Dispatcher>(TagConstants.UNITY_DISPATCHER);
            foreach (string path in paths)
            {
                tasks.Add(Task.Run(() =>
                {
                    dispatcher.Invoke(async () =>
                    {
                        if (!SceneManager.GetSceneByPath(path).isLoaded)
                        {
                            AsyncOperation op = EditorSceneManager.LoadSceneAsyncInPlayMode(path, parameters);

                            while (!op.isDone)
                            {
                                await Task.Yield();
                            }
                        }
                    });
                }));
            }
            return tasks;
        }
#else
        protected async void Start()
        {
            List<string> bootstrapSceneNames = new();
            bootstrapSceneNames.Add("UI");
            await Task.WhenAll(LoadScenesByNameAsync(bootstrapSceneNames));
        }

        private List<Task> LoadScenesByNameAsync(List<string> names)
        {
            List<Task> tasks = new();
            Dispatcher dispatcher = this.FindFirstComponentWithTag<Dispatcher>(TagConstants.UNITY_DISPATCHER);
            foreach (string name in names)
            {
                tasks.Add(Task.Run(() =>
                {
                    dispatcher.Invoke(async () =>
                    {
                        if (!SceneManager.GetSceneByPath(name).isLoaded)
                        {
                            AsyncOperation op = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);

                            while (!op.isDone)
                            {
                                await Task.Yield();
                            }
                        }
                    });
                }));
            }
            return tasks;
        }
#endif
    }
}
