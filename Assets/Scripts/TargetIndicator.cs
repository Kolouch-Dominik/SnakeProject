using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    [field: SerializeField] public Food Target;
    [field: SerializeField] public float HideDistance { get; private set; }

    private void Update()
    {
        var dir = Target.transform.position - transform.position;

        if (dir.magnitude < HideDistance)
        {
            SetIndicatorActive(false);
        }
        else
        {
            SetIndicatorActive(true);
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
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

