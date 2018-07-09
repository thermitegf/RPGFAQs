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
            Pauser.SetPause(value, this);
            _menuOpened = value;
        }
    }
	// Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonUp(InputConstants.MENU_AXIS))
        {
            MenuOpened = !MenuOpened;
        }
	}
}
