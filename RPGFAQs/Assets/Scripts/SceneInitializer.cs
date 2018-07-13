using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class SceneInitializer : MonoBehaviour {
    public GameObject Player;
    public GameObject PlayerCamera;
    public GameObject Follower;
    public GameObject Canvas;
    public GameObject CanvasEvents;
    public GameObject PauseMenu;
    public GameObject InventoryMenu;
    public GameObject DialogueMenu;
    public GameObject DialogueLoader;

    private Vector2 _exit;

    void Awake()
    {
        // This manager should persist through all scene changes.
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadScene(string sceneName, Vector2? exitPosition)
    {
        if(exitPosition != null)
        {
            _exit = exitPosition.Value;
        }
        SceneManager.LoadScene(sceneName);
    }

    public void Initialize(SceneData data)
    {
        // The canvas is always necessary for something.
        var canvas = SpawnCanvas();
        // Pausing is the most important thing, so it's added as soon as possible.
        SpawnPauseMenu(canvas);
        // Player control is the second most important thing.
        if (data.RequirePlayer)
            SpawnPlayer(_exit);
        if (data.RequireInventory)
            SpawnInventoryMenu(canvas);
        if (data.RequireDialogue)
            SpawnDialogueMenu(canvas);
    }

    GameObject SpawnCanvas()
    {
        var canvas = Instantiate(Canvas);
        Instantiate(CanvasEvents);
        return canvas;
    }

    void SpawnPlayer(Vector2 exit)
    {
        GameObject player;
        player = Instantiate(Player, exit, Quaternion.identity);
        var camera = Instantiate(PlayerCamera, player.transform.position, Quaternion.identity);
        // Set the camera to a position where it can actually see stuff.
        camera.transform.position += new Vector3(0, 0, -10f);
        camera.GetComponent<CameraFollow>().Target = player;
        GameObject previousTarget = player;
        for (int i = 0; i < 3; i++)
        {
            var follower = Instantiate(Follower, player.transform.position, Quaternion.identity);
            follower.GetComponent<Follow>().Target = previousTarget;
            previousTarget = follower;
        }
    }

    void SpawnInventoryMenu(GameObject canvas)
    {
        Instantiate(InventoryMenu, canvas.transform);
    }

    void SpawnPauseMenu(GameObject canvas)
    {
        Instantiate(PauseMenu, canvas.transform);
    }

    void SpawnDialogueMenu(GameObject canvas)
    {
        var menu = Instantiate(DialogueMenu, canvas.transform);
        var loader = Instantiate(DialogueLoader, Vector2.zero, Quaternion.identity);
        loader.GetComponent<DialogueRunner>().dialogueUI = menu.GetComponent<DialogueManager>();
    }
}
