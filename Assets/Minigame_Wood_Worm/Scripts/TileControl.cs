using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTS.Woodworm
{
    public class TileControl : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public TileControl leftTile, rightTile, topTile, bottomTile;
        public Vector3Int pos;

        public void Setup()
        {
            GameManager.Instance.OnAutoTile += AutoTile;
            GameManager.Instance.InitTile(this, 0);
        }
        public void RebakeMap(int map)
        {
            GameManager.Instance.InitTile(this, map);
        }
        public void AutoTile()
        {
            spriteRenderer.sprite = GameManager.Instance.getSpriteTilemap.GetSprite(pos);
        }

        private void OnDestroy()
        {
            GameManager.Instance.getSpriteTilemap.SetTile(pos, null);
            GameManager.Instance.OnAutoTile -= AutoTile;
        }
    }
}
