using UnityEngine;
using UnityEngine.InputSystem;

public class HandBehaviour : MonoBehaviour
{
    [SerializeField]
    private Animator m_Anim;
    private float m_Side;
    public bool m_Grab;
    public bool m_Hover;

    private Vector3 m_LastPos;

    [SerializeField]
    private float m_offset;

    void Start()
    {
        #if !UNITY_EDITOR
        Cursor.visible = false;
        #endif
        m_LastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        m_Anim.SetBool("Hover", m_Hover);
        m_Anim.SetBool("Grab", m_Grab);

        float dx = (m_LastPos.x - transform.position.x)/Time.deltaTime*0.1f;
        m_Side = Mathf.Lerp(m_Side, dx, 0.1f);
        m_Side = Mathf.Clamp(m_Side, -1, 1);
        if (m_Grab || m_Hover) m_Side *= 0.1f;

        float r = Mathf.Clamp01(m_Side);
        float l = Mathf.Clamp01(-m_Side);
        m_Anim.SetLayerWeight(1, l);
        m_Anim.SetLayerWeight(2, r);
        m_LastPos = transform.position;
        
        Vector3 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3 offset =  Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()).direction.normalized * m_offset;
        transform.position = pos + offset;
    }

    private void LateUpdate()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3 offset =  Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()).direction.normalized * m_offset;
        transform.position = pos + offset;
    }

    void Reset()
    {
        m_Anim = GetComponent<Animator>();
    }
}
