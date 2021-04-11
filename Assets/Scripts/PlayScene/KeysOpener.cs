using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class KeysOpener : MonoBehaviour, IUnityAdsListener
{
    public Text KeysCounter;

    public GameObject StickersWindow;
    public GameObject YouBoxWindow;
    public GameObject PromoWindow;

    public GameObject StickerLock;
    public GameObject YouBoxLock;
    public GameObject PromoLock;

#if UNITY_IOS
    private string gameId = "3960506";
#elif UNITY_ANDROID
    private string gameId = "3960507";
#endif

    // Start is called before the first frame update
    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId);

        if (!PlayerPrefs.HasKey("Keys"))
        {
            PlayerPrefs.SetInt("Keys", 0);
        }
        if (PlayerPrefs.HasKey("GotPromo"))
        {
            PromoLock.SetActive(false);
        }
        if (PlayerPrefs.HasKey("GotYouBox"))
        {
            YouBoxLock.SetActive(false);
        }
        if (PlayerPrefs.HasKey("GotSticker"))
        {
            StickerLock.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        KeysCounter.text = PlayerPrefs.GetInt("Keys").ToString();
    }

    public void OpenWindow(string window)
    {
        if (PlayerPrefs.GetInt("Keys") <= 0)
        {
            Toast.Instance.Show("Недостаточно ключей!");
            return;
        }

        switch (window)
        {
            case "stickers":
                StartCoroutine(ShowAnimation(StickerLock.GetComponent<Animator>(), StickersWindow, StickerLock));
                PlayerPrefs.SetInt("GotSticker",1);
                break;
            case "youbox":
                StartCoroutine(ShowAnimation(YouBoxLock.GetComponent<Animator>(), YouBoxWindow, YouBoxLock));
                PlayerPrefs.SetInt("GotYouBox", 1);
                break;
            case "promo":
                StartCoroutine(ShowAnimation(PromoLock.GetComponent<Animator>(), PromoWindow, PromoLock));
                PlayerPrefs.SetInt("GotPromo", 1);
                break;
        }
        PlayerPrefs.SetInt("Keys", PlayerPrefs.GetInt("Keys") - 1);
        EventManage.CallOnResourceUpdate("Keys");
    }

    private string ShowAds = "";

    public void ShowStickers()
    {
        ShowAds = "stickers";
        ShowOneAd();
    }

    public void ShowYouBox()
    {
        ShowAds = "youbox";
        ShowOneAd();
    }

    public void ShowPromo()
    {
        ShowAds = "promo";
        ShowOneAd();
    }
    public void CopyText(string textToCopy)
    {
        TextEditor editor = new TextEditor
        {
            text = textToCopy
        };
        editor.SelectAll();
        editor.Copy();
    }
    private string placementIdVideo = "rewardedVideo";
    public void ShowOneAd()
    {
        if (Advertisement.IsReady(placementIdVideo))
        {
            Advertisement.Show(placementIdVideo);
        }
        else
        {
            Toast.Instance.Show("Нет доступных видео");
        }
    }

    private IEnumerator ShowAnimation(Animator anim, GameObject win, GameObject Lock)
    {
        anim.Play("OpenLock");
        float time = anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 + 0.75f;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        Lock.SetActive(false);
        win.SetActive(true);

    }

    public void OnUnityAdsReady(string placementId)
    {

    }

    public void OnUnityAdsDidError(string message)
    {

    }

    public void OnUnityAdsDidStart(string placementId)
    {
        print(200);
    }

    public void CloseWindow(string window)
    {
        switch (window)
        {
            case "promo":
                PromoWindow.SetActive(false);
                break;
            case "youbox":
                YouBoxWindow.SetActive(false);
                break;
            case "stickers":
                StickersWindow.SetActive(false);
                break;
            case "copy":
                PromoCopy.SetActive(false);
                break;
        }
    }

    public GameObject PromoCopy;

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        switch (ShowAds)
        {
            case "stickers":
                Application.OpenURL("https://t.me/addstickers/Speedzee");
                StickersWindow.SetActive(false);
                break;
            case "youbox":
                Application.OpenURL("https://youbox.com.ua/?utm_source=speedzee&utm_medium=app&utm_content=link");
                YouBoxWindow.SetActive(false);
                break;
            case "promo":
                PromoCopy.SetActive(true);
                PromoWindow.SetActive(false);
                break;

        }
        ShowAds = "";
    }
}
