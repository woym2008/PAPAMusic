using PAPA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCtrl : MonoBehaviour
{
    public Animator m_PlantAnim;
    public Animator m_RightWrongAnim;

    //public Transform CameraPoint;
    //public Camera m_Camera;

    public bool interrupt = false;

    readonly int rightHash = Animator.StringToHash("push_right");
    readonly int wrongHash = Animator.StringToHash("push_wrong");

    readonly int readyrightHash = Animator.StringToHash("ready_push_right");
    readonly int readywrongHash = Animator.StringToHash("ready_push_wrong");

    public int HP;

    public Transform EnergyTarget;
    public Transform EnergyPoint;
    private Vector3 CacheEnergyPointPosition;
    private Dictionary<int,Vector3> _cacheTargetPositions = new Dictionary<int, Vector3>();

    private void Awake()
    {
        Screen.SetResolution(360, 540, false);
    }

    // Start is called before the first frame update
    void Start()
    {
        RhythmManager.Instance.m_Trig += OnTrig;
        HP = 0;
        CacheEnergyPointPosition = EnergyPoint.transform.position;
        _cacheTargetPositions.Add(HP, EnergyPoint.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        //if(m_Camera != null && CameraPoint != null)
        //{
        //    m_Camera.transform.position = CameraPoint.position;
        //}
        if(EnergyPoint != null && EnergyTarget!=null)
        {            
            int curprocess = HP % 5;
            float target =
                CacheEnergyPointPosition.y + 
                (EnergyTarget.transform.position.y - CacheEnergyPointPosition.y) * ((float)curprocess * 0.2f);
            float newy = Mathf.Lerp(EnergyPoint.transform.position.y, target, 0.1f);
            EnergyPoint.transform.position = new Vector3(
                EnergyPoint.transform.position.x,
                newy,
                EnergyPoint.transform.position.z
                );
        }

    }

    void OnTrig(float power)
    {        
        //Debug.Log("OnTrig: " + power);
        if (power < 0.2f)
        {
            m_RightWrongAnim.Play(readyrightHash);
            //Anim.SetBool("right", true);
            //Anim.SetBool("wrong", false);
            HP++;

            if (HP % 5 == 0 && HP > 0)
            {
                CacheEnergyPointPosition = EnergyTarget.transform.position;
                _cacheTargetPositions[HP] = CacheEnergyPointPosition;

            }
        }
        else
        {
            //下降之前 调整基础位置
            if (HP % 5 == 0 && HP > 0)
            {
                if (_cacheTargetPositions.TryGetValue(HP - 5, out Vector3 target))
                {
                    CacheEnergyPointPosition = target;
                }
            }

            m_RightWrongAnim.Play(readywrongHash);
            //Anim.SetBool("right", false);
            //Anim.SetBool("wrong", true);       

            HP--;
            if (HP <= 0)
            {
                HP = 0;
            }          
        }

        m_PlantAnim.SetInteger("growpower", HP);
    }

    bool bRight = false;
    bool bWrong = false;
    //public void OnFinishRight()
    //{
    //    bWrong = false;
    //    bRight = false;
    //    Anim.SetBool("right", bRight);
    //    Anim.SetBool("wrong", bWrong);
    //    Debug.Log("OnFinishRight");
    //}

    //public void OnFinishWrong()
    //{
    //    Debug.Log("OnFinishWrong");
    //    bWrong = false;
    //    bRight = false;
    //    Anim.SetBool("right", bRight);
    //    Anim.SetBool("wrong", bWrong);
    //}
}
