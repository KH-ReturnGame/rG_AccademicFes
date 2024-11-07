using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public int Firedirection = 1;  // 발사 방향 (1: 위쪽, -1: 아래쪽)
    public float bulletSpeed = 10f; // 총알 속도

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 발사 방향에 따른 속도 설정
        rb.velocity = new Vector2(0, bulletSpeed * Firedirection);
    }

    void Update()
    {
        // 화면을 벗어나면 총알을 삭제
        if (transform.position.y > 10f || transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
}