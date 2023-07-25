using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform taget;
    private Vector3 offset = new Vector3(0, 0, -10);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;
    void Update()
    {
        Vector3 tagetPosition = taget.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, tagetPosition,ref velocity, smoothTime);
    }
}
//transform.position = new Vector3(
//taget.position.x,
//Mathf.Clamp(taget.position.y, 0f, 1000f),
//transform.position.z);