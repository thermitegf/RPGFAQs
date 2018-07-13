﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    private SceneInitializer scenes;

    public void Start()
    {
        scenes = FindObjectOfType<SceneInitializer>();
    }

    public void StartGame()
    {
        scenes.LoadScene("Overworld", Vector2.zero);
    }
}
