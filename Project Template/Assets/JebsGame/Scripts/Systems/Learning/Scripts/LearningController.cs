using JesbReadingGame.Skeletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.System.Learning
{
    public class LearningController : GlobalController
    {
        public LearningModel model;
        public LearningView view;

        private void Awake()
        {
            if (!model)
                model = GetComponent<LearningModel>();

            if (!view)
                view = GetComponent<LearningView>();

            if (!model || !view)
            {
                Debug.LogError("Missing system components!");
                gameObject.SetActive(false);
            }

            // Components connection
            view.viewModel = new LearningView.LearningViewModel(model);

            model.view = view; // Optional
        }

        private void Start()
        {
            // ...
        }

        // ...

    }

}