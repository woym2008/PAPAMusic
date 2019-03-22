using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PAPA;

public class UICtrl : MonoBehaviour
{
    public float speed = 0.5f;
    float alpha = 0.0f;
    Image m_Image;

    // Start is called before the first frame update
    void Start()
    {
        m_Image = this.gameObject.GetComponent<Image>();
        alpha = m_Image.color.a;

        RhythmManager.Instance.m_Trig += OnTrig;
    }

    // Update is called once per frame
    void Update()
    {
        if(alpha > 0)
        {
            alpha -= speed* Time.deltaTime;
            m_Image.color = new Color(m_Image.color.r, m_Image.color.g, m_Image.color.b, alpha);
        }
    }

    private void OnDestroy()
    {
        RhythmManager.Instance.m_Trig -= OnTrig;
    }

    void OnTrig(float power)
    {
        Debug.Log("OnTrig " + power);
        alpha = (1 - power);
        if(alpha > 0.8f)
        {
            m_Image.color = new Color(m_Image.color.r, m_Image.color.g, m_Image.color.b, alpha);
        }        
    }
}
