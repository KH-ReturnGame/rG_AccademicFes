using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;         // 플레이어 이동 속도
    public float boundaryX = 8.5f;    // 화면 경계
    public float boundaryY = 4.5f;      // Y축 경계값
    private Rigidbody2D rb;
    private int Iscool = 0;
    public GameObject Bullet_obj;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 사용자 입력을 받아 X축과 Y축으로 이동할 방향을 결정합니다.
        float moveInputX = Input.GetAxis("Horizontal"); // -1(왼쪽) ~ 1(오른쪽) 사이의 값 반환
        float moveInputY = Input.GetAxis("Vertical");   // -1(아래) ~ 1(위) 사이의 값 반환
        Vector2 movement = new Vector2(moveInputX * speed, moveInputY * speed);

        // Rigidbody2D의 속도를 설정하여 이동
        rb.velocity = movement;

        // 화면 경계 체크
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -boundaryX, boundaryX); // X축 경계 제한
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -boundaryY, boundaryY); // Y축 경계 제한
        transform.position = clampedPosition;
        
        if(Input.GetKey(KeyCode.Space) && Iscool == 0)
        {
            StartCoroutine(FireBullet());
        }
    }

    IEnumerator FireBullet()
    {
        Iscool = 1;
        Instantiate(Bullet_obj, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(0.25f);
        
        Iscool = 0;
        
        yield return null;
    }

    IEnumerator Die()
    {
        yield return null;
    }
}
