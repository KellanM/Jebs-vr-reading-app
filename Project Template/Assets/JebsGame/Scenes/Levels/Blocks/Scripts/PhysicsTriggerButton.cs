using UnityEngine;
using UnityHelpers;
using System.Collections.Generic;
using System.Linq;

public class PhysicsTriggerButton : MonoBehaviour
{
    public Transform top;
    public float value;
    public List<float> values = new List<float>();

    void FixedUpdate()
    {
        value = 0;
        if (values.Count > 0)
            value = values.Min();
        var topBounds = top.GetTotalBounds(Space.Self);
        top.localPosition = -Vector3.up * topBounds.extents.y - Vector3.up * value * topBounds.extents.y;
        values.Clear();
    }

    public void TriggerStay(TreeCollider.CollisionInfo colInfo)
    {
        Transform other = colInfo.otherCollider.transform;
        var otherBounds = other.GetBounds(Space.World, true);
        var selfBounds = colInfo.sender.transform.GetBounds(Space.Self, true);

        Vector3 bottomMostPos = other.position - Vector3.up * otherBounds.extents.y;
        Vector3 buttonTop = transform.position + transform.up * selfBounds.extents.y;

        float penetrationDistance = Mathf.Abs(bottomMostPos.y - buttonTop.y);
        var value = Mathf.Clamp01(penetrationDistance / selfBounds.size.y);
        values.Add(value);
    }
}
