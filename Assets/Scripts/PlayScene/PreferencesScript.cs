using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

public class PreferencesScript : MonoBehaviour
{
    public GameObject AudioPlayer;
    public GameObject PreferencesWindow;
    public GameObject MutedBtn;
    public GameObject VibrationBtn;

    public Sprite redButton;
    public Sprite greenButton;

   
    private AudioSource mAudio;
    // Start is called before the first frame update
    void Start()
    {
        mAudio = AudioPlayer.GetComponent<AudioSource>();
        mAudio.volume = PlayerPrefs.GetFloat("Volume");
        mAudio.Play();
        if (PlayerPrefs.GetFloat("Volume") == 0f)
        {
            MutedBtn.GetComponent<Image>().sprite = redButton;
        } 
        else
        {
            MutedBtn.GetComponent<Image>().sprite = greenButton;
        }
        if (PlayerPrefs.GetInt("Vibration") == 1)
        {
            VibrationBtn.GetComponent<Image>().sprite = greenButton;
        }
        else
        {
            VibrationBtn.GetComponent<Image>().sprite = redButton;
        }
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    

    public void MuteUnmute()
    {
        if (mAudio.volume == 1f)
        {
            PlayerPrefs.SetFloat("Volume", 0f);
            mAudio.volume = 0f;
            mAudio.Play();
            MutedBtn.GetComponent<Image>().sprite = redButton;
        }
        else
        {
            PlayerPrefs.SetFloat("Volume", 1f);
            mAudio.volume = 1f;
            mAudio.Play();
            MutedBtn.GetComponent<Image>().sprite = greenButton;
        }
        PlayerPrefs.Save();
    }

    public void Vibration()
    {
        if (PlayerPrefs.GetInt("Vibration") == 1)
        {
            VibrationBtn.GetComponent<Image>().sprite = redButton;
            PlayerPrefs.SetInt("Vibration", 0);
        }
        else
        {
            VibrationBtn.GetComponent<Image>().sprite = greenButton;
            PlayerPrefs.SetInt("Vibration", 1);
        }
        PlayerPrefs.Save();
    }

    public void ClosePreferences()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("Paused", 0);
        PreferencesWindow.SetActive(false);
        PlayerPrefs.Save();
    }

    public void openVK()
    {
        Process.Start("vk://vk.com/apleeks_company");
    }

    public void openInst()
    {
        Process.Start("instagram://user?username=@speedzee_game");
    }
}

