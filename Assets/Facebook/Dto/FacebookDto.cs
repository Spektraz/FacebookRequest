using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Facebook.Dto
{
  [Serializable]
  public class FacebookDto : MonoBehaviour
  {
    [JsonProperty("request")] public  string FacebookId { get;  set; }
    [JsonProperty("userName")] public string FacebookName { get; set; }
    [JsonProperty("to")] public  string FacebookFriendId { get;  set; }
  }
}
