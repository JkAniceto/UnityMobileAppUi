using UnityEngine;
using UnityEngine.SceneManagement;

public class RegisterAccountButton : MonoBehaviour
{
    public void LoadWorkerRegistrationScene()
    {
        SceneManager.LoadScene("RegistrationPage");
    }
}
