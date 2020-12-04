using JesbReadingGame.Skeletons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using JesbReadingGame.Helpers;
using JebsReadingGame.Globals;

namespace JebsReadingGame.System.Progression
{
    public class ProgressionModel : GlobalModel
    {
        [HideInInspector]
        public ProgressionView view; // Optional

        public ProgressionConfigurationAsset asset;

        public ProgressionPersistent persistent = new ProgressionPersistent();

        [Header("Updated by Controller")]
        public GamemodeGroup currentGamemodeGroup;
        public Gamemode currentGamemode;
        public Level currentLevel;
    }

    // Persistent model: Persistent between scenes
    public class ProgressionPersistent
    {
        string fileName = "ProgressionState.json";
        bool resetOnError = true;
        bool firstLevelUnlocked = true;

        ProgressionState _state;
        public ProgressionState state
        {
            get
            {
                if (_state == null)
                {
                    try
                    {
                        _state = FileHelpers.ReadJson<ProgressionState>(fileName);

                        if (_state == null)
                        {
                            Debug.LogWarning("State was empty. Resetting state!");
                            _state = new ProgressionState(firstLevelUnlocked);
                            Save();
                        }
                    }
                    catch(Exception e)
                    {
                        Debug.LogError("Exception found while getting state: " + e.Message);

                        if (resetOnError)
                        {
                            Debug.LogWarning("Resetting state!");
                            _state = new ProgressionState(firstLevelUnlocked);
                            Save();
                        }
                    }
                    
                }
                return _state;
            }
            set
            {
                _state = value;
                //SaveIntoJson(value);      Doing this every write operation is very expensive. Please call Save() before leaving the scene
            }
        }

        public void Save()
        {
            FileHelpers.WriteJson<ProgressionState>(fileName, _state);
            Debug.Log("ProgressionModel.peristent - SAVE!");
        }
    }

    // Configuration model: Can be shared between models. Cannot be modified in build
    [CreateAssetMenu(menuName = "JesbReadingGame/Progression Configuration Asset", order = 1)]
    public class ProgressionConfigurationAsset : ScriptableObject
    {
        public GamemodeUnlockingStrategy howToUnlockGamemodes;
        public LevelUnlockingStrategy howToUnlockLevels;
    }
}
