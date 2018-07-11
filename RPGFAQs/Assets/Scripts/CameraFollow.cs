using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject Target;
    [Range(0f, 1f)]
    public float DeadZoneScale = 0.7f;
    private Rect _deadZone;
    private Vector2 _previousTargetPosition;
    private const float SAFE_AREA_MAX = 1f;
    

	// Use this for initialization
	void Start () {
        var safeSize = new Vector2(Camera.main.pixelWidth * DeadZoneScale, Camera.main.pixelHeight * DeadZoneScale);
        var cameraPos = Vector3.zero + (Camera.main.WorldToScreenPoint(transform.position) * (SAFE_AREA_MAX - DeadZoneScale));
        _deadZone = new Rect(cameraPos, safeSize);
        _previousTargetPosition = Target.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        var targetPoint = Camera.main.WorldToScreenPoint(Target.transform.position);

        var movement = Vector3.zero;

        // If the target escapes the dead zone, chase it.
        if (targetPoint.x < _deadZone.xMin || targetPoint.x > _deadZone.xMax)
        {
            movement.x = -(_previousTargetPosition.x - Target.transform.position.x);
        }
        if (targetPoint.y < _deadZone.yMin || targetPoint.y > _deadZone.yMax)
        {
            movement.y = -(_previousTargetPosition.y - Target.transform.position.y);
        }
        transform.Translate(movement);
        _previousTargetPosition = Target.transform.position;
    }
}
