using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Follow : MonoBehaviour {

    public StablePlayerMovement Target;
    private LinkedList<Vector2> _movements = new LinkedList<Vector2>();
    private int _movementCount;

	// Use this for initialization
	void Start () {
        // Record one second's worth of movements.
        _movementCount = Mathf.RoundToInt(1f / Time.deltaTime);
        StartCoroutine(SampleFramerate());
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(_movementCount);
        _movements.AddLast(Target.PreviousMovement);
        if (_movements.Count < _movementCount)
        {
            return;
        }
        var movement = _movements.First.Value;
        _movements.RemoveFirst();
        transform.Translate(movement);
	}

    private IEnumerator SampleFramerate()
    {
        const int sampleCount = 10;
        // Defines the fraction of a second movement delay that followers have 
        // compared with their target
        const float fractionOfSecond = 0.3f;
        int[] samples = new int[sampleCount];
        for(int i = 0; i < sampleCount; i++)
        {
            samples[i] = Mathf.RoundToInt(1f / Time.deltaTime);
            Debug.Log(samples[i]);
            yield return null;
        }
        _movementCount = Mathf.RoundToInt((samples.Sum() / sampleCount) * fractionOfSecond);
    }
}
