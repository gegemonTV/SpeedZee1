using UnityEngine;
using Firebase.Database;

public class FirstStartScript : MonoBehaviour
{
    FirebaseDatabase _database;
    // Start is called before the first frame update
    void Start()
    {
        _database = FirebaseDatabase.DefaultInstance;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
