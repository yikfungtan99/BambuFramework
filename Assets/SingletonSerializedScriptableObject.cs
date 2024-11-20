using Sirenix.OdinInspector;
using UnityEngine;

namespace BambuFramework
{
    public class SingletonSerializedScriptableObject<T> : SerializedScriptableObject where T : SerializedScriptableObject
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    // Load all assets of type T from the Resources folder
                    T[] assets = Resources.LoadAll<T>("Singleton");

                    if (assets.Length > 0)
                    {
                        instance = assets[0]; // Take the first one
                    }
                    else
                    {
                        Bambu.Log($"No instances of {typeof(T).Name} found in Resources.");
                    }
                }

                return instance;
            }
        }

    }
}
