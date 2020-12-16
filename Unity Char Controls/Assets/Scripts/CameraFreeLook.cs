using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFreeLook : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 0.4f;

    [SerializeField] private float x1;
    [SerializeField] private float x2;


    // Update is called once per frame

    void Update()
    {
        //Input.GetMouseButton(0) && Input.GetMouseButton(1) BOTH MOUSE BUTTONs

        if (Input.GetMouseButton(2))
        {
            x1 = Input.mousePosition.x;
        }

        if (Input.GetMouseButton(2))
        {
            x2 = Input.mousePosition.y;

            if (x1 > x2)
            {
                transform.Rotate(0, (Input.mousePosition.y * Time.deltaTime * _rotationSpeed), 0);
            }

            if (x1 < x2)
            {
                transform.Rotate(0, -(Input.mousePosition.y * Time.deltaTime * _rotationSpeed), 0);
            }
        }

        if(Input.GetMouseButtonDown(3))
        {
            transform.localEulerAngles = new Vector3(0, 0, 0) * Time.deltaTime;
        }

        if (false)
        {
            //ADD CAMERA SCROLL
        }
    }
}
