using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    private InputManager inputManager;
    private SwipeDetector swipeDetector;
    private RaycastHit2D hit;

    private void Awake()
    {
        inputManager = InputManager.Instance;
        swipeDetector = SwipeDetector.Instance;
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
        //inputManager.OnChangePosition += ChangePosition;
        swipeDetector.OnSwipeDetected += SwipeDetected;
    }

    private void OnDisable()
    {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
        //inputManager.OnChangePosition -= ChangePosition;
        swipeDetector.OnSwipeDetected -= SwipeDetected;
    }

    private void SwipeStart(Vector2 position, double time)
    {
        //Debug.Log($"SwipeStart position : {Utils.ScreenToWorld(position)}");
        //swipeInfo.startPosition = position;
        //swipeInfo.startTime = time;

        Ray ray = new Ray(position, Vector3.forward);
        Debug.DrawRay(ray.origin, ray.direction, Color.green, 10f);

        hit = Physics2D.Raycast(ray.origin, ray.direction, 1f);
        if (hit.collider != null)
        {
            Debug.Log($"SwipeStart hit.collider.name : {hit.collider.name}");
            hit.collider.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
        }

        if (hit.rigidbody != null)
        {
            hit.rigidbody.velocity = Vector2.zero;
        }

            //Debug.DrawRay(position, new Vector3(position.x, position.y, -10), Color.green, 10f);
        }
    private void SwipeEnd(Vector2 position, double time)
    {
        if (hit.collider != null)
        {
            hit.collider.transform.localScale += new Vector3(-0.1f, -0.1f, -0.1f);
        }
    }

    private void SwipeDetected(Vector2 startPosition, Vector2 endPosition)
    {
        //hit.collider.transform.position = endPosition;
        if (hit.rigidbody != null)
        {
            Debug.Log($"SwipeDetected hit.rigidbody.name : {hit.rigidbody.name}");
            //hit.rigidbody.drag = 10;
            //hit.rigidbody.AddForce(Vector2.down * 10);
            hit.rigidbody.velocity = endPosition - startPosition;
        }
    }
}
