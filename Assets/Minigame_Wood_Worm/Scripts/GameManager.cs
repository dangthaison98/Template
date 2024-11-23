using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

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

        List<Dictionary<Vector2, TileControl>> chunk = new List<Dictionary<Vector2, TileControl>>();

        public LevelData levelData;

        public Tilemap getSpriteTilemap;
        public TileBase tileBase;
        public Action OnAutoTile;

        private List<SaveData> saveDatas = new List<SaveData>();

        public void InitTile(TileControl tile, int count)
        {
            if (chunk.Count == 0)
            {
                chunk.Add(new Dictionary<Vector2, TileControl>());
            }
            chunk[count].Add(tile.transform.position, tile);
        }

        private void Awake()
        {
            Application.targetFrameRate = 60;

            int level = PlayerPrefs.GetInt("Level", 0) % levelData.levels.Length;
            Instantiate(levelData.levels[level]);
        }

        bool isDoneFall;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Undo();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                Restart();
            }

            if (fallChunk >= 0)
            {
                isDoneFall = true;
                foreach (var key in chunk[fallChunk])
                {
                    key.Value.transform.position = Vector2.MoveTowards(key.Value.transform.position, key.Key + Vector2.down, 15 * Time.deltaTime);
                    if ((Vector2)key.Value.transform.position != key.Key + Vector2.down)
                    {
                        isDoneFall = false;
                    }
                }

                if (isDoneFall)
                {
                    for (int i = 0; i < chunk.Count; i++)
                    {
                        TileControl[] tempTileList = chunk[i].Values.ToArray();
                        chunk[i].Clear();
                        foreach (var item in tempTileList)
                        {
                            item.RebakeMap(i);
                        }
                    }
                    fallChunk = -1;
                    CheckFall();
                    return;
                }
            }
        }

        #region Tile Control
        public void DestroyTile(Vector3 pos)
        {
            foreach (var map in chunk)
            {
                if (map.ContainsKey(pos))
                {
                    TileControl tempTile = null;
                    if (map.ContainsKey(pos + Vector3.left))
                    {
                        tempTile = map[pos + Vector3.left];
                    }
                    else if (map.ContainsKey(pos + Vector3.right))
                    {
                        tempTile = map[pos + Vector3.right];
                    }
                    else if (map.ContainsKey(pos + Vector3.up))
                    {
                        tempTile = map[pos + Vector3.up];
                    }
                    else if (map.ContainsKey(pos + Vector3.down))
                    {
                        tempTile = map[pos + Vector3.down];
                    }
                    tileHolder.tiles.Remove(map[pos]);
                    getSpriteTilemap.SetTile(map[pos].pos, null);
                    map[pos].gameObject.SetActive(false);
                    map.Remove(pos);
                    OnAutoTile?.Invoke();

                    if(map.Count == 0)
                    {
                        chunk.Remove(map);
                        CheckFall();
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
                            if (map.TryGetValue(tempList[0].transform.position + Vector3.left, out tempTile))
                            {
                                if (!tempDictionary.ContainsKey(tempList[0].transform.position + Vector3.left))
                                {
                                    tempDictionary.Add(tempTile.transform.position, tempTile);
                                    tempList.Add(tempTile);
                                }
                            }
                            if (map.TryGetValue(tempList[0].transform.position + Vector3.right, out tempTile))
                            {
                                if (!tempDictionary.ContainsKey(tempList[0].transform.position + Vector3.right))
                                {
                                    tempDictionary.Add(tempTile.transform.position, tempTile);
                                    tempList.Add(tempTile);
                                }
                            }
                            if (map.TryGetValue(tempList[0].transform.position + Vector3.up, out tempTile))
                            {
                                if (!tempDictionary.ContainsKey(tempList[0].transform.position + Vector3.up))
                                {
                                    tempDictionary.Add(tempTile.transform.position, tempTile);
                                    tempList.Add(tempTile);
                                }
                            }
                            if (map.TryGetValue(tempList[0].transform.position + Vector3.down, out tempTile))
                            {
                                if (!tempDictionary.ContainsKey(tempList[0].transform.position + Vector3.down))
                                {
                                    tempDictionary.Add(tempTile.transform.position, tempTile);
                                    tempList.Add(tempTile);
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

                    CheckFall();
                    return;
                }
            }

            CheckFall();
        }

        int fallChunk = -1;
        public void CheckFall()
        {
            bool isFall;
            foreach (var map in chunk) 
            {
                isFall = true;
                foreach (var key in map)
                {
                    if (!map.ContainsKey(key.Key + Vector2.down) && (Physics2D.Raycast(key.Key + Vector2.down, Vector2.down, 0.1f) || key.Key.y < 1))
                    {
                        isFall = false;
                        break;
                    }

                    Vector3 headPos = PlayerControl.instance.movement.Count > 0 ? PlayerControl.instance.movement[0] : PlayerControl.instance.transform.position;

                    if ((Vector2)headPos == key.Key + Vector2.down ||
                        (Vector2)PlayerControl.instance.currentHeadPos == key.Key + Vector2.down ||
                        (Vector2)PlayerControl.instance.currentBodyPos == key.Key + Vector2.down)
                    {
                        if (!PlayerControl.instance.CheckInsideFallingChunk(map))
                        {
                            isFall = false;
                            break;
                        }
                    }
                }
                if (isFall)
                {
                    fallChunk = chunk.IndexOf(map);
                    PlayerControl.instance.canControl = false;
                    return;
                }

                PlayerControl.instance.canControl = true;
            }

            CheckWin();
        }
        bool isWin;
        void CheckWin()
        {
            if(tileHolder.tiles.Count == tileHolder.countDemoBlock)
            {
                Vector3 lockDemoPos = Vector3.zero;

                Vector3 lockTilePos = new Vector3(100, 0.5f, 0);

                for (int i = 1; i < tileHolder.tiles.Count; i++)
                {
                    if(tileHolder.tiles[i].transform.position.y == 0.5f)
                    {
                        if(tileHolder.tiles[i].transform.position.x < lockTilePos.x)
                        {
                            lockTilePos.x = tileHolder.tiles[i].transform.position.x;
                        }
                    }
                }

                foreach (var pos in tileHolder.demoShape.cellBounds.allPositionsWithin)
                {
                    if (tileHolder.demoShape.GetTile(pos) != null) 
                    { 
                        lockDemoPos = pos;
                        break;
                    }
                }
                for (int i = 0; i < tileHolder.tiles.Count; i++)
                {
                    if (!tileHolder.demoShape.GetTile(Vector3Int.FloorToInt(lockDemoPos + tileHolder.tiles[i].transform.position - lockTilePos)))
                    {
                        return;
                    }
                }

                if(isWin) return;
                isWin = true;
                PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
                PlayerControl.instance.canControl = false;

                for (int i = 0; i < tileHolder.tiles.Count; i++)
                {
                    tileHolder.tiles[i].spriteRenderer.sprite = tileHolder.demoShape.GetSprite(Vector3Int.FloorToInt(lockDemoPos + tileHolder.tiles[i].transform.position - lockTilePos));
                    if (tileHolder.demoShape.GetTransformMatrix(Vector3Int.FloorToInt(lockDemoPos + tileHolder.tiles[i].transform.position - lockTilePos)).m00 == -1)
                    {
                        tileHolder.tiles[i].spriteRenderer.flipX = true;
                    }
                    else
                    {
                        tileHolder.tiles[i].spriteRenderer.flipX = false;
                    }
                }

                Invoke(nameof(Restart), 2);
            }
        }
        #endregion

        #region Save Load
        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        public void Save()
        {
            SaveData saveData = new SaveData();
            saveData.wormDirection = PlayerControl.instance.faceDirection;

            saveData.headIndex = PlayerControl.instance.GetSpriteIndex(0);
            saveData.tailIndex = PlayerControl.instance.GetSpriteIndex(1);

            saveData.headDir = PlayerControl.instance.headDirection.transform.parent.rotation;
            saveData.tailDir = PlayerControl.instance.tailDirection.transform.parent.rotation;

            saveData.headPos = PlayerControl.instance.transform.position;
            saveData.bodyPos = PlayerControl.instance.body.transform.position;
            saveData.tailPos = PlayerControl.instance.tail.transform.position;
            saveData.tiles = new List<TileControl>(tileHolder.tiles);
            saveData.chunk = new List<Dictionary<Vector2, TileControl>>(chunk.Count);
            chunk.ForEach((item) =>
            {
                saveData.chunk.Add(new Dictionary<Vector2, TileControl>(item));
            });

            saveDatas.Add(saveData);
        }
        public void Undo()
        {
            if (saveDatas.Count == 0 || isWin) return;

            SaveData saveData = saveDatas.Last();

            PlayerControl.instance.UndoPlayer(saveData);

            tileHolder.tiles = saveData.tiles;
            chunk = saveData.chunk;

            saveDatas.Remove(saveData);

            foreach (var map in chunk)
            {
                foreach(var key in map)
                {
                    key.Value.transform.position = key.Key;
                }
            }
            foreach(var tile in tileHolder.tiles)
            {
                tile.gameObject.SetActive(true);
                getSpriteTilemap.SetTile(tile.pos, tileBase);
            }
            OnAutoTile?.Invoke();
        }
        public void Hint()
        {
            tileHolder.ShowHint();
        }
        #endregion
    }

    [System.Serializable]
    public class SaveData
    {
        public Direction wormDirection;

        public int headIndex, tailIndex;
        public Quaternion headDir, tailDir;

        public Vector3 headPos;
        public Vector3 bodyPos;
        public Vector3 tailPos;
        public List<TileControl> tiles = new List<TileControl>();
        public List<Dictionary<Vector2, TileControl>> chunk = new List<Dictionary<Vector2, TileControl>>();
    }
}
