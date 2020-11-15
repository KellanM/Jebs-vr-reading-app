using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.System.Currency
{
    // Scene-specific model
    public class CurrencyModel : MonoBehaviour
    {
        [HideInInspector]
        public CurrencyView view;

        public CurrencyConfigurationAsset asset;

        public CurrencyPersistent persistent = new CurrencyPersistent();

        public int streakCoins = 5;

        // Not needed yet
        /*
            public CurrencySystemAsset asset;
        */
    }

    // Persistent model: Persistent between scenes and launches
    public class CurrencyPersistent
    {
        string totalCoinsKey = "totalCoins";

        public int totalCoins;

        internal void LoadValues()
        {
            totalCoins = PlayerPrefs.GetInt(totalCoinsKey);
        }
        internal void SaveValues()
        {
            PlayerPrefs.SetInt(totalCoinsKey, totalCoins);
        }
    }

    // Configuration model: Can be shared between models. Cannot be modified in build
    [CreateAssetMenu(menuName = "JesbReadingGame/Currency Configuration Asset", order = 1)]
    public class CurrencyConfigurationAsset : ScriptableObject
    {
        public int configurationValue = 0;
    }

}
