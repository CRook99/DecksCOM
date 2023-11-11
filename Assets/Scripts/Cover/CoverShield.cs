using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverShield : MonoBehaviour
{
    [SerializeField] GameObject model;

    private void Start()
    {
        model.SetActive(false);
    }

    private void Update()
    {

    }

    [ContextMenu("Show")]
    public void Show()
    {
        model.SetActive(true);
    }

    public void Show(float scale)
    {
        model.SetActive(true);
        SetScale(scale);
    }

    [ContextMenu("Hide")]
    public void Hide()
    {
        model.SetActive(false);
    }

    public void SetScale(float scale)
    {
        model.transform.localScale = scale * Vector3.one;
    }
}
