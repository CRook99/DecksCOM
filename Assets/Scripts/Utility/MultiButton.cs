using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MultiButton : Button
{
    List<Renderer> objectUI;

    protected override void Awake()
    {
        objectUI = GetComponentsInChildren<Renderer>().ToList();
    }
    
    protected virtual IEnumerator TweenColorFromCurrent(Color to, float duration)
    {
        float elapsed = 0f;
        while (elapsed <= duration)
        {
            objectUI.ForEach(x => x.material.color = Color.Lerp(x.material.color, to, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }
        objectUI.ForEach(x => x.material.color = to);
    }

    protected override void DoStateTransition(Selectable.SelectionState state, bool instant)
    {
        if (state == SelectionState.Pressed)
        {
            Debug.Log("Pressed");
            StopAllCoroutines();
            StartCoroutine(TweenColorFromCurrent(colors.pressedColor, colors.fadeDuration));
        }
        else if (state == SelectionState.Highlighted)
        {
            Debug.Log("Highlighted");
            StartCoroutine(TweenColorFromCurrent(colors.highlightedColor, colors.fadeDuration));
        }
        else if (state == SelectionState.Normal)
        {
            StartCoroutine(TweenColorFromCurrent(colors.normalColor, colors.fadeDuration));
        }
    }
}
