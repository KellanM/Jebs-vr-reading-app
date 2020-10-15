using JebsReadingGame.GamemodeManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JebsReadingGame.CurrencySystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JebsReadingGame.CurrencySystem
{
    public class CurrencySystemController : MonoBehaviour
    {
        public CurrencySystemModel model;
        public CurrencySystemView view;

        private void Awake()
        {
            view.viewModel = new CurrencySystemView.ViewModel(model);
            model.view = view;
        }

        private void Start()
        {
            // Subscribe to events
            GamemodeManagerView.singleton.onPositiveStreak.AddListener(EarnCoins);
        }

        void EarnCoins()
        {
            model.persistent.totalCoins += model.streakCoins;
            view.onCoinsEarned.Invoke(model.streakCoins);
        }

        public void ResetTotalCoins()
        {
            model.persistent.totalCoins = 0;
        }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(CurrencySystemController))]
public class CurrencySystemControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CurrencySystemController myScript = (CurrencySystemController)target;
        if (GUILayout.Button("RESET TOTAL COINS"))
        {
            myScript.ResetTotalCoins();
        }
    }
}
#endif

