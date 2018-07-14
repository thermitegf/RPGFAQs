using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Selectable FirstSelectedButton;
    private SceneInitializer scenes;

    public void Start()
    {
        scenes = FindObjectOfType<SceneInitializer>();
        FirstSelectedButton.Select();
    }

    public void StartGame()
    {
        scenes.LoadScene("PrisonYard", Vector2.zero);
    }
}
