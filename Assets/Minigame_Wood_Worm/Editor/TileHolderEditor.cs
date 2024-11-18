using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DTS.Woodworm
{
    [CustomEditor(typeof(TileHolder))]
    public class TileHolderEditor : Editor
    {
        TileHolder tileHolder;

        private void OnEnable()
        {
            tileHolder = FindObjectOfType<TileHolder>();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Create Blocks"))
            {
                tileHolder.CreateBlocks();
            }
        }
    }
}
