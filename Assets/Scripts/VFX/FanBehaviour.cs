using UnityEngine;

public class FanBehaviour : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer m_LampRenderer;
    private Material m_LampMaterial;
    [SerializeField]
    private Transform[] m_Papers;
    private float smoothActive;
    [SerializeField]
    private Transform m_Blades;
    public bool IsActive;
    // Start is called before the first frame update
    void Awake()
    {
        m_LampMaterial = new Material(m_LampRenderer.materials[1]);
        m_LampRenderer.materials[1] = m_LampMaterial;
        smoothActive = 0;
    }

    // Update is called once per frame
    void Update()
    {
        smoothActive = Mathf.Lerp(smoothActive, IsActive?1:0, 0.1f);
        float st = (0.75f + 0.25f * Mathf.Sin(Time.time * 9.0f));
        Color c = Color.Lerp(Color.black, Color.white, smoothActive * st * st);
        m_LampRenderer.materials[1].SetColor("_EmissionColor", c);
        Quaternion q = Quaternion.Euler(0,0,90-smoothActive*90*(1f+0.25f*Mathf.Sin(Time.time*20.0f)));
        foreach(Transform t in m_Papers)
        {
            t.localRotation = q;
        }
        if (IsActive) m_Blades.Rotate(smoothActive*1000*Time.deltaTime,0,0);
    }
}
