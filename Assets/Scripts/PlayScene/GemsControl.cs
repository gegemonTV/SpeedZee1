using Firebase.Database;
using UnityEngine;

public class GemsControl : MonoBehaviour
{
    private DatabaseReference _reference;
    // Start is called before the first frame update
    void Start()
    {
        _reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void IncreaseGems(string loopEnded)
    {
        _reference.Child("users").Child(PlayerPrefs.GetString("AUTH_ID")).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                print("Faulted");
            }
            if (task.IsCompleted)
            {
                PlayerPrefs.SetInt("Gems", System.Convert.ToInt32(task.Result.Value));
            }
        });
        if (loopEnded == "true")
        {
            switch (PlayerPrefs.GetInt("LeverType"))
            {
                case 0:
                    switch (PlayerPrefs.GetInt("ZeroUpgrades"))
                    {
                        case 0:
                            PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") + 1 * PlayerPrefs.GetInt("VIP"));
                            break;
                        case 1:
                            PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") + 2 * PlayerPrefs.GetInt("VIP"));
                            break;
                        case 2:
                            PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") + 3 * PlayerPrefs.GetInt("VIP"));
                            break;
                        case 3:
                            PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") + 4 * PlayerPrefs.GetInt("VIP"));
                            break;
                        case 4:
                            PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") + 5 * PlayerPrefs.GetInt("VIP"));
                            break;
                    }
                    break;
                case 1:
                    switch (PlayerPrefs.GetInt("OneUpgrades"))
                    {
                        case 0:
                            PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") + 3 * PlayerPrefs.GetInt("VIP"));
                            break;
                        case 1:
                            PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") + 4 * PlayerPrefs.GetInt("VIP"));
                            break;
                        case 2:
                            PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") + 5 * PlayerPrefs.GetInt("VIP"));
                            break;
                        case 3:
                            PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") + 6 * PlayerPrefs.GetInt("VIP"));
                            break;
                        case 4:
                            PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") + 7 * PlayerPrefs.GetInt("VIP"));
                            break;
                    }
                    break;
                case 2:
                    switch (PlayerPrefs.GetInt("TwoUpgrades"))
                    {
                        case 0:
                            PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") + 5 * PlayerPrefs.GetInt("VIP"));
                            break;
                        case 1:
                            PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") + 6 * PlayerPrefs.GetInt("VIP"));
                            break;
                        case 2:
                            PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") + 7 * PlayerPrefs.GetInt("VIP"));
                            break;
                        case 3:
                            PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") + 8 * PlayerPrefs.GetInt("VIP"));
                            break;
                        case 4:
                            PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") + 9 * PlayerPrefs.GetInt("VIP"));
                            break;
                    }
                    break;
            }
            EventManage.CallOnResourceUpdate("Gems");
            _reference.Child("users").Child(PlayerPrefs.GetString("AUTH_ID")).Child("gems").SetValueAsync(PlayerPrefs.GetInt("Gems"));
        }
    }
}
