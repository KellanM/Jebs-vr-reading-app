using OVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFadeOut : MonoBehaviour
{
    bool fadeIn = true;
    bool fadeOut;

    MeshRenderer mr;

    private void Start()
    {
        mr = transform.GetChild(0).GetComponent<MeshRenderer>();
        fadeIn = true;
        Invoke("FadeOutNow", 0.2f);
    }

    private void Update()
    {
        if (fadeIn)
        {
            mr.material.color = Color.Lerp(mr.material.color, Color.blue, 9f * Time.deltaTime);
            float factor = Mathf.Pow(2, 1.6f);
            Color color = new Color((255f/49f) * factor, (255f/91f) * factor, (255f/191f) * factor);
            mr.material.SetColor("_EmissionColor", Color.Lerp(mr.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor"), color, Time.deltaTime * 9f));
        }
        if (fadeOut)
        {
            mr.material.color = Color.Lerp(mr.material.color, new Color(0f, 0f, 0f, 0f), 9f * Time.deltaTime);
            float factor = Mathf.Pow(2, -10f);
            Color color = new Color((255f / 49f) * factor, (255f / 91f) * factor, (255f / 191f) * factor);
            mr.material.SetColor("_EmissionColor", Color.Lerp(mr.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor"), color, Time.deltaTime * 9f));
        }
    }

    void FadeOutNow()
    {
        fadeIn = false;
        fadeOut = true;
        Invoke("DestroyNow", 0.5f);
    }

    void DestroyNow()
    {
        Destroy(gameObject);
    }
}
