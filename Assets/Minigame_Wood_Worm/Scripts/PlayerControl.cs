using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTS.Woodworm
{
    public class PlayerControl : MonoBehaviour
    {
        List<Vector3> movement = new List<Vector3>();

        private void Update()
        {
            InputDirection();
            MoveInGrid();
        }

        void InputDirection()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (movement.Count > 0)
                {
                    movement.Add(movement[movement.Count - 1] + Vector3.right);
                }
                else
                {
                    movement.Add(transform.position + Vector3.right);
                }
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                if (movement.Count > 0)
                {
                    movement.Add(movement[movement.Count - 1] + Vector3.left);
                }
                else
                {
                    movement.Add(transform.position + Vector3.left);
                }
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                if (movement.Count > 0)
                {
                    movement.Add(movement[movement.Count - 1] + Vector3.up);
                }
                else
                {
                    movement.Add(transform.position + Vector3.up);
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (movement.Count > 0)
                {
                    movement.Add(movement[movement.Count - 1] + Vector3.down);
                }
                else
                {
                    movement.Add(transform.position + Vector3.down);
                }
            }

        }
        void MoveInGrid()
        {
            if (movement.Count > 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, movement[0], 5 * Time.deltaTime);
                if(transform.position == movement[0])
                {
                    movement.RemoveAt(0);
                }
            }
        }
    }
}
