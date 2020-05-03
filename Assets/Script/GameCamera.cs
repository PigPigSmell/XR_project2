using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 40.0f;
    private float pitch = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // yaw += speedH * Input.GetAxis("Mouse X");
        // pitch -= speedV * Input.GetAxis("Mouse Y");
         yaw += speedH * Input.acceleration.x;
         pitch -= speedV * Input.acceleration.y;

         transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}
