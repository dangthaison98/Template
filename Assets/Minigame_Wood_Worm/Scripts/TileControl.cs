using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DTS.Woodworm
{
    public class TileControl : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        [HideInInspector] public Vector3Int pos;

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
            if (!gameObject.activeSelf) return;

            spriteRenderer.sprite = GameManager.Instance.getSpriteTilemap.GetSprite(pos);
            if(GameManager.Instance.getSpriteTilemap.GetTransformMatrix(pos).m00 == -1)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }
    }
}
