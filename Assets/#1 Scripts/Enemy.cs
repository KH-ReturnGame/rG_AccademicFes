using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float enemyHP;
    public Sprite deadSprite;  // Assign this in the Inspector to the sprite for when the enemy dies
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        enemyHP = 2;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyHP <= 0)
        {
            StartCoroutine(Die());
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "bullet")
        {
            enemyHP -= 1;
        }
        else if(other.tag == "Player")
        {
            
        }
    }

    IEnumerator Die()
    {
        gameObject.tag = "Dead";

        spriteRenderer.sprite = deadSprite;

        yield return new WaitForSeconds(0.75f);

        Destroy(gameObject);
    }
}
