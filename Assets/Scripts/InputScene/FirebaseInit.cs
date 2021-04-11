using UnityEngine;
using UnityEngine.Events;
using Firebase;
using Firebase.Extensions;

public class FirebaseInit : MonoBehaviour
{
    public UnityEvent OnFirebaseInitialized = new UnityEvent();
    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError($"Failed to init Firebase with {task.Exception}");
                return;
            }

            OnFirebaseInitialized.Invoke();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnButton()
    {
    }
}
