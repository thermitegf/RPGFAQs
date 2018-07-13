using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData : MonoBehaviour {
    public bool RequirePlayer;
    public bool RequirePauseMenu;
    public bool RequireInventory;
    public bool RequireDialogue;
    public GameObject Canvas;

    private void Awake()
    {
        FindObjectOfType<SceneInitializer>().Initialize(this);
    }
}
