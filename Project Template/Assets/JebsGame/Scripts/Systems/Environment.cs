using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.Globals
{
    public static class Constants
    {
        public const string alphabet = "abcdefghijklmnopqrstuvwxyz";
    } 

    public enum Activity
    {
        None,
        LetterRecognition,
        LetterSequencing,
        LetterMissing,
        LetterPairing,
        All
    }

    public enum LetterGroup
    {
        None,
        AtoG,
        HtoM,
        NtoT,
        UtoZ,
        All
    }

    public enum SceneState
    {
        None,
        Loading,
        Tutorial,
        Gameplay,
        Minigame,
        Paused
    }

    public enum GamemodeUnlockingStrategy
    {
        UnlockAllLevelsToUnlockGamemode,
        UnlockFirstLevelToUnlockGamemode
    }

    public enum LevelUnlockingStrategy
    {
        UnlockNextFromCurrent,
        UnlockNextFromLearning
    }
}
