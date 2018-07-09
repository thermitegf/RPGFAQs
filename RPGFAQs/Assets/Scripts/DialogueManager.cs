using Assets;
using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using Yarn;
using Yarn.Unity;

[System.Serializable]
public class DialogueManager : DialogueUIBehaviour
{

    void Start()
    {
        if (_dialogueWindow == null)
        {
            _dialogueWindow = GameObject.Find("LightBackground");
            _dialogueLoader = GameObject.Find("DialogueLoader").GetComponent<DialogueRunner>();
            //_dialogueAssets = new ResourceUtilities().GetAssetBundle("dialogue");
            //_dialogueLoader.AddScript(ErrorHandler);
            _windowTransform = _dialogueWindow.GetComponent<RectTransform>();
            _nameMesh = GameObject.Find("Name").GetComponent<TextMeshProUGUI>();
            _nameMesh.text = null;
            _dialogueMesh = GameObject.Find("Dialogue").GetComponent<TextMeshProUGUI>();
            _dialogueMesh.text = null;
            ToAnimate.ForEach((p) => _animators.Add(p.GetComponent<Animator>()));
        }
    }

    private List<DialogueSource> _talkables = new List<DialogueSource>();
    //private AssetBundle _dialogueAssets;
    private Dictionary<string, TextAsset> _dialogueCache = new Dictionary<string, TextAsset>();
    private DialogueSource _currentlyActivatedTalkable;
    private DialogueRunner _dialogueLoader;
    private GameObject _dialogueWindow;
    private RectTransform _windowTransform;
    private TextMeshProUGUI _nameMesh;
    private TextMeshProUGUI _dialogueMesh;
    private List<Animator> _animators = new List<Animator>();
    private bool _displayOnTop = false;
    private Action _onDialogueExit;

    [SerializeField]
    public List<GameObject> ToAnimate = new List<GameObject>();
    public TextAsset ErrorHandler;
    public float TextSpeed = 0.03f;

    public void RegisterTalkable(DialogueSource talkable)
    {
        _talkables.Add(talkable);
    }

    public void UnregisterTalkable(DialogueSource talkable)
    {
        _talkables.Remove(talkable);
    }

    public DialogueSource ActivateNearestTalkable(Vector3 position, float talkingRange)
    {
        DialogueSource bestTarget = null;
        // Defines the maximum range at which a talkable may be talked to.
        float closestDistance = talkingRange;
        foreach (DialogueSource talkable in _talkables)
        {
            Vector3 directionToTarget = talkable.transform.root.position - position;
            //Debug.Log(directionToTarget);
            float squaredDistanceToTarget = directionToTarget.sqrMagnitude;
            if (squaredDistanceToTarget < closestDistance)
            {
                closestDistance = squaredDistanceToTarget;
                bestTarget = talkable;
            }
        }
        if (bestTarget == null)
        {
            _currentlyActivatedTalkable?.SetTalkable(false);
            _currentlyActivatedTalkable = null;
        }
        else
        {
            if (_currentlyActivatedTalkable != bestTarget)
            {
                _currentlyActivatedTalkable?.SetTalkable(false);
            }
            bestTarget.SetTalkable(true);
            _currentlyActivatedTalkable = bestTarget;
        }
        // Uncomment to see where the selected talkable is in relation to the given position
        //Debug.Log($"Talkables count: {_talkables.Count}, Best target distance: {closestDistance}, Current Talkable: {_currentlyActivatedTalkable?.transform?.position}");
        return _currentlyActivatedTalkable;
    }

    public void StartDialogue(TextAsset dialogueFile, bool displayOnTop, Action onDialogueExit)
    {
        if (dialogueFile == null)
        {
            // Here we fall back on the ErrorHandler message in the event that no dialogueFile was provided.
            dialogueFile = ErrorHandler;
        }
        _onDialogueExit = onDialogueExit;
        _displayOnTop = displayOnTop;
        if (!_dialogueCache.ContainsKey(dialogueFile.name))
        {
            _dialogueCache.Add(dialogueFile.name, dialogueFile);
            _dialogueLoader.AddScript(dialogueFile);
        }
        // By convention, all Yarn start nodes will be the file name (minus extension) plus "Start".
        _dialogueLoader.StartDialogue(dialogueFile.name + "Start");
    }

    //private TextAsset LoadTextAsset(string fileName)
    //{
    //    if (_dialogueCache.ContainsKey(fileName))
    //    {
    //        return _dialogueCache[fileName];
    //    }
    //    else
    //    {
    //        var extensionAdded = fileName + ".json";
    //        TextAsset asset = _dialogueAssets.LoadAsset<TextAsset>(extensionAdded);
    //        _dialogueCache.Add(fileName, asset);
    //        _dialogueLoader.AddScript(asset);
    //        return asset;
    //    }
    //}

    public override IEnumerator DialogueStarted()
    {
        _animators.ForEach(p => p.SetBool("IsShown", true));
        Vector3 currentPosition = _windowTransform.anchoredPosition;
        if (_displayOnTop)
        {
            currentPosition.y = 475f;
        }
        else
        {
            currentPosition.y = 0f;
        }
        _windowTransform.anchoredPosition = currentPosition;
        yield break;
    }

    public override IEnumerator RunLine(Line line)
    {
        var spaceIndex = line.text.IndexOf(' ');
        _nameMesh.text = line.text.Substring(0, spaceIndex - 1);
        var dialogue = line.text.Substring(spaceIndex + 1);

        // Wait a frame for Input.GetButtonUp to change
        yield return null;

        StringBuilder sb = new StringBuilder();
        var interval = TextSpeed;
        var counter = 0f;
        foreach (char c in dialogue)
        {
            while (counter < interval)
            {
                if (Input.GetButtonUp(InputConstants.SUBMIT_AXIS))
                {
                    _dialogueMesh.text = dialogue;
                    break;
                }
                counter += Time.deltaTime;
                yield return null;
            }
            counter = 0f;
            sb.Append(c);
            _dialogueMesh.text = sb.ToString();
        }
        // Wait a frame for Input.GetButtonUp to change
        yield return null;
        while (!Input.GetButtonUp(InputConstants.SUBMIT_AXIS))
        {
            yield return null;
        }
    }

    public override IEnumerator RunOptions(Options optionsCollection, OptionChooser optionChooser)
    {
        yield break;
    }

    public override IEnumerator RunCommand(Command command)
    {
        Debug.Log("Command: " + command.text);
        yield break;
    }

    public override IEnumerator DialogueComplete()
    {
        _animators.ForEach(p => p.SetBool("IsShown", false));
        _onDialogueExit?.Invoke();
        _onDialogueExit = null;
        yield break;
    }
}