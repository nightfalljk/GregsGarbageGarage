using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreaderAnimator : MonoBehaviour
{
    [SerializeField]
    private Transform m_Gear1;
    [SerializeField]
    private Transform m_Gear2;

    // Update is called once per frame
    void Update()
    {
        m_Gear1.Rotate(new Vector3(Time.deltaTime*-60f,0,0));
        m_Gear2.Rotate(new Vector3(Time.deltaTime * -60f, 0, 0));
    }
}
