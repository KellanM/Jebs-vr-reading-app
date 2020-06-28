using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    public void StartDelay(float length)
    {
        Invoke("DestroyNow", length);
    }

    void DestroyNow()
    {
        Destroy(gameObject);
    }
}
