using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePoop : MonoBehaviour
{
    public GameObject Poop;
    public float spawnInterval = 1.0f; // 장애물 생성 간격
    public float spawnRangeX = 8.0f; // x축 범위 설정
    private float timer = 0.0f; // 시간 누적을 위한 변수

    void Update()
    {
        // 시간이 일정 간격만큼 지나면 장애물 생성
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnObstacle();
            timer = 0.0f; // 타이머 초기화
        }
    }

    void SpawnObstacle()
    {
        // 장애물이 생성될 랜덤 x 위치 설정
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPosition = new Vector3(randomX, transform.position.y, 0);

        float randomRotationZ = Random.Range(0f, 360f); // Z축을 기준으로 회전
        Quaternion randomRotation = Quaternion.Euler(0, 0, randomRotationZ);
        
        // 장애물 생성
        Instantiate(Poop, spawnPosition, randomRotation);
    }
}
