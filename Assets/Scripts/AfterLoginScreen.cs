using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class AfterLoginScreen : MonoBehaviour
{
    public static AfterLoginScreen instance;
    [SerializeField] Image googleProfilePic;
    [SerializeField]TextMeshProUGUI method;

    [SerializeField] TextMeshProUGUI email;
    [SerializeField] TextMeshProUGUI username;




    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(gameObject);
        }
    }

    public void RestartApp()
    {
        AuthManager.instance.SignOutFromGoogle();

        AuthManager.instance.SignOut();
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public void GoogleAuthCompleted(string _email, string _username)
    {
        method.text = "Google Auth Succesful";
        googleProfilePic.gameObject.SetActive(true);
        email.text = _email;
        username.text = _username;
        SlideIn();


    }

    public void EmailRegCompleted(string _email, string _username)
    {
        method.text = "Email Sign up Succesful";
        googleProfilePic.gameObject.SetActive(false);
        email.text = _email;
        username.text = _username;
        SlideIn();


    }

    public void EmailLogCompleted(string _email, string _username)
    {
      
        method.text = "Email Log In Succesful";
       
        googleProfilePic.gameObject.SetActive(false);
       
        email.text = _email;
        
        username.text = _username;
        
        SlideIn();
        


    }

    public void SlideIn()
    {
        UIManager.instance.SlideScreen(this.GetComponent<RectTransform>());
    }


}
