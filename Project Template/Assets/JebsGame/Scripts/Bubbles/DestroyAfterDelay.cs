using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    bool scale;
    
    public void StartDelay(float length)
    {
        Invoke("DestroyNow", length);
    }

    public void StartDelayWithScale(float length)
    {
        Invoke("StartScale", length);
    }

    void DestroyNow()
    {
        Destroy(gameObject);
    }

    void StartScale()
    {
        scale = true;
        Invoke("DestroyNow", 0.5f);
    }

    private void Update()
    {
        if (scale)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 6f * Time.deltaTime);
        }
    }
}
