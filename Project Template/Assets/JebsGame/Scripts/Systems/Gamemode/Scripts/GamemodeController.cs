using JebsReadingGame.Globals;
using JebsReadingGame.System.Engagement;
using JebsReadingGame.System.Learning;
using JebsReadingGame.System.Progression;
using JebsReadingGame.System.Scene;
using JesbReadingGame.Skeletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.System.Gamemode
{
    public class GamemodeController : GlobalController
    {
        public GamemodeModel model;
        public GamemodeView view;

        private void Awake()
        {
            if (!model)
                model = GetComponent<GamemodeModel>();

            if (!view)
                view = GetComponent<GamemodeView>();

            if (!model || !view)
            {
                Debug.LogError("Missing system components!");
                gameObject.SetActive(false);
            }

            // Components connection
            view.viewModel = new GamemodeView.GamemodeViewModel(model);

            model.view = view; // Optional
        }


        private void Start()
        {
            // Listen to events invoked by other systems
            EngagementView.singleton.onBored.AddListener(OnBored);
            EngagementView.singleton.onFrustrated.AddListener(OnFrustated);
            LearningView.singleton.onForgottenSkill.AddListener(OnForgottenSkill);
            SceneView.singleton.onSceneChange.AddListener(OnSceneChange);

            // Get letter group
            model.currentLetterGroup = ProgressionView.singleton.viewModel.GetLastUnlockedLevel(model.activity);
            Debug.Log("LETTER GROUP: " + model.currentLetterGroup);

            // Get initial difficulty
            model.difficultyLerp = EngagementView.singleton.viewModel.GetDifficultyLerp(model.activity,model.currentLetterGroup);
            Debug.Log("DIFFICULTY LERP: " + model.difficultyLerp);

            // ...

            // Debug - Get learning lerp for this activity and this lettter group
            float currentLetterGroupLearningLerp = LearningView.singleton.viewModel.GetLearningLerp(model.activity, model.currentLetterGroup);
            Debug.Log("LETTER GROUP LEARNING LERP: " + currentLetterGroupLearningLerp);

        }
        
        void OnBored()
        {
            Debug.Log("Player is bored! Let's increase difficulty");

            model.difficultyLerp += model.difficultyStep;

            if (model.difficultyLerp > 1.0f)
            {
                model.difficultyLerp = 1.0f;

                // The player is rocking it!
            }

            // ...

            view.onDifficultyUpdate.Invoke(model.difficultyLerp);
        }

        void OnFrustated()
        {
            Debug.Log("Player is frustrated! Let's reduce difficulty");

            model.difficultyLerp -= model.difficultyStep;

            if (model.difficultyLerp < 0.0f)
            {
                model.difficultyLerp = 0.0f;

                // If the gamemode cannot be more "easified", should we consider that the current activity for the current letter groups is forgotten?
            }

            // ...

            view.onDifficultyUpdate.Invoke(model.difficultyLerp);
        }

        void OnForgottenSkill(Activity activity)
        {
            Debug.Log($"Player has forgotten the skill related to gamemode {activity.ToString()}! Let's stop time and start minigame for that gamemode");

            // ...
        }

        void OnSceneChange(string nextScene)
        {
            Debug.Log("Scene will change in next frame. Save values!");

            //EngagementView.singleton.viewModel.SetDifficultyLerp(model.activity, model.currentLetterGroup, model.difficultyLerp);

            // ...

        }
    }
 
}
