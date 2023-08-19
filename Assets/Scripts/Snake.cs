using System;
using System.Collections.Generic;
using UnityEngine;
using static GameManagerScript;

public class Snake : MonoBehaviour
{
    [field: SerializeField] private Transform bodyPart;
    private Vector2 Direction { get; set; }
    private float timeToMove = 1f;
    private float elapsedTime;
    private List<Transform> snakeParts;
    private Vector3 newPartSpawnPosition;

    private void Awake()
    {
        float gameSpeedSecValue = GameData.Instance.GameSpeed / 10;
        timeToMove = 1.1f - gameSpeedSecValue;
    }

    private void Start()
    {
        Direction = Vector2.right;
        elapsedTime = timeToMove;
        snakeParts = new List<Transform>
        {
            transform
        };
    }

    void Update()
    {
        elapsedTime -= Time.deltaTime;
        if (elapsedTime < 0)
        {
            elapsedTime = timeToMove;

            newPartSpawnPosition = snakeParts[snakeParts.Count - 1].position;

            for (int i = snakeParts.Count - 1; i > 0; i--)
            {
                snakeParts[i].position = snakeParts[i - 1].position;
            }

            transform.position = new Vector3(
                MathF.Round(transform.position.x + Direction.x),
                MathF.Round(transform.position.y + Direction.y),
                0
            );
        }
    }
    public void Turn(Directions direction)
    {
        Direction = direction switch
        {
            Directions.Left when Direction != Vector2.right => Vector2.left,
            Directions.Right when Direction != Vector2.left => Vector2.right,
            Directions.Up when Direction != Vector2.down => Vector2.up,
            Directions.Down when Direction != Vector2.up => Vector2.down,
            _ => throw new NotImplementedException(),
        };
    }

    private void AddBodyPart()
    {
        Transform newBodyPart = Instantiate(bodyPart);
        newBodyPart.position = newPartSpawnPosition;

        snakeParts.Add(newBodyPart);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            AddBodyPart();
        }
        else if (other.CompareTag("Wall") || other.CompareTag("SnakeBody"))
        {
            GameManagerScript.Instance.GameOver();
        }

    }


}
