using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float _fireballSpeed = 10.0f;
    public int _fireballDamage = 1;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0, _fireballSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if(player != null)
        {
            player.Hurt(_fireballDamage);
        }

        Destroy(this.gameObject);
    }
}
