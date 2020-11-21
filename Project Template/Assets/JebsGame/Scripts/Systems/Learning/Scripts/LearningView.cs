using JesbReadingGame.Skeletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using JebsReadingame.Events;
using System;
using JebsReadingGame.Globals;

namespace JebsReadingGame.System.Learning
{
    public class LearningView : GlobalView
    {
        // Singleton
        static LearningView _singleton;
        public static LearningView singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = GameObject.FindObjectOfType<LearningView>();

                return _singleton;
            }
        }

        // ViewModel
        public sealed class LearningViewModel
        {
            LearningModel model;

            // Properties
            public LearningPersistent persistent { get { return model.persistent; } }
            public int configurationValue { get { return model.asset.configurationValue; } } // placeholder

            // Constructor
            public LearningViewModel(LearningModel model)
            {
                this.model = model;
            }

            // Functions
            public void SetLearningLerp(Activity activity, LetterGroup letterGroup, float learningLerp)
            {
                for (int i = 0; i < persistent.state.activities.Length; i++)
                {
                    if (persistent.state.activities[i].activity == activity)
                    {
                        for (int j = 0; j < persistent.state.activities[i].letterGroups.Length; j++)
                        {
                            if (persistent.state.activities[i].letterGroups[j].letterGroup == letterGroup)
                            {
                                persistent.state.activities[i].letterGroups[j].learningLerp = learningLerp;
                                model.persistent.Save();
                            }
                        }
                    }
                }

                Debug.LogError("Learning lerp not found!");
            }

            public void SetLearningLerp(Activity activity, char letter, float learningLerp)
            {
                for (int i = 0; i < persistent.state.activities.Length; i++)
                {
                    if (persistent.state.activities[i].activity == activity)
                    {
                        for (int j = 0; j < persistent.state.activities[i].letters.Length; j++)
                        {
                            if (persistent.state.activities[i].letters[j].letter == letter)
                            {
                                persistent.state.activities[i].letters[j].learningLerp = learningLerp;
                                model.persistent.Save();
                            }
                        }
                    }
                }

                Debug.LogError("Learning lerp not found!");
            }

            public float GetLearningLerp(Activity activity, LetterGroup letterGroup)
            {
                for (int i = 0; i < persistent.state.activities.Length; i++)
                {
                    if (persistent.state.activities[i].activity == activity)
                    {
                        for (int j = 0; j < persistent.state.activities[i].letterGroups.Length; j++)
                        {
                            if (persistent.state.activities[i].letterGroups[j].letterGroup == letterGroup)
                                return persistent.state.activities[i].letterGroups[j].learningLerp;
                        }
                    }
                }

                Debug.LogError("Learning lerp not found!");

                return 0.0f;
            }

            public float GetLearningLerp(Activity activity, char letter)
            {
                for (int i = 0; i < persistent.state.activities.Length; i++)
                {
                    if (persistent.state.activities[i].activity == activity)
                    {
                        for (int j = 0; j < persistent.state.activities[i].letters.Length; j++)
                        {
                            if (persistent.state.activities[i].letters[j].letter == letter)
                                return persistent.state.activities[i].letters[j].learningLerp;
                        }
                    }
                }

                Debug.LogError("Learning lerp not found!");

                return 0.0f;
            }
        }

        public LearningViewModel viewModel;

        // Events
        public ActivityEvent onForgottenSkill = new ActivityEvent();
        public ActivityLetterGroupEvent onLevelMastered = new ActivityLetterGroupEvent();
    }
}
