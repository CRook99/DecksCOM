using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverShield : MonoBehaviour
{
    [SerializeField] GameObject model;

    [ContextMenu("Show")]
    public void Show()
    {
        model.SetActive(true);
    }

    [ContextMenu("Hide")]
    public void Hide()
    {
        model.SetActive(false);
    }
}
