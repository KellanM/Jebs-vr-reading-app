using UnityEngine;
using UnityHelpers;

public class StackStability : MonoBehaviour
{
    private IGrabbable GrabbableSelf { get { if (_grabbableSelf == null) _grabbableSelf = GetComponent<IGrabbable>(); return _grabbableSelf; } }
    private IGrabbable _grabbableSelf;
    private PhysicsTransform PhysicsSelf { get { if (_physicsSelf == null) _physicsSelf = GetComponent<PhysicsTransform>(); return _physicsSelf; } }
    private PhysicsTransform _physicsSelf;

    public float groundDistDetection = 0.001f;
    public bool isGrounded;
    public bool isStacked;
    private bool _prevStacked;
    private StackStability undercube;

    void Update()
    {
        CheckStackStatus();
        AnchorToStack();
    }

    private void CheckStackStatus()
    {
        var bounds = transform.GetTotalBounds(Space.World);
        RaycastHit raycastHit;
        Debug.DrawRay(transform.position, Vector3.down * (bounds.extents.y + groundDistDetection), Color.green);
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out raycastHit, bounds.extents.y + groundDistDetection);
        if (isGrounded)
        {
            var otherCube = raycastHit.transform.GetComponentInParent<StackStability>();
            if (otherCube != null && otherCube.isGrounded)
            {
                undercube = otherCube;
                isStacked = GrabbableSelf.GetGrabCount() <= 0;
            }
            else
                isStacked = false;
        }
        else
            isStacked = false;
    }

    private void AnchorToStack()
    {
        if (isStacked)
        {
            if (!_prevStacked)
            {
                var cubeBounds = undercube.transform.GetTotalBounds(Space.Self);
                Vector3 upAlignedAxis = GetAxisAlignedTo(undercube.transform, Vector3.up);
                Vector3 stackedPosition = undercube.transform.position + upAlignedAxis * cubeBounds.size.y;

                Vector3 selfUpAligned = GetAxisAlignedTo(transform, Vector3.up);
                Vector3 selfForwardAligned = GetAxisAlignedTo(transform, Vector3.forward);
                Quaternion stackedOrientation = Quaternion.LookRotation(selfForwardAligned, selfUpAligned);

                PhysicsSelf.position = stackedPosition;
                PhysicsSelf.rotation = stackedOrientation.Shorten();
            }

            PhysicsSelf.striveForPosition = true;
            PhysicsSelf.striveForOrientation = true;
        }
        else
        {
            if (GrabbableSelf.GetGrabCount() <= 0)
            {
                PhysicsSelf.striveForPosition = false;
                PhysicsSelf.striveForOrientation = false;
            }
        }

        _prevStacked = isStacked;
    }

    public static Vector3 GetAxisAlignedTo(Transform orientedObject, Vector3 worldDirection)
    {
        Vector3 upAlignedAxis = orientedObject.up;
        float closestDot = Vector3.Dot(upAlignedAxis, Vector3.up);
        float otherDot = Vector3.Dot(orientedObject.right, Vector3.up);
        if (Mathf.Abs(otherDot) > Mathf.Abs(closestDot))
        {
            upAlignedAxis = orientedObject.right;
            closestDot = otherDot;
        }
        otherDot = Vector3.Dot(orientedObject.forward, Vector3.up);
        if (Mathf.Abs(otherDot) > Mathf.Abs(closestDot))
        {
            upAlignedAxis = orientedObject.forward;
            closestDot = otherDot;
        }
        if (otherDot < 0)
            upAlignedAxis *= -1;

        return upAlignedAxis;
    }
}
