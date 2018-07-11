using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Follow : MonoBehaviour {

    public GameObject Target;
    private Vector2 _previousPosition;
    private LinkedList<Vector2> _movements = new LinkedList<Vector2>();
    private int _movementCount;

	// Use this for initialization
	void Start () {
        // Record one second's worth of movements.
        _movementCount = 0;
        _previousPosition = Target.transform.position;
        StartCoroutine(SampleFramerate());
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(_movementCount);
        _movements.AddLast((Vector2)Target.transform.position - _previousPosition);
        // Don't move while the framerate is being calculated and initial movements are being
        // queued up.
        if (_movementCount == 0 || _movements.Count < _movementCount)
        {
            // Keep tracking previous position during the execution of SampleFramerate()
            _previousPosition = Target.transform.position;
            return;
        }
        var movement = _movements.First.Value;
        _movements.RemoveFirst();
        transform.Translate(movement);
        _previousPosition = Target.transform.position;
	}

    private IEnumerator SampleFramerate()
    {
        const int sampleCount = 10;
        // Defines the fraction of a second movement delay that followers have 
        // compared with their target
        const float fractionOfSecond = 0.4f;
        var samples = new int[sampleCount];
        for(int i = 0; i < sampleCount; i++)
        {
            samples[i] = Mathf.RoundToInt(1f / Time.deltaTime);
            Debug.Log(samples[i]);
            yield return null;
        }
        _movementCount = Mathf.RoundToInt((samples.Sum() / sampleCount) * fractionOfSecond);
    }
}
