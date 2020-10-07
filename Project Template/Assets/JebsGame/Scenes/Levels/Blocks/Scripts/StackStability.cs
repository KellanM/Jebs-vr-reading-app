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
                isStacked = true;
            }
            else
                isStacked = false;
        }
        else
            isStacked = false;
    }

    private void AnchorToStack()
    {
        if (GrabbableSelf.GetGrabCount() <= 0)
        {
            if (isStacked)
            {
                var cubeBounds = undercube.transform.GetTotalBounds(Space.Self);
                Vector3 upAlignedAxis = undercube.transform.GetAxisAlignedTo(Vector3.up);
                Vector3 stackedPosition = undercube.transform.position + upAlignedAxis * cubeBounds.size.y;

                Vector3 selfUpAligned = transform.GetAxisAlignedTo(Vector3.up);
                Vector3 selfForwardAligned = transform.GetAxisAlignedTo(Vector3.forward);
                Quaternion stackedOrientation = Quaternion.LookRotation(selfForwardAligned, selfUpAligned);

                PhysicsSelf.position = stackedPosition;
                PhysicsSelf.rotation = stackedOrientation.Shorten();

                PhysicsSelf.counteractGravity = true;
                PhysicsSelf.striveForPosition = true;
                PhysicsSelf.striveForOrientation = true;
            }
            else
            {
                PhysicsSelf.counteractGravity = false;
                PhysicsSelf.striveForPosition = false;
                PhysicsSelf.striveForOrientation = false;
            }
        }

        // _prevStacked = isStacked;
    }
}
