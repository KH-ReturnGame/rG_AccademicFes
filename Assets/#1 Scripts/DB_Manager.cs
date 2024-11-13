using System.Collections;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
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
    const string URL = "https://script.google.com/macros/s/AKfycbzTfYNEbhWZZfBAGuAYJXdoUUirLw5_XlL3DqCVkKpjP9ETLxLvw3cdS7NyXZD4N75m/exec";
    public GoogleData GD;
    string id, pass;
    public TMP_InputField idpass;
    public Button btn;

    public void SetRankBtn()
    {
        string id_pass = idpass.text.Trim();
        string[] s = id_pass.Split('_');
        btn.interactable = false;
        idpass.interactable = false;
        if (s.Length == 2)
        {
            StartCoroutine(SetRank(1,GetComponent<Player>().last_time.ToString(CultureInfo.InvariantCulture), s[0], s[1]));
        }
        else
        {
            idpass.text = "학번_이름 을 잘 입력하세요";
        }
        
    }
    public IEnumerator SetRank(int game, string data, string id, string pass)
    {
        // Register
        yield return StartCoroutine(Register(id, pass));

        // Login
        yield return StartCoroutine(Login(id, pass));

        Debug.Log(GD.msg);
        if (GD.msg == "로그인 실패")
        {
            idpass.text = "로그인 실패";
            btn.interactable = true;
            idpass.interactable = true;
            yield break;
        }

        // SetValue
        yield return StartCoroutine(GetValue(game));
        Debug.Log(GD.value + " / " + float.Parse(data));
        if ((GD.value == "" ? 0f : float.Parse(GD.value)) < float.Parse(data))
        {
            string value = game + "_" + data;
            yield return StartCoroutine(SetValue(value));
        }
        yield return StartCoroutine(GetComponent<Player>().Restart());
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
                yield return StartCoroutine(Response(www.downloadHandler.text));
            }
        }
    }
    IEnumerator Response(string json)
    {
        GD = JsonUtility.FromJson<GoogleData>(json);

        if (GD.result == "ERROR")
        {
            Debug.Log(GD.order + "을 실행할 수 없습니다. 에러 메시지 : " + GD.msg);
        }
        else
        {
            Debug.Log(GD.order + "을 실행했습니다. 메시지 : " + GD.msg);

            if (GD.order == "getValue")
            {
                Debug.Log("");
            }
        }
        yield return new WaitForSeconds(0f);
    }
}