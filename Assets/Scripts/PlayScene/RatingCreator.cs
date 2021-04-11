using Firebase.Database;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatingCreator : MonoBehaviour
{
    FirebaseDatabase _database;
    //var values = null;

    public GameObject ThisWindow;

    [SerializeField]
    private Transform SpawnPoint = null;

    [SerializeField]
    private GameObject item = null;

    [SerializeField]
    private RectTransform content = null;

    [SerializeField]
    private Sprite[] Places;

    public bool isStartDrawing = false;
    public ICollection<string> itemNames = null;

    private string[] Names = null;

    public Text NickText;
    public Text GemsText;

    private List<GameObject> SpawnedItems = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        _database = FirebaseDatabase.DefaultInstance;

        NickText.text = PlayerPrefs.GetString("AUTH_ID");
    }

    void OnEnable()
    {
        _database.GetReference("users").OrderByChild("gems").LimitToLast(10).ValueChanged += HandleValueChanged;
    }

    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        foreach (var Items in SpawnedItems)
        {
            Destroy(Items);
        }
        SpawnedItems.Clear();

        var c = 0;

        Stack<string> nick = new Stack<string>();
        Stack<long> gems = new Stack<long>();

        print(args.Snapshot.Children);

        foreach (DataSnapshot leader in args.Snapshot.Children)
        {
            if (leader.Key == "gems")
            {
                continue;
            }
            print(leader.Key);// + ": " + leader.Child("gems").Value.ToString());
            nick.Push(leader.Key);
            print(nick.Peek());
            gems.Push((long)leader.Child("gems").Value);
            print(gems.Peek());
        }

        content.sizeDelta = new Vector2(0, args.Snapshot.ChildrenCount * 225);
        foreach (DataSnapshot leader in args.Snapshot.Children)
        {
            if (leader.Key == "gems")
            {
                continue;
            }
            float spawnY = c * 215;
            //newSpawn Position
            Vector3 pos = new Vector3(SpawnPoint.position.x, -spawnY, 0);
            //instantiate item
            GameObject SpawnedItem = Instantiate(item, pos, SpawnPoint.rotation) as GameObject;
            SpawnedItems.Add(SpawnedItem);
            //setParent
            SpawnedItem.transform.SetParent(SpawnPoint, false);
            //get ItemDetails Component
            PlayerItemDetails itemDetails = SpawnedItem.GetComponent<PlayerItemDetails>();
            
            itemDetails.PlayerName.text = nick.Pop();
            itemDetails.GemsCounter.text = gems.Pop().ToString();
            itemDetails.PlaceImg.sprite = Places[c];
            //print(ey + ": " + leader.Child("gems").Value.ToString());
            c += 1;
            if (c == 10)
            {
                break;
            }
        }
    }

    public void CloseWindow()
    {
        ThisWindow.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnDisable()
    {
        _database.GetReference("users").OrderByChild("gems").LimitToLast(10).ValueChanged -= HandleValueChanged;
    }
}
