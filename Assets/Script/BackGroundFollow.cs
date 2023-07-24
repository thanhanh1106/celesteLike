using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundFollow : MonoBehaviour
{
    Transform camera;
    public Vector3 PositionOffset;
    Vector3 velocity;
    private void Start()
    {
        camera = Camera.main.transform;
    }
    private void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, camera.position + PositionOffset, ref velocity, 0.00001f);
    }
}
