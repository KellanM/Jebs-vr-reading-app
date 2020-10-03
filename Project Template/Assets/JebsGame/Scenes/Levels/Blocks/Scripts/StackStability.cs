using UnityEngine;
using UnityHelpers;

public class StackStability : MonoBehaviour
{
    private IGrabbable GrabbableSelf { get { if (_grabbableSelf == null) _grabbableSelf = GetComponent<IGrabbable>(); return _grabbableSelf; } }
    private IGrabbable _grabbableSelf;
    private PhysicsTransform PhysicsSelf { get { if (_physicsSelf == null) _physicsSelf = GetComponent<PhysicsTransform>(); return _physicsSelf; } }
    private PhysicsTransform _physicsSelf;

    public bool isGrounded;
    public bool isStacked;
    private bool _prevStacked;

    void Update()
    {
        CheckStackStatus();
        AnchorToStack();
    }

    private void CheckStackStatus()
    {
        var bounds = transform.GetTotalBounds(Space.World);
        RaycastHit raycastHit;
        Debug.DrawRay(transform.position, Vector3.down * (bounds.extents.y + 0.01f), Color.green);
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out raycastHit, bounds.extents.y + 0.01f);
        if (isGrounded)
        {
            var otherCube = raycastHit.transform.GetComponentInParent<StackStability>();
            if (otherCube != null && otherCube.isGrounded)
            {
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
                PhysicsSelf.position = transform.position;
                PhysicsSelf.rotation = transform.rotation;
                _prevStacked = true;
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

            _prevStacked = false;
        }
    }
}
