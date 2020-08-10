using UnityEngine;
using UnityHelpers;

public class MagneticPairing : MonoBehaviour
{
    private bool recentlyUnpaired;

    void OnTriggerEnter(Collider other)
    {
        var otherGrabbable = other.GetComponentInParent<IGrabbable>();
        var selfGrabbable = GetComponentInParent<IGrabbable>();
        if (!recentlyUnpaired && otherGrabbable != null && selfGrabbable != null && otherGrabbable.GetGrabCount() > 0 && selfGrabbable.GetGrabCount() > 0)
        {
            //Debug.Log("Trigger entered " + other.name);
            var otherLetter = other.GetComponentInParent<PairedLetter>();
            if (otherLetter != null && !otherLetter.pairedLetterStandIn.IsLetterSet() && otherLetter.gameObject.activeSelf)
            {
                //Debug.Log("Other is not set");
                var selfLetter = GetComponentInParent<PairedLetter>();
                if (!selfLetter.pairedLetterStandIn.IsLetterSet())
                {
                    //Debug.Log("Self is not set");
                    selfLetter.PairWith(otherLetter);
                    //selfLetter.GetComponent<GrabbablePhysicsTransform>().OnStretch.AddListener(OnStretched);
                }
            }
        }
    }

    public void Disconnect()
    {
        Disconnect(GetComponentInParent<IGrabbable>());
    }
    public void Disconnect(IGrabbable caller)
    {
        var firstGrabber = caller.GetGrabber(0);
        caller.Ungrab(firstGrabber);

        var pairedSelf = GetComponentInParent<PairedLetter>();
        var objectPairedWith = pairedSelf.objectPairedWith;
        recentlyUnpaired = true;
        objectPairedWith.GetComponent<MagneticPairing>().recentlyUnpaired = true;
        pairedSelf.Unpair();
    }
    private void GrabDisconnectedLetter(IGrabbable caller)
    {
        var firstGrabber = caller.GetGrabber(0);
        if (firstGrabber.info != null && firstGrabber.info.parent != null)
        {
            var pairedSelf = GetComponentInParent<PairedLetter>();
            if (pairedSelf != null)
            {
                var objectPairedWith = pairedSelf.objectPairedWith;
                if (objectPairedWith != null)
                {
                    var otherManipulator = objectPairedWith.GetComponent<IGrabbable>();
                    if (otherManipulator != null)
                    {
                        objectPairedWith.transform.position = firstGrabber.info.parent.TransformPoint(firstGrabber.localPosition);
                        otherManipulator.Grab(otherManipulator.CreateLocalInfo(firstGrabber.info, firstGrabber.maxForce));
                    }
                    else
                        Debug.LogError("MagneticPairing: Could not grab other object properly, object paired with is missing an IGrabbable component");
                }
                else
                    Debug.LogError("MagneticPairing: Could not grab other object properly, object paired with is null");
            }
            else
                Debug.LogError("MagneticPairing: Could not grab other object properly, missing PairedLetter component");
        }
        else
            Debug.LogError("MagneticPairing: Could not grab other object properly, grabber is missing parent info");
    }

    public void OnStretched(IGrabbable caller, float amount)
    {
        Disconnect(caller);
        GrabDisconnectedLetter(caller);
    }
    public void OnCompletelyUngrabbed(IGrabbable caller, LocalInfo localInfo)
    {
        recentlyUnpaired = false;
    }
}
