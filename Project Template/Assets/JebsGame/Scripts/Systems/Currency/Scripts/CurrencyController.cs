using JebsReadingGame.System.Gamemode;
using JebsReadingGame.System.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JebsReadingGame.System.Currency;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JebsReadingGame.System.Currency
{
    public class CurrencyController : MonoBehaviour
    {
        public CurrencyModel model;
        public CurrencyView view;

        private void Awake()
        {
            view.viewModel = new CurrencyView.CurrencyViewModel(model);
            model.view = view;
            model.persistent.LoadValues();
        }

        private void Start()
        {
            // Subscribe to events
            GamemodeView.singleton.onPositiveStreakCompleted.AddListener(EarnCoins);
            Scene.SceneView.singleton.onSceneChange.AddListener(SceneChanged);
        }

        void SceneChanged(string sceneName)
        {
            model.persistent.SaveValues();
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
[CustomEditor(typeof(CurrencyController))]
public class CurrencySystemControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CurrencyController myScript = (CurrencyController)target;
        if (GUILayout.Button("RESET TOTAL COINS"))
        {
            myScript.ResetTotalCoins();
        }
    }
}
#endif

