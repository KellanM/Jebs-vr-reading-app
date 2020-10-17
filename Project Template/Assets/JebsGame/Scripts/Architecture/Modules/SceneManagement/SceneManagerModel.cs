using UnityEngine;

namespace JebsReadingGame.SceneManager
{
    public class SceneManagerModel : MonoBehaviour
    {
        [HideInInspector]
        public SceneManagerView view;

        public SceneManagerPersistent persistent = new SceneManagerPersistent();
    }

    // Persistent model: Persistent between scenes and launches
    public class SceneManagerPersistent
    {
        private string currentSceneKey = "currentScene";

        public string currentScene;

        internal void LoadValues()
        {
            currentScene = PlayerPrefs.GetString(currentSceneKey);
        }
        internal void SaveValues()
        {
            PlayerPrefs.SetString(currentSceneKey, currentScene);
        }
    }
}