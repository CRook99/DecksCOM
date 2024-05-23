using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CancelZoneScript : MonoBehaviour
{
    public Material Material;

    void Start()
    {
        Image image = GetComponent<Image>();
        RectTransform rect = image.GetComponent<RectTransform>();

        float aspectRatio = rect.rect.width / rect.rect.height;
        Material.SetFloat("_AspectRatio", aspectRatio);
        image.material = Material;
    }
}
