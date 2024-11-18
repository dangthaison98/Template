using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTS.Woodworm
{
    public class TileControl : MonoBehaviour
    {
        public TileControl leftTile, rightTile, topTile, bottomTile;
        public int countNeighbor;

        public void Setup()
        {
            GameManager.Instance.map.Add(transform.position, this);
        }
        public void AutoTile()
        {

        }
    }
}
