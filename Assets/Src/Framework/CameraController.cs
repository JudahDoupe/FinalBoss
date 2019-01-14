using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraController : MonoBehaviour {

    public float MoveSpeed = 5;
    public float DragSpeed = 5;
    public float ZoomSpeed = 5;

    private Camera _camera;
    private PostProcessingProfile _postProfile;

    private Vector3 _dragOrigin;
    private Vector3 _cameraOrigin;

    private Vector3 _targetPosition;
    private Vector3 _targetRootPosition;

    void Start()
    {
        _camera = GetComponent<Camera>();
        _targetRootPosition = new Vector3(0,0,0);
        _targetPosition = _targetRootPosition - transform.forward * 1;
        _postProfile = GetComponent<PostProcessingBehaviour>().profile;
    }

    void LateUpdate()
    {
        var move = Drag() || Rotate() || Zoom();
        transform.parent.position = Vector3.Lerp(transform.parent.position, _targetRootPosition, Time.smoothDeltaTime * MoveSpeed);
        transform.localPosition = Vector3.Lerp(transform.localPosition, _targetPosition, Time.smoothDeltaTime * MoveSpeed);

        var depthOfField = _postProfile.depthOfField.settings;
        depthOfField.focusDistance = Vector3.Distance(transform.position, _targetRootPosition);
        depthOfField.aperture = 0.55f * Mathf.Pow(depthOfField.focusDistance,2) - 5.15f * depthOfField.focusDistance + 12.44f;
        _postProfile.depthOfField.settings = depthOfField;
    }

    private bool Drag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _dragOrigin = Input.mousePosition;
            _cameraOrigin = _targetRootPosition;
        }
        if (Input.GetMouseButton(0))
        {
            var speed = DragSpeed * _targetPosition.y;
            Vector3 pos = _camera.ScreenToViewportPoint(Input.mousePosition - _dragOrigin);
            Vector3 move = new Vector3(pos.x * speed, 0, pos.y * speed);
            move = transform.parent.TransformDirection(move);
            _targetRootPosition = _cameraOrigin - move;
            return true;
        }
        return false;
    }
    private bool Rotate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _dragOrigin = Input.mousePosition;
            _cameraOrigin = _targetRootPosition;
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 pos = _camera.ScreenToViewportPoint(Input.mousePosition - _dragOrigin);
            transform.parent.Rotate(Vector3.up, pos.x * DragSpeed * 36);
            _dragOrigin = Input.mousePosition;
            return true;
        }
        return false;
    }
    private bool Zoom()
    {
        var scroll = Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        if (scroll == 0) return false;
        var target = _targetPosition + transform.parent.InverseTransformDirection(transform.forward) * scroll;
        if (target.y > 0 && target.y < 2)
        {
            _targetPosition = target;
        }
        return true;
    }
}
