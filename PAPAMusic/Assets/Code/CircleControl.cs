/* ========================================================
 *	类名称：CircleControl
 *	作 者：Zhangfan
 *	创建时间：2019-02-25 18:19:35
 *	版 本：V1.0.0
 *	描 述：
* ========================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PAPA
{
    public class CircleControl : MonoBehaviour
    {
        public enum CircleState
        {
            Waiting,
            Stop,
            Running,
            Miss,
        }
        public DrawCircle m_Circle;

        //public float Speed = 1.0f;
        public float m_CurrectTime;

        public float m_Speed = 1f;
        public float m_StartTime = 0.0f;
                
        public float m_TargetTime;
        public float m_StopTime;

        public float m_EnterTime;
        public float m_MissTime;

        CircleState m_State;
        public CircleState State
        {
            get
            {
                return m_State;
            }
        }

        private void Awake()
        {
            m_Circle = this.gameObject.GetComponent<DrawCircle>();
        }

        private void OnEnable()
        {
            m_Circle.radius = 0;
            m_CurrectTime = 0;
            m_State = CircleState.Running;

            float startR = m_StartTime * Mathf.Abs(m_Speed);
            m_Circle.radius = startR;
        }

        private void OnDisable()
        {
            m_State = CircleState.Waiting;
        }

        private void Update()
        {
            switch(m_State)
            {
                case CircleState.Running:
                    {
                        m_Circle.radius += m_Speed * Time.deltaTime;
                        m_CurrectTime += Time.deltaTime;

                        if (m_CurrectTime > m_MissTime)
                        {
                            m_State = CircleState.Miss;
                        }
                    }
                    break;
                case CircleState.Miss:
                    {
                        m_Circle.radius += m_Speed * Time.deltaTime;
                        m_CurrectTime += Time.deltaTime;

                        if (m_CurrectTime > m_StopTime)
                        {
                            RhythmManager.Instance.OnMiss();
                            m_State = CircleState.Stop;
                        }
                    }
                    break;
            }

            float t = Mathf.Clamp(m_TargetTime - Mathf.Abs(m_CurrectTime - m_TargetTime), 0, m_TargetTime);

            float alpha = Mathf.Lerp(0, 1, t / m_TargetTime);

            m_Circle.SetAlpha(alpha);

            //m_Circle.radius += m_Speed * Time.deltaTime;
            //m_CurrectTime += Time.deltaTime;
            //if (m_CurrectTime > m_StopTime)
            //{
            //    m_State = CircleState.Stop;
            //}


        }

        public bool TryTriggerRhythm(out float power)
        {
            power = 1;
            if(m_CurrectTime < m_EnterTime)
            {
                Debug.Log("m_EnterTime: " + m_EnterTime + " m_CurrectTime: " + m_CurrectTime);
                power = Mathf.Clamp(Math.Abs((m_CurrectTime - m_EnterTime) / (m_EnterTime)), 0, 1);

                return true;
            }
            else if(m_CurrectTime > m_MissTime)
            {
                Debug.Log("m_MissTime: " + m_MissTime + " m_CurrectTime: " + m_CurrectTime);
                power = Mathf.Clamp(Math.Abs((m_CurrectTime - m_MissTime) / (m_EnterTime)), 0, 1);

                return true;
            }
            else
            {
                Debug.Log("enter miss an enter: " + m_CurrectTime);
                power = 0;
                return true;
            }

            return false;
        }

        public void ClickOver()
        {
            m_State = CircleState.Stop;
        }
    }
}
