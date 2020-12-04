using JebsReadingGame.Globals;
using JebsReadingGame.System.Gamemode;
using JebsReadingGame.System.Learning;
using JesbReadingGame.Skeletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.System.Progression
{
    public class ProgressionController : GlobalController
    {
        public ProgressionModel model;
        public ProgressionView view;

        Activity currentActivity;

        //[Header("Debug")]
        //[SerializeField]
        int currentGmdGroupIndex = -1;
        //[SerializeField]
        int currentGmdIndex = -1;
        //[SerializeField]
        int currentLvlIndex = -1;

        private void Awake()
        {
            if (!model)
                model = GetComponent<ProgressionModel>();

            if (!view)
                view = GetComponent<ProgressionView>();

            if (!model || !view)
            {
                Debug.LogError("Missing system components!");
                gameObject.SetActive(false);
            }

            // Components connection
            view.viewModel = new ProgressionView.ProgressionViewModel(model);

            model.view = view; // Optional
        }

        private void Start()
        {
            LearningView.singleton.onLevelMastered.AddListener(OnLevelMastered);

            currentActivity = GamemodeView.singleton.viewModel.activity;

            UpdateCurrentProgression(currentActivity);
        }

        // Not optimal. Assuming that there won't be gamemodes with the same activity
        void UpdateCurrentProgression(Activity activity)
        {
            GamemodeGroup gamemodeGroup = null;
            Gamemode gamemode = null;

            bool nestedBreak = false;
            for (int i = 0; i < model.persistent.state.gamemodeGroups.Length; i++)
            {
                gamemodeGroup = model.persistent.state.gamemodeGroups[i];

                if (gamemodeGroup.unlocked)
                {
                    currentGmdGroupIndex = i;
                    currentGmdIndex = -1;
                    currentLvlIndex = -1;

                    for (int j = 0; j < model.persistent.state.gamemodeGroups[i].gamemodes.Length; j++)
                    {
                        if (model.persistent.state.gamemodeGroups[i].gamemodes[j].unlocked)
                        {
                            currentGmdIndex = j;
                            currentLvlIndex = -1;

                            gamemode = model.persistent.state.gamemodeGroups[i].gamemodes[j]; // shortcut

                            // Current progression for the given activity
                            if (gamemode.activity == activity)
                            {
                                Debug.Log("Hemos encontrado " + activity.ToString());
                                for (int k = 0; k < gamemode.levels.Length; k++)
                                {
                                    if (gamemode.levels[k].unlocked)
                                    {
                                        currentLvlIndex = k;

                                        model.currentGamemodeGroup = model.persistent.state.gamemodeGroups[currentGmdGroupIndex];
                                        model.currentGamemode = model.persistent.state.gamemodeGroups[currentGmdGroupIndex].gamemodes[currentGmdIndex];
                                        model.currentLevel = model.persistent.state.gamemodeGroups[currentGmdGroupIndex].gamemodes[currentGmdIndex].levels[currentLvlIndex];
                                    }
                                }

                                nestedBreak = true;

                                break;
                            }
                        }

                        if (nestedBreak)
                            break;
                    }
                }   
            }

            Debug.Log("Updated checkpoint to GMDGROUP:" + currentGmdGroupIndex + " - GMD:" + currentGmdIndex + " - LVL:" + currentLvlIndex);
        }

        // Public for debug tool
        public void Next()
        {
            UnlockFrom(currentGmdGroupIndex, currentGmdIndex, currentLvlIndex);

            UpdateCurrentProgression(currentActivity);
        }

        // Public for debug tool
        public void Reset()
        {
            model.persistent.state = new ProgressionState(true);
            model.persistent.Save();

            UpdateCurrentProgression(currentActivity);
        }

        void OnLevelMastered(Activity activity, LetterGroup masteredLetterGroup)
        {
            switch(model.asset.howToUnlockLevels)
            {
                // Completing a level in activity 2 unlocks a level in activity 1 if existing (forces order)
                case LevelUnlockingStrategy.UnlockNextFromCurrent:
                    Next();
                    break;
                // Completing a level in activty 2 unlocks a level in activty 2
                case LevelUnlockingStrategy.UnlockNextFromLearning:
                    UnlockFrom(activity, masteredLetterGroup);
                    break;
            }
        }

        void OnGamemodeRepaired(GamemodeGroup gmdGroup, Gamemode gmd)
        {
            gmd.broken = false;

            // Next()?

            model.persistent.Save();
        }

        void UnlockFrom(Activity activity, LetterGroup masteredLetterGroup)
        {
            // Find indexes

            int currentGmdGroupIndex = -1;
            int currentGmdIndex = -1;
            int masteredLvlIndex = -1;

            for (int i = 0; i < model.persistent.state.gamemodeGroups.Length; i++)
            {
                for (int j = 0; j < model.persistent.state.gamemodeGroups[i].gamemodes.Length; j++)
                {
                    Gamemode gamemode = model.persistent.state.gamemodeGroups[i].gamemodes[j];

                    // Assuming that each there won't be gamemodes for the same activity
                    if (gamemode.activity == activity)
                    {
                        currentGmdGroupIndex = i;
                        currentGmdIndex = j;
                    }
                }
            }

            if (currentGmdGroupIndex < 0 || currentGmdIndex < 0)
            {
                Debug.LogError("Gamemode was not found for that activity (" + activity.ToString() + ")!");
                return;
            }

            for (int i = 0; i < model.persistent.state.gamemodeGroups[currentGmdGroupIndex].gamemodes[currentGmdIndex].levels.Length; i++)
            {
                LetterGroup letterGroup = model.persistent.state.gamemodeGroups[currentGmdGroupIndex].gamemodes[currentGmdIndex].levels[i].letterGroup;

                if (letterGroup == masteredLetterGroup)
                {
                    masteredLvlIndex = i;
                }
            }

            if (currentGmdGroupIndex < 0 || currentGmdIndex < 0)
            {
                Debug.LogError("Level was not found for that letter group (" + masteredLetterGroup.ToString() + ")! [gamemodeGroup: " + currentGmdGroupIndex + ", gamemode: " + currentGmdIndex + "]");
                return;
            }

            // Perform unlock

            UnlockFrom(currentGmdGroupIndex, currentGmdIndex, masteredLvlIndex);

        }

        void UnlockFrom(int gmdGroupIndex, int gmdIndex, int lvlIndex)
        {
            Debug.Log("Unlocking from GMGROUP:" + gmdGroupIndex + " - GMD:" + gmdIndex + " - LvlIndex:" + lvlIndex);

            // Find what to unlock next (level, gamemode, gamemodeGroup)

            bool unlockNextGmd = false;
            int lvlIndexToUnlockForTheSameGmd = -1;

            bool breakingPolicy = true;

            // If there is a next level to unlock
            if (lvlIndex < model.persistent.state.gamemodeGroups[gmdGroupIndex].gamemodes[gmdIndex].levels.Length - 1)
            {
                lvlIndexToUnlockForTheSameGmd = lvlIndex + 1;

                if (model.asset.howToUnlockGamemodes == GamemodeUnlockingStrategy.UnlockFirstLevelToUnlockGamemode && lvlIndex == 0)
                {
                    breakingPolicy = false;
                    unlockNextGmd = true;
                }
            }
            // If the mastered level is the last level for that gamemode, then unlock the first level of the next gamemode
            else
            {
                if (model.asset.howToUnlockGamemodes == GamemodeUnlockingStrategy.UnlockAllLevelsToUnlockGamemode)
                {
                    unlockNextGmd = true;
                }
            }

            int gmdGroupToUnlock = -1;
            int gmdToUnlock = -1;

            if (unlockNextGmd)
            {
                // If there is a next gamemode in the same gamemode group
                if (gmdIndex < model.persistent.state.gamemodeGroups[gmdGroupIndex].gamemodes.Length - 1)
                {
                    gmdToUnlock = gmdIndex + 1;
                }
                // If we need to unlock the next gamemode group
                else
                {
                    // If there is a next gamemode group to unlock
                    if (gmdGroupIndex < model.persistent.state.gamemodeGroups.Length - 1)
                    {
                        gmdGroupToUnlock = gmdGroupIndex + 1;

                        // If that new gamemode group contains at least 1 gamemode
                        if (model.persistent.state.gamemodeGroups[gmdGroupToUnlock].gamemodes.Length > 0)
                            gmdToUnlock = 0;
                    }
                    // If not, you completed the game!
                    else
                    {
                        view.onGameFinished.Invoke();
                    }
                }
            }

            // Unlock
            GamemodeGroup newGmdGroup = null;
            Gamemode newGmd = null;
            Level newLvl = null;
            Level extraNewLvl = null;

            if (gmdGroupToUnlock >= 0)
            {
                newGmdGroup = model.persistent.state.gamemodeGroups[gmdGroupToUnlock];

                // If the unlock is not necesary, abort the event invoke (set to -1 again)
                if (newGmdGroup.unlocked)
                {
                    gmdGroupToUnlock = -1;
                }
                else
                {
                    newGmdGroup.unlocked = true;
                }
            }

            if (gmdToUnlock >= 0)
            {
                int gmdGroupOfNewGmd;

                if (gmdGroupToUnlock >= 0)
                {
                    gmdGroupOfNewGmd = gmdGroupToUnlock;
                }
                else
                {
                    gmdGroupOfNewGmd = gmdGroupIndex;
                }

                // Filter forbidden operations (unlocking gamemode without having completed all previous gamemodes)
                if (CanUnlock(gmdGroupOfNewGmd, gmdToUnlock) || !breakingPolicy)
                {
                    newGmd = model.persistent.state.gamemodeGroups[gmdGroupOfNewGmd].gamemodes[gmdToUnlock];

                    newGmd.unlocked = true;

                    // Unlock its first level (*)

                    if (newGmd.levels.Length > 0)
                    {
                        extraNewLvl = newGmd.levels[0];
                        extraNewLvl.unlocked = true;
                    }
                }
                else
                {
                    // React to forbidden operations

                    Break(gmdGroupIndex, gmdIndex); // Break current gamemode before unlocking

                    // Prevent invoke events for interrupted operations

                    newGmd = null;
                }
                
            }

            if (lvlIndexToUnlockForTheSameGmd >= 0)
            {
                newLvl = model.persistent.state.gamemodeGroups[gmdGroupIndex].gamemodes[gmdIndex].levels[lvlIndexToUnlockForTheSameGmd];

                newLvl.unlocked = true;
            }

            // Save changes persistently

            model.persistent.Save();

            // Invoke unlock-related events

            GamemodeGroup currentGmdGroup = model.persistent.state.gamemodeGroups[gmdGroupIndex];
            Gamemode currentGamemode = model.persistent.state.gamemodeGroups[gmdGroupIndex].gamemodes[gmdIndex];
            Level masteredLevel = model.persistent.state.gamemodeGroups[gmdGroupIndex].gamemodes[gmdIndex].levels[lvlIndex];

            if (newLvl != null)
            {
                view.onLevelUp.Invoke(currentGmdGroup, currentGamemode, newLvl);
            }

            if (newGmdGroup != null)
            {
                view.onGamemodeGroupUnlocked.Invoke(newGmdGroup);
            }

            if (newGmd != null)
            {
                if (newGmdGroup != null)
                {
                    view.onGamemodeUnlocked.Invoke(newGmdGroup, newGmd);
                }
                else
                {
                    view.onGamemodeUnlocked.Invoke(currentGmdGroup, newGmd);
                }

                if (extraNewLvl != null)
                {
                    view.onLevelUp.Invoke(newGmdGroup, newGmd, extraNewLvl); // (*) The first level of the new gamemode is unlocked too
                }
            }
        }

        bool CanUnlock(int gmdGroupIndex, int gmdIndex)
        {
            List<Gamemode> previousGamemodes = new List<Gamemode>();

            for (int i = 0; i < gmdGroupIndex - 1; i++)
            {
                previousGamemodes.AddRange(model.persistent.state.gamemodeGroups[i].gamemodes);
            }

            for (int j = 0; j < gmdIndex - 1; j++)
            {
                previousGamemodes.Add(model.persistent.state.gamemodeGroups[gmdGroupIndex].gamemodes[j]);
            }

            for (int j = 0; j < previousGamemodes.Count; j++)
            {
                // It's forbidden to unlock gamemodes if some previous gamemode is locked
                if (!previousGamemodes[j].unlocked)
                {
                    Debug.Log("The gamemode for activity " + previousGamemodes[j].activity.ToString() + " is locked!");
                    return false;
                }

                // It's forbidden to unlock gamemode if some level for any previous gamemode is locked
                for (int k = 0; k < previousGamemodes[j].levels.Length; k++)
                {
                    if (!previousGamemodes[j].levels[k].unlocked)
                    {
                        Debug.Log("The gamemode for activity " + previousGamemodes[j].activity.ToString() + " has locked levels!");
                        return false;
                    }
                }
            }

            return true;
        }

        void Break(int gmdGroup, int gmdIndex)
        {
            Gamemode gmd = model.persistent.state.gamemodeGroups[gmdGroup].gamemodes[gmdIndex];

            if (!gmd.broken)
            {
                gmd.broken = true;

                // Event
                view.onGamemodeBroken.Invoke(model.persistent.state.gamemodeGroups[gmdGroup], gmd);
            }
        }
    }
}