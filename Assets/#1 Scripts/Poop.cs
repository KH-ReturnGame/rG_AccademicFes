using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poop : MonoBehaviour
{
    BoxCollider2D boxCollider2D;
    AudioSource myaudio;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        myaudio = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "ground" || other.gameObject.tag == "Player")
        {
            myaudio.Play();
            Destroy(gameObject, myaudio.clip.length);
        }
    }
}
