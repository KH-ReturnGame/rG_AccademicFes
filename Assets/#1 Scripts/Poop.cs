using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poop : MonoBehaviour
{
    BoxCollider2D boxCollider2D;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "ground" || other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
