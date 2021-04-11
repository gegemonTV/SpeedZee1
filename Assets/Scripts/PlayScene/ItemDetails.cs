
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;

public class ItemDetails : MonoBehaviour
{


    public Text Nick = null;
    public Text Cash = null;

    public void swipeLeftEvent()
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(PlayerPrefs.GetString("AUTH_ID")).Child("GetRequests").Child(Nick.text).RemoveValueAsync();
    }

    public void onEndPos()
    {
        transform.localPosition = new Vector3(0, transform.localPosition.y);
    }

    public void swipeRightEvent()
    {
        if (PlayerPrefs.GetInt("Gems") < System.Convert.ToInt64(Cash.text))
        {
            Toast.Instance.Show("У вас неостаточно средств!");
            
        }
        else
        {
            
            FirebaseDatabase.DefaultInstance.GetReference("users").Child(Nick.text).Child("gems").GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Toast.Instance.Show("Попробуйте позже");
                }
                else
                {
                    long res = (long) task.Result.Value;
                    FirebaseDatabase.DefaultInstance.GetReference("users").Child(Nick.text).Child("gems").SetValueAsync(res + System.Convert.ToInt32(Cash.text));
                    PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") - System.Convert.ToInt32(Cash.text));
                    EventManage.CallOnResourceUpdate("Gems");
                }
            });
            
            FirebaseDatabase.DefaultInstance.GetReference("users").Child(PlayerPrefs.GetString("AUTH_ID")).Child("GetRequests").Child(Nick.text).RemoveValueAsync();

        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
