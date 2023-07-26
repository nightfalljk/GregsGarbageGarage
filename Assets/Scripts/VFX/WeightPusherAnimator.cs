using UnityEngine;
using VFX;

public class WeightPusherAnimator : PusherAnimator
{

    [SerializeField]
    private Transform m_Broom;
    [SerializeField]
    private Transform m_Tank;
    [SerializeField]
    private MeshRenderer m_ScaleRenderer;

    public float Weight;
    public float HeavyThreshhold;
    // Start is called before the first frame update
    void Awake()
    {
        m_ScaleRenderer.materials[1] = new Material(m_ScaleRenderer.materials[1]);
        m_ScaleRenderer.materials[2] = new Material(m_ScaleRenderer.materials[2]);
    }

    // Update is called once per frame
    void Update()
    {
        m_Broom.localPosition = Vector3.right * -.75f * Extension;
        m_Tank.localScale = new Vector3(1, (1 - Extension) * 0.6f + 0.4f, 1);

        m_ScaleRenderer.materials[2].SetTextureOffset("_MainTex",new Vector2(0,(Weight>HeavyThreshhold)?0.5f:0));
        m_ScaleRenderer.materials[1].SetTextureOffset("_MainTex", new Vector2(Weight*0.5f, 0));
    }
}
