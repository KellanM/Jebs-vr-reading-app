using JesbReadingGame.Skeletons;
using JebsReadingGame.Serializables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace JebsReadingGame.System.Learning
{
    public class LearningModel : GlobalModel
    {
        [HideInInspector]
        public LearningView view; // Optional

        public LearningConfigurationAsset asset;

        public LearningPersistent persistent = new LearningPersistent();
    }

    // Persistent model: Persistent between scenes
    public class LearningPersistent
    {
        string path = "/LearningState.json";

        LearningState _state;
        public LearningState state
        {
            get
            {
                if (_state == null)
                    _state = LoadFromJson();
                return _state;
            }
            set
            {
                _state = value;
                SaveIntoJson(value);
            }
        }

        public void Save() // Needed to save changes when .state is not completelely reasigned but just modified
        {
            SaveIntoJson(state);
        }

        LearningState LoadFromJson()
        {
            FileInfo file = new FileInfo(Application.persistentDataPath + path);
            file.Directory.Create();
            string stringifiedState = File.ReadAllText(Application.persistentDataPath + path);

            return JsonUtility.FromJson<LearningState>(stringifiedState);
        }

        void SaveIntoJson(LearningState state)
        {
            string stringifiedState = JsonUtility.ToJson(state);

            FileInfo file = new FileInfo(Application.persistentDataPath + path);
            file.Directory.Create();
            File.WriteAllText(Application.persistentDataPath + path, stringifiedState);
        }
    }

    // Configuration model: Can be shared between models. Cannot be modified in build
    [CreateAssetMenu(menuName = "JesbReadingGame/Learning Configuration Asset", order = 1)]
    public class LearningConfigurationAsset : ScriptableObject
    {
        public int configurationValue = 0;
    }
}
