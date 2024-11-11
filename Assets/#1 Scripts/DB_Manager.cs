using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[System.Serializable]
public class GoogleData
{
    public string order, result, msg, value;
}

public class DB_Manager : MonoBehaviour
{
    const string URL = "https://script.google.com/macros/s/AKfycbxO-1D34Uh8E0ZnuBpTSpEU3JJ8pCQQuYBxjSwBmMwxrZGFTmvQFu0JxQD23B7EUENJ/exec";
    public GoogleData GD;
    string id, pass;

    private void Start()
    {
        StartCoroutine(ProcessRanks());
    }

    IEnumerator ProcessRanks()
    {
        yield return StartCoroutine(SetRank(3, "70", "20718", "이건희"));
        yield return StartCoroutine(SetRank(3, "80", "20715", "심상균"));
        yield return StartCoroutine(SetRank(3, "90", "20706", "나현서"));
    }

    IEnumerator SetRank(int game, string data, string id, string pass)
    {
        // Register
        yield return StartCoroutine(Register(id, pass));

        // Login
        yield return StartCoroutine(Login(id, pass));

        // SetValue
        string value = game + "_" + data;
        yield return StartCoroutine(SetValue(value));
    }

    IEnumerator Register(string id, string pass)
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "register");
        form.AddField("id", id);
        form.AddField("pass", pass);
        yield return StartCoroutine(Post(form));
    }

    IEnumerator Login(string id, string pass)
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "login");
        form.AddField("id", id);
        form.AddField("pass", pass);
        yield return StartCoroutine(Post(form));
    }

    IEnumerator SetValue(string value)
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "setValue");
        form.AddField("value", value);
        yield return StartCoroutine(Post(form));
    }

    IEnumerator GetValue(int gameNumber)
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "getValue");
        form.AddField("value", gameNumber.ToString());
        yield return StartCoroutine(Post(form));
    }

    IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                Response(www.downloadHandler.text);
            }
        }
    }
    void Response(string json)
    {
        if (string.IsNullOrEmpty(json)) return;
        GD = JsonUtility.FromJson<GoogleData>(json);

        if (GD.result == "ERROR")
        {
            Debug.Log(GD.order + "을 실행할 수 없습니다. 에러 메시지 : " + GD.msg);
            return;
        }

        Debug.Log(GD.order + "을 실행했습니다. 메시지 : " + GD.msg);

        if (GD.order == "getValue")
        {
            
        }
    }
}