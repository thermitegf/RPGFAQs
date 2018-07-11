using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHider : MonoBehaviour {

    public GameObject TabMenu;

	// Use this for initialization
	void Start () {
        TabMenu.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonUp(InputConstants.INV_AXIS))
        {
            if (!Pauser.IsPaused)
            {
                Pauser.Pause(this);
                TabMenu.SetActive(true);
            }
            else
            {
                // Only close the menu and unpause if this menu is the only thing causing the game to pause.
                if (Pauser.PauseLocks.Count == 1 && Pauser.PauseLocks.Contains(this))
                {
                    Pauser.Unpause(this);
                    TabMenu.SetActive(false);
                }
            }
        }

	}
}
