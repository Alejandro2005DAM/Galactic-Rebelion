using System;
using System.Collections;
using UnityEngine;

public class LoginNetwork : MonoBehaviour
{
    

   
public void CheckUser(string userName, string pass, Action<Response>response){
    StartCoroutine(CO_CheckUser(userName,pass,response));
}

private IEnumerator CO_CheckUser(string userName, string pass, Action<Response> response)
{
        WWWForm form = new WWWForm();
        form.AddField("userName", userName);
        form.AddField("pass",pass);

        WWW w = new WWW("http://localhost/game/checkUser.php",form);
        yield return w;
        Debug.Log(w.text);
        response(JsonUtility.FromJson<Response>(w.text));
}


}


