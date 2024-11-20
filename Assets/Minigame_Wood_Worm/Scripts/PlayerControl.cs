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
        public Transform body;
        public Transform tail;

        [Header("Layer")]
        public LayerMask groundLayer;

        [HideInInspector] public Direction faceDirection = Direction.Right;
        [HideInInspector] public List<Vector3> movement = new List<Vector3>();
        [HideInInspector] public Vector3 currentHeadPos;
        [HideInInspector] public Vector3 currentBodyPos;

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
                body.position = Vector2.MoveTowards(body.position, currentHeadPos, 10 * Time.deltaTime);
                tail.position = Vector2.MoveTowards(tail.position, currentBodyPos, 10 * Time.deltaTime);
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
                        currentBodyPos = body.position;
                        faceDirection = Direction.Left;
                    }
                    break;
                case 1:
                    if (faceDirection != Direction.Left)
                    {
                        if (Physics2D.Raycast(transform.position, Vector2.right, 1, groundLayer)) return;

                        GameManager.Instance.Save();
                        movement.Add(transform.position + Vector3.right);
                        currentHeadPos = transform.position;
                        currentBodyPos = body.position;
                        faceDirection = Direction.Right;
                    }
                    break;
                case 2:
                    if (faceDirection != Direction.Down)
                    {
                        if (Physics2D.Raycast(transform.position, Vector2.up, 1, groundLayer)) return;

                        GameManager.Instance.Save();
                        movement.Add(transform.position + Vector3.up);
                        currentHeadPos = transform.position;
                        currentBodyPos = body.position;
                        faceDirection = Direction.Up;
                    }
                    break;
                case 3:
                    if (faceDirection != Direction.Up)
                    {
                        if (Physics2D.Raycast(transform.position, Vector2.down, 1, groundLayer)) return;

                        GameManager.Instance.Save();
                        movement.Add(transform.position + Vector3.down);
                        currentHeadPos = transform.position;
                        currentBodyPos = body.position;
                        faceDirection = Direction.Down;
                    }
                    break;
            }

            if (movement.Count > 0)
                GameManager.Instance.DestroyTile(movement[0]);
        }

        private Vector2 boxCheck = new Vector2(1.2f, 0.1f);
        void CheckFall()
        {
            if(!Physics2D.OverlapBox(body.position, boxCheck, 0))
            {
                if(Physics2D.Raycast(transform.position, Vector2.down, 1) 
                    || Physics2D.Raycast(body.position, Vector2.down, 1) 
                    || Physics2D.Raycast(tail.position, Vector2.down, 1))
                {
                    return;
                }

                movement.Add(transform.position + Vector3.down);
                currentHeadPos = body.position + Vector3.down;
                currentBodyPos = tail.position + Vector3.down;
                GameManager.Instance.CheckFall();
            }
        }
    }

    public enum Direction
    {
        Left, Right, Up, Down
    }
}
