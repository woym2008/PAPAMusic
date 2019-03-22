/* ========================================================
 *	类名称：RhythmManager
 *	作 者：Zhangfan
 *	创建时间：2019-03-07 19:54:57
 *	版 本：V1.0.0
 *	描 述：
* ========================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static PAPA.CircleControl;

namespace PAPA
{
    public class RhythmManager : MonoBehaviour
    {
        public static RhythmManager Instance;

        public List<RhythmGenerator> gemeratorList = new List<RhythmGenerator>();

        public delegate void OnTrig(float power);
        public OnTrig m_Trig;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
        }

        public void OnClick()
        {
            for(int i=0;i< gemeratorList.Count;++i)
            {
                CircleControl circle = gemeratorList[i].GetCircle();
                float score = 0;
                if(circle != null && circle.TryTriggerRhythm(out score))
                {
                    if(m_Trig != null)
                    {
                        Debug.LogWarning("circle.ClickOver");
                        gemeratorList[i].ReleaseCircle();
                        m_Trig.Invoke(score);
                    }
                }
                else
                {
                    if (m_Trig != null)
                    {
                        m_Trig.Invoke(1);
                    }
                }

                break;
            }
        }

        public void OnMiss()
        {
            if (m_Trig != null)
            {
                m_Trig.Invoke(1);
            }
        }
    }
}
