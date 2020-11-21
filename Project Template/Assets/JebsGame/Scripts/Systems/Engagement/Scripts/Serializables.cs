using JebsReadingGame.Globals;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.System.Engagement
{
    [Serializable]
    public class LetterGroupDifficultyState
    {
        public LetterGroup letterGroup;
        public float difficultyLerp;

        public LetterGroupDifficultyState(LetterGroup letterGroup)
        {
            this.letterGroup = letterGroup;
            this.difficultyLerp = 0.0f;
        }
    }

    [Serializable]
    public class ActivityDifficultyState
    {
        public Activity activity;
        public LetterGroupDifficultyState[] letterGroups;

        public ActivityDifficultyState(Activity activity)
        {
            // Lists init
            List<LetterGroupDifficultyState> letterGroups = new List<LetterGroupDifficultyState>();

            // Letter groups
            letterGroups.Add(new LetterGroupDifficultyState(LetterGroup.AtoG));
            letterGroups.Add(new LetterGroupDifficultyState(LetterGroup.HtoM));
            letterGroups.Add(new LetterGroupDifficultyState(LetterGroup.NtoT));
            letterGroups.Add(new LetterGroupDifficultyState(LetterGroup.UtoZ));

            //
            this.activity = activity;
            this.letterGroups = letterGroups.ToArray();
        }
    }

    [Serializable]
    public class DifficultyState
    {
        public ActivityDifficultyState[] activities;

        public DifficultyState()
        {
            // Lists init
            List<ActivityDifficultyState> activities = new List<ActivityDifficultyState>();

            // Activities
            activities.Add(new ActivityDifficultyState(Activity.LetterRecognition));
            activities.Add(new ActivityDifficultyState(Activity.LetterSequencing));
            activities.Add(new ActivityDifficultyState(Activity.LetterMissing));
            activities.Add(new ActivityDifficultyState(Activity.LetterPairing));

            //
            this.activities = activities.ToArray();
        }
    }
}
