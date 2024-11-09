using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public Text timerText; // UI 텍스트 참조
    public float elapsedTime = 0.0f; // 경과 시간

    void Start()
    {
        // 타이머 초기화
        elapsedTime = 0.0f;
    }

    void Update()
    {
        // 경과 시간 증가
        elapsedTime += Time.deltaTime;
        
        // 시간을 "00:00" 형식으로 업데이트
        int minutes = Mathf.FloorToInt(elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(elapsedTime % 60F);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}