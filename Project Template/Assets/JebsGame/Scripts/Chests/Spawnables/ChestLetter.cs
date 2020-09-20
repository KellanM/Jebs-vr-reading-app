using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChestLetter : Spawnable
{
    public char value;
    public string materialColorProperty = "_Color";
    public GameObject[] fonts;

    GameObject currentFont;

    public void UpdateFont()
    {
        int index = Random.Range(0, fonts.Length - 1);

        for (int i = 0; i < fonts.Length; i++)
        {
            fonts[i].SetActive(i == index);

            if (i == index)
                currentFont = fonts[i];
        }
    }

    public void UpdateColor(Color c)
    {
        TextMeshPro tmpro = currentFont.GetComponent<TextMeshPro>();
        MeshRenderer mr = currentFont.GetComponent<MeshRenderer>();

        if (tmpro)
        {
            tmpro.color = new Color32((byte)(c.r * 255), (byte)(c.g * 255), (byte)(c.b * 255), (byte)(c.a * 255));
        }
        else if (mr)
        {
            mr.material.SetColor(materialColorProperty, c);
        }
    }
}
