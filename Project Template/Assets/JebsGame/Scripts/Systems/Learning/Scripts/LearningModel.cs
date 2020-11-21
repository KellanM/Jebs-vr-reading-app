using JesbReadingGame.Skeletons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using JesbReadingGame.Helpers;
using JebsReadingGame.System.Engagement;

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
        string fileName = "LearningState.json";
        bool resetOnError = true;

        LearningState _state;
        public LearningState state
        {
            get
            {
                if (_state == null)
                {
                    try
                    {
                        _state = FileHelpers.ReadJson<LearningState>(fileName);

                        if (_state == null)
                        {
                            Debug.LogWarning("State was empty. Resetting state!");
                            _state = new LearningState();
                            Save();
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception found while getting state: " + e.Message);

                        if (resetOnError)
                        {
                            Debug.LogWarning("Resetting state!");
                            _state = new LearningState();
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
            FileHelpers.WriteJson<LearningState>(fileName, _state);
        }
    }

    // Configuration model: Can be shared between models. Cannot be modified in build
    [CreateAssetMenu(menuName = "JesbReadingGame/Learning Configuration Asset", order = 1)]
    public class LearningConfigurationAsset : ScriptableObject
    {
        public int configurationValue = 0;
    }
}
