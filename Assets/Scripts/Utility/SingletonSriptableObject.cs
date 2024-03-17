using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DEVSOC2024
{
    public class SingletonSriptableObject<T> : ScriptableObject where T: SingletonSriptableObject<T>
    {
        private static T singleton;
        public static T Singleton
        {
            get
            {
                if(singleton == null)
                {
                    T[] assets = Resources.LoadAll<T>("");
                    if (assets == null || assets.Length<1)
                    {
                        Debug.LogError("Could not find Singleton Scriptable Object in Resources");

                    }
                    else if(assets.Length>1)
                    {
                        Debug.LogWarning("Multiple Instances of Singleton found");
                    }
                    singleton = assets[0];
                }
                return singleton;
            }
        }
    }
}
