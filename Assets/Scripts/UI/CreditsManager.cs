using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Credits;
    // Start is called before the first frame update
    public void ToggleCredits()
    {
        Credits.SetActive(!Credits.active);
    }
}
