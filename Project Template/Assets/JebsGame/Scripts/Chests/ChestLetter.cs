using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChestLetter : MonoBehaviour
{
    public char value;

    public XRBaseInteractor currentInteractor;
    XRBaseInteractor previousInteractor;

    XRGrabInteractable interactable;

    private void Start()
    {
        interactable = GetComponent<XRGrabInteractable>();
        interactable.onSelectEnter.AddListener(SetGrabbed);
        interactable.onSelectExit.AddListener(SetUngrabbed);
    }

    public void SetGrabbed(XRBaseInteractor interactor)
    {
        currentInteractor = interactor;
    }

    public void SetUngrabbed(XRBaseInteractor interactor)
    {
        currentInteractor = null;
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
    }
}
