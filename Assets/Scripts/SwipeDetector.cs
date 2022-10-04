using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetector : Singleton<SwipeDetector>
{
    struct SwipeInfo
    {
        public Vector2 startPosition;
        public double startTime;
        public Vector2 endPosition;
        public double endTime;

        private Vector2 Direction { get => endPosition - startPosition; }
        public Vector2 Direction2D { get => new Vector2(Direction.x, Direction.y).normalized; }
    }

    public delegate void Swipe(Vector2 startPosition, Vector2 endPosition);
    public event Swipe OnSwipeDetected;

    [SerializeField]
    private float minSwipeDistance = .2f;
    [SerializeField]
    private float maxSwipeTime = 1f;
    [SerializeField, Range(0f, 1f)]
    private float directionThreshold = .7f;
    [SerializeField]
    private GameObject trail;

    private SwipeInfo swipeInfo;
    private InputManager inputManager;

    private void Awake()
    {
        inputManager = InputManager.Instance;
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
        inputManager.OnChangePosition += ChangePosition;
    }

    private void OnDisable()
    {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
        inputManager.OnChangePosition -= ChangePosition;

    }

    private void SwipeStart(Vector2 position, double time)
    {

        Debug.Log($"SwipeStart position : {Utils.ScreenToWorld(position)}");
        swipeInfo.startPosition = position;
        swipeInfo.startTime = time;

        trail.SetActive(false);
        trail.SetActive(true);
        trail.transform.position = position;
    }
    private void SwipeEnd(Vector2 position, double time)
    {
        trail.transform.position = position;
        //trail.SetActive(false);

        swipeInfo.endPosition = position;
        swipeInfo.endTime = time;

        DetectSwipe();
    }

    private void ChangePosition(Vector2 position, double time)
    {
        trail.transform.position = position;
    }

    private void DetectSwipe()
    {
        if (Vector3.Distance(swipeInfo.startPosition, swipeInfo.endPosition) > minSwipeDistance &&
            (swipeInfo.endTime - swipeInfo.startTime) < maxSwipeTime)
        {
            OnSwipeDetected?.Invoke(swipeInfo.startPosition, swipeInfo.endPosition);

            Debug.DrawLine(swipeInfo.startPosition, swipeInfo.endPosition, Color.red, 5f);
            PrintSwipeDirection(SwipeDirection());
        }
    }

    private Vector2 SwipeDirection()
    {
        if (Vector2.Dot(Vector2.up, swipeInfo.Direction2D) > directionThreshold)
            return Vector2.up;
        else if (Vector2.Dot(Vector2.down, swipeInfo.Direction2D) > directionThreshold)
            return Vector2.down;
        else if (Vector2.Dot(Vector2.left, swipeInfo.Direction2D) > directionThreshold)
            return Vector2.left;
        else if (Vector2.Dot(Vector2.right, swipeInfo.Direction2D) > directionThreshold)
            return Vector2.right;
        else
            return swipeInfo.Direction2D;
    }

    private void PrintSwipeDirection(Vector2 direction)
    {
        if (direction == Vector2.up)
             Debug.Log("Swipe UP");
        else if (direction == Vector2.down)
            Debug.Log("Swipe DOWN");
        else if (direction == Vector2.left)
            Debug.Log("Swipe LEFT");
        else if (direction == Vector2.right)
            Debug.Log("Swipe RIGHT");
        else
            Debug.Log($"Swipe {direction}");
    }


}
