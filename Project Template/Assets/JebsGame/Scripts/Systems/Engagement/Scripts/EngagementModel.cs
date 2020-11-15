using JesbReadingGame.Skeletons;
using JebsReadingGame.Serializables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace JebsReadingGame.System.Engagement
{
    public class EngagementModel : GlobalModel
    {
        [HideInInspector]
        public EngagementView view; // Optional

        public EngagementConfigurationAsset asset;

        public EngagementPersistent persistent = new EngagementPersistent(/*Application.persistentDataPath + "/DifficultyState.json"*/);

        [Header("References")]
        public Transform head;
        public Transform leftHand;
        public Transform rightHand;

        [Header("Updated by Controller")]
        public float realtimeEngagementEstimation;
    }

    // Persistent model: Persistent between scenes
    public class EngagementPersistent
    {
        string path = "/DifficultyState.json";

        DifficultyState _state;
        public DifficultyState state
        {
            get {
                if (_state == null)
                    _state = LoadFromJson();
                return _state;
            }
            set {
                _state = value;
                SaveIntoJson(value);
            }
        }

        public void Save() // Needed to save changes when .state is not completelely reasigned but just modified
        {
            SaveIntoJson(state);
        }

        DifficultyState LoadFromJson()
        {
            FileInfo file = new FileInfo(Application.persistentDataPath + path);
            file.Directory.Create();
            string stringifiedState = File.ReadAllText(Application.persistentDataPath + path);

            return JsonUtility.FromJson<DifficultyState>(stringifiedState);
        }

        void SaveIntoJson(DifficultyState state)
        {
            string stringifiedState = JsonUtility.ToJson(state);

            FileInfo file = new FileInfo(Application.persistentDataPath + path);
            file.Directory.Create();
            File.WriteAllText(Application.persistentDataPath + path, stringifiedState);
        }
    }

    // Configuration model: Can be shared between models. Cannot be modified in build
    [CreateAssetMenu(menuName = "JesbReadingGame/Engagement Configuration Asset", order = 1)]
    public class EngagementConfigurationAsset : ScriptableObject
    {
        public int configurationValue = 0;
    }
}
