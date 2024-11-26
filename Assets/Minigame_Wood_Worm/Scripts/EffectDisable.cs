using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTS.Woodworm
{
    public class EffectDisable : MonoBehaviour
    {
        public string effectName;

        bool isApplicationQuitting;
        private void OnApplicationQuit()
        {
            isApplicationQuitting = true;
        }

        private void OnDisable()
        {
            if (Application.isPlaying) return;

            PoolManager.Instance.Despawn(effectName, gameObject);
        }
    }
}
