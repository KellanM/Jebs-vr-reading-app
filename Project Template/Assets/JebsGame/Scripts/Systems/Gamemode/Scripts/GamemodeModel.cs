using JesbReadingGame.Skeletons;
using JebsReadingGame.Serializables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JebsReadingGame.Enums;

namespace JebsReadingGame.System.Gamemode
{
    public class GamemodeModel : GlobalModel
    {
        [HideInInspector]
        public GamemodeView view; // Optional

        public GamemodeConfigurationAsset asset;

        public GamemodePersistent persistent = new GamemodePersistent();
 
        public Activity activity; // Scene-specific

        public float difficultyStep = 0.1f; // How much difficulty changes on each diffulty increase/decrease

        public int streakLength = 3; // how many small win/fails complete a positive/negative combo or streak

        public string[] availableScenes;

        [Header("Updated by Controller")]
        public LetterGroup currentLetterGroup;
        public int currentCombo;
        public float difficultyLerp;
        public int gameplayLetterWins;
        public int gameplayLetterFails;
        public int gameplayLetterGroupWins;
        public int gameplayLetterGroupFails;
        public Item nextEquipableItem;
    }

    // Persistent model: Persistent between scenes
    public class GamemodePersistent
    {
        string persistentValueKey = "key";

        public int persistentValue
        {
            get { return PlayerPrefs.GetInt(persistentValueKey); }
            set { PlayerPrefs.SetInt(persistentValueKey, value); }
        }
    }

    // Configuration model: Can be shared between models. Cannot be modified in build
    [CreateAssetMenu(menuName = "JesbReadingGame/Gamemode Configuration Asset", order = 1)]
    public class GamemodeConfigurationAsset : ScriptableObject
    {
        public int configurationValue = 0;
    }
}
