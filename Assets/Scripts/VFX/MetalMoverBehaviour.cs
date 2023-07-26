using UnityEngine;

public class MetalMoverBehaviour : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer m_LampRenderer;
    private Material m_LampMaterial;
    private float smoothActive;
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
        smoothActive = Mathf.Lerp(smoothActive, IsActive ? 1 : 0, 0.1f);
        float st = (0.75f + 0.25f * Mathf.Sin(Time.time * 9.0f));
        Color c = Color.Lerp(Color.black, Color.white, smoothActive * st * st);
        m_LampRenderer.materials[1].SetColor("_EmissionColor", c);
    }
}
