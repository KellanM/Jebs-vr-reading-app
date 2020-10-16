using JebsReadingGame.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JebsReadingGame.GamemodeManager
{
    public class GamemodeManagerView : MonoBehaviour
    {
        // Singleton
        static GamemodeManagerView _singleton;
        public static GamemodeManagerView singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = GameObject.FindObjectOfType<GamemodeManagerView>();

                return _singleton;
            }
        }

        // ViewModel
        public sealed class ViewModel
        {
            GamemodeManagerModel model;

            // Properties
            public int streakLength { get { return model.streakLength; } }

            // Constructor
            public ViewModel(GamemodeManagerModel model)
            {
                this.model = model;
            }
        }
        public ViewModel viewModel;

        // Events
        public UnityEvent onPositiveStreak;
    }
}
