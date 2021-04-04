using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.Systems.Dialogue
{
    [Serializable]
    public class TutorialLine
    {
        public AK.Wwise.Event instruction;
        public AK.Wwise.Event reminder;
        public AK.Wwise.Event completion;
    }

    // Scene-specific model
    public class DialogueModel : MonoBehaviour
    {
        [HideInInspector]
        public DialogueView view;

        public string letterGroupState = "";

        public GameObject postEventsHere;

        [Header("Gamemode events")]
        public AK.Wwise.Event onLetterGroupWin;
        public AK.Wwise.Event onLetterGroupFail;
        public AK.Wwise.Event onLetterWin;
        public AK.Wwise.Event onLetterFail;
        public AK.Wwise.Event onSkillWin;
        public AK.Wwise.Event onSkillFail;
        public AK.Wwise.Event onTip; // Small optional help
        public AK.Wwise.Event onPositiveLetterGroupStreakCompleted;
        public AK.Wwise.Event onPositiveLetterGroupStreakBroken;
        public AK.Wwise.Event onNegativeLetterGroupStreakCompleted;
        public AK.Wwise.Event onNegativeLetterGroupStreakBroken;
        public AK.Wwise.Event onGamemodeRepaired;
        public AK.Wwise.Event onSceneChangeRequest;

        [Header("Engagement events")]
        public AK.Wwise.Event onBored;
        public AK.Wwise.Event onEngaged;
        public AK.Wwise.Event onFrustrated;
        public AK.Wwise.Event onDistractorRequired;

        [Header("Learning events")]
        public AK.Wwise.Event onForgottenSkill;
        public AK.Wwise.Event onLevelMastered;
        public AK.Wwise.Event onNewHighestStreak;
        public AK.Wwise.Event onNewLowestStreak;

        [Header("Progression events")]
        public AK.Wwise.Event onLevelUp;
        public AK.Wwise.Event onGamemodeUnlocked;
        public AK.Wwise.Event onGamemodeGroupUnlocked;
        public AK.Wwise.Event onGamemodeBroken;
        public AK.Wwise.Event onGameFinished;

        [Header("Currency events")]
        public AK.Wwise.Event onCoinsEarned;

        [Header("Tutorial")]

        // Storage/Repository
        public float hintWaitTime = 15.0f;
        public List<TutorialLine> tutorial = new List<TutorialLine>();
    }

}
