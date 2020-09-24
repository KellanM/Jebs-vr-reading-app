using UnityEngine;
using UnityHelpers;

public class UnifiedLetter : MonoBehaviour
{
    public Letter[] availableLetters;

    private int currentIndex = -1;
    private bool isUppercase;

    public void SetLetter(int index)
    {
        currentIndex = index;
        for (int i = 0; i < availableLetters.Length; i++)
            availableLetters[i].gameObject.SetActive(i == currentIndex);
    }
    public void SetCase(bool uppercase)
    {
        isUppercase = uppercase;
        if (currentIndex >= 0 && currentIndex < availableLetters.Length)
        {
            var letterCases = availableLetters[currentIndex].GetComponent<letter_meshes>();
            if (letterCases != null)
            {
                letterCases.upperCaseMesh.SetActive(isUppercase);
                letterCases.lowerCaseMesh.SetActive(!isUppercase);
            }
            else
                Debug.LogError("Could not find letter_meshes component on object " + availableLetters[currentIndex].name);
        }
    }
    public void SetPhysicsFollow(Transform followTransform)
    {
        var letterPhysics = GetComponentInParent<MimicTransform>();
        //var letterPhysics = GetComponentInParent<PhysicsTransform>();
        if (letterPhysics != null)
            letterPhysics.other = followTransform;
        else
            Debug.LogError("UnifiedLetter: Could not find PhysicsTransform component on letter " + name);
    }

    public int GetCurrentIndex()
    {
        return currentIndex;
    }
    public bool IsUppercase()
    {
        return isUppercase;
    }
    public bool IsLetterSet()
    {
        return GetCurrentIndex() >= 0;
    }

    public void Clear()
    {
        SetLetter(-1);
        SetCase(true);
    }
}
