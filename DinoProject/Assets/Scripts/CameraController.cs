using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform followTo;
    private const float followSpeed = 0.01f;

    void Update()
    {
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, followTo.position.x, followSpeed), Mathf.Lerp(transform.position.y, followTo.position.y, followSpeed), transform.position.z);
    }
}
