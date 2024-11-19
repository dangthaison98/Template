using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTS.Woodworm
{
    public class TileControl : MonoBehaviour
    {
        public TileControl leftTile, rightTile, topTile, bottomTile;

        public void Setup()
        {
            GameManager.Instance.InitTile(this, 0);
        }
        public void RebakeMap(int map)
        {
            GameManager.Instance.InitTile(this, map);
        }
        public void AutoTile()
        {

        }
    }
}
