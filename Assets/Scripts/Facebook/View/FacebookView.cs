using System;
using System.Collections.Generic;
using Facebook.Service;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using StarSoccerSlim.Patterns.MVC.Controller;

namespace Facebook.View
{
    public class FacebookView : StarSoccerSlim.Patterns.MVC.View.View
    {
        [SerializeField] private Text facebookState;
        [SerializeField] private Text friendsText;
        [SerializeField] private List<FacebookButton> facebookButtons;
        private Dictionary<RequestGet, FacebookButton> facebookButtonsDictionary =
            new Dictionary<RequestGet, FacebookButton>();
        private void Awake()
        {
            facebookButtons.ForEach(x => facebookButtonsDictionary.Add(x.ButtonRequestGet, x));
        }

        public void AddListener(RequestGet requestGet, Action action)
        {
            facebookButtonsDictionary[requestGet].AddListener(action);
        }

        public void Update()
        {
          Controller.Execute();
        }

        public void SetData(string name)
        {
            facebookState.text = name;
        }

        public void SetFriend(string friend)
        {
            friendsText.text = friend;
        }
        public void RemoveListener()
        {
            facebookButtons.ForEach(x=> x.RemoveListener());
        }
        public void OnDestroy()
        {
            RemoveListener();
        }

        protected override IController CreateController() => new FacebookController(this);
    }

    [Serializable]
    public class FacebookButton : FacebookData<FacebookInternalData, string, RequestGet>
    {
        [SerializeField] private Button button;
        [SerializeField] private RequestGet buttonRequestGet;
        public RequestGet ButtonRequestGet => buttonRequestGet;
        public void AddListener(Action action)
        {
            button.onClick.AddListener(action.Invoke);
        }
        public void RemoveListener()
        {
            button.onClick.RemoveAllListeners();
        }
    }
    [Serializable]
    public class FacebookController : Controller<FacebookView> 
    {
        private FacebookButton facebookButton;
        public FacebookController(FacebookView view) : base(view)
        {
            facebookButton = new FacebookButton();
            facebookButton.Init();
        }

        public override void AddListeners()
        {
            base.AddListeners();
            View.AddListener(RequestGet.LogIn, facebookButton.LogIn);
            View.AddListener(RequestGet.LogOut, facebookButton.LogOut);
            View.AddListener(RequestGet.ShareLink, facebookButton.Share);
            View.AddListener(RequestGet.InviteFriend, facebookButton.InviteFriend);
        }

        public override void RemoveListeners()
        {
            base.RemoveListeners();
            View.RemoveListener();
        }

        private void OnHideUnity(bool isGameShown)
        {
            if (!isGameShown)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
        
        public override void Execute()
        {
            View.SetData(facebookButton.GetUserName());
            View.SetFriend(facebookButton.GetUserFriend());
        }
    }
    public class FacebookInternalData : InternalData<RequestGet, string> { }
    public enum RequestGet
    {
        LogIn = 0,
        LogOut = 1,
        ShareLink = 2,
        InviteFriend = 3
    }
}
