using JebsReadingGame.Globals;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.System.Progression
{
    [Serializable]
    public class Level
    {
        public LetterGroup letterGroup;
        public bool unlocked;

        public Level(LetterGroup letterGroup, bool unlocked)
        {
            this.letterGroup = letterGroup;
            this.unlocked = unlocked;
        }
    }

    [Serializable]
    public class Gamemode
    {
        public Activity activity;
        public Level[] levels;
        public bool unlocked;

        public Gamemode(Activity activity, Level[] levels, bool unlocked)
        {
            this.activity = activity;
            this.levels = levels;
            this.unlocked = unlocked;
        }
    }

    [Serializable]
    public class GamemodeGroup
    {
        public Gamemode[] gamemodes;
        public bool unlocked;

        public GamemodeGroup(Gamemode[] gamemodes, bool unlocked)
        {
            this.gamemodes = gamemodes;
            this.unlocked = unlocked;
        }
    }

    [Serializable]
    public class ProgressionState
    {
        public GamemodeGroup[] gamemodeGroups;

        public ProgressionState(bool firstLevelUnlocked)
        {
            // Lists init
            List<Level> levels = new List<Level>();
            List<Gamemode> gamemodes = new List<Gamemode>();
            List<GamemodeGroup> gamemodeGroups = new List<GamemodeGroup>();

            // Levels
            levels.Add(new Level(LetterGroup.AtoG, firstLevelUnlocked)); // (!)
            levels.Add(new Level(LetterGroup.HtoM, false));
            levels.Add(new Level(LetterGroup.NtoT, false));
            levels.Add(new Level(LetterGroup.UtoZ, false));

            // Gamemodes
            gamemodes.Add(new Gamemode(Activity.LetterRecognition, levels.ToArray(), firstLevelUnlocked));  // (!)
            gamemodes.Add(new Gamemode(Activity.LetterSequencing, levels.ToArray(), false));
            gamemodes.Add(new Gamemode(Activity.LetterMissing, levels.ToArray(), false));
            gamemodes.Add(new Gamemode(Activity.LetterPairing, levels.ToArray(), false));

            // Gamemode groups
            gamemodeGroups.Add(new GamemodeGroup(gamemodes.ToArray(), firstLevelUnlocked));  // (!)

            // 
            this.gamemodeGroups = gamemodeGroups.ToArray();
        }
    }
}
