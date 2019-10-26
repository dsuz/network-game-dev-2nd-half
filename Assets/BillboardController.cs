using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardController : MonoBehaviour
{
    void Start()
    {
        Vector3 scale = transform.localScale;
        scale.x = scale.x * (-1);
        transform.localScale = scale;
    }

    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }
}
