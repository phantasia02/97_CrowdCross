using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    const string NAME_FORMAT = "{0} Singleton";

    static T sharedInstance;

    public static T SharedInstance
    {
        get
        {
            if (sharedInstance == null)
            {
                sharedInstance = FindOrCreateInstance();
            }
            return sharedInstance;
        }
    }

    public static T FindOrCreateInstance()
    {
        var instanceType = typeof(T);

        var instances = (T[])FindObjectsOfType(instanceType);
        if (instances.Length > 1)
        {
            Debug.LogError("Error");
        }

        T instance;
        if (instances.Length == 0)
        {
            //var name = string.Format(NAME_FORMAT, instanceType);
            //instance = new GameObject(name, instanceType).GetComponent<T>();
            instance = null;
        }
        else
        {
            instance = instances[0];
        }

        sharedInstance = instance;

        return instance;
    }
}
