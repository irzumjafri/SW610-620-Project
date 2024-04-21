using Firebase;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.Events;

public class FirebaseInit : MonoBehaviour
{
    public UnityEvent OnFirebaseInitialized = new UnityEvent();

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                UnityEngine.Debug.LogError($"Failed to initialize Firebase with error: {task.Exception}");
                return;
            }
        });
        UnityEngine.Debug.Log("Firebase initialized");

            OnFirebaseInitialized.Invoke();


    }
}