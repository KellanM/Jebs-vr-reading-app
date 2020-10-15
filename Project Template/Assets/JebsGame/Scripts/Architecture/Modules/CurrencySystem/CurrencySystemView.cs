using JebsReadingGame.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JebsReadingGame.CurrencySystem
{
    public class CurrencySystemView : MonoBehaviour
    {
        // Singleton
        static CurrencySystemView _singleton;
        public static CurrencySystemView singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = GameObject.FindObjectOfType<CurrencySystemView>();

                return _singleton;
            }
        }

        // ViewModel
        public sealed class ViewModel
        {
            CurrencySystemModel model;

            // Properties
            public int totalCoins { get { return model.persistent.totalCoins; } }
            public int streakCoins { get { return model.streakCoins; } }

            // Constructor
            public ViewModel(CurrencySystemModel model)
            {
                this.model = model;
            }
        }
        public ViewModel viewModel;

        // Events
        public IntEvent onCoinsEarned;
    }
}
