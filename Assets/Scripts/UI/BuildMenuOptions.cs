using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class BuildMenuOptions : MonoBehaviour
{
    public CoRo dummy;
    [System.Serializable] public class UnityEventBool:UnityEvent<bool> {}
    public static UnityEventBool OnBuildOptionsSetActive = new UnityEventBool();

    [SerializeField] public string title;
    [SerializeField] public string description;

    public void Update()
    {
        if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            SetActive(false);
        }
    }

    public void SetActive(bool b)
    {
        if (this.gameObject.activeSelf != b)
        {
            dummy.StartCoroutine(DelayEventOnBuildMenuOptionActive(b));
        }
        gameObject.SetActive(b);
    }

    public void ToggleActive()
    {
        if (this.gameObject.activeSelf)
        {
            SetActive(false);
        }
        else
        {
            SetActive(true);
        }
    }
    
    IEnumerator DelayEventOnBuildMenuOptionActive(bool b)
    {
        if (b)
        {
            OnBuildOptionsSetActive.Invoke(true);
        }
        else
        {
            yield return new WaitForSecondsRealtime(0.5f);
            OnBuildOptionsSetActive.Invoke(false);
        }
    }
    
}
