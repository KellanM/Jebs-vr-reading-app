using JebsReadingGame.Systems.Gamemode;
using JebsReadingGame.Systems.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JebsReadingGame.Systems.Currency;
using JebsReadingGame.Globals;
using TMPro;
using JebsReadingGame.Helpers;
using JebsReadingGame.Systems.Dialogue;
using JebsReadingGame.Letters;
using JebsReadingGame.Systems.Progression;
using JebsReadingGame.Systems.Engagement;
using JebsReadingGame.Systems.Learning;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JebsReadingGame.Systems.Dialogue
{
    public class DialogueController : MonoBehaviour
    {
        public DialogueModel model;
        public DialogueView view;

        [Header("Debug")]
        [TextArea]
        public string log;
        public string inbox;
        public TextMeshPro logPanel;

        private void Awake()
        {
            view.viewModel = new DialogueView.DialogueViewModel(model);
            model.view = view;
        }

        private void Start()
        {
            // Wwise events for every gamemode event
            GamemodeView.singleton.onLetterGroupWin.AddListener(OnLetterGroupWin);
            GamemodeView.singleton.onLetterGroupFail.AddListener(OnLetterGroupFail);
            GamemodeView.singleton.onLetterWin.AddListener(OnLetterWin);
            GamemodeView.singleton.onLetterFail.AddListener(OnLetterFail);
            GamemodeView.singleton.onGamemodeRepaired.AddListener(OnGamemodeRepaired);
            GamemodeView.singleton.onSceneChangeRequest.AddListener(OnSceneChangeRequest);
            GamemodeView.singleton.onTip.AddListener(OnTip);
            GamemodeView.singleton.onSkillWin.AddListener(OnSkillWin);
            GamemodeView.singleton.onSkillFail.AddListener(OnSkillFail);
            GamemodeView.singleton.onPositiveLetterGroupStreak.AddListener(OnPositiveLetterGroupStreak);
            GamemodeView.singleton.onNegativeLetterGroupStreak.AddListener(OnNegativeLetterGroupStreak);

            // Wwise events for every engagement event
            EngagementView.singleton.onBored.AddListener(OnBored);
            EngagementView.singleton.onEngaged.AddListener(OnEngaged);
            EngagementView.singleton.onFrustrated.AddListener(OnFrustrated);
            EngagementView.singleton.onDistractorRequired.AddListener(OnDistractorRequired);

            // Wwise events for every learning event
            LearningView.singleton.onForgottenSkill.AddListener(OnForgottenSkill);
            LearningView.singleton.onLevelMastered.AddListener(OnLevelMastered);
            LearningView.singleton.onNewHighestStreak.AddListener(OnNewHighestStreak);
            LearningView.singleton.onNewLowestStreak.AddListener(OnNewLowestStreak);

            // Wwise events for every progression event
            ProgressionView.singleton.onLevelUp.AddListener(OnLevelUp);
            ProgressionView.singleton.onGamemodeUnlocked.AddListener(OnGamemodeUnlocked);
            ProgressionView.singleton.onGamemodeGroupUnlocked.AddListener(OnGamemodeGroupUnlocked);
            ProgressionView.singleton.onGamemodeBroken.AddListener(OnGamemodeBroken);
            ProgressionView.singleton.onGameFinished.AddListener(OnGameFinished);

            // Wwise events for every currency event
            CurrencyView.singleton.onCoinsEarned.AddListener(OnCoinsEarned);
        }

        private void Update()
        {
            UpdateLog();   
            if (logPanel) logPanel.text = log;

            model.letterGroupState = "letterGroup" + ProgressionView.singleton.viewModel.currentLetterGroup.ToString();
        }

        void OnLetterGroupWin(Activity activity, LetterGroup letterGroup)
        {
            if (model.onLetterGroupWin != null) model.onLetterGroupWin.Post(model.postEventsHere);
        }

        void OnLetterGroupFail(Activity activity, LetterGroup letterGroup)
        {
            if (model.onLetterGroupFail != null) model.onLetterGroupFail.Post(model.postEventsHere);
        }

        void OnLetterWin(Activity activity, char letter)
        {
            if (model.onLetterWin != null) model.onLetterWin.Post(model.postEventsHere);
        }

        void OnLetterFail(Activity activity, char letter)
        {
            if (model.onLetterFail != null) model.onLetterFail.Post(model.postEventsHere);
        }

        void OnSkillWin(Activity activity)
        {
            if (model.onSkillWin != null) model.onSkillWin.Post(model.postEventsHere);
        }

        void OnSkillFail(Activity activity)
        {
            if (model.onSkillFail != null) model.onSkillFail.Post(model.postEventsHere);
        }

        void OnTip()
        {
            if (model.onTip != null) model.onTip.Post(model.postEventsHere);
        }

        void OnPositiveLetterGroupStreak(Activity activity, LetterGroup letterGroup, int streak)
        {
            if (model.onPositiveLetterGroupStreakCompleted != null) model.onPositiveLetterGroupStreakCompleted.Post(model.postEventsHere);
        }

        void OnNegativeLetterGroupStreak(Activity activity, LetterGroup letterGroup, int streak)
        {
            if (model.onNegativeLetterGroupStreakCompleted != null) model.onNegativeLetterGroupStreakCompleted.Post(model.postEventsHere);
        }

        void OnGamemodeRepaired(GamemodeGroup group, Progression.Gamemode gamemode)
        {
            if (model.onGamemodeRepaired != null) model.onGamemodeRepaired.Post(model.postEventsHere);
        }

        void OnSceneChangeRequest(string newScene)
        {
            if (model.onSceneChangeRequest != null) model.onSceneChangeRequest.Post(model.postEventsHere);
        }

        void OnBored()
        {
            if (model.onBored != null) model.onBored.Post(model.postEventsHere);
        }

        void OnEngaged()
        {
            if (model.onEngaged != null) model.onEngaged.Post(model.postEventsHere);
        }

        void OnFrustrated()
        {
            if (model.onFrustrated != null) model.onFrustrated.Post(model.postEventsHere);
        }

        void OnDistractorRequired()
        {
            if (model.onDistractorRequired != null) model.onDistractorRequired.Post(model.postEventsHere);
        }

        void OnForgottenSkill(Activity activity, LetterGroup letterGroup)
        {
            if (model.onForgottenSkill != null) model.onForgottenSkill.Post(model.postEventsHere);
        }

        void OnLevelMastered(Activity activity, LetterGroup letterGroup)
        {
            if (model.onLevelMastered != null) model.onLevelMastered.Post(model.postEventsHere);
        }

        void OnNewHighestStreak(Activity activity, LetterGroup letterGroup, int streak)
        {
            if (model.onNewHighestStreak != null) model.onNewHighestStreak.Post(model.postEventsHere);
        }

        void OnNewLowestStreak(Activity activity, LetterGroup letterGroup, int streak)
        {
            if (model.onNewLowestStreak != null) model.onNewLowestStreak.Post(model.postEventsHere);
        }

        void OnLevelUp(GamemodeGroup gorup, Progression.Gamemode gamemode, Level level)
        {
            if (model.onLevelUp != null) model.onLevelUp.Post(model.postEventsHere);
        }

        void OnGamemodeUnlocked(GamemodeGroup gorup, Progression.Gamemode gamemode)
        {
            if (model.onGamemodeUnlocked != null) model.onGamemodeUnlocked.Post(model.postEventsHere);
        }

        void OnGamemodeGroupUnlocked(GamemodeGroup gorup)
        {
            if (model.onGamemodeGroupUnlocked != null) model.onGamemodeGroupUnlocked.Post(model.postEventsHere);
        }

        void OnGamemodeBroken(GamemodeGroup gorup, Progression.Gamemode gamemode)
        {
            if (model.onGamemodeBroken != null) model.onGamemodeBroken.Post(model.postEventsHere);
        }

        void OnGameFinished()
        {
            if (model.onGameFinished != null) model.onGameFinished.Post(model.postEventsHere);
        }

        void OnCoinsEarned(int coins)
        {
            if (model.onCoinsEarned != null) model.onCoinsEarned.Post(model.postEventsHere);
        }

        void UpdateLog()
        {
            log = "Dialogue System\n"
            + "(...)";
        }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(DialogueController))]
public class DialogueSystemControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DialogueController myScript = (DialogueController)target;

        /*
        if (GUILayout.Button("BUTTON"))
        {
            // ...
        }
        */
    }
}
#endif

