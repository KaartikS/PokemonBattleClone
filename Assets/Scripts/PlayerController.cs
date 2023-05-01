using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private Vector2 input;
    public bool isMoving;
    private Animator animator;
    public LayerMask solidObjectsLayer;
    public LayerMask longGrassLayer;

    private void Awake()
    {
        // get a reference to the animator when this script is loaded
        animator = GetComponent<Animator>();
    }
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
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                isMoving = true;
                // new target position is the transform's position plus the inputs in each directionm
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                // call coroutine if isWalkable is true
                if (IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
            }
        }

        animator.SetBool("isMoving", isMoving);
    }

    // Coroutine to make the player move towards the target slowly over each update cycle
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


        CheckForEncounters();
    }

    private void CheckForEncounters()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, longGrassLayer) != null)
        {
            if(UnityEngine.Random.Range(1, 101) <= 10)
            {
                Debug.Log("You encoutered an enemy!!");
            }
        }
    }

    // Returns a bool to see if the target position is not overlapping with the solidObjectsLayer. 
    private bool IsWalkable (Vector3 targetPos)
    {
        if(Physics2D.OverlapCircle(targetPos, 0.1f, solidObjectsLayer) != null)
        {
            isMoving = false;
            return false;
        }
        return true;
    }
}

