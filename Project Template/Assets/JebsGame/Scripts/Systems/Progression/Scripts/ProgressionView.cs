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
            public GamemodeGroup currentGamemodeGroup { get { return model.currentGamemodeGroup; } }
            public Gamemode currentGamemode { get { return model.currentGamemode; } }
            public Level currentLevel { get { return model.currentLevel; } }

            // Constructor
            public ProgressionViewModel(ProgressionModel model)
            {
                this.model = model;
            }

            // Functions
            public LetterGroup GetLastUnlockedLetterGroup(Activity activity)
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

            // Assuming that each there won't be gamemodes for the same activity
            public bool IsUnlocked(Activity activity)
            {
                /*
                if (gamemodeGroup < 0 || gamemodeGroup > persistent.state.gamemodeGroups.Length - 1)
                {
                    Debug.LogError("Gamemode group not found!");
                    return false;
                }
                */

                List<Gamemode> totalGamemodes = new List<Gamemode>();

                for (int i = 0; i < persistent.state.gamemodeGroups.Length; i++)
                {
                    totalGamemodes.AddRange(persistent.state.gamemodeGroups[i].gamemodes);
                }

                for (int i = 0; i < totalGamemodes.Count; i++)
                {
                    if (totalGamemodes[i].activity == activity)
                    {
                        Gamemode gamemode = totalGamemodes[i];
                        return gamemode.unlocked;
                    }
                }

                Debug.LogError("Activity not found!");
                return false;
            }

            // Assuming that each there won't be gamemodes for the same activity
            public bool IsUnlocked(Activity activity, LetterGroup letterGroup)
            {
                /*
                if (gamemodeGroup < 0 || gamemodeGroup > persistent.state.gamemodeGroups.Length - 1)
                {
                    Debug.LogError("Gamemode group not found!");
                    return false;
                }
                */

                List<Gamemode> totalGamemodes = new List<Gamemode>();

                for (int i = 0; i < persistent.state.gamemodeGroups.Length; i++)
                {
                    totalGamemodes.AddRange(persistent.state.gamemodeGroups[i].gamemodes);
                }

                for (int i = 0; i < totalGamemodes.Count; i++)
                {
                    for (int j = 0; j < totalGamemodes[i].levels.Length; j++)
                    {
                        if (totalGamemodes[i].levels[j].letterGroup == letterGroup)
                        {
                            return totalGamemodes[i].levels[j].unlocked;
                        }
                    }
                }

                Debug.LogError("Activity or letter group not found!");
                return false;
            }
        }

        public ProgressionViewModel viewModel;

        // Events
        public LevelEvent onLevelUp = new LevelEvent();
        public GamemodeEvent onGamemodeUnlocked = new GamemodeEvent();
        public GamemodeGroupEvent onGamemodeGroupUnlocked = new GamemodeGroupEvent();

        // Due to favouritsm (learning) or due to trying to unlock a gamemode without having completed the previous ones (progression)
        public GamemodeEvent onGamemodeBroken = new GamemodeEvent();

        // All the levels, gamemodes and gamemode groups have been unlocked
        public UnityEvent onGameFinished = new UnityEvent();
    }
}