using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayShooter : MonoBehaviour

{
    private Camera _camera;
    private bool _currentlyAiming = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _currentlyAiming = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            _currentlyAiming = false;
        }

        if (_currentlyAiming)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 point = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
                Ray ray = _camera.ScreenPointToRay(point);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit)) //return a bool if we hit something
                {
                    Debug.Log("1: " + ray);

                    GameObject hitObject = hit.transform.gameObject; //check what object we hit at the hit cords...
                    ReactiveTarget target = hitObject.GetComponent<ReactiveTarget>(); //check for the reactive target script on the hit object... if we cant find a script target = null;
                    
                    Debug.Log(target);

                    if(target != null)
                    {
                        target.ReactToHit();
                    } else
                    {
                        StartCoroutine(SphereIndicator(hit.point));
                    }
                }
            }
        }
    }

    private IEnumerator SphereIndicator(Vector3 pos)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = pos;

        yield return new WaitForSeconds(1);
        Destroy(sphere);
    }
}
