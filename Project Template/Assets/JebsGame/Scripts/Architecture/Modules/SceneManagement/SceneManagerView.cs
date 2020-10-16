using UnityEngine;
using UnityEngine.Events;

namespace JebsReadingGame.SceneManager
{
    [System.Serializable]
    public class SceneChangeEvent : UnityEvent<string> {}

    public class SceneManagerView : MonoBehaviour
    {
        // Singleton
        static SceneManagerView _singleton;
        public static SceneManagerView singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = GameObject.FindObjectOfType<SceneManagerView>();

                return _singleton;
            }
        }

        // ViewModel
        public sealed class ViewModel
        {
            SceneManagerModel model;

            // Properties
            public string currentScene { get { return model.persistent.currentScene; } }

            // Constructor
            public ViewModel(SceneManagerModel model)
            {
                this.model = model;
            }
        }
        public ViewModel viewModel;

        // Events
        public SceneChangeEvent onSceneChange;
    }
}