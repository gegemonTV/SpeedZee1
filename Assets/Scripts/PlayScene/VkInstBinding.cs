using Firebase.Database;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Util;
using UnityEngine;

public class VkInstBinding : MonoBehaviour
{
    private string VkId = "";
    private string InstId = "";
    private string currentUrl = "";
    private int system;
    private FirebaseDatabase _database;
    WebViewObject webViewObject;
    // Start is called before the first frame update
    void Start()
    {
        _database = FirebaseDatabase.DefaultInstance;
        webViewObject = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
        webViewObject.Init(
            wkContentMode: 0,
            ld: (msg) =>
            {
                HttpEncoder.Current = HttpEncoder.Default;
                currentUrl = msg;
                Uri uri = new Uri(msg);
                if (currentUrl.Substring(0, 42).Equals("https://speedzee.github.io/auth/blank.html"))
                {

                    Debug.Log("HiddenBro!");

                    var query = currentUrl.Substring(43);
                    Debug.Log(query);

                    if (system == 1)
                    {
                        var parsed = HttpUtility.ParseQueryString(query);
                        Debug.Log(parsed.Get("access_token"));
                        Debug.Log(parsed.Get("user_id"));

                        var token = parsed.Get("access_token");
                        var user_id = parsed.Get("user_id");

                        GetRequestVk("https://api.vk.com/method/users.get?", token, user_id);

                        Debug.Log(VkId);

                        _database.GetReference("users").Child(PlayerPrefs.GetString("AUTH_ID")).Child("coins").GetValueAsync().ContinueWith(task =>
                        {
                            if (task.IsFaulted)
                            {
                                Debug.Log("Something Went Wrong");
                            }
                            else if (task.IsCompleted)
                            {
                                DataSnapshot snapshot = task.Result;
                                if (!snapshot.Exists)
                                {
                                    _database.GetReference("users").Child(PlayerPrefs.GetString("AUTH_ID")).Child("vk_user_alias").SetValueAsync(VkId);
                                }
                            }
                        });
                    }
                    else
                    {
                        var parsed = HttpUtility.ParseQueryString(query);
                        Debug.Log(parsed.Get("code").Substring(0, parsed.Get("code").Length - 2));

                        var code = parsed.Get("code").Substring(0, parsed.Get("code").Length - 2);

                        GetRequestInst("https://api.instagram.com/oauth/access_token?", code, "205457887848346", "f9a9d375796fccd272ce9a00bc26bf7b", "https://speedzee.github.io/auth/blank.html");

                        Debug.Log(InstId);

                        _database.GetReference("users").Child(PlayerPrefs.GetString("AUTH_ID")).Child("coins").GetValueAsync().ContinueWith(task =>
                        {
                            if (task.IsFaulted)
                            {
                                Debug.Log("Something Went Wrong");
                            }
                            else if (task.IsCompleted)
                            {
                                DataSnapshot snapshot = task.Result;
                                if (!snapshot.Exists)
                                {
                                    _database.GetReference("users").Child(PlayerPrefs.GetString("AUTH_ID")).Child("insta_user_alias").SetValueAsync(InstId);
                                }
                            }
                        });
                    }

                    PlayerPrefs.Save();
                    webViewObject.SetVisibility(false);

                }

                else
                {
                    Debug.Log(currentUrl);
                    Debug.Log(currentUrl.Substring(0, 42));
                }
                Debug.Log(msg);
            });
        webViewObject.SetMargins(0, 0, 0, 0, true);
        webViewObject.ClearCookies();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void VkUserAdd()
    {
        system = 1;
        webViewObject.SetVisibility(true);
        webViewObject.LoadURL("https://oauth.vk.com/authorize?client_id=7690413&redirect_uri=https://speedzee.github.io/auth/blank.html&scope=photos&response_type=token");
        webViewObject.EvaluateJS("Unity.call(location);");
    }

    public void InstUserAdd()
    {
        system = 0;
        webViewObject.SetVisibility(true);
        webViewObject.LoadURL("https://api.instagram.com/oauth/authorize?client_id=205457887848346&redirect_uri=https://speedzee.github.io/auth/blank.html&scope=user_profile,user_media&response_type=code");
        webViewObject.EvaluateJS("Unity.call(location);");
    }

    private void GetRequestVk(string uri, string token, string user_id)
    {
        var request = (HttpWebRequest)WebRequest.Create(uri);
        var postData = $"user_ids={user_id}";
        postData += $"&fields=screen_name";
        postData += $"&access_token={token}";
        postData += $"&v=5.126";

        var data = Encoding.ASCII.GetBytes(postData);

        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = data.Length;

        using (var stream = request.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
        }

        var response = (HttpWebResponse)request.GetResponse();

        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

        Debug.Log(responseString);

        var trueResponse = JObject.Parse(responseString.Substring(13, responseString.Length - 15));

        Debug.Log(responseString.Substring(13, responseString.Length - 15));

        VkId = trueResponse["screen_name"].ToString();
        Debug.Log(VkId);
    }

    private void GetRequestInst(string uri, string code, string app_id, string client_secret, string redirect_uri)
    {
        var request = (HttpWebRequest)WebRequest.Create(uri);

        var postData = $"client_id={app_id}";
        postData += $"&client_secret={client_secret}";
        postData += "&grant_type=authorization_code";
        postData += $"&redirect_uri={redirect_uri}";
        postData += $"&code={code}";

        var data = Encoding.ASCII.GetBytes(postData);

        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = data.Length;

        using (var stream = request.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
        }

        var response = (HttpWebResponse)request.GetResponse();

        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

        Debug.Log(responseString);

        var trueResponse = JObject.Parse(responseString);

        var marker = trueResponse["access_token"].ToString();

        postData = "fields=username";
        postData += $"&access_token={marker}";

        var getRequest = (HttpWebRequest)WebRequest.Create("https://graph.instagram.com/me?" + postData);



        data = Encoding.ASCII.GetBytes(postData);

        getRequest.Method = "GET";
        getRequest.ContentType = "application/x-www-form-urlencoded";

        response = (HttpWebResponse)getRequest.GetResponse();
        using (Stream dataStream = response.GetResponseStream())
        {
            StreamReader reader = new StreamReader(dataStream);
            string responseFromInst = reader.ReadToEnd();
            Debug.Log(responseFromInst);
            trueResponse = JObject.Parse(responseFromInst);

            InstId = trueResponse["username"].ToString();
            Debug.Log(InstId);
        }

        //Debug.Log(responseString);


    }
}
