using UnityEngine;
using VFX;

public class EyePusherAnimator : PusherAnimator
{
    [SerializeField]
    private Transform m_Broom;
    [SerializeField]
    private Transform m_Tank;
    [SerializeField]
    private Transform m_EyeL;
    [SerializeField]
    private Transform m_EyeR;
    [SerializeField]
    private Transform m_BrowL;
    [SerializeField]
    private Transform m_BrowR;
    [SerializeField]
    private MeshRenderer m_TankRenderer;

    [SerializeField]
    private AnimationCurve m_BrowCurve;
    [SerializeField]
    private AnimationCurve m_EyeCurve;

    // Start is called before the first frame update
    void Awake()
    {
        m_TankRenderer.material = new Material(m_TankRenderer.material);
    }

    // Update is called once per frame
    void Update()
    {
        m_Broom.localPosition = Vector3.right * -.75f * Extension;
        m_Tank.localScale = new Vector3(1, (1 - Extension) * 0.6f + 0.4f, 1);

        float t = Time.time % 1;

        Quaternion qe = Quaternion.Euler(0,m_EyeCurve.Evaluate(t)*60f,-10);
        m_EyeL.localRotation = qe;
        m_EyeR.localRotation = qe;

        Quaternion qb = Quaternion.Euler(0,0,m_BrowCurve.Evaluate(t)*30f);
        m_BrowL.localRotation = qb;
        m_BrowR.localRotation = qb;
    }

    public void SetColor(Color c)
    {
        m_TankRenderer.material.SetColor("_Color",c);
    }

}
