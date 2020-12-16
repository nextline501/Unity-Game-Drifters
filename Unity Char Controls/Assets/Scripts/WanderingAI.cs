using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class WanderingAI : MonoBehaviour
{
    public float aiSpeed = 3.0f;
    public float obstacleRange = 5.0f;
    private bool isAlive;

    [SerializeField] private GameObject fireballPrefab;
    private GameObject _fireball;

    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            transform.Translate(0, 0, aiSpeed * Time.deltaTime);
            
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.SphereCast(ray, 5f, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                if (hitObject.GetComponent<PlayerCharacter>())
                {
                    if (_fireball == null)
                    {
                        _fireball = Instantiate(fireballPrefab) as GameObject;
                        _fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
                        _fireball.transform.rotation = transform.rotation;
                        
                    }
                }
                else if(hit.distance < obstacleRange)
                {
                    float angle = Random.Range(-110, 110);
                    transform.Rotate(0, angle, 0);
                }
            }
        }
    }

    public void SetAlive(bool alive)
    {
        isAlive = alive;
    }
}
