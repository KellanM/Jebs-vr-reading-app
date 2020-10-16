using JebsReadingGame.CurrencySystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinsPanel : MonoBehaviour
{
    public TextMeshPro tmpro;

    public void UpdateCoinsCounter()
    {
        tmpro.text = "Coins: " + CurrencySystemView.singleton.viewModel.totalCoins;
    }
}
