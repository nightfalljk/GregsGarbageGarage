using UnityEngine;
using VFX;

public class NosePusherAnimator : PusherAnimator
{
    [SerializeField]
    private Transform m_Broom;
    [SerializeField]
    private Transform m_Tank;
    [SerializeField]
    private Transform m_Nose;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_Nose.localScale = new Vector3(1,1,(Mathf.Sin(Time.time)*0.2f)+1f);

        m_Broom.localPosition = Vector3.right * -.75f * Extension;
        m_Tank.localScale = new Vector3(1,(1-Extension) * 0.6f + 0.4f,1);
    }
}
