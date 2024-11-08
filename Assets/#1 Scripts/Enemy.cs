using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float enemyHP;
    public Sprite deadSprite;  // Assign this in the Inspector to the sprite for when the enemy dies
    private SpriteRenderer spriteRenderer;
    public GameObject bulletPrefab;  // Assign this in the Inspector to a Bullet prefab
    public float fireRate = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        enemyHP = 2;
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(FireBullets());
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
            if(enemyHP > 0)
            {
                StartCoroutine(Damage());
            }
        }
        else if(other.tag == "Player")
        {
            Player _player = other.GetComponent<Player>();
            _player.StartCoroutine(Die());
        }
    }

    IEnumerator Damage()
    {
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.25f);
        
        spriteRenderer.color = Color.white;
    }

    IEnumerator Die()
    {
        gameObject.tag = "Dead";

        spriteRenderer.sprite = deadSprite;

        yield return new WaitForSeconds(0.75f);

        Destroy(gameObject);
    }

    IEnumerator FireBullets()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(fireRate);
        StartCoroutine(FireBullets());
    
    }
}