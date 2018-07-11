using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    private GameObject _pauseMenu;
    private Animator _animator;
    private bool _menuOpened = false;
    public bool MenuOpened
    {
        get { return _menuOpened; }
        private set
        {
            _animator.SetBool("IsPaused", value);
            if (value)
            {
                Pauser.Pause(this);
            }
            else
            {
                Pauser.Unpause(this);
            }
            _menuOpened = value;
        }
    }
	// Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        // Because there's no scenario under which the pause menu should become unavailable, we don't
        // require any checks (e.g. pause state, dialogue state, etc) other than input for the pause menu
        // to function.
        if (Input.GetButtonUp(InputConstants.MENU_AXIS))
        {
            MenuOpened = !MenuOpened;
        }
	}
}
