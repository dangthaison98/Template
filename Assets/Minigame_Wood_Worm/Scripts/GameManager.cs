using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTS.Woodworm
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<GameManager>();
                }
                return instance;
            }
            private set
            {
                instance = value;
            }
        }


        public Dictionary<Vector2, Transform> map = new Dictionary<Vector2, Transform>();
        
        public void DestroyTile(Vector3 pos)
        {
            if (map.ContainsKey(pos))
            {
                //Destroy
                map.Remove(pos);
            }
        }
    }
}
