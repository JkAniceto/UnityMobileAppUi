using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
public class LogoutButton : MonoBehaviour
{
    public void Logout()
    {
        StartCoroutine(LogoutRequest());
    }

    private IEnumerator LogoutRequest()
    {
        string accessToken = PlayerPrefs.GetString("AccessToken");

        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:8000/api/logout", "");
        www.SetRequestHeader("Authorization", accessToken);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            PlayerPrefs.DeleteKey("AccessToken");
            PlayerPrefs.DeleteKey("user_role");
            Debug.Log("Logout Successful!");
            SceneManager.LoadScene("LoginPage");
        }
        else
        {
            Debug.LogError("Logout Failed: " + www.error);
            Debug.LogError("Response Code: " + www.responseCode);
        }
    }
}
