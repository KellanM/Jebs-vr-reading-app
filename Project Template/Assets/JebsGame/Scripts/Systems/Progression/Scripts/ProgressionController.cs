using JesbReadingGame.Skeletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.System.Progression
{
    public class ProgressionController : GlobalController
    {
        public ProgressionModel model;
        public ProgressionView view;

        private void Awake()
        {
            if (!model)
                model = GetComponent<ProgressionModel>();

            if (!view)
                view = GetComponent<ProgressionView>();

            if (!model || !view)
            {
                Debug.LogError("Missing system components!");
                gameObject.SetActive(false);
            }

            // Components connection
            view.viewModel = new ProgressionView.ProgressionViewModel(model);

            model.view = view; // Optional
        }

        private void Start()
        {
            // ...
        }

        // ...

    }

}