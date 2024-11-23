using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTS.Woodworm
{
    public class PlayerControl : MonoBehaviour
    {
        public static PlayerControl instance;

        public bool canControl;

        [Header("Part")]
        public SpriteRenderer head;
        public SpriteRenderer mount;
        public Transform body;
        public SpriteRenderer tail;
        public Transform headDirection;
        public Transform tailDirection;

        [Header("Layer")]
        public LayerMask groundLayer;

        [HideInInspector] public Direction faceDirection = Direction.Right;
        [HideInInspector] public List<Vector3> movement = new List<Vector3>();
        [HideInInspector] public Vector3 currentHeadPos;
        [HideInInspector] public Vector3 currentBodyPos;

        [Header("Sprite")]
        public Sprite mountOpen;
        public Sprite mountClose;
        public List<Sprite> headSprite = new List<Sprite>();
        public List<Sprite> tailSprite = new List<Sprite>();

        private void Awake()
        {
            instance = this;
        }
        private void Update()
        {
            MoveInGrid();

            if(canControl)
                GetInputPC();
        }

        void GetInputPC()
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveDirection((int)Direction.Left);
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveDirection((int)Direction.Right);
            }
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveDirection((int)Direction.Up);
            }
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveDirection((int)Direction.Down);
            }
        }

        void MoveInGrid()
        {
            if (movement.Count > 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, movement[0], 15 * Time.deltaTime);

                if (isFall)
                {
                    headDirection.transform.parent.position = transform.position;
                    body.transform.position = Vector2.MoveTowards(body.transform.position, currentHeadPos, 15 * Time.deltaTime);
                    tail.transform.position = Vector2.MoveTowards(tail.transform.position, currentBodyPos, 15 * Time.deltaTime);
                }

                if (transform.position == movement[0])
                {
                    isFall = false;
                    movement.RemoveAt(0);
                    CheckFall();
                }
            }
        }

        public void MoveDirection(int direction)
        {
            if (movement.Count > 0) return;

            switch (direction) 
            { 
                case 0:
                    if (faceDirection != Direction.Right)
                    {
                        if(Physics2D.Raycast(transform.position, Vector2.left, 1, groundLayer)) return;

                        GameManager.Instance.Save();
                        movement.Add(transform.position + Vector3.left);
                        currentHeadPos = transform.position;
                        currentBodyPos = body.transform.position;
                        headDirection.parent.transform.position = movement[0];

                        body.transform.position = currentHeadPos;
                        tail.transform.position = currentBodyPos;

                        faceDirection = Direction.Left;
                        mount.transform.parent.eulerAngles = new Vector3(0, 0, 180);
                        headDirection.parent.eulerAngles = new Vector3(0,0,180);

                        mount.sprite = mountClose;
                        
                        CheckSprite();
                    }
                    else
                    {
                        if (Physics2D.Raycast(body.transform.position, Vector2.left, 1, groundLayer)) return;

                        GameManager.Instance.Save();

                        tail.transform.position = (tail.transform.position - body.transform.position).x == 0 ? tail.transform.position : transform.position;
                        transform.position = body.transform.position + Vector3.left;
                        headDirection.parent.transform.position = transform.position;

                        currentHeadPos = body.transform.position;
                        currentBodyPos = tail.transform.position;
                        GameManager.Instance.DestroyTile(transform.position);

                        faceDirection = Direction.Left;
                        mount.transform.parent.eulerAngles = new Vector3(0, 0, 180);
                        headDirection.parent.eulerAngles = new Vector3(0, 0, 180);

                        mount.sprite = mountClose;

                        CheckSprite();
                        CheckFall();
                    }
                    break;
                case 1:
                    if (faceDirection != Direction.Left)
                    {
                        if (Physics2D.Raycast(transform.position, Vector2.right, 1, groundLayer)) return;

                        GameManager.Instance.Save();
                        movement.Add(transform.position + Vector3.right);
                        currentHeadPos = transform.position;
                        currentBodyPos = body.transform.position;
                        headDirection.parent.transform.position = movement[0];
                        body.transform.position = currentHeadPos;
                        tail.transform.position = currentBodyPos;
                        faceDirection = Direction.Right;
                        mount.transform.parent.eulerAngles = new Vector3(0, 0, 0);
                        headDirection.parent.eulerAngles = new Vector3(0, 0, 0);

                        mount.sprite = mountClose;
                        
                        CheckSprite();
                    }
                    else
                    {
                        if (Physics2D.Raycast(body.transform.position, Vector2.right, 1, groundLayer)) return;

                        GameManager.Instance.Save();

                        tail.transform.position = (tail.transform.position - body.transform.position).x == 0 ? tail.transform.position : transform.position;
                        transform.position = body.transform.position + Vector3.right;
                        headDirection.parent.transform.position = transform.position;

                        currentHeadPos = body.transform.position;
                        currentBodyPos = tail.transform.position;
                        GameManager.Instance.DestroyTile(transform.position);

                        faceDirection = Direction.Right;
                        mount.transform.parent.eulerAngles = new Vector3(0, 0, 0);
                        headDirection.parent.eulerAngles = new Vector3(0, 0, 0);

                        mount.sprite = mountClose;

                        CheckSprite();
                        CheckFall();
                    }
                    break;
                case 2:
                    if (faceDirection != Direction.Down)
                    {
                        if (Physics2D.Raycast(transform.position, Vector2.up, 1, groundLayer)) return;

                        GameManager.Instance.Save();
                        movement.Add(transform.position + Vector3.up);
                        currentHeadPos = transform.position;
                        currentBodyPos = body.transform.position;
                        headDirection.parent.transform.position = movement[0];
                        body.transform.position = currentHeadPos;
                        tail.transform.position = currentBodyPos;
                        faceDirection = Direction.Up;
                        mount.transform.parent.eulerAngles = new Vector3(0, 0, 90);
                        headDirection.parent.eulerAngles = new Vector3(0, 0, 90);

                        mount.sprite = mountClose;
                        
                        CheckSprite();
                    }
                    else
                    {
                        if (Physics2D.Raycast(body.transform.position, Vector2.up, 1, groundLayer)) return;

                        GameManager.Instance.Save();

                        tail.transform.position = (tail.transform.position - body.transform.position).y == 0 ? tail.transform.position : transform.position;
                        transform.position = body.transform.position + Vector3.up;
                        headDirection.parent.transform.position = transform.position;

                        currentHeadPos = body.transform.position;
                        currentBodyPos = tail.transform.position;
                        GameManager.Instance.DestroyTile(transform.position);

                        faceDirection = Direction.Up;
                        mount.transform.parent.eulerAngles = new Vector3(0, 0, 90);
                        headDirection.parent.eulerAngles = new Vector3(0, 0, 90);

                        mount.sprite = mountClose;

                        CheckSprite();
                        CheckFall();
                    }
                    break;
                case 3:
                    if (faceDirection != Direction.Up)
                    {
                        if (Physics2D.Raycast(transform.position, Vector2.down, 1, groundLayer)) return;

                        GameManager.Instance.Save();
                        movement.Add(transform.position + Vector3.down);
                        currentHeadPos = transform.position;
                        currentBodyPos = body.transform.position;
                        headDirection.parent.transform.position = movement[0];
                        body.transform.position = currentHeadPos;
                        tail.transform.position = currentBodyPos;
                        faceDirection = Direction.Down;
                        mount.transform.parent.eulerAngles = new Vector3(0, 0, -90);
                        headDirection.parent.eulerAngles = new Vector3(0, 0, -90);

                        mount.sprite = mountClose;

                        CheckSprite();
                    }
                    else
                    {
                        if (Physics2D.Raycast(body.transform.position, Vector2.down, 1, groundLayer)) return;

                        GameManager.Instance.Save();

                        tail.transform.position = (tail.transform.position - body.transform.position).y == 0 ? tail.transform.position : transform.position;
                        transform.position = body.transform.position + Vector3.down;
                        headDirection.parent.transform.position = transform.position;

                        currentHeadPos = body.transform.position;
                        currentBodyPos = tail.transform.position;
                        GameManager.Instance.DestroyTile(transform.position);

                        faceDirection = Direction.Down;
                        mount.transform.parent.eulerAngles = new Vector3(0, 0, -90);
                        headDirection.parent.eulerAngles = new Vector3(0, 0, -90);

                        mount.sprite = mountClose;

                        CheckSprite();
                        CheckFall();
                    }
                    break;
            }

            if (movement.Count > 0)
                GameManager.Instance.DestroyTile(movement[0]);
        }

        private Vector2 boxCheck = new Vector2(1.2f, 0.1f);
        bool isFall;
        void CheckFall()
        {
            mount.sprite = mountOpen;

            if (!Physics2D.OverlapBox(body.transform.position, boxCheck, 0))
            {
                if(Physics2D.Raycast(transform.position, Vector2.down, 1) 
                    || Physics2D.Raycast(body.transform.position, Vector2.down, 1) 
                    || Physics2D.Raycast(tail.transform.position, Vector2.down, 1))
                {
                    return;
                }

                isFall = true;
                movement.Add(transform.position + Vector3.down);
                currentHeadPos += Vector3.down;
                currentBodyPos += Vector3.down;
                GameManager.Instance.CheckFall();
            }
        }

        RaycastHit2D raycastHit;
        public bool CheckInsideFallingChunk(Dictionary<Vector2, TileControl> chunk)
        {
            Vector3 headPos = movement.Count > 0 ? movement[0] : transform.position;

            raycastHit = Physics2D.Raycast(headPos, Vector2.down, 1);
            if (raycastHit && !chunk.ContainsKey(raycastHit.transform.position))
            {
                return false;
            }
            raycastHit = Physics2D.Raycast(currentHeadPos, Vector2.down, 1);
            if (raycastHit && !chunk.ContainsKey(raycastHit.transform.position))
            {
                return false;
            }
            raycastHit = Physics2D.Raycast(currentBodyPos, Vector2.down, 1);
            if (raycastHit && !chunk.ContainsKey(raycastHit.transform.position))
            {
                return false;
            }



            isFall = true;
            if (movement.Count > 0)
            {
                movement[0] += Vector3.down;
                currentHeadPos += Vector3.down;
                currentBodyPos += Vector3.down;
                body.transform.position = currentHeadPos;
                tail.transform.position = currentBodyPos;
            }
            else
            {
                movement.Add(transform.position + Vector3.down);
                currentHeadPos += Vector3.down;
                currentBodyPos += Vector3.down;
            }
            return true;
        }

        void CheckSprite()
        {
            //Head
            switch (faceDirection)
            {
                case Direction.Right:
                    head.sprite = headSprite[0];
                    break;
                case Direction.Left:
                    head.sprite = headSprite[1];
                    break;
                case Direction.Up:
                    head.sprite = headSprite[2];
                    break;
                case Direction.Down:
                    head.sprite = headSprite[3];
                    break;
            }

            //Tail
            Vector3 dir = currentHeadPos - currentBodyPos;
            if (dir == Vector3.right)
            {
                tail.sprite = tailSprite[0];
                tailDirection.parent.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (dir == Vector3.left)
            {
                tail.sprite = tailSprite[1];
                tailDirection.parent.eulerAngles = new Vector3(0, 0, 180);
            }
            else if (dir == Vector3.up)
            {
                tail.sprite = tailSprite[2];
                tailDirection.parent.eulerAngles = new Vector3(0, 0, 90);
            }
            else if (dir == Vector3.down)
            {
                tail.sprite = tailSprite[3];
                tailDirection.parent.eulerAngles = new Vector3(0, 0, -90);
            }
        }
        public int GetSpriteIndex(int bodyPart)
        {
            switch (bodyPart)
            {
                case 0:
                    return headSprite.IndexOf(head.sprite);
                case 1:
                    return tailSprite.IndexOf(tail.sprite);
            }
            return 0;
        }
        public void UndoPlayer(SaveData saveData)
        {
            faceDirection = saveData.wormDirection;

            head.sprite = headSprite[saveData.headIndex];
            tail.sprite = tailSprite[saveData.tailIndex];

            headDirection.transform.parent.position = saveData.headPos;
            mount.transform.parent.rotation = saveData.headDir;
            headDirection.transform.parent.rotation = saveData.headDir;
            tailDirection.transform.parent.rotation = saveData.tailDir;

            transform.position = saveData.headPos;
            body.transform.position = saveData.bodyPos;
            tail.transform.position = saveData.tailPos;
        }
    }

    public enum Direction
    {
        Left, Right, Up, Down
    }
}
