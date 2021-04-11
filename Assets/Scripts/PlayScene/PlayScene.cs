using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayScene : MonoBehaviour
{
    
    private Animator LeverAnimator;

    private int ESecs;
    
    public GameObject El;
    public GameObject Star;
    public GameObject Brilliant;

    //public GameObject VipTime;
    /*
        public Text updT;
        public Text VipT;
        */

    public Scrollbar ShopScroll;

    public GameObject ElLever;
    public GameObject StarLever;
    public GameObject BrilliantLever;

    public GameObject EnergyText;
    public GameObject GemsText;

    public GameObject ShopWindow;
    public GameObject PrefWindow;
    public GameObject VIPWindow;
    public GameObject RatingWindow;
    public GameObject ExchangeWindow;
    public GameObject FortuneWindow;

    public Slider EnergySlider;

    private void Start()
    {

        /* if (PlayerPrefs.HasKey("VipUntil"))
         {
             VipTime.SetActive(true);
         }
         UpdatesTime.SetActive(true);

         */


        switch (PlayerPrefs.GetInt("LeverType")) {
            case 0:
                El.SetActive(true);
                Star.SetActive(false);
                Brilliant.SetActive(false);
                LeverAnimator = ElLever.GetComponent<Animator>();
                break;
            case 1:
                El.SetActive(false);
                Star.SetActive(true);
                Brilliant.SetActive(false);
                LeverAnimator = StarLever.GetComponent<Animator>();
                break;
            case 2:
                El.SetActive(false);
                Star.SetActive(false);
                Brilliant.SetActive(true);
                LeverAnimator = BrilliantLever.GetComponent<Animator>();
                break;
        }

        float enFloat = PlayerPrefs.GetFloat("Energy");
        EnergySlider.value = enFloat / 28800f;
        StartCoroutine(TimeUpdate());
        EventManage.CallOnResourceUpdate("Gems");
        EventManage.CallOnResourceUpdate("Energy");
        EventManage.CallOnResourceUpdate("Upgrade");
        EventManage.CallOnResourceUpdate("Keys");
    }
    
    private void Update()
    { 

        if (PlayerPrefs.GetFloat("Energy") != 0)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (LeverAnimator.GetFloat("Speed") <= 3f)
                {
                    LeverAnimator.SetFloat("Speed", LeverAnimator.GetFloat("Speed") + 0.25f);
                }
            }
            else
            {
                if (LeverAnimator.GetFloat("Speed") > 1f)
                {
                    LeverAnimator.SetFloat("Speed", LeverAnimator.GetFloat("Speed") - 0.2f * Time.deltaTime);
                    print("DECREASESPEED!");
                }
            }
        }
        else
        {
            LeverAnimator.SetFloat("Speed", 0);
        }
    }

    private IEnumerator TimeUpdate()
    {
        while (true)
        {
            if (PlayerPrefs.GetFloat("Energy") - 1f > 0)
            {
                PlayerPrefs.SetFloat("Energy", PlayerPrefs.GetFloat("Energy") - 1f);
            }
            else
            {
                PlayerPrefs.SetFloat("Energy", 0);
            }
            EventManage.CallOnResourceUpdate("Energy");
            yield return new WaitForSeconds(1f);
        }    
    }


    public void VipWindowOpen()
    {
        VIPWindow.SetActive(true);
    }

    public void UpgradeWindowOpen()
    {
        
        ShopWindow.SetActive(true);
        ShopScroll.value = 0.62f;
    }

    public void AddKeysOpen()
    {
        ShopWindow.SetActive(true);
        ShopScroll.value = 0f;
    }

    // в следующем обновлении
    public void AttackScene()
    {

    }

    public void GemAdsWindowOpen()
    {
        
        ShopWindow.SetActive(true);
        ShopScroll.value = 0.03f;
    }

    public void ShopWindowOpen()
    {
        ShopWindow.SetActive(true);
        ShopScroll.value = 1f;
    }

    public void FortuneWindowOpen()
    {
        FortuneWindow.SetActive(true);
    }

    public void ExchangeWindowOpen()
    {
        ExchangeWindow.SetActive(true);
    }

    public void PreferencesWindow()
    {
        PrefWindow.SetActive(true);
    }

    public void EnergyWindowOpen()
    {
        ShopWindow.SetActive(true);
        ShopScroll.value = 0f;
    }

    public void RatingWindowOpen()
    {
        PlayerPrefs.SetInt("Paused", 1);
        RatingWindow.SetActive(true);
    }
    
}
