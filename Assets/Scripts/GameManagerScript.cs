using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManagerScript : MonoBehaviour
{
    [field: SerializeField] public int AreaWidth { get; set; }
    [field: SerializeField] public int AreaHeight { get; set; }
    [field: SerializeField] private GameObject WallPrefab;
    [field: SerializeField] private int pixelDistToDetect = 20;
    [field: SerializeField] private GameObject gameOverPanel;
    [field: SerializeField] private GameObject foodPrefab;
    [field: SerializeField] private Snake snake;
    public static GameManagerScript Instance { get; private set; }
    public enum Directions { Right, Left, Down, Up };
    private Vector2 startPosition;
    private Vector2 endPosition;
    private bool isGameOver = false;



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

        if (swipeDist < pixelDistToDetect)
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
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        isGameOver = true;
        Destroy(GameObject.Find("GameData"));
    }
    private void GenerateWalls()
    {
        int left = -AreaWidth / 2;
        int down = -AreaHeight / 2;

        float offsetX = 0, offsetY = 0;
        if (AreaWidth % 2 == 1) offsetX = .5f;
        if (AreaHeight % 2 == 1) offsetY = .5f;


        CreateWall(new Vector3(left, offsetY, 0f), new Vector3(1f, AreaHeight, 1f), new Vector2(.9f, 1));
        CreateWall(new Vector3(left + AreaWidth, offsetY, 0f), new Vector3(1f, AreaHeight, 1f), new Vector2(.9f, 1));
        CreateWall(new Vector3(offsetX, down + AreaHeight, 0f), new Vector3(AreaWidth, 1f, 1f), new Vector2(1, .9f));
        CreateWall(new Vector3(offsetX, down, 0f), new Vector3(AreaWidth, 1f, 1f), new Vector2(1, .9f));
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
            CreateWall(randPosition, Vector3.one, new Vector2(0.9f, 0.9f));
        }
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
        if (GameData.Instance.GenerateObstacles)
        {
            GenerateObstacles();
        }
    }

    private void CreateWall(Vector3 position, Vector3 scale, Vector2 colliderSize)
    {
        GameObject Wall = Instantiate(WallPrefab, position, Quaternion.identity);
        Wall.GetComponent<BoxCollider2D>().size = colliderSize;
        Wall.transform.localScale = scale;
    }

}
