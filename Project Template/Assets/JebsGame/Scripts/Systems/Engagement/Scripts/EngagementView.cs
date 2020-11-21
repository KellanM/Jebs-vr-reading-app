using JesbReadingGame.Skeletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using JebsReadingame.Events;
using System;
using JebsReadingGame.Globals;

namespace JebsReadingGame.System.Engagement
{
    public class EngagementView : GlobalView
    {
        // Singleton
        static EngagementView _singleton;
        public static EngagementView singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = GameObject.FindObjectOfType<EngagementView>();

                return _singleton;
            }
        }

        // ViewModel
        public sealed class EngagementViewModel
        {
            EngagementModel model;

            // Properties
            public EngagementPersistent persistent { get { return model.persistent; } }
            public int configurationValue { get { return model.asset.configurationValue; } } // placeholder

            public float realtimeEngagementEstimation { get { return model.realtimeEngagementEstimation; } }

            // Constructor
            public EngagementViewModel(EngagementModel model)
            {
                this.model = model;
            }

            // Functions
            public void SetDifficultyLerp(Activity activity, LetterGroup letterGroup, float difficultyLerp)
            {
                for (int i = 0; i < persistent.state.activities.Length; i++)
                {
                    if (persistent.state.activities[i].activity == activity)
                    {
                        for (int j = 0; j < persistent.state.activities[i].letterGroups.Length; j++)
                        {
                            if (persistent.state.activities[i].letterGroups[j].letterGroup == letterGroup)
                            {
                                persistent.state.activities[i].letterGroups[j].difficultyLerp = difficultyLerp;
                                model.persistent.Save();
                            }
                        }
                    }
                }

                Debug.LogError("Difficulty lerp not found!");
            }

            public float GetDifficultyLerp(Activity activity, LetterGroup letterGroup)
            {
                for (int i = 0; i < persistent.state.activities.Length; i++)
                {
                    if (persistent.state.activities[i].activity == activity)
                    {
                        for (int j = 0; j < persistent.state.activities[i].letterGroups.Length; j++)
                        {
                            if (persistent.state.activities[i].letterGroups[j].letterGroup == letterGroup)
                                return persistent.state.activities[i].letterGroups[j].difficultyLerp;
                        }
                    }
                }

                Debug.LogError("Difficulty lerp not found!");

                return 0.0f;
            }
        }

        public EngagementViewModel viewModel;

        // Events
        public UnityEvent onBored = new UnityEvent();
        public UnityEvent onEngaged = new UnityEvent();
        public UnityEvent onFrustrated = new UnityEvent();
    }
}
