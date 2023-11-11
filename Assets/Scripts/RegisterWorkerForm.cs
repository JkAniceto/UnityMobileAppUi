using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class RegisterWorkerForm : MonoBehaviour
{
    [SerializeField] private TMP_InputField FirstnameInputField;
    [SerializeField] private TMP_InputField MiddlenameInputField;
    [SerializeField] private TMP_InputField LastnameInputField;
    [SerializeField] private TMP_InputField SuffixInputField;
    [SerializeField] private TMP_InputField EmailInputField;
    [SerializeField] private TMP_InputField ContactnumberInputField;
    [SerializeField] private TMP_InputField BirthdateInputField;
    [SerializeField] private TMP_Dropdown Gender;
    [SerializeField] private TMP_InputField StreetAddressInputField;
    [SerializeField] private TMP_InputField CityAddressInputField;

    [SerializeField] private TMP_InputField UsernameInputField;
    [SerializeField] private TMP_InputField PasswordInputField;
    [SerializeField] private TMP_InputField ConfirmpasswordInputField;

    public void registerWorker()
    {
        StartCoroutine(SendRegistrationWorker());
    }

    private IEnumerator SendRegistrationWorker()
    {
        WWWForm form = new WWWForm();
        form.AddField("firstname", FirstnameInputField.text);
        form.AddField("middlename", MiddlenameInputField.text);
        form.AddField("lastname", LastnameInputField.text);
        form.AddField("suffix", SuffixInputField.text);
        form.AddField("email", EmailInputField.text);
        form.AddField("contact_number", ContactnumberInputField.text);
        form.AddField("selectedDate", BirthdateInputField.text);
        form.AddField("gender", Gender.options[Gender.value].text);
        form.AddField("streetaddress", StreetAddressInputField.text);
        form.AddField("cityaddress", CityAddressInputField.text);
        form.AddField("username", UsernameInputField.text);
        form.AddField("password", PasswordInputField.text);
        form.AddField("confirmregisterpassword", ConfirmpasswordInputField.text);

        using (UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:8000/api/registerWorker", form))
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
