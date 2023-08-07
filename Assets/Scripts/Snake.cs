using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameManagerScript;

public class Snake : MonoBehaviour
{
    private Vector2 Direction { get; set; }
    private float timeToMove = 1f;
    private float elapsedTime;
    private List<Transform> snakeParts;
    private Vector3 newPartSpawnPosition;

    [field: SerializeField] public Transform BodyPart { get; set; }

    private void Awake()
    {
        timeToMove = 1.1f - (GameData.Instance.GameSpeed / 10);
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
                snakeParts[i].position = snakeParts[i - 1].position;

            transform.position = new Vector3(
                MathF.Round(transform.position.x + Direction.x),
                MathF.Round(transform.position.y + Direction.y),
                0
            );
        }
    }

    private void AddBodyPart()
    {
        Transform bodyPart = Instantiate(BodyPart);
        bodyPart.position = newPartSpawnPosition;

        snakeParts.Add(bodyPart);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food")) AddBodyPart();
        else if (other.CompareTag("Wall") || other.CompareTag("SnakeBody"))
            GameManagerScript.Instance.GameOver();

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
}
