using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        [HideInInspector] public TileHolder tileHolder;

        public Dictionary<Vector2, TileControl> mapBlockHolder = new Dictionary<Vector2, TileControl>();
        List<Dictionary<Vector2, TileControl>> chunk = new List<Dictionary<Vector2, TileControl>>();
        
        public void InitTile(TileControl tile, int count)
        {
            mapBlockHolder.Add(tile.transform.position, tile);
            if (chunk.Count == 0)
            {
                chunk.Add(new Dictionary<Vector2, TileControl>());
            }
            chunk[count].Add(tile.transform.position, tile);
        }

        private void Update()
        {
            if (fallChunk >= 0)
            {
                foreach (var key in chunk[fallChunk])
                {
                    key.Value.transform.position = Vector2.MoveTowards(key.Value.transform.position, key.Key + Vector2.down, 10 * Time.deltaTime);
                    if ((Vector2)key.Value.transform.position == key.Key + Vector2.down)
                    {
                        mapBlockHolder.Clear();
                        for (int i = 0; i < chunk.Count; i++)
                        {
                            chunk[i].Clear();
                            foreach(var item in chunk[i])
                            {
                                item.Value.RebakeMap(i);
                            }
                        }
                        fallChunk = -1;
                        CheckFall();
                        return;
                    }
                }

            }
        }

        public void DestroyTile(Vector3 pos)
        {
            foreach (var map in chunk)
            {
                if (map.ContainsKey(pos))
                {
                    TileControl tempTile = null;
                    if (map[pos].leftTile)
                    {
                        map[pos].leftTile.rightTile = null;
                        tempTile = map[pos].leftTile;
                    }
                    if (map[pos].rightTile)
                    {
                        map[pos].rightTile.leftTile = null;
                        tempTile = map[pos].rightTile;
                    }
                    if (map[pos].topTile)
                    {
                        map[pos].topTile.bottomTile = null;
                        tempTile = map[pos].topTile;
                    }
                    if (map[pos].bottomTile)
                    {
                        map[pos].bottomTile.topTile = null;
                        tempTile = map[pos].bottomTile;
                    }
                    tileHolder.tiles.Remove(map[pos]);
                    Destroy(map[pos].gameObject);
                    map.Remove(pos);
                    mapBlockHolder.Remove(pos);

                    if(map.Count == 0)
                    {
                        chunk.Remove(map);
                        return;
                    }

                    if (tempTile)
                    {
                        Dictionary<Vector2, TileControl> tempDictionary = new Dictionary<Vector2, TileControl>();
                        List<TileControl> tempList = new List<TileControl>();
                        tempDictionary.Add(tempTile.transform.position, tempTile);
                        tempList.Add(tempTile);
                        while(tempList.Count > 0)
                        {
                            if (tempList[0].leftTile)
                            {
                                if (!tempDictionary.ContainsKey(tempList[0].leftTile.transform.position))
                                {
                                    tempDictionary.Add(tempList[0].leftTile.transform.position, tempList[0].leftTile);
                                    tempList.Add(tempList[0].leftTile);
                                }
                            }
                            if (tempList[0].rightTile)
                            {
                                if (!tempDictionary.ContainsKey(tempList[0].rightTile.transform.position))
                                {
                                    tempDictionary.Add(tempList[0].rightTile.transform.position, tempList[0].rightTile);
                                    tempList.Add(tempList[0].rightTile);
                                }
                            }
                            if (tempList[0].topTile)
                            {
                                if (!tempDictionary.ContainsKey(tempList[0].topTile.transform.position))
                                {
                                    tempDictionary.Add(tempList[0].topTile.transform.position, tempList[0].topTile);
                                    tempList.Add(tempList[0].topTile);
                                }
                            }
                            if (tempList[0].bottomTile)
                            {
                                if (!tempDictionary.ContainsKey(tempList[0].bottomTile.transform.position))
                                {
                                    tempDictionary.Add(tempList[0].bottomTile.transform.position, tempList[0].bottomTile);
                                    tempList.Add(tempList[0].bottomTile);
                                }
                            }
                            tempList.RemoveAt(0);
                        }

                        if(tempDictionary.Count != map.Count)
                        {
                            chunk.Add(tempDictionary);

                            List<Vector2> listPos = map.Keys.ToList();
                            for(int i = listPos.Count - 1; i >= 0; i--)
                            {
                                if (tempDictionary.ContainsKey(listPos[i]))
                                {
                                    listPos.Remove(listPos[i]);
                                }
                            }
                            foreach (var key in listPos)
                            {
                                tempList.Add(map[key]);
                            }

                            map.Clear();
                            foreach (var key in tempList)
                            {
                                map.Add(key.transform.position, key);
                            }
                        }
                    }

                    return;
                }
            }

            CheckFall();
        }
        public void GetNeighborTile(TileControl tile)
        {
            Vector3 pos = tile.transform.position + Vector3.left;
            if (chunk[0].ContainsKey(pos))
            {
                tile.leftTile = chunk[0][pos];
            }
            pos = tile.transform.position + Vector3.right;
            if (chunk[0].ContainsKey(pos))
            {
                tile.rightTile = chunk[0][pos];
            }
            pos = tile.transform.position + Vector3.up;
            if (chunk[0].ContainsKey(pos))
            {
                tile.topTile = chunk[0][pos];
            }
            pos = tile.transform.position + Vector3.down;
            if (chunk[0].ContainsKey(pos))
            {
                tile.bottomTile = chunk[0][pos];
            }
        }

        int fallChunk = -1;
        void CheckFall()
        {
            bool isFall;
            foreach (var map in chunk) 
            {
                isFall = true;
                foreach (var key in map)
                {
                    if (!key.Value.bottomTile && mapBlockHolder.ContainsKey(key.Key + Vector2.down))
                    {
                        isFall = false;
                        break;
                    }
                    if(key.Key.y < 1)
                    {
                        isFall = false;
                        break;
                    }
                }
                if (isFall)
                {
                    fallChunk = chunk.IndexOf(map);
                    return;
                }
            }
        }
    }
}
