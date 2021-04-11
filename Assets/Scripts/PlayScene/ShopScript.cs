using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class ShopScript : MonoBehaviour, IUnityAdsListener
{
    private FirebaseDatabase _database;

    public GameObject ThisWindow;

    public GameObject Star;
    public GameObject El;
    public GameObject Brilliant;

    public GameObject ElLever;
    public GameObject StarLever;
    public GameObject BrilliantLever;

    private ShopObject StarItem;
    private ShopObject BrilliantItem;

    public GameObject StarGemsButton;
    public GameObject StarPayButton;
    public GameObject StarChooseButton;

    public GameObject BrilliantGemsButton;
    public GameObject BrilliantPayButton;
    public GameObject BrilliantChooseButton;

    public GameObject StarUpgradesImage;
    public GameObject BrilliantUpgradesImage;

    public GameObject StarUpgrades;
    public GameObject BrilliantUpgrades;

    public Button GemsButton0;
    public Button GemsButton1;
    public Button GemsButton2;
    public Button GemsButton3;

    public Button EnergyButton0;
    public Button EnergyButton1;

    public Button KeysButton0;
    public Button KeysButton1;

    private Animator LeverAnimator;

    public void ProfiBuy()
    {
        PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") + 500000);
        PlayerPrefs.SetFloat("Energy", PlayerPrefs.GetFloat("Energy") + 194400f);
        PlayerPrefs.SetInt("VIP", 2);
        PlayerPrefs.SetString("VipUntil", (System.DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 2678400).ToString());
        _database.GetReference("users").Child(PlayerPrefs.GetString("AUTH_ID")).Child("VipUntil").SetValueAsync(System.DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 2678400);
        _database.GetReference("users").Child(PlayerPrefs.GetString("AUTH_ID")).Child("VIP").SetValueAsync(2);
        EventManage.CallOnResourceUpdate("Gems");
        EventManage.CallOnResourceUpdate("Energy");
        EventManage.CallOnResourceUpdate("Keys");
    }

    public void StarterBuy()
    {
        PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") + 100000);
        PlayerPrefs.SetFloat("Energy", PlayerPrefs.GetFloat("Energy") + 86400f);
        PlayerPrefs.SetInt("VIP", 2);
        PlayerPrefs.SetString("VipUntil", (System.DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 25200).ToString());
        _database.GetReference("users").Child(PlayerPrefs.GetString("AUTH_ID")).Child("VipUntil").SetValueAsync(System.DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 25200);
        _database.GetReference("users").Child(PlayerPrefs.GetString("AUTH_ID")).Child("VIP").SetValueAsync(2);
        EventManage.CallOnResourceUpdate("Gems");
        EventManage.CallOnResourceUpdate("Energy");
        EventManage.CallOnResourceUpdate("Keys");
    }

    public void BuyGems(int gems)
    {
        PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") + gems);
        EventManage.CallOnResourceUpdate("Gems");
    }

    public void BuyEnergy(float energy)
    {
        PlayerPrefs.SetFloat("Energy", PlayerPrefs.GetFloat("Energy") + energy);
        EventManage.CallOnResourceUpdate("Energy");
    }

    public void BuyKeys(int keys)
    {
        PlayerPrefs.SetInt("Keys", PlayerPrefs.GetInt("Keys") + keys);
        EventManage.CallOnResourceUpdate("Keys");
    }

    public void ShowNextAdGems(int id,int gems, int counter)
    {
        if (counter == 1)
        {
            GemsAdd = gems;
            ShowOneAd();
        } else
        {
            GemsAdd = 0;
            switch (id)
            {
                case 1:
                    GemsButton1.onClick.RemoveAllListeners();
                    GemsButton1.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
                        ShowNextAdGems(id, gems, counter - 1);
                    }));
                    GemsButton1.gameObject.GetComponentsInChildren<Text>()[0].text = $"({counter - 1}) бесплатно";
                    ShowOneAd();
                    break;
                case 2:
                    GemsButton2.onClick.RemoveAllListeners();
                    GemsButton2.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
                        ShowNextAdGems(id, gems, counter - 1);
                    }));
                    GemsButton2.gameObject.GetComponentsInChildren<Text>()[0].text = $"({counter - 1}) бесплатно";
                    ShowOneAd();
                    break;
                case 3:
                    GemsButton3.onClick.RemoveAllListeners();
                    GemsButton3.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
                        ShowNextAdGems(id, gems, counter - 1);
                    }));
                    GemsButton3.gameObject.GetComponentsInChildren<Text>()[0].text = $"({counter - 1}) бесплатно";
                    ShowOneAd();
                    break;
            }
        }
    }

    public void ShowNextAdEnergy(int id, float energy, int counter)
    {
        if (counter == 1)
        {
            EnergyAdd = energy;
            ShowOneAd();
        }
        else
        {
            EnergyAdd = 0;
            switch (id)
            {
                case 1:
                    EnergyButton1.onClick.RemoveAllListeners();
                    EnergyButton1.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
                        ShowNextAdEnergy(id, energy, counter - 1);
                    }));
                    EnergyButton1.gameObject.GetComponentsInChildren<Text>()[0].text = $"({counter - 1}) бесплатно";
                    ShowOneAd();
                    break;
                
            }
        }
    }
    public void ShowNextAdKeys(int id, int keys, int counter)
    {
        if (counter == 1)
        {
            KeysAdd = keys;
            ShowOneAd();
        }
        else
        {
            KeysAdd = 0;
            switch (id)
            {
                case 1:
                    KeysButton1.onClick.RemoveAllListeners();
                    KeysButton1.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
                        ShowNextAdKeys(id, keys, counter - 1);
                    }));
                    EnergyButton1.gameObject.GetComponentsInChildren<Text>()[0].text = $"({counter - 1}) бесплатно";
                    ShowOneAd();
                    break;

            }
        }
    }

    public void BuyEnergyForAds(float energy)
    {
        switch (energy)
        {
            case 86400f:
                AdsCounter = 1;
                break;
            case 194400f:
                AdsCounter = 2;
                break;
        }
        EnergyAdd = energy;
        preserveOther = 10;
        ShowOneAd();
    }

    public void BuyGemsForAds(int gems)
    {
        preserveOther = 10;
        GemsAdd = gems;
        ShowOneAd();

    }
    public void BuyKeysForAds(int keys)
    {
        switch (keys)
        {
            case 1:
                AdsCounter = 1;
                break;
            case 3:
                AdsCounter = 4;
                break;
            
        }
        preserveOther = 10;
        KeysAdd = keys;
        ShowOneAd();

    }
    private int KeysAdd = 0;

    public void ChangeToy(string toy)
    {
        switch (toy)
        {
            case "El":
                El.SetActive(true);
                Star.SetActive(false);
                Brilliant.SetActive(false);
                PlayerPrefs.SetInt("LeverType", 0);
                EventManage.CallOnResourceUpdate("Upgrade");
                break;
            case "Star":
                El.SetActive(false);
                Star.SetActive(true);
                Brilliant.SetActive(false);
                PlayerPrefs.SetInt("LeverType", 1);
                EventManage.CallOnResourceUpdate("Upgrade");
                break;
            case "Brilliant":
                El.SetActive(false);
                Star.SetActive(false);
                Brilliant.SetActive(true);
                PlayerPrefs.SetInt("LeverType", 2);
                EventManage.CallOnResourceUpdate("Upgrade");
                break;
        }
    }

    public void BuyGemsToy(string toy)
    {
        switch (toy)
        {
            case "Star":
                if (PlayerPrefs.GetInt("Gems") < 500000)
                {
                    Toast.Instance.Show("У вас недостаточно кристаллов!");
                    break;
                }

                PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") - 500000);
                StarUpgradesImage.SetActive(true);
                StarChooseButton.GetComponent<Button>().interactable = true;
                StarGemsButton.SetActive(false);
                StarPayButton.SetActive(false);
                StarItem.SetState(true);
                _database.GetReference("users").Child(PlayerPrefs.GetString("AUTH_ID")).Child("StarBought").SetValueAsync(1);
                break;
            case "Brilliant":
                if (PlayerPrefs.GetInt("Gems") < 1000000)
                {
                    Toast.Instance.Show("У вас недостаточно кристаллов!");
                    break;
                }
                PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") - 1000000);
                BrilliantUpgradesImage.SetActive(true);
                BrilliantChooseButton.GetComponent<Button>().interactable = true;
                BrilliantGemsButton.SetActive(false);
                BrilliantPayButton.SetActive(false);
                BrilliantItem.SetState(true);
                _database.GetReference("users").Child(PlayerPrefs.GetString("AUTH_ID")).Child("BrilliantBought").SetValueAsync(1);
                break;

        }
        StarItem.SaveState();
        BrilliantItem.SaveState();
    }

    public void BuyMoneyToy(string toy)
    {
        switch (toy)
        {
            case "Star":
                StarChooseButton.GetComponent<Button>().interactable = true;
                StarGemsButton.SetActive(false);
                StarPayButton.SetActive(false);
                StarItem.SetState(true);
                StarUpgradesImage.SetActive(true);
                _database.GetReference("users").Child(PlayerPrefs.GetString("AUTH_ID")).Child("StarBought").SetValueAsync(1);
                break;
            case "Brilliant":
                BrilliantUpgradesImage.SetActive(true);
                BrilliantChooseButton.GetComponent<Button>().interactable = true;
                BrilliantGemsButton.SetActive(false);
                BrilliantPayButton.SetActive(false);
                BrilliantItem.SetState(true);
                _database.GetReference("users").Child(PlayerPrefs.GetString("AUTH_ID")).Child("BrilliantBought").SetValueAsync(1);
                break;

        }
        StarItem.SaveState();
        BrilliantItem.SaveState();
    }

