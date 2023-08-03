using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Pipes;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance { get; private set; }
    public enum Directions { Right, Left, Down, Up };
    [field: SerializeField, Header("GameArea")] public GameObject WallPrefab { get; private set; }
    // max 45x28, min 7x7
    [field: SerializeField] public int AreaWidth { get; set; }
    [field: SerializeField] public int AreaHeight { get; set; }

    [field: SerializeField, Header("Food")] public GameObject FoodPrefab { get; private set; }

    [field: SerializeField, Header("Controlls")] public Snake snake;
    private Vector2 startPosition;
    private Vector2 endPosition;
    [field: SerializeField] public int PixelDistToDetect = 20;

    public Bounds GameArea { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        GameArea = new Bounds(new Vector3(0, 0, 0), new Vector3(AreaWidth, AreaHeight));
        Debug.Log(GameArea);
        GenerateWalls();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startPosition = touch.position;
                endPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                endPosition = touch.position;
                Swipe();
            }
        }
    }

    void Swipe()
    {
        float swipeDist = (endPosition - startPosition).magnitude;

        if (swipeDist < PixelDistToDetect)
            return;

        Vector2 swipeDir = (endPosition - startPosition).normalized;
        float xSwipe = swipeDir.x;
        float ySwipe = swipeDir.y;

        if (Mathf.Abs(xSwipe) > Mathf.Abs(ySwipe))
        {
            if (xSwipe > 0)
                snake.Turn(Directions.Right);
            else snake.Turn(Directions.Left);
        }
        else
        {
            if (ySwipe > 0)
                snake.Turn(Directions.Up);
            else snake.Turn(Directions.Down);
        }
    }

    private void GenerateWalls()
    {
        if (AreaWidth > 45) AreaWidth = 45;
        else if (AreaWidth < 7) AreaWidth = 7;
        if (AreaHeight > 28) AreaHeight = 28;
        else if (AreaHeight < 7) AreaHeight = 7;

        var left = -AreaWidth / 2;
        var down = -AreaHeight / 2;

        float offsetX = 0, offsetY = 0;
        if (AreaWidth % 2 == 1) offsetX = .5f;
        if (AreaHeight % 2 == 1) offsetY = .5f;

        GameObject leftWall = Instantiate(WallPrefab, new Vector3(left, offsetY, 0f), Quaternion.identity);
        leftWall.transform.localScale = new Vector3(1f, AreaHeight, 1f);

        GameObject rightWall = Instantiate(WallPrefab, new Vector3(left + AreaWidth, offsetY, 0f), Quaternion.identity);
        rightWall.transform.localScale = new Vector3(1f, AreaHeight, 1f);

        GameObject topWall = Instantiate(WallPrefab, new Vector3(offsetX, down + AreaHeight, 0f), Quaternion.identity);
        topWall.transform.localScale = new Vector3(AreaWidth, 1f, 1f);

        GameObject bottomWall = Instantiate(WallPrefab, new Vector3(offsetX, down, 0f), Quaternion.identity);
        bottomWall.transform.localScale = new Vector3(AreaWidth, 1f, 1f);
    }

    internal void GameOver()
    {
        //TODO:
        Time.timeScale = 0f;
    }
}
