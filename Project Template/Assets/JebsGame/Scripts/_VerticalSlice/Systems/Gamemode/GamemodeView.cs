using JebsReadingGame.Skeletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using JebsReadingGame.Events;
using JebsReadingGame.Globals;
using JebsReadingGame.Log;
using JebsReadingGame.Systems.Learning;

namespace JebsReadingGame.Systems.Gamemode
{
    public class GamemodeView : GlobalView
    {
        // Singleton
        static GamemodeView _singleton;
        public static GamemodeView singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = GameObject.FindObjectOfType<GamemodeView>();

                return _singleton;
            }
        }

        // ViewModel
        public sealed class GamemodeViewModel
        {
            GamemodeModel model;

            // Properties
            public int persistentValue { get { return model.persistent.persistentValue; } } // placeholder
            // public int countLetterGroupStreakAfter { get { return model.asset.countLetterGroupStreakAfter; } }

            public Activity activity { get { return model.activity; } }

            public int currentLetterGroupStreak { get { return model.currentLetterGroupStreak; } set { model.currentLetterGroupStreak = value; } }

            // public Item nextEquipableItem { get { return model.nextEquipableItem; } } -> Store system is not implemented yet

            public int gameplayLetterWins { get { return model.gameplayLetterWins; } set { model.gameplayLetterWins = value; } }
            public int gameplayLetterFails { get { return model.gameplayLetterFails; } set { model.gameplayLetterFails = value; } }
            public int gameplayLetterGroupWins { get { return model.gameplayLetterGroupWins; } set { model.gameplayLetterGroupWins = value; } }
            public int gameplayLetterGroupFails { get { return model.gameplayLetterGroupFails; } set { model.gameplayLetterGroupFails = value; } }

            // Constructor
            public GamemodeViewModel(GamemodeModel model)
            {
                this.model = model;
            }
        }
        public GamemodeViewModel viewModel;

        // Events
        public ActivityLetterGroupEvent onLetterGroupWin = new ActivityLetterGroupEvent();
        public ActivityLetterGroupEvent onLetterGroupFail = new ActivityLetterGroupEvent();

        public ActivityLetterEvent onLetterWin = new ActivityLetterEvent();
        public ActivityLetterEvent onLetterFail = new ActivityLetterEvent();

        public ActivityEvent onSkillWin = new ActivityEvent();
        public ActivityEvent onSkillFail = new ActivityEvent();

        public UnityEvent onTip = new UnityEvent(); // Small optional help

        public StreakEvent onPositiveLetterGroupStreak = new StreakEvent();
        public StreakEvent onNegativeLetterGroupStreak = new StreakEvent();

        public GamemodeEvent onGamemodeRepaired = new GamemodeEvent();

        public StringEvent onSceneChangeRequest = new StringEvent();

        // Function
        public void DoLetterGroupWin(Activity activity, LetterGroup letterGroup)
        {
            viewModel.gameplayLetterGroupWins++;

            onLetterGroupWin.Invoke(activity, letterGroup);

            LogService.singleton.Log("LETTER GROUP WIN! Activity: " + activity.ToString() + " - Letter Group: " + letterGroup.ToString());

            if (viewModel.currentLetterGroupStreak < 0)
            {
                onNegativeLetterGroupStreak.Invoke(activity, letterGroup, viewModel.currentLetterGroupStreak);
                viewModel.currentLetterGroupStreak = 1;
            }
            else
            {
                viewModel.currentLetterGroupStreak++;

                if (viewModel.currentLetterGroupStreak > LearningView.singleton.viewModel.currentLetterGroupLearning.highestLetterGroupStreak)
                {
                    onPositiveLetterGroupStreak.Invoke(activity, letterGroup, viewModel.currentLetterGroupStreak);
                    viewModel.currentLetterGroupStreak = 0;
                }
            }
        }

        public void DoLetterGroupFail(Activity activity, LetterGroup letterGroup)
        {
            viewModel.gameplayLetterGroupFails++;

            onLetterGroupFail.Invoke(activity, letterGroup);

            LogService.singleton.Log("LETTER GROUP FAIL! Activity: " + activity.ToString() + " - Letter Group: " + letterGroup.ToString());

            if (viewModel.currentLetterGroupStreak > 0)
            {
                onPositiveLetterGroupStreak.Invoke(activity, letterGroup, viewModel.currentLetterGroupStreak);
                viewModel.currentLetterGroupStreak = -1;
            }
            else
            {
                viewModel.currentLetterGroupStreak--;

                if (viewModel.currentLetterGroupStreak < LearningView.singleton.viewModel.currentLetterGroupLearning.lowestLetterGroupStreak)
                {
                    onNegativeLetterGroupStreak.Invoke(activity, letterGroup, viewModel.currentLetterGroupStreak);
                    viewModel.currentLetterGroupStreak = 0;
                }
            }
        }

        public void DoLetterWin(Activity activity, LetterGroup letterGroup, char letter)
        {
            onLetterWin.Invoke(activity, letter);

            viewModel.gameplayLetterWins++;

            if (viewModel.currentLetterGroupStreak < 0)
            {
                // onNegativeLetterGroupStreak.Invoke(activity, letterGroup, viewModel.currentLetterGroupStreak);
                viewModel.currentLetterGroupStreak = 0;
            }

            LogService.singleton.Log("LETTER WIN! Activity: " + activity.ToString() + " - Letter: " + letter.ToString());
        }

        public void DoLetterFail(Activity activity, LetterGroup letterGroup, char letter)
        {
            onLetterFail.Invoke(activity, letter);

            viewModel.gameplayLetterFails++;

            if (viewModel.currentLetterGroupStreak > 0)
            {
                // onPositiveLetterGroupStreak.Invoke(activity, letterGroup, viewModel.currentLetterGroupStreak);
                viewModel.currentLetterGroupStreak = 0;
            }

            LogService.singleton.Log("LETTER WIN! Activity: " + activity.ToString() + " - Letter: " + letter.ToString());
        }

        public void DoSkillWin(Activity activity)
        {
            // ...
            onSkillWin.Invoke(activity);
        }

        public void DoSkillFail(Activity activity)
        {
            // ...
            onSkillFail.Invoke(activity);          
        }

    }
}
