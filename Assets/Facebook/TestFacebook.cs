using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
 
public class TestFacebook : MonoBehaviour {
 
    public Text FriendsText;
    public Text Fixed;
    private static AccessToken aToken;
    private static List<string> perms = new List<string>() {"public_profile", "email", "user_friends"};

    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(() =>
                {
                    if (FB.IsInitialized)
                    {
                        FB.ActivateApp();
                        Fixed.text = "1";
                    }
                    else
                        Debug.LogError("Couldn't initialize");
                    
                },
                isGameShown =>
                {
                    if (!isGameShown)
                        Time.timeScale = 0;
                    else
                        Time.timeScale = 1;
                });
        }
        else
        {
            FB.ActivateApp();
        }
    }

    public void FacebookLogin()
    {
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }
 
    public void FacebookLogout()
    {
        FB.LogOut();
    }

    public void FacebookShare()
    {
        FB.ShareLink(new System.Uri("https://resocoder.com"), "Check it out!",
            "Good programming tutorials lol!",
            new System.Uri("https://resocoder.com/wp-content/uploads/2017/01/logoRound512.png"));
    }
 
    #region Inviting
    public void FacebookGameRequest()
    {
        FB.AppRequest("Hey! Come and play this awesome game!", title: "Reso Coder Tutorial");
    }

    private  void AuthCallback(IResult result)
    {
        Fixed.text = "Ger";
        if (FB.IsLoggedIn)
        {
            aToken = AccessToken.CurrentAccessToken;
          //  FriendsText.text = aToken.UserId;
            FB.API("me?fields=name", HttpMethod.GET, NameCallBack);
        }
        else
        {
            Debug.LogError("User cancelled login");
        }
    }
    private  void NameCallBack(IResult result)
    {
        IDictionary dict = Facebook.MiniJSON.Json.Deserialize(result.RawResult) as IDictionary;
        string fbname = dict["name"].ToString();
            //var g = result.ResultDictionary["name"];
            FriendsText.text = fbname;
    }
    public void FacebookInvite()
    {
  //      FB.Mobile.AppInvite(new System.Uri("https://play.google.com/store/apps/details?id=com.tappybyte.byteaway"));
    }
    #endregion
 
    public void GetFriendsPlayingThisGame()
    {
        string query = "/me/friends";
        FB.API(query, HttpMethod.GET, result =>
        {
            var dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
            var friendsList = (List<object>)dictionary["data"];
            FriendsText.text = string.Empty;
            foreach (var dict in friendsList)
                FriendsText.text += ((Dictionary<string, object>)dict)["name"];
        });
    }
}