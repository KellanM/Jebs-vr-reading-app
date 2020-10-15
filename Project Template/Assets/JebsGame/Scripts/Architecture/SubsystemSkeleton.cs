using JebsReadingGame.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JebsReadingGame.Abstracts
{
    // Persistent model: Persistent between scenes and launches
    public class Persistent
    {
        string persistentValueKey = "persistent";

        public int persistentValue
        {
            get { return PlayerPrefs.GetInt(persistentValueKey); }
            set { PlayerPrefs.SetInt(persistentValueKey, value); }
        }
    }

    // Configuration model: Can be shared between models. Cannot be modified in build
    [CreateAssetMenu(menuName = "JesbReadingGame/Skeleton Asset", order = 1)]
    public class Asset : ScriptableObject
    {
        public int configurationValue = 0;
    }

    // Scene-specific model
    public abstract class Model : MonoBehaviour
    {
        [HideInInspector]
        public View view;

        public Asset asset;

        public Persistent persistent = new Persistent();

        public float readonlyValue = 0.0f;
        public float modifiableValue = 0.0f;
    }

    // View
    public abstract class View : MonoBehaviour
    {
        // Singleton
        static View _singleton;
        public static View singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = GameObject.FindObjectOfType<View>();

                return _singleton;
            }
        }

        // ViewModel
        public sealed class ViewModel
        {
            Model model;

            // Properties
            public int persistentValue { get { return model.persistent.persistentValue; } }
            public int configurationValue { get { return model.asset.configurationValue; } }
            public float readonlyValue { get { return model.readonlyValue; } }
            public float modifiableValue
            {
                get { return model.modifiableValue; }
                set
                {
                    model.modifiableValue = value;
                    model.view.onValueChanged.Invoke(value);
                }
            }

            // Constructor
            public ViewModel(Model model)
            {
                this.model = model;
            }
        }
        public ViewModel viewModel;

        // Events
        public FloatEvent onValueChanged;
        public UnityEvent onOtherEvent;
    }

    // Controller
    public class Controller : MonoBehaviour
    {
        public Model model;
        public View view;

        private void Awake()
        {
            view.viewModel = new View.ViewModel(model);
            model.view = view;
        }

        private void Start()
        {
            // Subscribe to events
        }
    }
}