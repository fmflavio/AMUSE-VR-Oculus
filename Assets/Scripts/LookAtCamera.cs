using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private GameObject camera;
    private Vector3 v;
    public bool FollowCamera = true;
    void Start()
    {
        camera = GameObject.Find("Camera (head)");
    }

    void Update() {
        if (FollowCamera) { // Code for the menu to follow the camera.	
            v = camera.transform.position - transform.position;
            v.x = v.z = 0.0f;
            transform.LookAt(camera.transform.position - v);
            transform.Rotate(0, 180, 0);
        }
    }
}
