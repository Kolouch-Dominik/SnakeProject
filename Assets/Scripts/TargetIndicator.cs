using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    [field: SerializeField] private Food target;
    [field: SerializeField] private float hideDistance;

    private void Update()
    {
        Vector3 dir = target.transform.position - transform.position;

        if (dir.magnitude < hideDistance)
        {
            SetIndicatorActive(false);
        }
        else
        {
            SetIndicatorActive(true);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    private void SetIndicatorActive(bool value)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(value);
        }
    }
}

