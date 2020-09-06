using UnityEngine;
using UnityHelpers;
using System.Collections.Generic;
using System.Linq;

public class LettersSpawner : MonoBehaviour
{
    public string lettersPoolName = "Letters";
    public SpawnPicker spawnPicker;
    [Tooltip("How many letter pairs to spawn (make sure there are enough spawn points in SpawnPicker)")]
    public int letterPairs = 4;
    [Tooltip("The indices of the letters that can be spawned (0-25)")]
    public List<int> spawnableIndices = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6 });
    
    private List<int> pickedLetters = new List<int>();
    private ObjectPool<Transform> lettersPool;
    private List<UnifiedLetter> spawnedLetters = new List<UnifiedLetter>();

    void Start()
    {
        lettersPool = PoolManager.GetPool(lettersPoolName);
    }

    public UnifiedLetter[] Respawn()
    {
        Clear();

        for (int i = 0; i < letterPairs; i++)
        {
            var uppercaseLetter = lettersPool.Get<UnifiedLetter>();
            var lowercaseLetter = lettersPool.Get<UnifiedLetter>();
            spawnedLetters.Add(uppercaseLetter);
            spawnedLetters.Add(lowercaseLetter);

            int letterIndex = PickRandomLetter();
            Transform uppercaseAnchor = spawnPicker.GetSpawnPoint();
            Transform lowercaseAnchor = spawnPicker.GetSpawnPoint();

            uppercaseLetter.transform.position = uppercaseAnchor.position;
            uppercaseLetter.SetLetter(letterIndex);
            uppercaseLetter.SetCase(true);
            uppercaseLetter.SetPhysicsFollow(uppercaseAnchor);

            lowercaseLetter.transform.position = lowercaseAnchor.position;
            lowercaseLetter.SetLetter(letterIndex);
            lowercaseLetter.SetCase(false);
            lowercaseLetter.SetPhysicsFollow(lowercaseAnchor);
        }

        return spawnedLetters.ToArray();
    }
    private int PickRandomLetter()
    {
        int letterIndex = -1;
        if (pickedLetters.Count != spawnableIndices.Count)
        {
            List<int> availableLetters = new List<int>(spawnableIndices);
            for (int i = pickedLetters.Count - 1; i >= 0; i--)
                availableLetters.Remove(pickedLetters[i]);
            
            int randomIndex = Random.Range(0, availableLetters.Count);
            letterIndex = availableLetters[randomIndex];
            pickedLetters.Add(availableLetters[randomIndex]);
        }

        return letterIndex;
    }
    public void UnpairAll()
    {
        foreach (var spawnedLetter in spawnedLetters)
        {
            if (spawnedLetter is PairedLetter)
                ((PairedLetter)spawnedLetter).Unpair();

            var letterGrabbable = spawnedLetter.GetComponentInParent<IGrabbable>();
            if (letterGrabbable != null)
                letterGrabbable.UngrabAll();
            else
                Debug.LogError("LettersSpawner: Could not ungrab letter, missing IGrabbable component");
        }
    }
    public void Clear()
    {
        UnpairAll();
        foreach (var spawnedLetter in spawnedLetters)
            lettersPool.Return(spawnedLetter.transform);

        spawnedLetters.Clear();
        pickedLetters.Clear();
        spawnPicker.Clear();
    }
}
