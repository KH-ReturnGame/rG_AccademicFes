using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float enemyHP;
    public Sprite deadSprite;
    private SpriteRenderer spriteRenderer;
    public GameObject bulletPrefab;
    public GameObject bulletPrefabBig;
    public float fireRate = 1.5f;
    // Start is called before the first frame update
    public int type; 
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
            _player.Die();
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
        if(type == 1)
        {
            Vector3 spawnPosition = transform.position + new Vector3(0, -0.5f, 0);

            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(fireRate);
            StartCoroutine(FireBullets());
        }
        else if(type == 2)
        {
            Vector3 spawnPositionL = transform.position + new Vector3(0.5f, -0.5f, 0);
            Vector3 spawnPositionR = transform.position + new Vector3(-0.5f, -0.5f, 0);
    
            GameObject bulletL = Instantiate(bulletPrefab, spawnPositionL, Quaternion.identity);
            GameObject bulletR = Instantiate(bulletPrefab, spawnPositionR, Quaternion.identity);
    
            yield return new WaitForSeconds(fireRate);
            StartCoroutine(FireBullets());
        }
        else if(type == 3)
        {
            Vector3 spawnPosition = transform.position + new Vector3(0, -0.75f, 0);
    
            GameObject bullet = Instantiate(bulletPrefabBig, spawnPosition, Quaternion.identity);
    
            yield return new WaitForSeconds(fireRate);
            StartCoroutine(FireBullets());
        }
    }
}