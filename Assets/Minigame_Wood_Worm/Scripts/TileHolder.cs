using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DTS.Woodworm
{
    public class TileHolder : MonoBehaviour
    {
        public List<TileControl> tiles = new List<TileControl>();

        private void Start()
        {
            foreach (TileControl tile in tiles)
            {
                tile.Setup();
            }
            foreach (TileControl tile in tiles)
            {
                GameManager.Instance.GetNeighborTile(tile);
            }
        }
    }
}
