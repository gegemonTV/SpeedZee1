using Firebase.Database;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ListCreator : MonoBehaviour
{
    FirebaseDatabase _database;
    Dictionary<string, object> values = null;
    
    [SerializeField]
    private Transform SpawnPoint = null;

    [SerializeField]
    private GameObject item = null;

    [SerializeField]
    private RectTransform content = null;

    [SerializeField]
    private int numberOfItems = 3;

    public bool isStartDrawing = false;
    public ICollection<string> itemNames = null;

    private string[] Names = null;

    private List<GameObject> SpawnedItems = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        _database = FirebaseDatabase.DefaultInstance;

    }

    void OnEnable()
    {
        _database.GetReference("users").Child(PlayerPrefs.GetString("AUTH_ID")).Child("GetRequests").OrderByValue().ValueChanged += HandleValueChanged;
    }

    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        foreach ( var Items in SpawnedItems)
        {
            Destroy(Items);
        }

        SpawnedItems.Clear();
        //print(SpawnedItems);
        values = (Dictionary<string, object>) args.Snapshot.Value;
        print(values);

        Names = values.Keys.ToArray<string>();
        print(Names);
        content.sizeDelta = new Vector2(0, Names.Length * 160);

        for (int i = 0; i < Names.Length; i++)
        {
            float spawnY = i * 155;
            //newSpawn Position
            Vector3 pos = new Vector3(SpawnPoint.position.x, -spawnY, 0);
            //instantiate item
            GameObject SpawnedItem = Instantiate(item, pos, SpawnPoint.rotation) as GameObject;
            SpawnedItems.Add(SpawnedItem);
            //setParent
            SpawnedItem.transform.SetParent(SpawnPoint, false);
            //get ItemDetails Component
            ItemDetails itemDetails = SpawnedItem.GetComponent<ItemDetails>();
            //set name
            itemDetails.Nick.text = Names[i];
            itemDetails.Cash.text = values[Names[i]].ToString();

            var vlue = 0; 
            args.Snapshot.Reference.Parent.Child(Names[i]).Child("gems").GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted)
                {
                    print("SMTH WENT WROING");
                }
                if (task.IsCompleted)
                {
                    DataSnapshot snap = task.Result;
                    vlue = (int) snap.Value;
                }
            });
            print(vlue);

            int I = i;
            /*itemDetails.AcceptButton.onClick.AddListener(() => {
                PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") - System.Convert.ToInt32(values[Names[I]]));
                args.Snapshot.Reference.Parent.Child(Names[I]).Child("gems").SetValueAsync(vlue + System.Convert.ToInt32(values[Names[I]]));
                args.Snapshot.Reference.Child(Names[I]).SetValueAsync(null);
                
            });*/

            /*itemDetails.RemoveFromList.onClick.AddListener(() =>
            {
                args.Snapshot.Reference.Child(Names[I]).SetValueAsync(null);
            });*/
        }
    }
   
    void OnDisable()
    {
        _database.GetReference("users").Child(PlayerPrefs.GetString("AUTH_ID")).Child("GetRequests").OrderByValue().ValueChanged -= HandleValueChanged;
    }
}