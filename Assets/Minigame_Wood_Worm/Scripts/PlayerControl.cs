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

        Direction faceDirection = Direction.Right;

        List<Vector3> movement = new List<Vector3>();

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
                if(transform.position == movement[0])
                {
                    movement.RemoveAt(0);
                }
            }
        }

        public void MoveDirection(Direction direction)
        {
            switch (direction) 
            { 
                case Direction.Left:
                    if (movement.Count == 0 && faceDirection != Direction.Right)
                    {
                        movement.Add(transform.position + Vector3.left);
                        faceDirection = Direction.Left;
                    }
                    break;
                case Direction.Right:
                    if (movement.Count == 0 && faceDirection != Direction.Left)
                    {
                        movement.Add(transform.position + Vector3.right);
                        faceDirection = Direction.Right;
                    }
                    break;
                case Direction.Up:
                    if (movement.Count == 0 && faceDirection != Direction.Down)
                    {
                        movement.Add(transform.position + Vector3.up);
                        faceDirection = Direction.Up;
                    }
                    break;
                case Direction.Down:
                    if (movement.Count == 0 && faceDirection != Direction.Up)
                    {
                        movement.Add(transform.position + Vector3.down);
                        faceDirection = Direction.Down;
                    }
                    break;
            }
        }
    }
}
