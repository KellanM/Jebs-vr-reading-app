using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneB : MonoBehaviour
{
    public CoinsPanel coinsPanel;

    void Start()
    {
        coinsPanel.UpdateCoinsCounter();
    }
}
