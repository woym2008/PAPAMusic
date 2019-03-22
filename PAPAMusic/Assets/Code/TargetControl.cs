using PAPA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetControl : MonoBehaviour
{    
    public float m_TargetTime;
    public float m_Speed = 1f;

    public DrawCircle m_Circle;

    private void Awake()
    {
        m_Circle = this.gameObject.GetComponent<DrawCircle>();
    }

    private void OnEnable()
    {
        m_Circle.radius = m_Speed * m_TargetTime;
    }
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
