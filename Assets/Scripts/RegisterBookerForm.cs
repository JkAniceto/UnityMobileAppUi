using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class RegisterBookerForm : MonoBehaviour
{

    [SerializeField] private TMP_InputField NameInputField;
    [SerializeField] private TMP_InputField EmailInputField;
    [SerializeField] private TMP_InputField ContactNumberInputField;
    [SerializeField] private TMP_InputField UsernameInputField;
    [SerializeField] private TMP_InputField PasswordInputField;
    [SerializeField] private TMP_InputField ConfirmPasswordInputField;
    [SerializeField] private TMP_Dropdown Gender;

    public void signupBooker()
    {
        StartCoroutine(SendRegistrationRequest());
    }
    private IEnumerator SendRegistrationRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", NameInputField.text);
        form.AddField("email", EmailInputField.text);
        form.AddField("contact_number", ContactNumberInputField.text);
        form.AddField("username", UsernameInputField.text);
        form.AddField("password", PasswordInputField.text);
        form.AddField("confirmPassword", ConfirmPasswordInputField.text);
        form.AddField("gender", Gender.options[Gender.value].text);

        using (UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:8000/api/registerBooker", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Registration Successful!");
                Debug.Log("Response: " + www.downloadHandler.text);
                SceneManager.LoadScene("LoginPage");
            }
            else
            {
                Debug.LogError("Registration Failed: " + www.error);
                Debug.LogError("Response Code: " + www.responseCode);
            }
        }
    }
}

