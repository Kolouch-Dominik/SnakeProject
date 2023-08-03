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
        Debug.Log(Bound);
        int randX = (int)Random.Range((Bound.min.x) + 1, Bound.max.x);
        int randY = (int)Random.Range((Bound.min.y) + 1, Bound.max.y);
        transform.position = new Vector3(randX, randY, 0);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) RandomizePos();
    }
}
