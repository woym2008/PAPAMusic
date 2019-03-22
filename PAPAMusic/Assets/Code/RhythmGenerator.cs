/* ========================================================
 *	类名称：Plant
 *	作 者：Zhangfan
 *	创建时间：2019-03-05 12:58:04
 *	版 本：V1.0.0
 *	描 述：
* ========================================================*/
using SonicBloom.Koreo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PAPA
{
    public class RhythmGenerator : MonoBehaviour
    {
        //List<CircleControl> m_Lists = new List<CircleControl>();
        Stack<CircleControl> m_Cache = new Stack<CircleControl>();
        Queue<CircleControl> m_Lists = new Queue<CircleControl>();

        List<CircleControl> m_MissLists = new List<CircleControl>();

        public Transform TargetPos;

        public GameObject Prefab;

        [EventID]
        public string eventID;

        private CircleControl CreateCircle()
        {
            if(m_Cache.Count > 0)
            {
                CircleControl retcircle = m_Cache.Pop();

                retcircle.gameObject.SetActive(true);

                //m_Lists.Add(retcircle);
                m_Lists.Enqueue(retcircle);

                return retcircle;
            }

            CircleControl circle = GameObject.Instantiate(Prefab).GetComponent<CircleControl>();

            circle.gameObject.SetActive(true);
            circle.transform.parent = TargetPos;
            circle.transform.position = TargetPos.position;

            //m_Lists.Add(circle);
            m_Lists.Enqueue(circle);

            return circle;
        }

        private void Start()
        {
            Koreographer.Instance.RegisterForEvents(eventID, Rhythm);
        }

        void OnDestroy()
        {
            if (Koreographer.Instance != null)
            {
                Koreographer.Instance.UnregisterForAllEvents(this);
            }
        }

        void Rhythm(KoreographyEvent evt)
        {
            CreateCircle();
        }

        private void Update()
        {
            if(m_Lists.Count > 0)
            {
                CircleControl c = m_Lists.Peek();

                switch (c.State)
                {
                    case CircleControl.CircleState.Miss:
                        {
                            m_MissLists.Add(c);
                            m_Lists.Dequeue();
                            break;
                        }
                }
            }

            if(m_MissLists.Count > 0)
            {
                for(int i = m_MissLists.Count-1;i>=0;--i)
                {
                    switch (m_MissLists[i].State)
                    {
                        case CircleControl.CircleState.Stop:
                            {
                                m_MissLists[i].gameObject.SetActive(false);
                                m_Cache.Push(m_MissLists[i]);

                                m_MissLists.Remove(m_MissLists[i]);
                                break;
                            }
                    }
                }
                
            }
                
        }

        public CircleControl GetCircle()
        {
            if(m_Lists.Count > 0)
            {
                return m_Lists.Peek();
            }

            return null;
        }

        public void ReleaseCircle()
        {
            CircleControl c = m_Lists.Peek();
            c.ClickOver();
            m_MissLists.Add(c);
            m_Lists.Dequeue();
        }
    }
}
