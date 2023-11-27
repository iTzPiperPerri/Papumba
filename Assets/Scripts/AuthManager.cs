using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Google;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net.Http;
using Firebase.Extensions;


public class AuthManager : MonoBehaviour
{
    public static AuthManager instance;

    //Firebase
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;
    
    

    //Google Auth
    string webClientId = "953907620803-ptnskpb61qg89f453flnj5judnbptf3o.apps.googleusercontent.com";
    GoogleSignInConfiguration configuration;


    //UI ELEMENTS
    public Image UserProfilePic;
    public string imageUrl;

    public TextMeshProUGUI infoText;

    public TMP_InputField SUemail, SUpassword, LIemail, LIpassword;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
            
        else 
        {
            Destroy(gameObject);
        }
           

        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
        CheckFirebaseDependencies();
    }


    //Firebase Initialization
    private void CheckFirebaseDependencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result == DependencyStatus.Available)
                    auth = FirebaseAuth.DefaultInstance;
                else
                    AddToInformation("Could not resolve all Firebase dependencies: " + task.Result.ToString());
            }
            else
            {
                AddToInformation("Dependency check was not completed. Error : " + task.Exception.Message);
            }
        });
    }

    void Start()
    {
        InitFirebase();
    }

    void InitFirebase() 
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }


    //------------Email Auth Start------------------------//


    //Sign Up
    void EmailSignUp(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Sign-up successful, user is now authenticated
            FirebaseUser newUser = task.Result.User;
            Debug.Log("User signed up successfully: " + newUser.Email);

            AfterLoginScreen.instance.EmailRegCompleted(newUser.Email, newUser.DisplayName);

        });
    }

    public void EmailSignUpClick()
    {
        Debug.LogError("Will call" + " " + SUemail);
        EmailSignUp(SUemail.text, SUpassword.text);
        Debug.LogError("Called" + " " + SUemail);

    }

    //Log in
    void EmailLogIn(string email, string password)
    {
        
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

           



            AfterLoginScreen.instance.EmailLogCompleted(result.User.Email, result.User.DisplayName);



        });


    }

    public void EmailLogInClick()
    {
        EmailLogIn(LIemail.text, LIpassword.text);
    }

   

    //------------Email Auth End------------------------//



    //------------Google Auth Start------------------------//

    //Google Sign in Fuction to be called on CTA
    public void GoogleSignInClick()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleAuthenticatedFinished);
    }


    //User is registered on Firebase 
    void OnGoogleAuthenticatedFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            Debug.LogError("Fault");
        }

        else if (task.IsCanceled)
        {
            Debug.LogError("Login Canceled");
        }

        else
        {
            Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, null);

            auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithCredentialAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithCredentialAsync encountered an error");
                    return;
                }

                user = auth.CurrentUser;

                AfterLoginScreen.instance.GoogleAuthCompleted(user.Email,user.DisplayName);

                StartCoroutine(LoadImage(CheckImageUrl(user.PhotoUrl.ToString())));

            });
        }

    }

    private string CheckImageUrl(string url) 
    {
        if(!string.IsNullOrEmpty(url))
        {
            return url;
        }

        return imageUrl;
    }

    IEnumerator LoadImage(string imageUri)
    {
        WWW www = new WWW(imageUri);

        

        yield return www;

        UserProfilePic.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.width), new Vector2(0, 0));
    }

    

    public void SignInWithGoogle() { OnSignIn(); }
    public void SignOutFromGoogle() { OnSignOut(); }


    private void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddToInformation("Calling SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleAuthenticatedFinished);
    }

    private void OnSignOut()
    {
        //AddToInformation("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();

    }

    public void OnDisconnect()
    {
        AddToInformation("Calling Disconnect");
        GoogleSignIn.DefaultInstance.Disconnect();
    }

    public void OnSignInSilently()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddToInformation("Calling SignIn Silently");

        GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnGoogleAuthenticatedFinished);
    }

    public void OnGamesSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = true;
        GoogleSignIn.Configuration.RequestIdToken = false;

        AddToInformation("Calling Games SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleAuthenticatedFinished);
    }

    private void AddToInformation(string str) { infoText.text += "\n" + str; }

    //------------Google Auth End------------------------//

    public void SignOut()
    {
        if (auth != null)
        {
            auth.SignOut();
            Debug.Log("User signed out from all authentication methods.");
        }
        else
        {
            Debug.LogWarning("FirebaseAuth instance is null. Make sure to initialize it first.");
        }
    }


}