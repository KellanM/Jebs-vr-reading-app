using UnityEngine;
using UnityEngine.SceneManagement;

namespace JebsReadingGame.SceneManager
{
    public class SceneManagerController : MonoBehaviour
    {
        public SceneManagerModel model;
        public SceneManagerView view;

        private void Awake()
        {
            view.viewModel = new SceneManagerView.ViewModel(model);
            model.view = view;

            model.persistent.LoadValues();
        }

        private void Start()
        {
            // Subscribe to events
            view.onSceneChange.AddListener(Goto);
        }

        public void Goto(string sceneName)
        {
            model.persistent.currentScene = sceneName;
            model.persistent.SaveValues();
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }
}