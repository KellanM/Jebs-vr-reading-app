using UnityEngine;

[System.Serializable]
public class PairEvent : UnityEngine.Events.UnityEvent<UnifiedLetter, UnifiedLetter> {}

public class PairedLetter : UnifiedLetter
{
    public UnifiedLetter pairedLetterStandIn;
    public UnifiedLetter objectPairedWith;

    [Space(10)]
    public PairEvent onPaired;
    public PairEvent onUnpaired;

    public void PairWith(UnifiedLetter other)
    {
        objectPairedWith = other;
        other.gameObject.SetActive(false);
        pairedLetterStandIn.SetLetter(other.GetCurrentIndex());
        pairedLetterStandIn.SetCase(other.IsUppercase());

        onPaired?.Invoke(this, objectPairedWith);
    }
    public void Unpair()
    {
        pairedLetterStandIn.Clear();
        if (objectPairedWith != null)
        {
            objectPairedWith.gameObject.SetActive(true);
            onUnpaired?.Invoke(this, objectPairedWith);
            objectPairedWith = null;
        }
    }
    public bool IsPaired()
    {
        return pairedLetterStandIn.IsLetterSet();
    }
}
