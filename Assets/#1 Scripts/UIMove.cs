using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMove : MonoBehaviour
{
    public GameObject playerObj;
    public Player player;
    float horizontalInput;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(horizontalInput, 0, 0) * 7.5f * Time.deltaTime;
        
        // 이동 적용
        playerObj.transform.Translate(movement);

        // 화면 밖으로 나가지 않도록 제한
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -8.0f, 8.0f); // x 좌표 제한 (화면 경계 설정)
        transform.position = clampedPosition;
    }

    public void UIMovingLeft()
    {
        horizontalInput = -1;
    }

    public void UIMovingRight()
    {
        horizontalInput = 1;
    }
}
