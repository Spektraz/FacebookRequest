using System;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace Facebook.Service
{
    public class FacebookService : MonoBehaviour
    {
        private static List<string> perms = new List<string>() {"public_profile", "email", "user_friends"};
        private static List<string> permss  = new List<string>() {"user_friends"};
        private static List<string> publishPermissions;
        public static string nameUser;
        public static string friendsUsers;
        public static Text t;
        private static AccessToken aToken;
        public static void Init()
        {
            if (!FB.IsInitialized)
            {
            FB.Init(() =>
                {
                    if (FB.IsInitialized)
                    {
                        FB.ActivateApp();
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
        public static void FacebookLogin()
        {
            FB.LogInWithReadPermissions(perms, AuthCallback);
        }
        public static void FacebookLogout()
        {
            FB.LogOut();
        }
        public  static void FacebookInviteFriend()
        {
            FB.AppRequest("Hey! Come and play this awesome game!", title: "StarSoccerSlim");
        }
    
        public static void FacebookShare()
        {
            FB.ShareLink(new Uri("https://play.google.com/store/apps/details?id=com.blizzard.wtcg.hearthstone&hl=ru&gl=US"), "Hi guys, download this game",
                "Good game come to banner and download this game!",
                new Uri("https://play-lh.googleusercontent.com/5sVlIU8ZO74V78Y2dRIyy-2akcp4DYswLCuMwcD6YfTvAEPOix3Rl4zhIO4TEasxDw=w1920-h969-rw"));
        }

        public static void FacebookShareFriend()
        {
            Uri g = new Uri("https://greyzoned.com/images/evilellf2 icon.png");
            Uri k = new Uri("https://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.Mobile.UserID : "guest"));
            FB.FeedShare(linkCaption: "Im playing this awesome game",
                picture: g,
                linkName: "Check out this game",
                link: k);
        }

        public static void GetFriendsPlayingThisGame(IResult result)
        {
           // string query = "/me/friends?fields=id,name"; //read_custom_friendlists
          //  FB.API(query, HttpMethod.GET, result =>
          //  {
                var dictionary = (Dictionary<string, object>)MiniJSON.Json.Deserialize(result.RawResult);
                var friendsList = (List<object>)dictionary["data"];
                friendsUsers = string.Empty;
                foreach (var dict in friendsList)
                    friendsUsers += ((Dictionary<string, object>)dict)["name"];
          //  });
        }

        public static void Acces(IResult results)
        { 
            FB.AppRequest("Test Message", null, null, null, null, null, null, callback: HandleResult);
            //FB.AppRequest("Come Play the game", null, new List<object>(){"read_custom_friendlists"}, null, null, null,
              //  "Hey", delegate(IAppRequestResult result) 
               // {
                //    Debug.Log(result.RawResult);
                 //   friendsUsers = result.RawResult;
               // });
            //FB.API("/me/friends?fields=id,name", HttpMethod.GET, GetFriendsPlayingThisGame);
        }
        public static void HandleResult(IResult result){
            if (result == null) return;
            if (!string.IsNullOrEmpty(result.Error)) friendsUsers += "Error Response: " + result.Error + "\n";
            else if (result.Cancelled) friendsUsers +=  "Cancelled Response: " + result.RawResult + "\n";
            else if (!string.IsNullOrEmpty(result.RawResult)) friendsUsers += "Success Response:" + result.RawResult + "\n";
            else friendsUsers+= "Empty Response\n";
            friendsUsers+=result.ToString();
        }
        public static void GetFriend()
        {
            //  FB.AppRequest("Test Message", null, null, null, null, null, null, callback: HandleResult);
           // FB.LogInWithPublishPermissions(permss, Acces);
            // FB.LogInWithReadPermissions(permss, Acces);
        }
        public static void GetFacebookFriends()
        {
            FB.API(string.Format("/me?fields=friends.limit({0}).fields(id)", 5), HttpMethod.GET, OnFriendsList);
        }

        private static void OnFriendsList(IGraphResult result)
        {
            try
            {
                Dictionary<string, object> dict = result.ResultDictionary as Dictionary<string, object>;
                object temp;
                List<object> friends;
                List<string> friendsIds;
                if (dict.TryGetValue("friends", out temp))
                {
                    dict = temp as Dictionary<string, object>;
                    if (dict.TryGetValue("data", out temp))
                    {
                        friends = temp as List<object>;
                        friendsIds = new List<string>(friends.Count);
                        for (int i = 0; i < friends.Count; i++)
                        {
                            var friendDict = friends[i] as Dictionary<string, object>;
                            var friend = new Dictionary<string, string>();
                            friend["id"] = friendDict["id"] as string;
                            friend["first_name"] = friendDict["first_name"] as string;
                            friendsIds.Add(friend["id"]);
                            friends.Add(friend);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log("geg");
            }
        }
        public static void InviteBestFriend()
        {
            FB.AppRequest(message: "This game is awesome, join me. now",
                title: "Invite your friends to join you"
                );
        }
        private static void AuthCallback(IResult result)
     {
         if (FB.IsLoggedIn)
         {
             aToken = AccessToken.CurrentAccessToken;
             nameUser = aToken.UserId;
             FB.API("me?fields=name", HttpMethod.GET, NameCallBack);
         }
         else
         {
             Debug.LogError("User cancelled login");
         }
     }
     private  static void NameCallBack(IResult result)
      {
          IDictionary dict = MiniJSON.Json.Deserialize(result.RawResult) as IDictionary;
          string fbname = dict["name"].ToString();
          nameUser = fbname;
      }
    }
    [Serializable]
    public class FacebookData <Value, InternalValue, Id> where Value : InternalData<Id, InternalValue>
    {
        public void LogIn()
        {
            FacebookService.FacebookLogin();
        }

        public string GetUserName()
        {
           return FacebookService.nameUser;
        }
        public void LogOut()
        {
            FacebookService.FacebookLogout();
        }
        public void Share()
        {
            FacebookService.FacebookShareFriend();
        }

        public void InviteFriend()
        {
       //     FacebookService.GetFriendsPlayingThisGame();
       FacebookService.GetFacebookFriends();
        }
        public void Init()
        {
            FacebookService.Init();
        }

        public string GetUserFriend()
        {
           return FacebookService.friendsUsers;
        }
    }
    [Serializable]
    public class InternalData<Id, InternalValue>
    {
        [Header("New Preset:")]
        [Space(1)]
        [SerializeField] protected Id id;
        [SerializeField] protected InternalValue value;
        
        public Id ID => id;
        public InternalValue Value => value;
    }
}
