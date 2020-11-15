using JebsReadingGame.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.Serializables
{
    [Serializable]
    public class Item
    {
        public string id;
        public Sprite icon;
        public GameObject prefab;

        public Item(string id, Sprite icon, GameObject prefab)
        {
            this.id = id;
            this.icon = icon;
            this.prefab = prefab;
        }
    }

    // Difficulty

    [Serializable]
    public class LetterGroupDifficultyState
    {
        public LetterGroup letterGroup;
        public float difficultyLerp;
    }

    [Serializable]
    public class ActivityDifficultyState
    {
        public Activity activity;
        public LetterGroupDifficultyState[] letterGroups;
    }

    [Serializable]
    public class DifficultyState
    {
        public ActivityDifficultyState[] activities;
    }

    // Learning

    [Serializable]
    public class LetterLearningState
    {
        public char letter;
        public float learningLerp;
    }

    [Serializable]
    public class LetterGroupLearningState
    {
        public LetterGroup letterGroup;
        public float learningLerp;
    }

    [Serializable]
    public class ActivityLearningState
    {
        public Activity activity;
        public LetterLearningState[] letters;
        public LetterGroupLearningState[] letterGroups;
    }

    [Serializable]
    public class LearningState
    {
        public ActivityLearningState[] activities;
    }

    // Progression

    [Serializable]
    public class Level
    {
        public LetterGroup letterGroup;
        public bool unlocked;
    }

    [Serializable]
    public class Gamemode
    {
        public Activity activity;
        public Level[] levels;
        public bool unlocked;
    }

    [Serializable]
    public class GamemodeGroup
    {
        public Gamemode[] gamemodes;
        public bool unlocked;
    }

    [Serializable]
    public class ProgressionState
    {
        public GamemodeGroup[] gamemodeGroups;
    }
}
