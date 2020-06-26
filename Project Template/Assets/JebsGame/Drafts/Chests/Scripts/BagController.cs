using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using Zinnia.Action;

public class BagController : MonoBehaviour
{
    public static BagController bag;

    public char searchForLetter = 'a';

    public UnityEvent positiveFeedback;
    public UnityEvent negativeFeedback;

    CrabFactory factory;

    private void Awake()
    {
        if (!bag) bag = this;
    }

    private void Start()
    {
        factory = CrabFactory.factory;
    }

    private void OnTriggerStay(Collider other)
    {
        ChestLetter letter = other.GetComponentInParent<ChestLetter>();
        letter.transform.parent = transform;

        if (letter && letter.currentInteractor == null)
        {
            Evaluate(letter,true);

            letter.gameObject.SetActive(false);
            //Destroy(letter.gameObject);
        }
    }

    public void Evaluate(ChestLetter letter, bool accepted)
    {
        if (letter.value == searchForLetter && accepted)
            positiveFeedback.Invoke();
        else if (letter.value != searchForLetter && !accepted)
            positiveFeedback.Invoke();
        else
            negativeFeedback.Invoke();

        factory.Restart();
    }
}
