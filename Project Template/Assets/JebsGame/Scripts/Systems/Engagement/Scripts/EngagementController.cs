using JesbReadingGame.Skeletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.System.Engagement
{
    public class EngagementController : GlobalController
    {
        public EngagementModel model;
        public EngagementView view;

        private void Awake()
        {
            if (!model)
                model = GetComponent<EngagementModel>();

            if (!view)
                view = GetComponent<EngagementView>();

            if (!model || !view)
            {
                Debug.LogError("Missing system components!");
                gameObject.SetActive(false);
            }

            // Components connection
            view.viewModel = new EngagementView.EngagementViewModel(model);

            model.view = view; // Optional
        }

        private void Start()
        {
            // ...
        }

        // ...

    }
  
}
