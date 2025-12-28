using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rank_Manager : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}


