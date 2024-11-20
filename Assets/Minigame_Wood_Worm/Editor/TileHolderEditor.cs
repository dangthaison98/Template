#if UNITY_EDITOR
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
            if (GUILayout.Button("Create Blocks", GUILayout.Height(50)))
            {
                tileHolder.CreateBlocks();
            }
        }
    }
}
#endif
