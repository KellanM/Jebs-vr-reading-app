using UnityEngine;
using UnityHelpers;

public class LetterReorient : MonoBehaviour
{
    private PhysicsTransform SelfPhysics { get { if (_selfPhysics == null) _selfPhysics = GetComponent<PhysicsTransform>(); return _selfPhysics; } }
    private PhysicsTransform _selfPhysics;

    public float timeToStill = 5;
    public float tolerance = 0.1f;

    private Quaternion prevRotation;
    private float timeSinceStop;

    void Update()
    {
        bool atIdentity = transform.rotation.SameOrientationAs(Quaternion.identity, tolerance);
        if (SelfPhysics.striveForOrientation && atIdentity)
            SelfPhysics.striveForOrientation = false;

        if (!atIdentity && !SelfPhysics.striveForOrientation)
        {
            if (transform.rotation.SameOrientationAs(prevRotation, tolerance))
            {
                if (Time.time - timeSinceStop >= timeToStill)
                {
                    SelfPhysics.striveForOrientation = true;
                }
            }
            else
                timeSinceStop = Time.time;
        }

        prevRotation = transform.rotation;
    }
}
