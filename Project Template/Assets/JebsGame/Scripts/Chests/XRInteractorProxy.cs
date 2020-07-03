using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class XRInteractorProxy : MonoBehaviour
{
    public XRDirectInteractor interactor;

    public UnityEvent onSelect;
    public UnityEvent onDeselect;

    void Awake()
    {
        interactor.onSelectEnter.AddListener(customSelectEnterEvent);
        interactor.onSelectExit.AddListener(customSelectExitEvent);
    }

    void customSelectEnterEvent(XRBaseInteractable interactable)
    {
        if (interactor.selectTarget)
            onSelect.Invoke();
    }

    void customSelectExitEvent(XRBaseInteractable interactable)
    {
        if (!interactor.selectTarget)
            onDeselect.Invoke();
    }
}
