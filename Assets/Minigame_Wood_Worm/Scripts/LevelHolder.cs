using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DTS.Woodworm
{
    public class LevelHolder : MonoBehaviour
    {
        public Tilemap demoShape;
        public List<TileControl> tiles = new List<TileControl>();

        private void Start()
        {
            foreach (TileControl tile in tiles)
            {
                tile.Setup();
            }
        }
    }
}
