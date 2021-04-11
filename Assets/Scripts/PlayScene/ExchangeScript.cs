using Firebase.Database;
using UnityEngine;

public class ExchangeScript : MonoBehaviour
{
    FirebaseDatabase _database;

    public GameObject ThisWindow;

    public GameObject RedPoint1;



    private string ToastString;

    public string CashText, InputName;

    private bool isToast;

    // Start is called before the first frame update
    void Start()
    {
        _database = FirebaseDatabase.DefaultInstance;

    }

    void OnEnable()
    {
        _database.GetReference("users").Child(PlayerPrefs.GetString("AUTH_ID")).Child("GetRequests").ChildAdded += ShowNotification;
    }
    // Update is called once per frame
    void Update()
    {
        if (isToast)
        {
            Toast.Instance.Show(ToastString);
            isToast = false;
        }
    }

    void OnDisable()
    {
        _database.GetReference("users").Child(PlayerPrefs.GetString("AUTH_ID")).Child("GetRequests").ChildAdded -= ShowNotification;
    }

    public void CloseWindow()
    {
        ThisWindow.SetActive(false);
        PlayerPrefs.SetInt("Paused", 0);
       
    }

    public void SetNick(string Nick)
    {
        
        InputName = Nick;
    }

    public void SetCash(string Cash)
    {
        
        CashText = Cash;
    }

   
    public void SendGems()
    {
        int cash = System.Convert.ToInt32(CashText);
        if (cash >= PlayerPrefs.GetInt("Gems") || cash <= 0)
        {
            ToastString = "Введите другое количество кристаллов!";
            isToast = true;
        }
        else
        {
            _database.GetReference("users").Child(InputName).Child("gems").GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    print("SMTH WENT WROING");
                }
                if (task.IsCompleted)
                {
                    DataSnapshot snap = task.Result;
                    if (!snap.Exists)
                    {
                        ToastString = "Нет такого пользователя!";
                        isToast = true;
                        print("Snap dont exists");
                    }
                    else
                    {
                        int value = System.Convert.ToInt32(snap.Value.ToString());
                        print(value);
                        _database.GetReference("users").Child(InputName).Child("gems").SetValueAsync(value + cash);
                        PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") - cash);
                    }
                }
            });
        }
    }

    public void RequestGems()
    {
        int cash = System.Convert.ToInt32(CashText);
        _database.GetReference("users").Child(InputName).Child("gems").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                print("SMTH WENT WROING");
            }
            if (task.IsCompleted)
            {
                DataSnapshot snap = task.Result;
                if (!snap.Exists)
                {
                    ToastString = "Нет такого пользователя!";
                    isToast = true;
                    print("Snap dont exists");

                }
                else
                {

                    _database.GetReference("users").Child(InputName).Child("GetRequest").SetValueAsync(0);
                    _database.GetReference("users").Child(InputName).Child("GetRequest").Child(PlayerPrefs.GetString("AUTH_ID")).SetValueAsync(cash);
                }
            }
        });
    }

    void ShowNotification(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        RedPoint1.SetActive(true);

        if (PlayerPrefs.GetInt("Vibration") == 1)
        {
            Handheld.Vibrate();
        }
    }

}
