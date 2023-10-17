using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] Rigidbody myRigid;
    [SerializeField] float moveSpeed = 15f;
    
    [SerializeField] int minSwipeRecognition = 500;
    Vector2 swipePosLastFrame;
    Vector2 swipePosCurrentFrame;
    Vector2 currentSwipe;

    bool isTravelling;
    Vector3 travelDirection;
    Vector3 nextCollisionPosition;

    Color solveColor;

    void Start() 
    {
        solveColor = Random.ColorHSV(.5f, 1);
        GetComponent<MeshRenderer>().material.color = solveColor; 
    }

    void FixedUpdate() 
    {
        if(isTravelling)
        {
            myRigid.velocity = moveSpeed * travelDirection;    
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up /2 ), 0.05f);
        int i = 0;
        while(i < hitColliders.Length)
        {
            GroundPiece ground = hitColliders[i].transform.GetComponent<GroundPiece>();
            if(ground && !ground.isColored)
            {
                ground.ChangeColor(solveColor);
            }
            i++;
        }

        if(nextCollisionPosition != Vector3.zero)
        {
            if(Vector3.Distance(transform.position, nextCollisionPosition) < 1)
            {
                isTravelling = false;
                travelDirection = Vector3.zero;
                nextCollisionPosition = Vector3.zero;
            }
        }

        if(isTravelling)
            return;

        if(Input.GetMouseButton(0))
        {
            swipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            if(swipePosLastFrame != Vector2.zero)
            {
                currentSwipe = swipePosCurrentFrame - swipePosLastFrame;

                if(currentSwipe.sqrMagnitude < minSwipeRecognition)
                {
                    return;
                }

                currentSwipe.Normalize();

                if(currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    //GO UP/DOWN
                    SetDestination(currentSwipe.y > 0 ? Vector3.forward : Vector3.back);
                }

                if(currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    //GO LEFT/RİGHT
                    SetDestination(currentSwipe.x > 0 ? Vector3.right : Vector3.left);
                }

            }

            swipePosLastFrame = swipePosCurrentFrame;
        }

        if(Input.GetMouseButtonUp(0))
        {
            swipePosLastFrame = Vector2.zero;
            currentSwipe = Vector2.zero;
        }

    }

    void SetDestination(Vector3 direction)
    {
        travelDirection = direction;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, direction, out hit, 100f))
        {
            nextCollisionPosition = hit.point;
        }

        isTravelling = true;
    }

}
