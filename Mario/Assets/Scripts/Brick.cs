using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    private ParticleSystem particle;
    private SpriteRenderer sr;
    private BoxCollider2D bc;

    private AudioSource audioSource;
    private void Awake()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();

        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.GetComponent<PlayerContoller>() &&
            collision.contacts[0].normal.y > 0.5f)
        {
            
            StartCoroutine(Break());
        }
    }

    public IEnumerator Break()
    {
        particle.Play();
        audioSource.Play();

        sr.enabled = false;
        bc.enabled = false;
        
        yield return new WaitForSeconds(particle.main.startLifetime.constantMax);
        Destroy(gameObject);
    }
}
