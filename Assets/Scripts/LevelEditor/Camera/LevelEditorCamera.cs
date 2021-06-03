using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorCamera : MonoBehaviour {

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 10f;

    [Header("Zoom")]
    [SerializeField] private float zoomAmount = 1;
    [SerializeField] public Vector2 zoomMaxOffset = Vector2.zero;

    [SerializeField] private float rotationSensitivity = 0.1f;
    [SerializeField] private Vector2 startRotation = Vector2.zero;
    [SerializeField] private float startZoom = 15;

    public float zoomOffset;

    private Transform pivot = null;
    private Transform anchor = null;
    private Camera cam = null;

    private Vector3 camStartPoint = Vector3.zero;
    private Vector2 camEuler = Vector2.zero;

    private void Start() {
        pivot = transform.Find("CameraPivot");
        anchor = pivot.Find("CameraAnchor");
        if (pivot == null) {
            Debug.LogError("Camera pivot is null. Assign it.");
        }
        if (anchor == null) {
            Debug.LogError("Camera anchor is null. Assign it.");
        }
        cam = Camera.main;

        // Initialize camera
        camStartPoint = anchor.transform.position - pivot.transform.position;
        camEuler = startRotation;
        RotatePivot();
        Zoom(-startZoom);
    }

    private void Update() {
        CheckInput();
    }

    private void CheckInput() {
        LevelEditorInput input = LevelEditor.Instance.Input;
        if (input.mWheel) {
            // Check wether we move or rotate
            if (input.shift) {
                MovePivot();
            }
            else if (input.ctrl) {
                Zoom(input.mouseInput.y);
            }
            else {
                RotatePivot();
            }
        }
        if (input.scroll != 0) {
            Zoom(input.scroll);
        }

        // Ortographic on/off
        if (input.numpad == 5) {
            cam.orthographic = !cam.orthographic;
        }

    }

    private void RotatePivot() {
        camEuler.x -= LevelEditor.Instance.Input.mouseInput.y * rotationSensitivity * Time.deltaTime;
        camEuler.y += LevelEditor.Instance.Input.mouseInput.x * rotationSensitivity * Time.deltaTime;
        camEuler.x = Mathf.Clamp(camEuler.x, -89, 89);
        pivot.transform.rotation = Quaternion.Euler(camEuler.x, camEuler.y, 0);
    }

    private void MovePivot() {
        Vector3 movementVector = new Vector3(LevelEditor.Instance.Input.mouseInput.x, LevelEditor.Instance.Input.mouseInput.y, 0) * movementSpeed * (zoomOffset / zoomMaxOffset.y) * Time.deltaTime;
        movementVector = pivot.rotation * movementVector;
        transform.position += movementVector;
    }

    private void Zoom(float dir) {
        zoomOffset = Mathf.Clamp(zoomOffset + dir * zoomAmount, -zoomMaxOffset.y, -zoomMaxOffset.x);
        anchor.transform.position = pivot.transform.position + pivot.transform.rotation * camStartPoint + cam.transform.forward * zoomOffset;
        cam.orthographicSize = -zoomOffset;   
    }
}
