using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DTS.Woodworm
{
    public class TileHolder : MonoBehaviour
    {
        public int wormPos;
        public int cameraSize = 15;

        public Tilemap demoShape;
        [HideInInspector] public int countDemoBlock;

        [HideInInspector] public List<TileControl> tiles = new List<TileControl>();
        public Vector2Int size;

        Action OnShowHint;

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

            GameManager.Instance.virtualCamera.m_Lens.OrthographicSize = cameraSize;
            PlayerControl.instance.transform.parent.position = new Vector3(wormPos, 0, 0);

            foreach (TileControl tile in tiles)
            {
                tile.Setup();
                OnShowHint += tile.DestroyTile;
            }

            GameManager.Instance.OnAutoTile?.Invoke();
        }

        public void ShowHint()
        {
            OnShowHint?.Invoke();
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
        //public List<int> hint = new List<int>();
        public void CreateBlocks()
        {
            if (tile == null) return;

            //hint.Clear();

            foreach (TileControl tile in tiles) 
            {
                if (tile)
                {
                    //if (tile.IsSafe)
                    //{
                    //    hint.Add(1);
                    //}
                    //else
                    //{
                    //    hint.Add(0);
                    //}
                    DestroyImmediate(tile.gameObject);
                }
            }

            tiles.Clear();

            int count = 0;
            for(int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    var tl = Instantiate(tile, startPos + new Vector2(x + 0.5f, y + 0.5f), Quaternion.identity, transform).GetComponent<TileControl>();
                    //tl.IsSafe = hint[count] == 1 ? true : false;
                    count++;
                    tiles.Add(tl);
                }
            }
        }
        #endif
    }
}
