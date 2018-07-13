using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSource : MonoBehaviour {

    public TextAsset DialogueFile;
    private Animator _animator;
    private DialogueManager _dialogueManager;
    private Camera _camera;

	// Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
        _dialogueManager = FindObjectOfType<DialogueManager>();
        _dialogueManager.RegisterTalkable(this);
        _camera = Camera.main;
	}
	
	public void SetTalkable(bool isTalkable)
    {
        _animator.SetBool("CanSpeak", isTalkable);
    }

    public void InitiateDialogue(StablePlayerMovement speaker)
    {
        int screenHeight = _camera.pixelHeight;
        // Don't render the dialogue window on the top half of the screen if the speech bubble 
        // is also on the top half.
        var onTopHalf = _camera.WorldToScreenPoint(transform.position).y > screenHeight / 2;
        //Debug.Log($"{screenHeight}, {_camera.WorldToScreenPoint(transform.position).y}");
        _dialogueManager.StartDialogue(DialogueFile, 
                                       !onTopHalf, 
                                       () =>
                                       {
                                           speaker.IsTalking = false;
                                           // Yeah, I know. This probably uses reflection or some shit.
                                           speaker.StartCoroutine("SpeechCooldown");
                                       });
    }
}
