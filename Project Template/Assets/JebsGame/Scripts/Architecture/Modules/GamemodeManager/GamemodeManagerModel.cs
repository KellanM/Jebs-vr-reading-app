using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.GamemodeManager
{
    // Scene-specific model (unique for each gamemode)
    public class GamemodeManagerModel : MonoBehaviour
    {
        [HideInInspector]
        public GamemodeManagerView view;

        public int streakLength = 3;

        // Not needed yet
        /*
            public GamemodeManagerAsset asset;
            public GamemodeManagerPersistent persistent = new GamemodeManagerPersistent();
        */
    }
}
