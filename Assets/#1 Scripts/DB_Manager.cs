/*using System.Collections;
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
        idpass.text = "";
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
}*/

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine.UI;

public class DB_Manager : MonoBehaviour
{
    class Rank
    {
        public string name;
        public string score;

        public Rank(string name, string score)
        {
            this.name = name;
            this.score = score;
        }
    }
    
    public TMP_InputField id_;
    public Button btn;

    public DatabaseReference reference { get; set; }

    void Start()
    {
        
    }

    public void SetBtn()
    {
        string id = id_.text.Trim();
        string time = GetComponent<Player>().last_time.ToString(CultureInfo.InvariantCulture);
        btn.interactable = false;
        id_.interactable = false;
        SetRank(1,id,time);
    }

    public void SetRank(int game, string id, string score)
    {
        switch (game)
        {
            case 1:
                FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
                    if (task.Result == DependencyStatus.Available)
                    {
                        var db = FirebaseDatabase.GetInstance("https://returngame-d8a65-default-rtdb.firebaseio.com/");
                        var reference = db.RootReference;
                        var rankRef = db.GetReference("Game1_Rank");

                        // 먼저 읽기
                        rankRef.GetValueAsync().ContinueWithOnMainThread(readTask => {
                            if (readTask.IsFaulted) {
                                Debug.LogError("Reading error: " + readTask.Exception);
                                return;
                            }

                            if (readTask.IsCompleted) {
                                DataSnapshot snapshot = readTask.Result;
                                
                                bool nameExists = false;
                                string existingKey = null;
                                string existingScore = "0.0";
                                // 여기서 snapshot으로부터 기존 데이터 확인
                                foreach (DataSnapshot data in snapshot.Children)
                                {
                                    if (data.Value is Dictionary<string, object> rank)
                                    {
                                        string existingName = rank["name"].ToString();
                                        existingScore = rank["score"].ToString();
                                        if (existingName == id)
                                        {
                                            // 해당 이름이 이미 존재
                                            nameExists = true;
                                            existingKey = data.Key;
                                            Debug.Log("이미 존재하는 사용자: " + existingName + ", 현재 점수: " + existingScore);
                                            break;

                                        }
                                    }
                                }

                                // 만약 기존 사용자가 있다면 점수만 업데이트
                                if (nameExists && existingKey != null) {
                                    if (float.Parse(existingScore) <= float.Parse(score))
                                    {
                                        // 새로운 점수로 업데이트
                                        // 단순히 score만 업데이트하고 싶다면 Child("score").SetValueAsync(score)로 가능
                                        reference.Child("Game1_Rank").Child(existingKey).Child("score").SetValueAsync(score)
                                            .ContinueWithOnMainThread(writeTask => {
                                                if (writeTask.IsFaulted) {
                                                    Debug.LogError("Failed to update data: " + writeTask.Exception);
                                                } else if (writeTask.IsCompleted) {
                                                    Debug.Log("Score successfully updated for existing user!");
                                                    StartCoroutine(GetComponent<Player>().Restart());
                                                }
                                            });
                                    }
                                }
                                else {
                                    // 기존 사용자가 없다면 새로 데이터 추가
                                    Rank newRank = new Rank(id, score);
                                    string json = JsonUtility.ToJson(newRank);
                                    string key = reference.Child("Game1_Rank").Push().Key;

                                    reference.Child("Game1_Rank").Child(key).SetRawJsonValueAsync(json)
                                        .ContinueWithOnMainThread(writeTask => {
                                            if (writeTask.IsFaulted) {
                                                Debug.LogError("Failed to write data: " + writeTask.Exception);
                                            } else if (writeTask.IsCompleted) {
                                                Debug.Log("Data successfully written!");
                                                StartCoroutine(GetComponent<Player>().Restart());
                                            }
                                        });
                                }
                            }
                        });
                    }
                    else
                    {
                        Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
                    }
                });
                break;
        }
    }
}
