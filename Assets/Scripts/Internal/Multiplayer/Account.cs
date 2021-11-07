using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;


public class Account : MonoBehaviour
{
    [SerializeField] InputField signupUsername = null;
    [SerializeField] InputField signupPassword = null;
    [SerializeField] InputField signupEmail = null;

    [SerializeField] InputField username = null;
    [SerializeField] InputField password = null;

    public void CreateAccount()
    {
        StartCoroutine(CreateAccountRequest());
    }

    IEnumerator CreateAccountRequest()
    {
        WWWForm data = new WWWForm();
        data.AddField("username", signupUsername.text);
        data.AddField("email", signupEmail.text);
        data.AddField("password", signupPassword.text);

        using (UnityWebRequest request = UnityWebRequest.Post("https://thingy.jahanrashidi.com/signup/index.php", data))
        {
            yield return request.SendWebRequest();

            if (request.downloadHandler.text.Contains("alert"))
            {
                signupUsername.text = "Something went wrong";
            }
            else
            {
                Launcher.instance.SetUsername(signupUsername.text);
                gameObject.SetActive(false);
                transform.parent.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    public void Login()
    {
        StartCoroutine(LoginRequest());
    }

    IEnumerator LoginRequest()
    {
        WWWForm data = new WWWForm();
        data.AddField("username", username.text);
        data.AddField("password", password.text);

        using (UnityWebRequest request = UnityWebRequest.Post("https://thingy.jahanrashidi.com/login/index.php", data))
        {
            yield return request.SendWebRequest();

            if (request.downloadHandler.text.Contains("alert"))
            {
                username.text = "Something went wrong";
            }
            else
            {
                Launcher.instance.SetUsername(request.downloadHandler.text);
                gameObject.SetActive(false);
                transform.parent.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
}
