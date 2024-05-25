using UnityEngine;

class OutlineObject
{
    public GameObject Object;
    public bool InUse;

    public OutlineObject(GameObject obj)
    {
        Object = obj;
        InUse = false;
    }

    public void Enable()
    {
        Object.SetActive(true);
        InUse = true;
    }

    public void Disable()
    {
        Object.SetActive(false);
        InUse = false;
    }

    public void Show()
    {
        Object.SetActive(true);
    }

    public void Hide()
    {
        Object.SetActive(false);
    }
}