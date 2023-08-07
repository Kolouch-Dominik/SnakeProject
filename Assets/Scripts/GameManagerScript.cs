using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance { get; private set; }
    public enum Directions { Right, Left, Down, Up };
    [field: SerializeField, Header("GameArea")] public GameObject WallPrefab { get; private set; }
    [field: SerializeField] public int AreaWidth { get; set; }
    [field: SerializeField] public int AreaHeight { get; set; }

    [field: SerializeField, Header("Food")] public GameObject FoodPrefab { get; private set; }

    [field: SerializeField, Header("Controlls")] public Snake snake;
    private Vector2 startPosition;
    private Vector2 endPosition;
    private bool isGameOver = false;
    [field: SerializeField] public int PixelDistToDetect = 20;

    [field: SerializeField, Header("UI")] public GameObject GameOverPanel { get; set; }

    public Bounds GameArea { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        isGameOver = false;
        Restart();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (isGameOver)
            {
                SceneManager.LoadScene("MenuScene");
            }

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
        var left = -AreaWidth / 2;
        var down = -AreaHeight / 2;

        float offsetX = 0, offsetY = 0;
        if (AreaWidth % 2 == 1) offsetX = .5f;
        if (AreaHeight % 2 == 1) offsetY = .5f;

        GameObject leftWall = Instantiate(WallPrefab, new Vector3(left, offsetY, 0f), Quaternion.identity);
        leftWall.GetComponent<BoxCollider2D>().size = new Vector2(.9f, 1);
        leftWall.transform.localScale = new Vector3(1f, AreaHeight, 1f);

        GameObject rightWall = Instantiate(WallPrefab, new Vector3(left + AreaWidth, offsetY, 0f), Quaternion.identity);
        rightWall.GetComponent<BoxCollider2D>().size = new Vector2(.9f, 1);
        rightWall.transform.localScale = new Vector3(1f, AreaHeight, 1f);

        GameObject topWall = Instantiate(WallPrefab, new Vector3(offsetX, down + AreaHeight, 0f), Quaternion.identity);
        topWall.GetComponent<BoxCollider2D>().size = new Vector2(1, .9f);
        topWall.transform.localScale = new Vector3(AreaWidth, 1f, 1f);

        GameObject bottomWall = Instantiate(WallPrefab, new Vector3(offsetX, down, 0f), Quaternion.identity);
        bottomWall.GetComponent<BoxCollider2D>().size = new Vector2(1, .9f);
        bottomWall.transform.localScale = new Vector3(AreaWidth, 1f, 1f);
    }

    private void GenerateObstacles()
    {

        int count = (AreaWidth - 2) * (AreaHeight - 2) / 20;
        int randX, randY;
        Vector3 randPosition;

        for (int i = 0; i < count; i++)
        {
            do
            {
                randX = (int)Random.Range(GameArea.min.x + 2f, GameArea.max.x - 1f);
                randY = (int)Random.Range(GameArea.min.y + 2f, GameArea.max.y - 1f);

                randPosition = new(randX, randY, 0);
            } while (Physics2D.OverlapCircleAll(randPosition, 1f).Length != 0);
            Instantiate(WallPrefab, new Vector3(randX, randY, 0), Quaternion.identity);
        }
    }

    public void GameOver()
    {
        GameOverPanel.SetActive(true);
        isGameOver= true;
    }

    private void Restart()
    {
        if (GameData.Instance != null)
        {
            AreaWidth = GameData.Instance.AreaWidth;
            AreaHeight = GameData.Instance.AreaHeight;
        }
        GameArea = new Bounds(new Vector3(0, 0, 0), new Vector3(AreaWidth, AreaHeight));
        GenerateWalls();
        GenerateObstacles();
        Destroy(GameObject.Find("GameData"));
    }
    
}
