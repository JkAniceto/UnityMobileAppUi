using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Networking;
using System.Security.Cryptography;
using System.Text;

[Serializable]
public class LoginResponse
{
    public string access_token;
    public string token_type;
    public int expires_in;
    public int status;
    public int user_role;
}

public class Loginform : MonoBehaviour
{
    [SerializeField] private TMP_InputField UsernameInputField;
    [SerializeField] private TMP_InputField PasswordInputField;

    public void saveLogin()
    {
        string username = UsernameInputField.text;
        string password = PasswordInputField.text;

        Debug.Log("Attempting to log in with username: " + username);
        StartCoroutine(PostRequest(username, password));
    }

    IEnumerator PostRequest(string username, string hashedPassword)
    {
        string url = "http://127.0.0.1:8000/api/login";

        // Create a JSON object to send data to the API
        string jsonData = "{\"username\":\"" + username + "\",\"password\":\"" + hashedPassword + "\"}";
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // Set up the request headers
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");

        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(postData);
            www.downloadHandler = new DownloadHandlerBuffer();
            foreach (var header in headers)
            {
                www.SetRequestHeader(header.Key, header.Value);
            }

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ProtocolError || www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("Error: " + www.error);
                Debug.LogError("Response Code: " + www.responseCode);
                Debug.LogError("Response Text: " + www.downloadHandler.text);
                Debug.LogError("No Register User Found");
            }
            else
            {
                // Parse the response data using the LoginResponse class
                LoginResponse responseData = JsonUtility.FromJson<LoginResponse>(www.downloadHandler.text);

                PlayerPrefs.SetString("AccessToken", responseData.token_type + " " + responseData.access_token);
                PlayerPrefs.SetInt("user_role", responseData.user_role);

                int userRole = responseData.user_role;
                if (userRole == 1)
                {
                    SceneManager.LoadScene("WorkerHomePage");
                }
                else if (userRole == 2)
                {
                    SceneManager.LoadScene("BookerHomePage");
                }
                else
                {
                    Debug.LogError("Unknown user role ID: " + userRole);
                }
            }
        }
    }
}