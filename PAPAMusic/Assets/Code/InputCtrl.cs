using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PAPA
{
    public class InputCtrl : MonoBehaviour
    {
        static public InputCtrl Instance;
        
        //RhythmGenerator r1;
        //RhythmGenerator r2;

        private void Awake()
        {
            Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnMouseDown()
        {
            //Debug.Log("OnMouseDown");

            RhythmManager.Instance.OnClick();

        }
    }
}

