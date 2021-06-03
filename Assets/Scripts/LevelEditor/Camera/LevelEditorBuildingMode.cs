using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorBuildingMode : MonoBehaviour {
    
    [Header("Snapping")]
    [SerializeField] private bool gridSnap = false;
    [SerializeField] private float gridScale = 2;

    [SerializeField] private float rotationSnapAmount = 45f;
    [SerializeField] private float rotationSensitivity = 300f;

    [SerializeField] private GameObject grid = null;
    [SerializeField] private float rayMaxDistance = 100;
    [SerializeField] private LayerMask rayMask = 1;
    [SerializeField] private Material ghostMaterial = null;

    private Vector3 buildPosition = Vector3.zero;
    private Vector3 previousBuildPosition = new Vector3(0, -200, 0);
    private Vector3 placementAnchor = Vector3.zero;

    private RaycastHit hit;
    private bool brush = false;

    // 1. WALLS 2. FLOORS
    private List<Category> categories = new List<Category>();
    private int currentCategory = 1;

    public GameObject buildObject = null;
    public GameObject ghostObject = null;


    private void Start() {
        categories.Add(new Category(LevelEditorItemDatabase.LoadWalls()));
        categories.Add(new Category(LevelEditorItemDatabase.LoadFloors()));

        SpawnGhost();
    }

    private void SpawnGhost() {
        Quaternion prevGhostRotation = Quaternion.identity;
        if (ghostObject != null) {
            prevGhostRotation = ghostObject.transform.rotation;
            Destroy(ghostObject);
        }
        if (buildObject != null) {
            ghostObject = Instantiate<GameObject>(buildObject, buildPosition, prevGhostRotation);
            if (ghostMaterial != null)
                ghostObject.GetComponentInChildren<MeshRenderer>().material = ghostMaterial;
            EntityManager.ChangeLayer(ghostObject, 2);
        }
    }

    private void Update() {
        CastRay();
        UpdateBuildPosition();
        CheckInput();
    }

    private void CastRay() {
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, rayMaxDistance, rayMask, QueryTriggerInteraction.Ignore);
        buildPosition = hit.collider != null ? hit.point : Camera.main.ScreenPointToRay(Input.mousePosition).origin + Camera.main.ScreenPointToRay(Input.mousePosition).direction * rayMaxDistance;
    }

    private void UpdateBuildPosition() {
        if (ghostObject == null) {
            Debug.LogError("Build tool ghost is null. Maybe a build item is not assigned");
            return;
        }
        if (LevelEditor.Instance.Input.r) {
            return;
        }
        // Setup anchoring
        SetupAnchor();

        if (gridSnap) {
            ghostObject.transform.position = new Vector3(Mathf.Round(buildPosition.x / gridScale) * gridScale, Mathf.Round(buildPosition.y / gridScale) * gridScale, Mathf.Round(buildPosition.z / gridScale) * gridScale);
        }
        else {
            ghostObject.transform.position = buildPosition;
        }
    }

    private void SetupAnchor() {
        if (LevelEditor.Instance.Input.x || LevelEditor.Instance.Input.y || LevelEditor.Instance.Input.z) {
            // Set anchor if it wasn't previously
            if (placementAnchor == Vector3.zero)
                placementAnchor = buildPosition;

            // Parse xyz inputs and shift inputs
            if (LevelEditor.Instance.Input.x) {
                if (LevelEditor.Instance.Input.shift) {
                    buildPosition = new Vector3(placementAnchor.x, buildPosition.y, buildPosition.z);
                }
                else {
                    buildPosition = new Vector3(buildPosition.x, placementAnchor.y, placementAnchor.z);
                }
            }
            else if (LevelEditor.Instance.Input.y) {
                if (LevelEditor.Instance.Input.shift) {
                    buildPosition = new Vector3(buildPosition.x, placementAnchor.y, buildPosition.z);
                }
                else {
                    placementAnchor.y -= LevelEditor.Instance.Input.mouseInput.y * (LevelEditor.Instance.Camera.zoomOffset / LevelEditor.Instance.Camera.zoomMaxOffset.y)*2;
                    buildPosition = new Vector3(placementAnchor.x, placementAnchor.y, placementAnchor.z);
                }
            }
            else if (LevelEditor.Instance.Input.z) {
                if (LevelEditor.Instance.Input.shift) {
                    buildPosition = new Vector3(buildPosition.x, buildPosition.y, placementAnchor.z);
                }
                else {
                    buildPosition = new Vector3(placementAnchor.x, placementAnchor.y, buildPosition.z);
                }
            }
        }
        // Set anchor to vec3.zero if no anchor keys are pressed.
        else {
            if (placementAnchor != Vector3.zero) {
                placementAnchor = Vector3.zero;
            }
        }
    }

    private void CheckInput() {
        RotateObject();
        if (LevelEditor.Instance.Input.leftMouse == 1) {
            BuildObjectSingle();
        }
        if (LevelEditor.Instance.Input.rightMouse == 1) {
            DeleteObject();
        }
        if (brush && LevelEditor.Instance.Input.rightMouseHold) {
            DeleteObject();
        }
        if (brush && LevelEditor.Instance.Input.leftMouseHold) {
            BuildObjectBrush();
        }
        if (LevelEditor.Instance.Input.bDown) {
            brush = !brush;
        }
        if (LevelEditor.Instance.Input.gDown) {
            gridSnap = !gridSnap;
        }
        if (LevelEditor.Instance.Input.hDown) {
            grid.SetActive(!grid.activeInHierarchy);
        }
        if (LevelEditor.Instance.Input.pgUpDown) {
            grid.transform.position = new Vector3(0, grid.transform.position.y + gridScale, 0);
        }
        if (LevelEditor.Instance.Input.pgDownDown) {
            grid.transform.position = new Vector3(0, grid.transform.position.y - gridScale, 0);
        }
        if (LevelEditor.Instance.Input.pressedNum != -1) {
            ChangeBuildObject(LevelEditor.Instance.Input.pressedNum);
        }
        // Grid scaling
        if (LevelEditor.Instance.Input.plusDown || LevelEditor.Instance.Input.minusDown) {
            ScaleGrid();
        }

    }

    private void DeleteObject() {
        LayerMask lm = LayerMask.GetMask("Default", "Water");
        RaycastHit delHit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out delHit, rayMaxDistance, lm, QueryTriggerInteraction.Ignore)) {
            Destroy(delHit.collider.gameObject);
        }            
    }

    private void RotateObject() {
        if (LevelEditor.Instance.Input.eDown) {
            // If smooth rotation was enabled the rotation can be a bit offset
            ghostObject.transform.Rotate(new Vector3(0, rotationSnapAmount - (ghostObject.transform.rotation.eulerAngles.y % rotationSnapAmount)));
        }
        else if (LevelEditor.Instance.Input.qDown) {
                ghostObject.transform.Rotate(new Vector3(0, -rotationSnapAmount - (ghostObject.transform.rotation.eulerAngles.y % rotationSnapAmount), 0));
        }
        else if (LevelEditor.Instance.Input.r) {
            ghostObject.transform.rotation = Quaternion.Euler(0, ghostObject.transform.rotation.eulerAngles.y - LevelEditor.Instance.Input.mouseInput.x * Time.deltaTime * rotationSensitivity, 0);
        }
    }

    private void BuildObjectSingle() {
        Instantiate(buildObject, ghostObject.transform.position, ghostObject.transform.rotation);
        previousBuildPosition = ghostObject.transform.position;
    }

    private void BuildObjectBrush() {
        if (Vector3.Distance(previousBuildPosition, buildPosition) >= gridScale) {
            Instantiate(buildObject, ghostObject.transform.position, ghostObject.transform.rotation);
            previousBuildPosition = ghostObject.transform.position;
        }
    }

    private void ChangeBuildObject(int index) {
        index = index == 0 ? 9 : --index;
        if (index == currentCategory) {
            buildObject = categories[index].GetItem(true);
        }
        else {
            buildObject = categories[index].GetItem(false);
        }
        SpawnGhost();
        currentCategory = index;
    }

    private void ScaleGrid() {
        if (LevelEditor.Instance.Input.minusDown)
            gridScale -= 0.1f;
        if (LevelEditor.Instance.Input.plusDown)
            gridScale += 0.1f;
        gridScale *= 10;
        Mathf.Round(gridScale);
        gridScale *= 0.1f;
        grid.transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2((1/gridScale) * 100, (1/gridScale) * 100);
    }

    private class Category {
        public Category(GameObject[] objList) {
            objects = objList;
        }
        private GameObject[] objects;
        private int currentObject = 0;
        public GameObject GetItem(bool increment) {
            if (objects.Length == 0)
                return null;
            if (increment)
                currentObject = currentObject++ >= objects.Length-1 ? 0 : currentObject++;
            return objects[currentObject];
        }
    }
}


