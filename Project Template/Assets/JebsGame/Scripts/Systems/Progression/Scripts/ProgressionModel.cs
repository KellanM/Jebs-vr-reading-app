using JesbReadingGame.Skeletons;
using JebsReadingGame.Serializables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace JebsReadingGame.System.Progression
{
    public class ProgressionModel : GlobalModel
    {
        [HideInInspector]
        public ProgressionView view; // Optional

        public ProgressionConfigurationAsset asset;

        public ProgressionPersistent persistent = new ProgressionPersistent();
    }

    // Persistent model: Persistent between scenes
    public class ProgressionPersistent
    {
        string path = "/ProgressionState.json";

        ProgressionState _state;
        public ProgressionState state
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

        ProgressionState LoadFromJson()
        {
            FileInfo file = new FileInfo(Application.persistentDataPath + path);
            file.Directory.Create();
            string stringifiedState = File.ReadAllText(Application.persistentDataPath + path);

            return JsonUtility.FromJson<ProgressionState>(stringifiedState);
        }

        void SaveIntoJson(ProgressionState state)
        {
            string stringifiedState = JsonUtility.ToJson(state);

            FileInfo file = new FileInfo(Application.persistentDataPath + path);
            file.Directory.Create();
            File.WriteAllText(Application.persistentDataPath + path, stringifiedState);
        }
    }

    // Configuration model: Can be shared between models. Cannot be modified in build
    [CreateAssetMenu(menuName = "JesbReadingGame/Progression Configuration Asset", order = 1)]
    public class ProgressionConfigurationAsset : ScriptableObject
    {
        public int configurationValue = 0;
    }
}
