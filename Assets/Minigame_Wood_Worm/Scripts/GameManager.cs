using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTS.Woodworm
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<GameManager>();
                }
                return instance;
            }
            private set
            {
                instance = value;
            }
        }


        public Dictionary<Vector2, TileControl> map = new Dictionary<Vector2, TileControl>();
        
        public void DestroyTile(Vector3 pos)
        {
            if (map.ContainsKey(pos))
            {
                if (map[pos].leftTile)
                {
                    map[pos].leftTile.rightTile = null;
                    map[pos].leftTile.countNeighbor--;
                }
                if (map[pos].rightTile)
                {
                    map[pos].rightTile.leftTile = null;
                    map[pos].rightTile.countNeighbor--;
                }
                if (map[pos].topTile)
                {
                    map[pos].topTile.bottomTile = null;
                    map[pos].topTile.countNeighbor--;
                }
                if (map[pos].bottomTile)
                {
                    map[pos].bottomTile.topTile = null;
                    map[pos].bottomTile.countNeighbor--;
                }
                Destroy(map[pos].gameObject);
                map.Remove(pos);
            }
        }
        public void GetNeighborTile(TileControl tile)
        {
            Vector3 pos = tile.transform.position + Vector3.left;
            if(map.ContainsKey(pos))
            {
                tile.leftTile = map[pos];
                tile.countNeighbor++;
            }
            pos = tile.transform.position + Vector3.right;
            if (map.ContainsKey(pos))
            {
                tile.rightTile = map[pos];
                tile.countNeighbor++;
            }
            pos = tile.transform.position + Vector3.up;
            if (map.ContainsKey(pos))
            {
                tile.topTile = map[pos];
                tile.countNeighbor++;
            }
            pos = tile.transform.position + Vector3.down;
            if (map.ContainsKey(pos))
            {
                tile.bottomTile = map[pos];
                tile.countNeighbor++;
            }
        }
    }
}
