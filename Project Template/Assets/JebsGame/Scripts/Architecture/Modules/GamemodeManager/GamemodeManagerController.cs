using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.GamemodeManager
{
    public class GamemodeManagerController : MonoBehaviour
    {
        public GamemodeManagerModel model;
        public GamemodeManagerView view;

        private void Awake()
        {
            view.viewModel = new GamemodeManagerView.ViewModel(model);
            model.view = view;
        }

        private void Start()
        {
            // Subscribe to events
        }
    }
}
