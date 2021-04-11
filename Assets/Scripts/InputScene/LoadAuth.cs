using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadAuth : MonoBehaviour
{
    private string AuthBefore;
    // Start is called before the first frame update
    void Start()
    {
        AuthBefore = PlayerPrefs.GetString("AUTH_ID", "");
        if (AuthBefore != "")
        {
            SceneManager.LoadScene("PlayScene");
        }
        else
        {
            PlayerPrefs.SetInt("LeverType", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
