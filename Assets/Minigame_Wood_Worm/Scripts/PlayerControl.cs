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
        public SpriteRenderer body;
        public SpriteRenderer tail;

        [Header("Layer")]
        public LayerMask groundLayer;

        [HideInInspector] public Direction faceDirection = Direction.Right;
        [HideInInspector] public List<Vector3> movement = new List<Vector3>();
        [HideInInspector] public Vector3 currentHeadPos;
        [HideInInspector] public Vector3 currentBodyPos;

        public Sprite[] headSprite;
        public Sprite[] bodySprite;
        public Sprite[] tailSprite;

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
                transform.position = Vector2.MoveTowards(transform.position, movement[0], 10 * Time.deltaTime);
                body.transform.position = Vector2.MoveTowards(body.transform.position, currentHeadPos, 10 * Time.deltaTime);
                tail.transform.position = Vector2.MoveTowards(tail.transform.position, currentBodyPos, 10 * Time.deltaTime);
                if (transform.position == movement[0])
                {
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
                        faceDirection = Direction.Left;
                        CheckSprite();
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
                        faceDirection = Direction.Right;
                        CheckSprite();
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
                        faceDirection = Direction.Up;
                        CheckSprite();
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
                        faceDirection = Direction.Down;
                        CheckSprite();
                    }
                    break;
            }

            if (movement.Count > 0)
                GameManager.Instance.DestroyTile(movement[0]);
        }

        private Vector2 boxCheck = new Vector2(1.2f, 0.1f);
        void CheckFall()
        {
            if(!Physics2D.OverlapBox(body.transform.position, boxCheck, 0))
            {
                if(Physics2D.Raycast(transform.position, Vector2.down, 1) 
                    || Physics2D.Raycast(body.transform.position, Vector2.down, 1) 
                    || Physics2D.Raycast(tail.transform.position, Vector2.down, 1))
                {
                    return;
                }

                movement.Add(transform.position + Vector3.down);
                currentHeadPos = body.transform.position + Vector3.down;
                currentBodyPos = tail.transform.position + Vector3.down;
                GameManager.Instance.CheckFall();
            }
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

            //Body
            Vector3 headPos = movement.Count > 0 ? movement[0] : transform.position;
            Vector3 dir = headPos - currentBodyPos;
            if (dir.x == -1 && dir.y > 0 && currentHeadPos.y > currentBodyPos.y)
            {
                body.sprite = bodySprite[2];
            }
            else if(dir.x == 1 && dir.y > 0 && currentHeadPos.y > currentBodyPos.y)
            {
                body.sprite = bodySprite[3];
            }
            else if(Mathf.Abs(dir.x) == 2)
            {
                body.sprite = bodySprite[1];
            }
            else
            {
                body.sprite = bodySprite[0];
            }

            //Tail
            dir = currentHeadPos - currentBodyPos;
            if (dir == Vector3.right)
            {
                tail.sprite = tailSprite[0];
            }
            else if (dir == Vector3.left)
            {
                tail.sprite = tailSprite[1];
            }
            else if (dir == Vector3.up)
            {
                tail.sprite = tailSprite[2];
            }
            else if (dir == Vector3.down)
            {
                tail.sprite = tailSprite[3];
            }
        }
    }

    public enum Direction
    {
        Left, Right, Up, Down
    }
}
