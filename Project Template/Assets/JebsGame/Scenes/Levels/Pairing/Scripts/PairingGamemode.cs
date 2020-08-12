using UnityEngine;
using System.Linq;
using TMPro;
using UnityHelpers;

public class PairingGamemode : MonoBehaviour
{
    public LettersSpawner lettersSpawner;
    public float gameTime = 25;
    public TextMeshProUGUI timeLeftLabel;
    private float gameStartedTime = float.MinValue;
    private PairedLetter[] spawnedLetters;

    void Update()
    {
        if (Time.time - gameStartedTime > gameTime || AreAllLettersMatched())
            RestartGame();

        timeLeftLabel.text = MathHelpers.SetDecimalPlaces((gameTime - (Time.time - gameStartedTime)), 1).ToString();
    }

    public void RestartGame()
    {
        if (spawnedLetters != null)
            foreach (var spawnedLetter in spawnedLetters)
                spawnedLetter.onPaired.RemoveListener(OnLetterPaired);

        spawnedLetters = lettersSpawner.Respawn().Cast<PairedLetter>().ToArray();
        foreach (var spawnedLetter in spawnedLetters)
            spawnedLetter.onPaired.AddListener(OnLetterPaired);
        gameStartedTime = Time.time;
    }

    public bool AreAllLettersMatched()
    {
        bool allMatched = true;
        if (spawnedLetters != null)
        {
            foreach (var spawnedLetter in spawnedLetters)
            {
                if (spawnedLetter.gameObject.activeSelf && !spawnedLetter.IsPaired())
                {
                    allMatched = false;
                    break;
                }
            }
        }
        else
            allMatched = false;

        return allMatched;
    }

    private void OnLetterPaired(UnifiedLetter caller, UnifiedLetter other)
    {
        if (caller.GetCurrentIndex() != other.GetCurrentIndex() || caller.IsUppercase() == other.IsUppercase())
        {
            //Paired something that is not the same letter or has the same case so disconnect
            //caller.GetComponentInParent<MagneticPairing>().Disconnect();
            lettersSpawner.UnpairAll();
        }
    }
}
