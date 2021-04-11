using UnityEngine;
using Firebase.Database;
using UnityEngine.SceneManagement;
using System;
using System.Web;
using System.Web.Util;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Text;
using System.IO;
using UnityEngine.UI;
using System.Collections;

public class SendData : MonoBehaviour
{


    private FirebaseDatabase _database;

    private string Nick = "";
    private string VkId = "";
    private string InstId = "";
    private string currentUrl = "";
    private int system;

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

                        _database.GetReference("users").Child(VkId).Child("gems").GetValueAsync().ContinueWith(task =>
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
                                    _database.GetReference("users").Child(VkId).Child("gems").SetValueAsync(0);
                                    _database.GetReference("users").Child(VkId).Child("vk_user_alias").SetValueAsync(VkId);
                                    _database.GetReference("users").Child(VkId).Child("insta_user_alias").SetValueAsync(InstId);
                                    _database.GetReference("users").Child(VkId).Child("turning_speed").SetValueAsync(0);
                                    _database.GetReference("users").Child(VkId).Child("VIP").SetValueAsync(1);
                                    _database.GetReference("users").Child(VkId).Child("ZeroUpgrades").SetValueAsync(0);
                                    _database.GetReference("users").Child(VkId).Child("OneUpgrades").SetValueAsync(0);
                                    _database.GetReference("users").Child(VkId).Child("TwoUpgrades").SetValueAsync(0);
                                    _database.GetReference("users").Child(VkId).Child("StarBought").SetValueAsync(0);
                                    _database.GetReference("users").Child(VkId).Child("BrilliantBought").SetValueAsync(0);

                                    PlayerPrefs.SetString("AUTH_ID", VkId);
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

                        _database.GetReference("users").Child(InstId).Child("gems").GetValueAsync().ContinueWith(task =>
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
                                        _database.GetReference("users").Child(InstId).Child("gems").SetValueAsync(0);
                                        _database.GetReference("users").Child(InstId).Child("vk_user_alias").SetValueAsync(VkId);
                                        _database.GetReference("users").Child(InstId).Child("insta_user_alias").SetValueAsync(InstId);
                                        _database.GetReference("users").Child(InstId).Child("turning_speed").SetValueAsync(0);
                                        _database.GetReference("users").Child(InstId).Child("VIP").SetValueAsync(1);
                                        _database.GetReference("users").Child(InstId).Child("ZeroUpgrades").SetValueAsync(0);
                                        _database.GetReference("users").Child(InstId).Child("OneUpgrades").SetValueAsync(0);
                                        _database.GetReference("users").Child(InstId).Child("TwoUpgrades").SetValueAsync(0);
                                        _database.GetReference("users").Child(InstId).Child("StarBought").SetValueAsync(0);
                                        _database.GetReference("users").Child(InstId).Child("BrilliantBought").SetValueAsync(0);

                                        PlayerPrefs.SetString("AUTH_ID", InstId);
                                    }
                                }
                            });
                    }

                    PlayerPrefs.SetFloat("Energy", 28800f);
                    PlayerPrefs.SetInt("ZeroUpgrade", 0);
                    PlayerPrefs.SetInt("OneUpgrade", 0);
                    PlayerPrefs.SetInt("TwoUpgrade", 0);
                    PlayerPrefs.SetInt("VIP", 1);
                    PlayerPrefs.SetInt("StarBought", 0);
                    PlayerPrefs.SetInt("BrilliantBought", 0);
                    PlayerPrefs.SetInt("WheelTurnings", 1);
                    PlayerPrefs.SetString("AUTH_ID", Nick);
                    PlayerPrefs.SetString("NextRotation", (System.DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 86400).ToString());
                    PlayerPrefs.Save();
                    webViewObject.SetVisibility(false);
                    SceneManager.LoadScene("PlayScene");

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

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            webViewObject.SetVisibility(false);
        }
    }

    public void SetNick(string nick)
    {
        Nick = nick;
    }

    public void SetInstId()
    {
        system = 0;
        webViewObject.SetVisibility(true);
        webViewObject.LoadURL("https://api.instagram.com/oauth/authorize?client_id=205457887848346&redirect_uri=https://speedzee.github.io/auth/blank.html&scope=user_profile,user_media&response_type=code");
        webViewObject.EvaluateJS("Unity.call(location);");

    }

    public void SetVkId()
    {
        system = 1;
        webViewObject.SetVisibility(true);
        webViewObject.LoadURL("https://oauth.vk.com/authorize?client_id=7690413&redirect_uri=https://speedzee.github.io/auth/blank.html&scope=photos&response_type=token");
        webViewObject.EvaluateJS("Unity.call(location);");
    }

    public void SendNewUser()
    {
        if (Nick.Contains(" "))
        {
            Toast.Instance.Show("Имя не может содержать пробелы");
        }
        else
        {
            if (Nick == "")
            {
                Toast.Instance.Show("Имя пустое");
            }
            else
            {
                _database.GetReference("users").Child(Nick).Child("coins").GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.Log("Something Went Wrong");
                    }
                    else if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result;
                        if (snapshot.Exists)
                        {
                            Toast.Instance.Show("Пользователь уже существует");
                        }
                        else
                        {
                            _database.GetReference("users").Child(Nick).Child("gems").SetValueAsync(0);
                            _database.GetReference("users").Child(Nick).Child("vk_user_alias").SetValueAsync(VkId);
                            _database.GetReference("users").Child(Nick).Child("insta_user_alias").SetValueAsync(InstId);
                            _database.GetReference("users").Child(Nick).Child("turning_number").SetValueAsync(0);
                            _database.GetReference("users").Child(Nick).Child("VIP").SetValueAsync(1);
                            _database.GetReference("users").Child(Nick).Child("ZeroUpgrades").SetValueAsync(0);
                            _database.GetReference("users").Child(Nick).Child("OneUpgrades").SetValueAsync(0);
                            _database.GetReference("users").Child(Nick).Child("TwoUpgrades").SetValueAsync(0);
                            _database.GetReference("users").Child(Nick).Child("StarBought").SetValueAsync(0);
                            _database.GetReference("users").Child(Nick).Child("BrilliantBought").SetValueAsync(0);

                            PlayerPrefs.SetFloat("Energy", 28800f);
                            PlayerPrefs.SetInt("ZeroUpgrade", 0);
                            PlayerPrefs.SetInt("OneUpgrade", 0);
                            PlayerPrefs.SetInt("TwoUpgrade", 0);
                            PlayerPrefs.SetInt("VIP", 1);
                            PlayerPrefs.SetInt("StarBought", 0);
                            PlayerPrefs.SetInt("BrilliantBought", 0);
                            PlayerPrefs.SetInt("WheelTurnings", 1);
                            PlayerPrefs.SetString("AUTH_ID", Nick);
                            PlayerPrefs.SetString("NextRotation", (System.DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 86400).ToString());
                            PlayerPrefs.Save();

                            StartCoroutine(OpenNextScene("PlayScene"));
                        }
                    }
                });

            }
        }
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

    AsyncOperation async;
    public GameObject progressSlider;
    private IEnumerator OpenNextScene(string name)
    {
        progressSlider.SetActive(true);
        async = SceneManager.LoadSceneAsync(name);
        async.allowSceneActivation = true;
        while (!async.isDone)
        {
            progressSlider.GetComponent<Slider>().value = async.progress / 0.9f;
            yield return new WaitForFixedUpdate();
        }
    }

}