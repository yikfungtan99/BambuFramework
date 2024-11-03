using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError($"{typeof(T)} initialization failed!");
                return null;
            }
            return _instance;
        }
    }

    public static bool Exist
    {
        get
        {
            return _instance != null;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            Debug.LogError($"{gameObject}: {typeof(T)} already exists!");
        }
    }
}
