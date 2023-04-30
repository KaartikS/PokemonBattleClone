using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private Vector2 input;
    private bool isMoving;

    private void Update()
    {
        // if not moving, check inputs
        if (!isMoving)
        {
            // Get movement input using GetAxisRaw (1 or -1)
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // prevent diagonal movement
            if (input.x != 0) input.y = 0;

            // if the input is not zero
            if(input != Vector2.zero)
            {
                isMoving = true;
                // new target position is the transform's position plus the inputs in each directionm
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                // call coroutine
                StartCoroutine(Move(targetPos));
            }
        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        while((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // once we reach this point in the co-routine, we set the transform's position to be equal to the target position
        // the player is also not moving at this point
        transform.position = targetPos;
        isMoving = false;
    }
}

