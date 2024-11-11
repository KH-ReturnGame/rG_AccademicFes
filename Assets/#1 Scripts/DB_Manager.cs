using System.Collections;
using System.Collections.Generic;
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
	const string URL = "https://script.google.com/macros/s/AKfycbyvmxjwFUBJ08r55aaTLg8JKZNvQxHGCDTh1gUoTlx-2X-sWRECmAMB0NIEdVmHFCH8/exec";
	public GoogleData GD;
	public InputField IDInput, PassInput, ValueInput;
	string id, pass;
	
	
	
	
	
	bool SetIDPass()
	{
		id = IDInput.text.Trim();
		pass = PassInput.text.Trim();

		if (id == "" || pass == "") return false;
		else return true;
	}
	
	public void Register()
	{
		if (!SetIDPass())
		{
			print("아이디 또는 비밀번호가 비어있습니다");
			return;
		}
		WWWForm form = new WWWForm();
		form.AddField("order", "register");
		form.AddField("id", id);
		form.AddField("pass", pass);

		StartCoroutine(Post(form));
	}
	
	public void Login()
	{
		if (!SetIDPass())
		{
			print("아이디 또는 비밀번호가 비어있습니다");
			return;
		}
		WWWForm form = new WWWForm();
		form.AddField("order", "login");
		form.AddField("id", id);
		form.AddField("pass", pass);

		StartCoroutine(Post(form));
	}
	
	
	void OnApplicationQuit()
	{
		WWWForm form = new WWWForm();
		form.AddField("order", "logout");

		StartCoroutine(Post(form));
	}


	public void SetValue()
	{
		WWWForm form = new WWWForm();
		form.AddField("order", "setValue");
		form.AddField("value", ValueInput.text);

		StartCoroutine(Post(form));
	}


	public void GetValue()
	{
		string input = ValueInput.text.Trim();

		WWWForm form = new WWWForm();
		form.AddField("order", "getValue");
		form.AddField("value", int.Parse(input));

		StartCoroutine(Post(form));
	}





	IEnumerator Post(WWWForm form)
	{
		using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) // 반드시 using을 써야한다
		{
			yield return www.SendWebRequest();

			if (www.isDone) Response(www.downloadHandler.text);
			else print("웹의 응답이 없습니다.");
			
			www.Dispose();
		}
		
	}


	void Response(string json)
	{
		if (string.IsNullOrEmpty(json)) return;

		GD = JsonUtility.FromJson<GoogleData>(json);

		if (GD.result == "ERROR")
		{
			print(GD.order + "을 실행할 수 없습니다. 에러 메시지 : " + GD.msg);
			return;
		}

		print(GD.order + "을 실행했습니다. 메시지 : " + GD.msg);

		if (GD.order == "getValue")
		{
			ValueInput.text = GD.value;
		}
	}
}