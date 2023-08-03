using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameManagerScript;

public class Snake : MonoBehaviour
{
    private Vector2 Direction { get; set; }
    private float timeToMove = .2f;
    private float elapsedTime;
    private List<Transform> snakeParts;

    [field: SerializeField] public Transform BodyPart { get; set; }

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

            for (int i = snakeParts.Count - 1; i > 0; i--)
                snakeParts[i].position = snakeParts[i - 1].position;

            transform.position = new Vector3(
                transform.position.x + Direction.x,
                transform.position.y + Direction.y,
                0
            );
        }
    }

    private void AddBodyPart()
    {
        Transform bodyPart = Instantiate(BodyPart);
        bodyPart.position = snakeParts[snakeParts.Count - 1].position;

        snakeParts.Add(bodyPart);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
            AddBodyPart();
        else if (other.CompareTag("Wall") || other.CompareTag("SnakeBody"))
        {
            GameManagerScript.Instance.GameOver();
        }
    }

    public void Turn(Directions direction)
    {
        Direction = direction switch
        {
            Directions.Left => Direction = Vector2.left,
            Directions.Right => Direction = Vector2.right,
            Directions.Up => Direction = Vector2.up,
            Directions.Down => Direction = Vector2.down,
            _ => throw new NotImplementedException(),
        };
    }
}