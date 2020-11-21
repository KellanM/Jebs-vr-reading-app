using JesbReadingGame.Skeletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using JebsReadingame.Events;
using System;
using JebsReadingGame.Globals;

namespace JebsReadingGame.System.Scene
{
    public class SceneView : GlobalView
    {
        // Singleton
        static SceneView _singleton;
        public static SceneView singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = GameObject.FindObjectOfType<SceneView>();

                return _singleton;
            }
        }

        // ViewModel
        public sealed class SceneViewModel
        {
            SceneModel model;

            // Properties
            public int persistentValue { get { return model.persistent.persistentValue; } }
            public int configurationValue { get { return model.asset.configurationValue; } } // placeholder

            public int currentScene { get { return model.currentScene; } }
            public SceneState state { get { return model.state; } }
            public float timeScale { get { return model.timeScale; } }

            // Constructor
            public SceneViewModel(SceneModel model)
            {
                this.model = model;
            }
        }

        public SceneViewModel viewModel;

        // Events
        public StringEvent onSceneChange = new StringEvent();
        public SceneStateEvent onStateChange = new SceneStateEvent();
        public FloatEvent onTimeScaleChange = new FloatEvent();

    }
}