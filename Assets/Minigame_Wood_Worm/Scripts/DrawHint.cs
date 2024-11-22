using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTS.Woodworm
{
    [ExecuteInEditMode]
    [RequireComponent (typeof(LineRenderer))]
    public class DrawHint : MonoBehaviour
    {

        public LineRenderer lineRenderer;

        private void OnEnable()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        private void Update()
        {
            lineRenderer.positionCount = transform.childCount;
            for (int i = 0; i < transform.childCount; i++)
            {
                lineRenderer.SetPosition(i, transform.GetChild(i).position);
            }
        }

        private void OnDestroy()
        {
            gameObject.SetActive(false);
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
    }
}
