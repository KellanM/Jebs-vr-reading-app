using JesbReadingGame.Skeletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using JebsReadingame.Events;
using JebsReadingGame.Globals;

namespace JebsReadingGame.System.Gamemode
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
            public int configurationValue { get { return model.asset.configurationValue; } } // placeholder

            public Activity activity { get { return model.activity; } }
            public LetterGroup currentLetterGroupCode { get { return model.currentLetterGroup; } }

            public int streakLength { get { return model.streakLength; } }
            public int currentCombo { get { return model.currentCombo; } }

            // public Item nextEquipableItem { get { return model.nextEquipableItem; } } -> Store system is not implemented yet

            public int gameplayLetterWins { get { return model.gameplayLetterWins; } }
            public int gameplayLetterFails { get { return model.gameplayLetterFails; } }
            public int gameplayLetterGroupWins { get { return model.gameplayLetterGroupWins; } }
            public int gameplayLetterGroupFails { get { return model.gameplayLetterGroupFails; } }

            public string[] availableScenes { get { return model.availableScenes; } }

            // Constructor
            public GamemodeViewModel(GamemodeModel model)
            {
                this.model = model;
            }
        }
        public GamemodeViewModel viewModel;

        // Events
        public IntIntEvent onLetterGroupWin = new IntIntEvent();
        public IntIntEvent onLetterGroupFail = new IntIntEvent();
        public IntCharEvent onLetterWin = new IntCharEvent();
        public IntCharEvent onLetterFail = new IntCharEvent();

        public UnityEvent onSkillWin = new UnityEvent();
        public UnityEvent onSkillFail = new UnityEvent();

        public UnityEvent onPositiveStreakCompleted = new UnityEvent();
        public UnityEvent onPositiveStreakBroken = new UnityEvent();
        public UnityEvent onNegativeStreakCompleted = new UnityEvent();
        public UnityEvent onNegativeStreakBroken = new UnityEvent();

        public FloatEvent onDifficultyUpdate = new FloatEvent();

        public StringEvent onSceneChangeRequest = new StringEvent();
    }
}
