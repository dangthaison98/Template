using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DTS.Woodworm
{
    public class TileHolder : MonoBehaviour
    {
        public GameObject tile;
        public Vector2Int startPos;
        public Vector2Int size;

        public List<TileControl> tiles = new List<TileControl>();

        private void Start()
        {
            GameManager.Instance.tileHolder = this;

            foreach (TileControl tile in tiles)
            {
                tile.Setup();
            }
            foreach (TileControl tile in tiles)
            {
                GameManager.Instance.GetNeighborTile(tile);
            }
        }

        public void CreateBlocks()
        {
            foreach (TileControl tile in tiles) 
            {
                DestroyImmediate(tile.gameObject);
            }

            tiles.Clear();

            for(int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    tiles.Add(Instantiate(tile, startPos + new Vector2(x + 0.5f, y + 0.5f), Quaternion.identity, transform).GetComponent<TileControl>());
                }
            }
        }
    }
}
