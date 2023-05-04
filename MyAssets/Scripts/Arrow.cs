using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This Script implements the arrow logic and how much dmg you deal to the enemy
 */
public class Arrow : MonoBehaviour
{
    public AudioClip sfxHit;
    private Rigidbody arrowRigidbody;
    public float damage = 20;
    // Start is called before the first frame update
    void Start()
    {
        arrowRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!arrowRigidbody.isKinematic && arrowRigidbody.velocity.magnitude > 0)
        {
            transform.forward = arrowRigidbody.velocity.normalized;
        }
    }

    private void OnCollisionEnter(Collision info)
    {
        AudioSource.PlayClipAtPoint(sfxHit, transform.position, 0.9f);
        arrowRigidbody.isKinematic = true;
        GetComponent<Collider>().enabled = false;
        transform.SetParent(info.transform);
        DealDamage(info.gameObject);
    }

    private void DealDamage(GameObject gameObject)
    {
        Target target = gameObject.GetComponent<Target>();

        if(target != null)
        {
            target.Hit(damage);
        }
    }
}
