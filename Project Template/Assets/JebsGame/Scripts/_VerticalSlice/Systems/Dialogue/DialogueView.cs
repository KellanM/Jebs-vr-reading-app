using JebsReadingGame.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JebsReadingGame.Systems.Dialogue
{
    public class DialogueView : MonoBehaviour
    {
        // Singleton
        static DialogueView _singleton;
        public static DialogueView singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = GameObject.FindObjectOfType<DialogueView>();

                return _singleton;
            }
        }

        // ViewModel
        public sealed class DialogueViewModel
        {
            DialogueModel model;

            // Properties
            public string letterGroupState { get { return model.letterGroupState; } }
            public float hintWaitTime { get { return model.hintWaitTime; } }
            public List<TutorialLine> tutorial { get { return model.tutorial; } }

            // Constructor
            public DialogueViewModel(DialogueModel model)
            {
                this.model = model;
            }
        }
        public DialogueViewModel viewModel;

        // Events
        // ...
    }
}
