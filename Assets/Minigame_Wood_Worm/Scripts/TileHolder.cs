using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DTS.Woodworm
{
    public class TileHolder : MonoBehaviour
    {
        public Tilemap demoShape;
        [HideInInspector] public int countDemoBlock;

        [HideInInspector] public List<TileControl> tiles = new List<TileControl>();
        public Vector2Int size;

        private void Start()
        {
            countDemoBlock = GetAmountOfDirtTiles();

            int tileIndex = 0;
            Vector3Int standPos = Vector3Int.zero;
            GameManager.Instance.getSpriteTilemap.ClearAllTiles();
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    standPos = new Vector3Int(100 + x, 100 + y);
                    tiles[tileIndex].pos = standPos;
                    tileIndex++;
                    GameManager.Instance.getSpriteTilemap.SetTile(standPos, GameManager.Instance.tileBase);
                }
            }

            GameManager.Instance.tileHolder = this;

            foreach (TileControl tile in tiles)
            {
                tile.Setup();
            }

            GameManager.Instance.OnAutoTile?.Invoke();
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
        public void CreateBlocks()
        {
            if (tile == null) return;

            foreach (TileControl tile in tiles) 
            {
                if (tile)
                {
                    DestroyImmediate(tile.gameObject);
                }
            }

            tiles.Clear();

            for(int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    tiles.Add(Instantiate(tile, startPos + new Vector2(x + 0.5f, y + 0.5f), Quaternion.identity, transform).GetComponent<TileControl>());
                }
            }
        }
        #endif
    }
}
