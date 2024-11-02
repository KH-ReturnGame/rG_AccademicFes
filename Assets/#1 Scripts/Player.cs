using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float moveSpeed; // 이동 속도
    public int PlayerHP;
    public GameObject _EndUI;

    void Start()
    {
        moveSpeed = 7.5f;
        PlayerHP = 3;
    }

    void Update()
    {
        // 좌우 방향 입력 받기
        float horizontalInput = Input.GetAxis("Horizontal"); // -1에서 1까지의 값 반환
        Vector3 movement = new Vector3(horizontalInput, 0, 0) * moveSpeed * Time.deltaTime;
        
        // 이동 적용
        transform.Translate(movement);

        // 화면 밖으로 나가지 않도록 제한
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -8.0f, 8.0f); // x 좌표 제한 (화면 경계 설정)
        transform.position = clampedPosition;

        if(PlayerHP == 0)
        {
            Time.timeScale = 0;
            _EndUI.SetActive(true);
            StartCoroutine(Restart());
            PlayerHP = 1;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Poop")
        {
            PlayerHP -= 1;
        }
    }

    IEnumerator Restart()
    {
        Debug.Log("??");

        Time.timeScale = 1;
        yield return new WaitForSeconds(3f);

        Debug.Log("??");
        
        Time.timeScale = 0;
        SceneManager.LoadScene("Main", LoadSceneMode.Single);

        yield return null;
    }
}