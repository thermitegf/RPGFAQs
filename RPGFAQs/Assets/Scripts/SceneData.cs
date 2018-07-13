using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData : MonoBehaviour {
    public bool RequirePlayer = true;
    public bool RequireInventory = true;
    public bool RequireDialogue = true;

    private void Awake()
    {
        FindObjectOfType<SceneInitializer>().Initialize(this);
    }
}
