using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTS.Woodworm
{
    public class DrawLine : MonoBehaviour
    {
        public LineRenderer lineRenderer;

        public Transform headPos;
        public Transform tailPos;
        private void LateUpdate()
        {
            lineRenderer.SetPosition(0, headPos.position);
            lineRenderer.SetPosition(1, transform.position);
            lineRenderer.SetPosition(2, tailPos.position);
        }
    }
}
