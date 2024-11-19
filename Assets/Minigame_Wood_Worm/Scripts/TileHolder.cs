using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DTS.Woodworm
{
    public class TileHolder : MonoBehaviour
    {
        public Tilemap demoShape;
        public int countDemoBlock;
        public List<TileControl> tiles = new List<TileControl>();

        private void Start()
        {
            countDemoBlock = GetAmountOfDirtTiles();

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

        public int GetAmountOfDirtTiles()
        {
            demoShape.CompressBounds();
            int amount = 0;
            foreach (var pos in demoShape.cellBounds.allPositionsWithin)
            {
                if (demoShape.GetTile(pos) != null) { amount += 1; }
            }

            return amount;
        }

#if UNITY_EDITOR
        public GameObject tile;
        public Vector2Int startPos;
        public Vector2Int size;
        public void CreateBlocks()
        {
            if (tile == null) return;

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
        #endif
    }
}
