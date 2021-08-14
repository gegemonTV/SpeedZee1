using Firebase.Database;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class UpgradesScript : MonoBehaviour, IUnityAdsListener
{
    private FirebaseDatabase _database;

    public GameObject OuterUpsText;

    public Slider ElSlider;
    public Slider StarSlider;
    public Slider BrilliantSlider;

    public GameObject OpenButton;

    private Toy El;
    private Toy Star;
    private Toy Brilliant;

    public GameObject Hint1;
    public GameObject Hint2;
    public GameObject Hint3;

    public Slider UpgradeSlider;

    public Button ElUpgrade;
    public Button StarUpgrade;
    public Button BrilliantUpgrade;

    public Button ElGemsUpgrade;
    public Button StarGemsUpgrade;
    public Button BrilliantGemsUpgrade;

    public GameObject ElUps;
    public GameObject StarUps;
    public GameObject BrilliantUps;

    public GameObject ElDone;
    public GameObject StarDone;
    public GameObject BrilliantDone;

    public Text ElText;
    public Text StarText;
    public Text BrilliantText;

    public Text ElGemsText;
    public Text StarGemsText;
    public Text BrilliantGemsText;

    private int AdsCounter;

    private string placementIdVideo = "rewardedVideo";

//#if UNITY_IOS
//    private string gameId = "3960506";
//#elif UNITY_ANDROID
    private string gameId = "3960507";
//#endif

    private int PreserveOther = 0;
    private void Start()
    {
        _database = FirebaseDatabase.DefaultInstance;

        El = new Toy("Zero");
        Star = new Toy("One");
        Brilliant = new Toy("Two");

        StartCoroutine(EnableDisableUpgrades());

        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId);

        ElUpgrade.onClick.RemoveAllListeners();
        ElUpgrade.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
            ShowNextAdUpgrade(0, El.GetUpgrade() + 2);
        }));
        ElUpgrade.gameObject.GetComponentsInChildren<Text>()[0].text = $"({El.GetUpgrade() + 2}) бесплатно";
        StarUpgrade.onClick.RemoveAllListeners();
        StarUpgrade.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
            ShowNextAdUpgrade(1, Star.GetUpgrade() + 4);
        }));
        StarUpgrade.gameObject.GetComponentsInChildren<Text>()[0].text = $"({Star.GetUpgrade() + 4}) бесплатно";
        BrilliantUpgrade.onClick.RemoveAllListeners();
        BrilliantUpgrade.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
            ShowNextAdUpgrade(2, Brilliant.GetUpgrade() + 6);
        }));
        BrilliantUpgrade.gameObject.GetComponentsInChildren<Text>()[0].text = $"({Brilliant.GetUpgrade() + 6}) бесплатно";
        
    }

    private void Update()
    {
        /*
         * TODO: Добавить ивенты для обновления внутри магазина 
         */      
        if (El.GetPrice() > PlayerPrefs.GetInt("Gems"))
        {
            ElGemsUpgrade.interactable = false;
        }
        else
        {
            ElGemsUpgrade.interactable = true;
        }
        if (Star.GetPrice() > PlayerPrefs.GetInt("Gems"))
        {
            StarGemsUpgrade.interactable = false;
        }
        else
        {
            StarGemsUpgrade.interactable = true;
        }
        if (Brilliant.GetPrice() > PlayerPrefs.GetInt("Gems"))
        {
            BrilliantGemsUpgrade.interactable = false;
        }
        else
        {
            BrilliantGemsUpgrade.interactable = true;
        }
    }

    public void ShowHint(string id)
    {
        switch (id)
        {
            case "El":
                Hint1.SetActive(true);
                break;
            case "Star":
                Hint2.SetActive(true);
                break;
            case "Brilliant":
                Hint3.SetActive(true);
                break;
        }
    }

    public void HintClose(string id)
    {
        switch (id)
        {
            case "El":
                Hint1.SetActive(false);
                break;
            case "Star":
                Hint2.SetActive(false);
                break;
            case "Brilliant":
                Hint3.SetActive(false);
                break;
        }
    }

    public void BuyUpgradeForGems(string toy)
    {
        switch (toy)
        {
            case "El":
                if (PlayerPrefs.GetInt("Gems") < El.GetPrice() || !PlayerPrefs.HasKey("Gems"))
                {
                    Toast.Instance.Show("Недостаточно кристаллов!");
                    return;
                }
                PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") - El.GetPrice());
                El.IncreaseUpgrade();
                El.SaveUpgrade();
                _database.GetReference("users").Child(PlayerPrefs.GetString("AUTH_ID")).Child("ZeroUpgrades").SetValueAsync(El.GetUpgrade());
                break;
            case "Star":
                if (PlayerPrefs.GetInt("Gems") < Star.GetPrice() || !PlayerPrefs.HasKey("Gems"))
                {
                    Toast.Instance.Show("Недостаточно кристаллов!");
                    return;
                }
                PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") - Star.GetPrice());
                Star.IncreaseUpgrade();
                Star.SaveUpgrade();
                _database.GetReference("users").Child(PlayerPrefs.GetString("AUTH_ID")).Child("OneUpgrades").SetValueAsync(Star.GetUpgrade());
                break;
            case "Brilliant":
                if (PlayerPrefs.GetInt("Gems") < Brilliant.GetPrice() || !PlayerPrefs.HasKey("Gems"))
                {
                    Toast.Instance.Show("Недостаточно кристаллов!");
                    return;
                }
                PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") - Brilliant.GetPrice());
                Brilliant.IncreaseUpgrade();
                Brilliant.SaveUpgrade();
                _database.GetReference("users").Child(PlayerPrefs.GetString("AUTH_ID")).Child("TwoUpgrades").SetValueAsync(Brilliant.GetUpgrade());
                break;
        }
    }

    private int TypeAdd;

    public void ShowNextAdUpgrade(int id, int counter)
    {
        if (counter == 1)
        {
            TypeAdd = id;
            ShowOneAd();
        }
        else
        {
            TypeAdd = -1;
            switch (id)
            {
                case 0:
                    ElUpgrade.onClick.RemoveAllListeners();
                    ElUpgrade.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
                        ShowNextAdUpgrade(id, counter - 1);
                    }));
                    ElUpgrade.gameObject.GetComponentsInChildren<Text>()[0].text = $"({counter - 1}) бесплатно";
                    ShowOneAd();
                    break;
                case 1:
                    StarUpgrade.onClick.RemoveAllListeners();
                    StarUpgrade.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
                        ShowNextAdUpgrade(id, counter - 1);
                    }));
                    StarUpgrade.gameObject.GetComponentsInChildren<Text>()[0].text = $"({counter - 1}) бесплатно";
                    ShowOneAd();
                    break;
                case 2:
                    BrilliantUpgrade.onClick.RemoveAllListeners();
                    BrilliantUpgrade.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
                        ShowNextAdUpgrade(id, counter - 1);
                    }));
                    BrilliantUpgrade.gameObject.GetComponentsInChildren<Text>()[0].text = $"({counter - 1}) бесплатно";
                    ShowOneAd();
                    break;
            }
        }
    }

    public void UpgradeForAds(int type)
    {
        //t = type;
        switch (type) 
        {
            
            case 0:
                AdsCounter = El.GetUpgrade() + 1;
                break;
            case 1:
                AdsCounter = Star.GetUpgrade() + 1;
                break;
            case 2:
                AdsCounter = Brilliant.GetUpgrade() + 1;
                break;
        }
        PreserveOther = 3;
        ShowOneAd();
    } 

    public void ShowOneAd()
    {
        if (Advertisement.IsReady(placementIdVideo))
        {
            PreserveOther = 3;
            Advertisement.Show(placementIdVideo);
        }
        else
        {
            print("No video Available");
        }
    }
   
    /*
     * TODO: убрать нахер поеботу ниже 
     */

    private IEnumerator EnableDisableUpgrades(){
        while (true)
        {
            if (El.GetUpgrade() == 4){
                ElUps.SetActive(false);
                ElDone.SetActive(true);
            }
            else {
                ElUps.SetActive(true);
                ElDone.SetActive(false);
            }
            if (Star.GetUpgrade() == 4){
                StarUps.SetActive(false);
                StarDone.SetActive(true);
            }
            else {
                if(PlayerPrefs.GetInt("StarBought") == 1){
                    StarUps.SetActive(true);
                }
                StarDone.SetActive(false);
            }
            if (Brilliant.GetUpgrade() == 4){
                BrilliantUps.SetActive(false);
                BrilliantDone.SetActive(true);
            }
            else {
                if(PlayerPrefs.GetInt("BrilliantBought") == 1){
                    BrilliantUps.SetActive(true);
                }
                BrilliantDone.SetActive(false);
            }
            yield return new WaitForFixedUpdate();
        }
    }


    /*
     * TODO: Убрать нахер поеботу выше
     */

    public void OnUnityAdsReady(string placementId)
    {
        print("OK");
    }

    public void OnUnityAdsDidError(string message)
    {
        print(404);
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        print(200);
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (PreserveOther == 3)
        {
            if (showResult == ShowResult.Finished)
            {
                if (TypeAdd != -1)
                {
                    switch (TypeAdd)
                    {
                        case 0:
                            El.IncreaseUpgrade();
                            if (El.GetUpgrade() == 4)
                            {
                                ElUps.SetActive(false);
                            }
                            ElUpgrade.onClick.RemoveAllListeners();
                            ElUpgrade.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
                                ShowNextAdUpgrade(0, El.GetUpgrade()+2);
                            }));
                            ElUpgrade.gameObject.GetComponentsInChildren<Text>()[0].text = $"({El.GetUpgrade() + 2}) бесплатно";
                            El.SaveUpgrade();
                            _database.GetReference("users").Child(PlayerPrefs.GetString("AUTH_ID")).Child("ZeroUpgrades").SetValueAsync(El.GetUpgrade());
                            break;
                        case 1:
                            Star.IncreaseUpgrade();
                            if (Star.GetUpgrade() == 4)
                            {
                                StarUps.SetActive(false);
                            }
                            StarUpgrade.onClick.RemoveAllListeners();
                            StarUpgrade.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
                                ShowNextAdUpgrade(1, Star.GetUpgrade()+4);
                            }));
                            StarUpgrade.gameObject.GetComponentsInChildren<Text>()[0].text = $"({Star.GetUpgrade()+4}) бесплатно";
                            Star.SaveUpgrade();
                            _database.GetReference("users").Child(PlayerPrefs.GetString("AUTH_ID")).Child("OneUpgrades").SetValueAsync(Star.GetUpgrade());
                            break;
                        case 2:
                            Brilliant.IncreaseUpgrade();
                            if (Brilliant.GetUpgrade() == 4)
                            {
                                BrilliantUps.SetActive(false);
                            }
                            BrilliantUpgrade.onClick.RemoveAllListeners();
                            BrilliantUpgrade.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
                                ShowNextAdUpgrade(2, Brilliant.GetUpgrade()+6);
                            }));
                            BrilliantUpgrade.gameObject.GetComponentsInChildren<Text>()[0].text = $"({Brilliant.GetUpgrade()+6}) бесплатно";
                            Brilliant.SaveUpgrade();
                            _database.GetReference("users").Child(PlayerPrefs.GetString("AUTH_ID")).Child("TwoUpgrades").SetValueAsync(Brilliant.GetUpgrade());
                            break;
                    }
                }
            }
            else if (showResult == ShowResult.Skipped)
            {
                // Do not reward the user for skipping the ad.
            }
            else if (showResult == ShowResult.Failed)
            {
                Debug.LogWarning("The ad did not finish due to an error.");
            }
        }
    }
}