#if UNITY_IOS
    private string gameId = "3960506";
#elif UNITY_ANDROID
    private string gameId = "3960507";
#endif

    private void Start()
    {
        switch (PlayerPrefs.GetInt("LeverType"))
        {
            case 0:
                LeverAnimator = ElLever.GetComponent<Animator>();
                break;
            case 1:
                LeverAnimator = StarLever.GetComponent<Animator>();
                break;
            case 2:
                LeverAnimator = BrilliantLever.GetComponent<Animator>();
                break;
        }

        GemsButton0.onClick.RemoveAllListeners();
        GemsButton0.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
            ShowNextAdGems(0, 1000, 1);
        }));
        GemsButton1.onClick.RemoveAllListeners();
        GemsButton1.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
            ShowNextAdGems(1, 10000, 2);
        }));
        GemsButton1.gameObject.GetComponentsInChildren<Text>()[0].text = $"({2}) бесплатно";
        GemsButton2.onClick.RemoveAllListeners();
        GemsButton2.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
            ShowNextAdGems(2, 100000, 3);
        }));
        GemsButton2.gameObject.GetComponentsInChildren<Text>()[0].text = $"({3}) бесплатно";
        GemsButton3.onClick.RemoveAllListeners();
        GemsButton3.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
            ShowNextAdGems(3, 300000, 5);
        }));
        GemsButton3.gameObject.GetComponentsInChildren<Text>()[0].text = $"({5}) бесплатно";

        EnergyButton0.onClick.RemoveAllListeners();
        EnergyButton0.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
            ShowNextAdEnergy(0, 86400f, 1);
        }));
        EnergyButton1.gameObject.GetComponentsInChildren<Text>()[0].text = "бесплатно";
        EnergyButton1.onClick.RemoveAllListeners();
        EnergyButton1.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
            ShowNextAdEnergy(1, 194400f, 2);
        }));
        EnergyButton1.gameObject.GetComponentsInChildren<Text>()[0].text = $"({2}) бесплатно";

        KeysButton0.onClick.RemoveAllListeners();
        KeysButton0.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
            ShowNextAdKeys(0, 1, 1);
        }));
        KeysButton1.gameObject.GetComponentsInChildren<Text>()[0].text = "бесплатно";
        KeysButton1.onClick.RemoveAllListeners();
        KeysButton1.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
            ShowNextAdKeys(1, 3, 2);
        }));
        KeysButton1.gameObject.GetComponentsInChildren<Text>()[0].text = $"({2}) бесплатно";
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId);

        StarItem = new ShopObject("Star");
        BrilliantItem = new ShopObject("Brilliant");

        _database = FirebaseDatabase.DefaultInstance;
        /*PlayerPrefs.SetFloat("Energy", 10f);
        PlayerPrefs.SetInt("Gems", 0);*/
        if (StarItem.GetState())
        {
            StarChooseButton.GetComponent<Button>().interactable = true;
            StarGemsButton.SetActive(false);
            StarPayButton.SetActive(false);
            StarUpgrades.SetActive(true);
        } else
        {
            StarChooseButton.GetComponent<Button>().interactable = false;
        }
        if (BrilliantItem.GetState())
        {
            BrilliantChooseButton.GetComponent<Button>().interactable = true;
            BrilliantGemsButton.SetActive(false);
            BrilliantPayButton.SetActive(false);
            BrilliantUpgrades.SetActive(true);
        }
        else
        {
            BrilliantChooseButton.GetComponent<Button>().interactable = false;
        }
    }

    public void CloseWindow()
    {
        ThisWindow.SetActive(false);
    }

    private void Update()
    {
        if (!StarItem.GetState())
        {
            if (PlayerPrefs.GetInt("Gems") < 500000)
            {
                StarGemsButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                StarGemsButton.GetComponent<Button>().interactable = true;
            }
        }
        if (!BrilliantItem.GetState())
        {
            if (PlayerPrefs.GetInt("Gems") < 1000000)
            {
                BrilliantGemsButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                BrilliantGemsButton.GetComponent<Button>().interactable = true;
            }
        }
    }

    private int preserveOther;
    private int GemsAdd = 0;
    private float EnergyAdd = 0;

    public void OnUnityAdsReady(string placementId)
    {
        print("OK");
    }

    public void OnUnityAdsDidError(string message)
    {
        print("Error: " + message);
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {

        if (preserveOther == 10)
        {
            if (showResult == ShowResult.Finished)
            {
                switch (PlayerPrefs.GetInt("LeverType"))
                        {
                            case 0:
                                LeverAnimator = ElLever.GetComponent<Animator>();
                                break;
                            case 1:
                                LeverAnimator = StarLever.GetComponent<Animator>();
                                break;
                            case 2:
                                LeverAnimator = BrilliantLever.GetComponent<Animator>();
                                break;
                        }
                LeverAnimator.SetFloat("Speed", 1f);
                if (GemsAdd != 0)
                {
                    PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") + GemsAdd);
                    EventManage.CallOnResourceUpdate("Gems");
                    GemsButton0.onClick.RemoveAllListeners();
                    GemsButton0.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
                        ShowNextAdGems(0, 1000, 1);
                    }));
                    GemsButton1.gameObject.GetComponentsInChildren<Text>()[0].text = $"({2}) бесплатно";
                    GemsButton1.onClick.RemoveAllListeners();
                    GemsButton1.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
                        ShowNextAdGems(1, 10000, 2);
                    }));
                    GemsButton2.gameObject.GetComponentsInChildren<Text>()[0].text = $"({3}) бесплатно";
                    GemsButton2.onClick.RemoveAllListeners();
                    GemsButton2.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
                        ShowNextAdGems(2, 100000, 3);
                    }));
                    GemsButton3.gameObject.GetComponentsInChildren<Text>()[0].text = $"({5}) бесплатно";
                    GemsButton3.onClick.RemoveAllListeners();
                    GemsButton3.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
                        ShowNextAdGems(3, 300000, 5);
                    }));
                    GemsAdd = 0;
                }
                if (EnergyAdd != 0)
                {
                    PlayerPrefs.SetFloat("Energy", PlayerPrefs.GetFloat("Energy") + EnergyAdd);
                    EventManage.CallOnResourceUpdate("Energy");
                    EnergyButton0.onClick.RemoveAllListeners();
                    EnergyButton0.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
                        ShowNextAdEnergy(0, 86400f, 1);
                    }));
                    EnergyButton1.gameObject.GetComponentsInChildren<Text>()[0].text = $"бесплатно";
                    EnergyButton1.onClick.RemoveAllListeners();
                    EnergyButton1.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
                        ShowNextAdEnergy(1, 54f * 60f * 60f , 2);
                    }));
                    EnergyButton1.gameObject.GetComponentsInChildren<Text>()[0].text = $"({2}) бесплатно";
                    EnergyAdd = 0;
                }
                if (KeysAdd != 0)
                {
                    PlayerPrefs.SetInt("Keys", PlayerPrefs.GetInt("Keys") + KeysAdd);
                    EventManage.CallOnResourceUpdate("Keys");
                    KeysButton0.onClick.RemoveAllListeners();
                    KeysButton0.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
                        ShowNextAdKeys(0, 1, 1);
                    }));
                    KeysButton0.gameObject.GetComponentsInChildren<Text>()[0].text = $"бесплатно";
                    KeysButton1.onClick.RemoveAllListeners();
                    KeysButton1.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
                        ShowNextAdKeys(1, 3, 2);
                    }));
                    KeysButton1.gameObject.GetComponentsInChildren<Text>()[0].text = $"({2}) бесплатно";
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
            preserveOther = 0;
        }
    }

    private string placementIdVideo = "rewardedVideo";
    private int AdsCounter;



    public void ShowOneAd()
    {
        if (Advertisement.IsReady(placementIdVideo))
        {
            preserveOther = 10;
            Advertisement.Show(placementIdVideo);
        }
        else
        {
            print("No video Available");
        }
    }

}

class ShopObject
{
    private bool isBought;
    private string Name;

    public ShopObject(string NameObject)
    {
        Name = NameObject;
        if (PlayerPrefs.GetInt(NameObject+"Bought") == 0)
        {
            isBought = false;
        }
        else
        {
            isBought = true;
        }
    }

    public void SetState(bool State)
    {
        isBought = State;
    }

    public bool GetState()
    {
        return isBought;
    }

    public void SaveState()
    {
        if (isBought)
        {
            PlayerPrefs.SetInt(Name + "Bought", 1);
        }
        else
        {
            PlayerPrefs.SetInt(Name + "Bought", 0);
        }
    }
}

