using JesbReadingGame.Skeletons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using JebsReadingGame.Globals;

namespace JebsReadingGame.System.Scene
{
    public class SceneModel : GlobalModel
    {
        [HideInInspector]
        public SceneView view; // Optional

        public SceneConfigurationAsset asset;

        public ScenePersistent persistent = new ScenePersistent();

        public int currentScene;
        public SceneState state;
        public float timeScale;
    }

    // Persistent model: Persistent between scenes
    public class ScenePersistent
    {
        string persistentValueKey = "key";

        public int persistentValue
        {
            get { return PlayerPrefs.GetInt(persistentValueKey); }
            set { PlayerPrefs.SetInt(persistentValueKey, value); }
        }
    }

    // Configuration model: Can be shared between models. Cannot be modified in build
    [CreateAssetMenu(menuName = "JesbReadingGame/Scene Configuration Asset", order = 1)]
    public class SceneConfigurationAsset : ScriptableObject
    {
        public int configurationValue = 0;
    }
}
