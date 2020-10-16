using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.CurrencySystem
{
    // Scene-specific model
    public class CurrencySystemModel : MonoBehaviour
    {
        [HideInInspector]
        public CurrencySystemView view;

        public CurrencySystemPersistent persistent = new CurrencySystemPersistent();

        public int streakCoins = 5;

        // Not needed yet
        /*
            public CurrencySystemAsset asset;
        */
    }

    // Persistent model: Persistent between scenes and launches
    public class CurrencySystemPersistent
    {
        string totalCoinsKey = "totalCoins";

        public int totalCoins
        {
            get { return PlayerPrefs.GetInt(totalCoinsKey); }
            set { PlayerPrefs.SetInt(totalCoinsKey, value); }
        }
    }

}
