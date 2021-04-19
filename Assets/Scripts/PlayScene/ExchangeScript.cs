using Firebase.Database;
using UnityEngine;

public class ExchangeScript : MonoBehaviour   // скрипт для реализации обмена между игроками
{
    FirebaseDatabase _database;              // используется база данных Firebase

    public GameObject ThisWindow;         // окно (нужно для его открытия/закрытия)

    public GameObject RedPoint1;          // красная точка, которая появляется при появлении входящих запросов, на которые нужно ответить



    private string ToastString;           // строка тоста (всплывающее окошко)

    public string CashText, InputName;

    private bool isToast;

    // Start is called before the first frame update
    void Start()
    {
        _database = FirebaseDatabase.DefaultInstance;

    }

    void OnEnable()
    {
        _database.GetReference("users").Child(PlayerPrefs.GetString("AUTH_ID")).Child("GetRequests").ChildAdded += ShowNotification;   // подписка на появление входящих запросов
    }
    // Update is called once per frame
    void Update()
    {
        if (isToast)
        {
            Toast.Instance.Show(ToastString);   // поакзывать всплывающее окошко, если надо
            isToast = false;
        }
    }

    void OnDisable()
    {
        _database.GetReference("users").Child(PlayerPrefs.GetString("AUTH_ID")).Child("GetRequests").ChildAdded -= ShowNotification;  //отписка от события
    }

    public void CloseWindow()       //закрытие окна
    {
        ThisWindow.SetActive(false);         
        PlayerPrefs.SetInt("Paused", 0);    //уже ни на что не влияет
       
    }

    public void SetNick(string Nick)     //обновление ника из строки ввода
    {
        
        InputName = Nick;
    }

    public void SetCash(string Cash)      //обновление кол-ва котправляемых кристаллов из строки ввода
    {
        
        CashText = Cash;
    }

   
    public void SendGems()   //отправка кристаллов
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

    public void RequestGems()        // отправка запроса на получение кристаллов
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
        RedPoint1.SetActive(true);         //показать красную точку

        if (PlayerPrefs.GetInt("Vibration") == 1)     //если вибрация включена
        {
            Handheld.Vibrate();                      // завибрировать
        }
    }

}
