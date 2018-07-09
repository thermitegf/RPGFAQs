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
            TabMenu.SetActive(!TabMenu.activeSelf);
        }
	}
}
