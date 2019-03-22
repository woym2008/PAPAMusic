using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PAPA
{
    public class DrawCircle : MonoBehaviour
    {
        public float ThetaScale = 0.01f;
        public float radius = 3f;
        private int Size;
        private LineRenderer LineDrawer;
        private float Theta = 0f;
        public float StartWidth = 1.0f;
        public float EndWidth = 1.0f;

        void Start()
        {
            LineDrawer = GetComponent<LineRenderer>();

            //float sizeValue = (2.0f * Mathf.PI) / ThetaScale;
            Size = (int)((1f / ThetaScale) + 2f);
            //Size = (int)sizeValue;
            //Size++;
            //LineDrawer.material = new Material(Shader.Find("Particles/Additive"));
            LineDrawer.startWidth = StartWidth;
            LineDrawer.endWidth = EndWidth;

            LineDrawer.positionCount = Size;
        }

        void Update()
        {
            Theta = 0;
            int sizevalue = (int)((1f / ThetaScale) + 2f);
            if (sizevalue != Size)
            {
                Size = sizevalue;
                LineDrawer.positionCount = Size;
            }

            for (int i = 0; i < Size; i++)
            {
                //theta += (2.0f * Mathf.PI * ThetaScale);            
                Theta += (2.0f * Mathf.PI * ThetaScale);
                float x = radius * Mathf.Cos(Theta);
                float y = radius * Mathf.Sin(Theta);
                x += gameObject.transform.position.x;
                y += gameObject.transform.position.y;

                LineDrawer.SetPosition(i, new Vector3(x, y, 0));
            }

            LineDrawer.startWidth = StartWidth;
            LineDrawer.endWidth = EndWidth;
            //    Theta = 0f;
            //Size = (int)((1f / ThetaScale) + 1f);
            //LineDrawer.positionCount = Size;
            //for (int i = 0; i < Size; i++)
            //{
            //    Theta += (2.0f * Mathf.PI * ThetaScale);
            //    float x = radius * Mathf.Cos(Theta);
            //    float y = radius * Mathf.Sin(Theta);
            //    LineDrawer.SetPosition(i, new Vector3(x, y, 0));
            //}
        }

        public void SetAlpha(float alpha)
        {
            if(LineDrawer != null)
            {
                LineDrawer.material.color = new Color(LineDrawer.material.color.r, LineDrawer.material.color.g, LineDrawer.material.color.b, alpha);
            }
        }
    }
}
