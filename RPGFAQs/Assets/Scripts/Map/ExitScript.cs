using Assets;
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour {

    public string SceneName;
    public Vector2 Destination;
    public bool ActivateOnWalk;
    private GameObject _player;
    private bool _playerColliding;
    private bool _alreadyLoading = false;
    private SceneInitializer _scenes;
    private const string PLAYER_TAG = "Player";

	// Use this for initialization
	void Start () {
        _player = FindObjectOfType<StablePlayerMovement>().gameObject;
        _scenes = FindObjectOfType<SceneInitializer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (_playerColliding && !_alreadyLoading && !Pauser.IsPaused)
        {
            if(ActivateOnWalk)
            {
                _alreadyLoading = true;
                _scenes.LoadScene(SceneName, Destination, true);
            }
            else if(Input.GetButtonUp(InputConstants.SUBMIT_AXIS))
            {
                _alreadyLoading = true;
                _scenes.LoadScene(SceneName, Destination, true);
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == PLAYER_TAG)
        {
            _playerColliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == PLAYER_TAG)
        {
            _playerColliding = false;
        }
    }
}
