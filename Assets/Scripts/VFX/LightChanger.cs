using UnityEngine;

public class LightChanger : MonoBehaviour
{
    public float time;

    public void SetTimeMorning()
    {
        this.time = 0.5f;
    }
    public void SetTimeNight()
    {
        this.time = 0.0f;
    }

    [Header("Day")]
    [SerializeField]
    private Color m_DayGradient1;
    [SerializeField]
    private Color m_DayGradient2;
    [SerializeField]
    private Color m_DayGradient3;
    [SerializeField]
    private Color m_DaySunColor;
    [SerializeField]
    private float m_DaySunStrength;
    [SerializeField]
    private Color m_DaySpotColor;
    [SerializeField]
    private float m_DaySpotStrength;

    [Header("Night")]
    [SerializeField]
    private Color m_NightGradient1;
    [SerializeField]
    private Color m_NightGradient2;
    [SerializeField]
    private Color m_NightGradient3;
    [SerializeField]
    private Color m_NightSunColor;
    [SerializeField]
    private float m_NightSunStrength;
    [SerializeField]
    private Color m_NightSpotColor;
    [SerializeField]
    private float m_NightSpotStrength;

    [Header("References")]
    [SerializeField]
    private Light m_Sun;
    [SerializeField]
    private Light m_Spot;

    // Update is called once per frame
    void Update()
    {
        ApplySettings(time);
    }

    private float CustomLerp(float t)
    {
        float a = t*t;
        float b = 1-((1-t)*(1-t));
        return Mathf.Lerp(a,b,t);
    }

    public void ApplySettings(float f)
    {
        f = CustomLerp((Mathf.Cos(f * Mathf.PI * 2) + 1f) * 0.5f);

        m_Sun.color = Color.Lerp(m_DaySunColor, m_NightSunColor, f);
        m_Sun.intensity = Mathf.Lerp(m_DaySunStrength, m_NightSunStrength, f);

        m_Spot.color = Color.Lerp(m_DaySpotColor, m_NightSpotColor, f);
        m_Spot.intensity = Mathf.Lerp(m_DaySpotStrength, m_NightSpotStrength, f);

        RenderSettings.ambientGroundColor = Color.Lerp(m_DayGradient1, m_NightGradient1, f);
        RenderSettings.ambientEquatorColor = Color.Lerp(m_DayGradient2, m_NightGradient2, f);
        RenderSettings.ambientSkyColor = Color.Lerp(m_DayGradient3, m_NightGradient3, f);
    }
}
