using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTS.Woodworm
{
    public class PlayerControl : MonoBehaviour
    {
        public enum Direction
        {
            Left, Right, Up, Down
        }

        public bool canControl;

        public GameObject body;
        public GameObject tail;

        Direction faceDirection = Direction.Right;

        List<Vector3> movement = new List<Vector3>();
        Vector2 currentHeadPos;
        Vector2 currentBodyPos;

        private void Update()
        {
            if(!canControl) return;

            GetInputPC();
            MoveInGrid();
        }

        void GetInputPC()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                MoveDirection(Direction.Left);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                MoveDirection(Direction.Right);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                MoveDirection(Direction.Up);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                MoveDirection(Direction.Down);
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
                }
            }
        }

        public void MoveDirection(Direction direction)
        {
            if (movement.Count > 0) return;

            switch (direction) 
            { 
                case Direction.Left:
                    if (faceDirection != Direction.Right)
                    {
                        if(Physics2D.Raycast(transform.position, Vector2.left, 1)) return;

                        movement.Add(transform.position + Vector3.left);
                        currentHeadPos = transform.position;
                        currentBodyPos = body.transform.position;
                        faceDirection = Direction.Left;
                    }
                    break;
                case Direction.Right:
                    if (faceDirection != Direction.Left)
                    {
                        if (Physics2D.Raycast(transform.position, Vector2.right, 1)) return;

                        movement.Add(transform.position + Vector3.right);
                        currentHeadPos = transform.position;
                        currentBodyPos = body.transform.position;
                        faceDirection = Direction.Right;
                    }
                    break;
                case Direction.Up:
                    if (faceDirection != Direction.Down)
                    {
                        if (Physics2D.Raycast(transform.position, Vector2.up, 1)) return;

                        movement.Add(transform.position + Vector3.up);
                        currentHeadPos = transform.position;
                        currentBodyPos = body.transform.position;
                        faceDirection = Direction.Up;
                    }
                    break;
                case Direction.Down:
                    if (faceDirection != Direction.Up)
                    {
                        if (Physics2D.Raycast(transform.position, Vector2.down, 1)) return;

                        movement.Add(transform.position + Vector3.down);
                        currentHeadPos = transform.position;
                        currentBodyPos = body.transform.position;
                        faceDirection = Direction.Down;
                    }
                    break;
            }
        }
    }
}
