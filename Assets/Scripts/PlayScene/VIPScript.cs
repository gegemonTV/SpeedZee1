using Firebase.Database;
using System;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class VIPScript : MonoBehaviour, IUnityAdsListener
{
    FirebaseDatabase _database;
#if UNITY_IOS
    private string gameId = "3960506";
#elif UNITY_ANDROID
    private string gameId = "3960507";
#endif

    public GameObject ThisWindow;

    public GameObject AdButton;
    public Text VIPText;

    private VipTimer timer;

    private string placementIdVideo = "rewardedVideo";
    private int PreserveOther = 0;
    // Start is called before the first frame update
    void Start()
    {
        Advertisement.Initialize(gameId);
        Advertisement.AddListener(this);
        _database = FirebaseDatabase.DefaultInstance;
        timer = new VipTimer(PlayerPrefs.GetFloat("VipTimer"), System.Convert.ToBoolean(PlayerPrefs.GetInt("VipEndless")));
    }

    // Update is called once per frame
    void Update()
    {
        VIPText.text = timer.ToString();
        if (timer.getState())
        {
            if (!timer.getIsEndless())
            {
                timer.decreaseTimer(Time.deltaTime);
            }
        }
        else
        {
            PlayerPrefs.SetInt("VIP", 1);
        }
        timer.saveState();
    }

    public void CloseThisWindow()
    {
        ThisWindow.SetActive(false);
        PlayerPrefs.SetInt("Paused", 0);
    }

    public void BoughtForMoney()
    {
        PlayerPrefs.SetInt("VIP", 2);
        timer.setIsEndless(true);
    }

    public void BoughtForAds()
    {
        if (Advertisement.IsReady(placementIdVideo))
        {
            PreserveOther = 5;
            Advertisement.Show(placementIdVideo);
        }
        else
        {
            print("No video Available");
        }
        
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (PreserveOther == 5)
        {
            if (showResult == ShowResult.Finished)
            {
                print("You Watched This");
                long today = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                PlayerPrefs.SetInt("VIP", 2);
                timer.restartTimer(86400f);
            }
            else if (showResult == ShowResult.Skipped)
            {
                // Do not reward the user for skipping the ad.
            }
            else if (showResult == ShowResult.Failed)
            {
                Debug.LogWarning("The ad did not finish due to an error.");
            }
            PreserveOther = 0;
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        if (placementId == placementIdVideo)
        {
            AdButton.GetComponent<Button>().interactable = true;
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        print("Error");
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        print("200");
    }

    public void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }
}
