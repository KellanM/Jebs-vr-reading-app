using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ExtendedDirectInteractor : XRDirectInteractor
{
    // The default interaction events are displayed in the inspector for any inherited class, even if it is empty

    // If this stops working, we can use the commented code

    /*
    public delegate void GrabEvent(XRBaseInteractable interactable, XRBaseInteractor interactor);

    public event GrabEvent onHover;
    public event GrabEvent onGrab;
    public event GrabEvent onUngrab;
    public event GrabEvent onUnhover;

    new void Start()
    {
        onHoverEnter.AddListener(Hover);
        onSelectEnter.AddListener(Grab);
        onSelectExit.AddListener(Ungrab);
        onHoverExit.AddListener(Unhover);
    }

    void Hover(XRBaseInteractable interactable)
    {
        onHover?.Invoke(interactable, this);
    }

    void Grab(XRBaseInteractable interactable)
    {
        onGrab?.Invoke(interactable, this);
    }

    void Ungrab(XRBaseInteractable interactable)
    {
        onUngrab?.Invoke(interactable, this);
    }

    void Unhover(XRBaseInteractable interactable)
    {
        onUnhover?.Invoke(interactable, this);
    }
    */
}
