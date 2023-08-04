using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RandomizePos();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void RandomizePos()
    {
        var Bound = GameManagerScript.Instance.GameArea;
        int randX, randY;
        do
        {
            randX = (int)Random.Range((Bound.min.x) + 1, Bound.max.x);
            randY = (int)Random.Range((Bound.min.y) + 1, Bound.max.y);
        } while (Physics2D.OverlapCircleAll(new Vector3(randX, randY, 0), 0.5f).Length != 0);
        transform.position = new Vector3(randX, randY, 0);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) RandomizePos();
    }
}
