using UnityEngine;
using UnityHelpers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

[System.Serializable]
public class ButtonEvent : UnityEvent<PhysicsTriggerButton> {}

public class PhysicsTriggerButton : MonoBehaviour
{
    public Transform top;
    [Tooltip("Sets how far the button needs to be depressed before registering a click")]
    public float clickValue = 0.6f;
    public bool isDown { get; private set; }
    public bool onDown { get; private set; }
    private bool prevDown;
    public bool isUp { get; private set; }
    public bool onUp { get; private set; }
    private bool prevUp;

    public ButtonEvent onButtonDown;
    public ButtonEvent onButtonUp;

    public float value { get; private set; }
    private List<float> values = new List<float>();

    void FixedUpdate()
    {
        CalculateDepressionValue();

        isDown = value >= clickValue;
        if (isDown && !prevDown)
        {
            onDown = true;
            onButtonDown?.Invoke(this);
        }
        else
            onDown = false;
        prevDown = isDown;

        isUp = !isDown;
        if (isUp && !prevUp)
        {
            onUp = true;
            onButtonUp?.Invoke(this);
        }
        else
            onUp = false;
        prevUp = isUp;
    }

    private void CalculateDepressionValue()
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

        Vector3 closestPointToBtn = other.position - otherBounds.extents.Multiply(transform.up);
        float otherY = transform.InverseTransformPoint(closestPointToBtn).y;
        float topY = selfBounds.center.y + selfBounds.extents.y;

        float penetrationDistance = Mathf.Abs(otherY - topY);
        var value = Mathf.Clamp01(penetrationDistance / selfBounds.size.y);
        values.Add(value);
    }
}
