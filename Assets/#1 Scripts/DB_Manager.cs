using System.Collections;
using System.Globalization;
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
    const string URL = "https://script.google.com/macros/s/AKfycbxO-1D34Uh8E0ZnuBpTSpEU3JJ8pCQQuYBxjSwBmMwxrZGFTmvQFu0JxQD23B7EUENJ/exec";
    public GoogleData GD;
    string id, pass;
    public InputField idpass;

    public void SetRankBtn()
    {
        string id_pass = idpass.text.Trim();
        string[] s = id_pass.Split('_');
        Debug.Log(GetComponent<Player>().last_time.ToString(CultureInfo.InvariantCulture));
        if (s.Length == 2)
        {
            string ss = GetComponent<Player>().last_time.ToString(CultureInfo.InvariantCulture);
            string[] ss2 = ss.Split('.');
            StartCoroutine(SetRank(1,ss2[0]+'/'+ss2[1] , s[0], s[1]));
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

        // SetValue
        string value = game + "_" + data;
        yield return StartCoroutine(SetValue(value));
        
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
                Debug.Log("dd");
            }
        }
        yield return new WaitForSeconds(0f);
    }
}