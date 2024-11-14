using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTS.Woodworm
{
    public class PlayerControl : MonoBehaviour
    {
        public enum FaceDirection
        {
            Left, Right, Up, Down
        }

        public bool canControl;

        FaceDirection faceDirection = FaceDirection.Right;

        List<Vector3> movement = new List<Vector3>();

        private void Update()
        {
            if(!canControl) return;

            InputDirection();
            MoveInGrid();
        }

        void InputDirection()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (movement.Count == 0 && faceDirection != FaceDirection.Left)
                {
                    movement.Add(transform.position + Vector3.right);
                    faceDirection = FaceDirection.Right;
                }
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                if (movement.Count == 0 && faceDirection != FaceDirection.Right)
                {
                    movement.Add(transform.position + Vector3.left);
                    faceDirection = FaceDirection.Left;
                }
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                if (movement.Count == 0 && faceDirection != FaceDirection.Down)
                {
                    movement.Add(transform.position + Vector3.up);
                    faceDirection = FaceDirection.Up;
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (movement.Count == 0 && faceDirection != FaceDirection.Up)
                {
                    movement.Add(transform.position + Vector3.down);
                    faceDirection = FaceDirection.Down;
                }
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
    }
}
