using JebsReadingGame.System.Gamemode;
using JesbReadingGame.Skeletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JebsReadingGame.System.Scene
{
    public class SceneController : GlobalController
    {
        public SceneModel model;
        public SceneView view;

        private void Awake()
        {
            if (!model)
                model = GetComponent<SceneModel>();

            if (!view)
                view = GetComponent<SceneView>();

            if (!model || !view)
            {
                Debug.LogError("Missing system components!");
                gameObject.SetActive(false);
            }

            // Components connection
            view.viewModel = new SceneView.SceneViewModel(model);

            model.view = view; // Optional
        }

        private void Start()
        {
            // ...

            GamemodeView.singleton.onSceneChangeRequest.AddListener(LoadSceneOnNextFrame);
        }

        // ...

        public void LoadSceneOnNextFrame(string scene)
        {
            // Check that the scene name exists and can be teleported
            // ...

            view.onSceneChange.Invoke(scene);

            StartCoroutine(LoadScene(scene));
        }

        IEnumerator LoadScene(string scene)
        {
            yield return new WaitForEndOfFrame();

            UnityEngine.SceneManagement.SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }

}