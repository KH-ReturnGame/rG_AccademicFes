using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float moveSpeed; // 이동 속도
    public int PlayerHP;
    public GameObject _EndUI;
    public Text timerTxt;
    AudioSource myaudio;
    public AudioClip Dieaudio;
    GameObject poopManager;
    public GameObject[] Hearts;
    public float last_time;

    void Start()
    {
        moveSpeed = 7.5f;
        PlayerHP = 5;
        myaudio = GetComponent<AudioSource>();

        poopManager = GameObject.Find("PoopManager");
    }

    void Update()
    {
        Move();

        if(PlayerHP == 0)
        {
            _EndUI.SetActive(true);
            CreatePoop _createPoop = poopManager.GetComponent<CreatePoop>();
            _createPoop.StopPoop();
            PlayerHP = 10000;
            GameObject game_manager = GameObject.FindGameObjectWithTag("manager");
            last_time = game_manager.GetComponent<GameTimer>().elapsedTime;
            // 시간을 "00:00" 형식으로 업데이트
            int minutes = Mathf.FloorToInt(last_time / 60F);
            int seconds = Mathf.FloorToInt(last_time % 60F);
            timerTxt.text = string.Format("{0:00}분 {1:00}초 생존!!", minutes, seconds);
        }
    }

    void Move()
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
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Poop")
        {
            Debug.Log(PlayerHP);
            Destroy(Hearts[PlayerHP - 1]);
            PlayerHP -= 1;
            myaudio.Play();
        }
    }

    public IEnumerator Restart()
    {
        AudioSource poopAudioSource = poopManager.GetComponent<AudioSource>();
        poopAudioSource.clip = Dieaudio;
        poopAudioSource.Play();
        
        yield return new WaitForSeconds(12.25f);

        Debug.Log("??");
        
        SceneManager.LoadScene("Main", LoadSceneMode.Single);

        yield return null;
    }
}