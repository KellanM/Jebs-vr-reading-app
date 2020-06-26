using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class FollowTransform : MonoBehaviour
{
    [Header("Following")]
    public Transform master;
    public Transform slave;
    public bool followPosition = true;
    public bool followRotation = false;
    
    [Header("Boundaries")]
    public SnapAxis axis;
    public float minLocal;
    public float maxLocal;

    [Header("Destination")]
    public Transform destination;
    public List<UnityEvent> steps = new List<UnityEvent>(2); // It has to have at least 2 element (min, max)

    [Header("Control")]
    public int initialStep;
    public bool minPositionIsSticky;
    public bool unsetMasterOnStepChange = false;

    // Slave
    Vector3 newLocalPos;
    Vector3 lastLocalPos;
    Vector3 initialLocalPos;
    Quaternion initialLocalRot;

    // Master
    Vector3 lastMasterPos;
    Quaternion lastMasterRot;

    bool following = false;
    int currentStep = 0;

    void Start()
    {
        if (!slave) slave = transform;

        initialLocalPos = slave.localPosition;
        initialLocalRot = slave.localRotation;

        lastLocalPos = initialLocalPos;

        GetWorldLimits(out Vector3 worldMin, out Vector3 worldMax); 
        Vector3 section = (worldMax - worldMin) / (steps.Count - 1);
        destination.position = worldMin + section * initialStep;

        if (steps.Count < 2) Debug.LogError("Steps has to have at least 2 element (min, max)");
    }

    // Update is called once per frame
    void Update()
    {
        if (master)
        {
            // Poke object can be disabled while in trigger and we need to detect that
            if (!master.gameObject.activeSelf)
            {
                UnsetMaster(master.gameObject);
                return;
            }

            if (!following)
            {
                lastMasterRot = master.rotation;
                lastMasterPos = master.position;
                following = true;
            }

            // Apply position
            if (followPosition)
            {
                slave.position = slave.position + (master.position - lastMasterPos);
                lastMasterPos = master.position;
            }

            // Apply rotation
            if (followRotation)
            {
                slave.rotation = slave.rotation * (master.rotation * Quaternion.Inverse(lastMasterRot));
                lastMasterRot = master.rotation;
            }
        }
        else
        {
            if (following)
            {
                following = false;
            }

            // Apply position
            if (followPosition)
                slave.position = destination.position;

            // Apply rotation
            if (followRotation)
                slave.rotation = destination.rotation;
        }

        ApplyBoundaries(slave);

        if (slave.localPosition != lastLocalPos)
        {
            CheckStepChange();
        }

        lastLocalPos = slave.localPosition;
    }

    public void SetMaster(GameObject masterObj)
    {
        master = masterObj.transform;

        lastMasterRot = master.rotation;
        lastMasterPos = master.position;
    }

    public void UnsetMaster(GameObject masterObj)
    {
        if (masterObj.transform == master) master = null;
    }

    public void MoveToDestination(Transform tsf)
    {
        tsf.position = destination.position;
        // tsf.rotation = destination.rotation;
    }

    public void ApplyBoundaries(Transform limitThis)
    {
        newLocalPos = initialLocalPos;

        switch (axis)
        {
            case SnapAxis.X:
                newLocalPos.x = Mathf.Clamp(limitThis.localPosition.x, minLocal, maxLocal);
                break;
            case SnapAxis.Y:
                newLocalPos.y = Mathf.Clamp(limitThis.localPosition.y, minLocal, maxLocal);
                break;
            case SnapAxis.Z:
                newLocalPos.z = Mathf.Clamp(limitThis.localPosition.z, minLocal, maxLocal);
                break;
        }

        limitThis.localPosition = newLocalPos;

        limitThis.localRotation = initialLocalRot;
    }

    public void CheckStepChange()
    {
        GetWorldLimits(out Vector3 minWorldPoint, out Vector3 maxWorldPoint);
        Vector3 section = (maxWorldPoint - minWorldPoint) / (steps.Count - 1);

        Vector3 stepWorldPoint;
        Vector3 closest = minWorldPoint + section * currentStep;
        int newStep = currentStep;

        for (int i = 0; i < steps.Count; i++)
        {
            stepWorldPoint = minWorldPoint + section * i;
            if (master) Debug.DrawLine(master.position,stepWorldPoint,Color.black);
            if (Vector3.Distance(stepWorldPoint, slave.position) < Vector3.Distance(closest, slave.position))
            {
                newStep = i;
                closest = minWorldPoint + section * newStep;
            }
        }

        if (currentStep != newStep)
        {
            steps[newStep].Invoke();
            currentStep = newStep;

            if (newStep != 0 || (newStep == 0 && minPositionIsSticky))
            {
                destination.position = closest;
            }

            if (unsetMasterOnStepChange && master)
            {
                UnsetMaster(master.gameObject);
            }
        }
    }

    public void GetWorldLimits (out Vector3 minWorldPoint, out Vector3 maxWorldPoint)
    {
        Vector3 minLocalPoint = initialLocalPos;
        Vector3 maxLocalPoint = initialLocalPos;

        switch (axis)
        {
            case SnapAxis.X:
                minLocalPoint.x = minLocal;
                maxLocalPoint.x = maxLocal;
                break;
            case SnapAxis.Y:
                minLocalPoint.y = minLocal;
                maxLocalPoint.y = maxLocal;
                break;
            case SnapAxis.Z:
                minLocalPoint.z = minLocal;
                maxLocalPoint.z = maxLocal;
                break;
        }

        minWorldPoint = slave.parent.TransformPoint(minLocalPoint);
        maxWorldPoint = slave.parent.TransformPoint(maxLocalPoint);
    }

}
