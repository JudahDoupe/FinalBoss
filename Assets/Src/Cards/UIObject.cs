using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Quaternion = UnityEngine.Quaternion;

public class UIObject : MonoBehaviour
{
    private Vector3 _targetPos;
    private Quaternion _targetRot;

	void Update () {
	    transform.localPosition = Vector3.Lerp(transform.localPosition,_targetPos, Time.deltaTime * 5);
	    transform.localRotation = Quaternion.RotateTowards(transform.localRotation, _targetRot, Time.deltaTime * 100);
    }

    public void MoveTo(Vector3 position, Vector3 eulerAngle)
    {
        var rotation = Quaternion.Euler(eulerAngle);
        if (Vector3.Distance(position, _targetPos) < 0.00001f &&
            Quaternion.Angle(rotation, _targetRot) < 0.00001f) return;

        _targetPos = position;
        _targetRot = rotation;
    }

    public void SnapTo(Vector3 position, Vector3 eulerAngle)
    {
        MoveTo(position, eulerAngle);
        transform.localPosition = _targetPos;
        transform.localRotation = _targetRot;
    }

    public void SetParent(Transform parent)
    {
        transform.parent = parent;
    }
}
