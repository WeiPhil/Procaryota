using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoader : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

}
