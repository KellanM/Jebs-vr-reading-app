using JesbReadingGame.Skeletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using JebsReadingame.Events;
using System;
using JebsReadingGame.Globals;

namespace JebsReadingGame.System.Progression
{
    public class ProgressionView : GlobalView
    {
        // Singleton
        static ProgressionView _singleton;
        public static ProgressionView singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = GameObject.FindObjectOfType<ProgressionView>();

                return _singleton;
            }
        }

        // ViewModel
        public sealed class ProgressionViewModel
        {
            ProgressionModel model;

            // Properties
            public ProgressionPersistent persistent { get { return model.persistent; } }
            public int configurationValue { get { return model.asset.configurationValue; } } // placeholder

            // Constructor
            public ProgressionViewModel(ProgressionModel model)
            {
                this.model = model;
            }

            // Functions
            public LetterGroup GetLastUnlockedLevel(Activity activity)
            {
                for (int i = 0; i < persistent.state.gamemodeGroups.Length; i++)
                {
                    for (int j = 0; j < persistent.state.gamemodeGroups[i].gamemodes.Length; j++)
                    {
                        if (persistent.state.gamemodeGroups[i].gamemodes[j].activity == activity)
                        {
                            Gamemode gamemode = persistent.state.gamemodeGroups[i].gamemodes[j];
                            for (int k = 0; k < gamemode.levels.Length; k++)
                            {
                                if (k == 0 && !gamemode.levels[k].unlocked)
                                    return LetterGroup.None;

                                else if (!gamemode.levels[k].unlocked)
                                    return gamemode.levels[k - 1].letterGroup;

                                else if (k == gamemode.levels.Length - 1 && gamemode.levels[k].unlocked)
                                    return LetterGroup.All;
                            }
                        }
                    }
                }

                Debug.LogError("Requested activity or letter group could not be found!");

                return LetterGroup.None;
            }

            public bool IsUnlocked(int gamemodeGroup)
            {
                if (gamemodeGroup < 0 || gamemodeGroup > persistent.state.gamemodeGroups.Length - 1)
                {
                    Debug.LogError("Gamemode group not found!");
                    return false;
                }

                return persistent.state.gamemodeGroups[gamemodeGroup].unlocked;
            }

            public bool IsUnlocked(int gamemodeGroup, Activity activity)
            {
                if (gamemodeGroup < 0 || gamemodeGroup > persistent.state.gamemodeGroups.Length - 1)
                {
                    Debug.LogError("Gamemode group not found!");
                    return false;
                }

                for (int i = 0; i < persistent.state.gamemodeGroups[gamemodeGroup].gamemodes.Length; i++)
                {
                    if (persistent.state.gamemodeGroups[gamemodeGroup].gamemodes[i].activity == activity)
                    {
                        Gamemode gamemode = persistent.state.gamemodeGroups[gamemodeGroup].gamemodes[i];
                        return gamemode.unlocked;
                    }
                }

                Debug.LogError("Activity not found!");
                return false;
            }

            public bool IsUnlocked(int gamemodeGroup, Activity activity, LetterGroup letterGroup)
            {
                if (gamemodeGroup < 0 || gamemodeGroup > persistent.state.gamemodeGroups.Length - 1)
                {
                    Debug.LogError("Gamemode group not found!");
                    return false;
                }

                for (int i = 0; i < persistent.state.gamemodeGroups[gamemodeGroup].gamemodes.Length; i++)
                {
                    if (persistent.state.gamemodeGroups[gamemodeGroup].gamemodes[i].activity == activity)
                    {
                        Gamemode gamemode = persistent.state.gamemodeGroups[gamemodeGroup].gamemodes[i];
                        for (int j = 0; j < gamemode.levels.Length; j++)
                        {
                            if (gamemode.levels[j].letterGroup == letterGroup)
                            {
                                return gamemode.levels[j].unlocked;
                            }
                        }
                    }
                }

                Debug.LogError("Activity or letter group not found!");
                return false;
            }
        }

        public ProgressionViewModel viewModel;

        // Events
        public UnityEvent onLevelUp = new UnityEvent();
    }
}