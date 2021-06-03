using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorInput : MonoBehaviour {

    [HideInInspector] public Vector2 mouseInput = Vector2.zero;
    [HideInInspector] public int leftMouse = 0;
    [HideInInspector] public int rightMouse = 0;
    [HideInInspector] public int scroll = 0;
    [HideInInspector] public int pressedNum = -1;
    [HideInInspector] public int numpad = -1;

    [HideInInspector] public bool mWheel = false;
    [HideInInspector] public bool mWheelDown = false;
    [HideInInspector] public bool shift = false;
    [HideInInspector] public bool ctrl = false;
    [HideInInspector] public bool leftMouseHold = false;
    [HideInInspector] public bool rightMouseHold = false;
    [HideInInspector] public bool x = false;
    [HideInInspector] public bool y = false;
    [HideInInspector] public bool z = false;
    [HideInInspector] public bool r = false;
    [HideInInspector] public bool bDown = false;
    [HideInInspector] public bool qDown = false;
    [HideInInspector] public bool eDown = false;
    [HideInInspector] public bool gDown = false;
    [HideInInspector] public bool hDown = false;
    [HideInInspector] public bool pgUpDown = false;
    [HideInInspector] public bool pgDownDown = false;
    [HideInInspector] public bool plusDown = false;
    [HideInInspector] public bool minusDown = false;

    private void Update() {
        CheckMouse();
        CheckKeyBoard();
    }

    private void CheckMouse() {
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        // Simplifies mouse wheel return value: Either -1, 0 or 1.
        scroll = Input.GetAxisRaw("Mouse ScrollWheel") > 0 ? 1 : Input.GetAxisRaw("Mouse ScrollWheel") < 0 ? -1 : 0;

        mWheel = Input.GetKey(KeyCode.Mouse2);
        mWheelDown = Input.GetKeyDown(KeyCode.Mouse2);

        // Returns 1 when you press the according mouse button, -1 when you release it. Otherwise 0
        leftMouse = Input.GetKeyDown(KeyCode.Mouse0) ? 1 : Input.GetKeyUp(KeyCode.Mouse0) ? -1 : 0;
        rightMouse = Input.GetKeyDown(KeyCode.Mouse1) ? 1 : Input.GetKeyUp(KeyCode.Mouse1) ? -1 : 0;

        leftMouseHold = Input.GetKey(KeyCode.Mouse0);
        rightMouseHold = Input.GetKey(KeyCode.Mouse1);
    }

    private void CheckKeyBoard() {
        shift = Input.GetKey(KeyCode.LeftShift);
        ctrl = Input.GetKey(KeyCode.LeftControl);
        x = Input.GetKey(KeyCode.X);
        y = Input.GetKey(KeyCode.Y);
        z = Input.GetKey(KeyCode.Z);
        r = Input.GetKey(KeyCode.R);

        eDown = Input.GetKeyDown(KeyCode.E);
        qDown = Input.GetKeyDown(KeyCode.Q);
        bDown = Input.GetKeyDown(KeyCode.B);
        gDown = Input.GetKeyDown(KeyCode.G);
        hDown = Input.GetKeyDown(KeyCode.H);
        pgUpDown = Input.GetKeyDown(KeyCode.PageUp);
        pgDownDown = Input.GetKeyDown(KeyCode.PageDown);
        plusDown = Input.GetKeyDown(KeyCode.KeypadPlus);
        minusDown = Input.GetKeyDown(KeyCode.KeypadMinus);

        numpad = GetNumpad();
        pressedNum = GetNum();
    }

    private int GetNumpad() {
        if (Input.GetKeyDown(KeyCode.Keypad0)) return 0;
        if (Input.GetKeyDown(KeyCode.Keypad1)) return 1;
        if (Input.GetKeyDown(KeyCode.Keypad2)) return 2;
        if (Input.GetKeyDown(KeyCode.Keypad3)) return 3;
        if (Input.GetKeyDown(KeyCode.Keypad4)) return 4;
        if (Input.GetKeyDown(KeyCode.Keypad5)) return 5;
        if (Input.GetKeyDown(KeyCode.Keypad6)) return 6;
        if (Input.GetKeyDown(KeyCode.Keypad7)) return 7;
        if (Input.GetKeyDown(KeyCode.Keypad8)) return 8;
        if (Input.GetKeyDown(KeyCode.Keypad9)) return 9;
        return -1;
    }

    private int GetNum() {
        if (Input.GetKeyDown(KeyCode.Alpha0)) return 0;
        if (Input.GetKeyDown(KeyCode.Alpha1)) return 1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) return 2;
        if (Input.GetKeyDown(KeyCode.Alpha3)) return 3;
        if (Input.GetKeyDown(KeyCode.Alpha4)) return 4;
        if (Input.GetKeyDown(KeyCode.Alpha5)) return 5;
        if (Input.GetKeyDown(KeyCode.Alpha6)) return 6;
        if (Input.GetKeyDown(KeyCode.Alpha7)) return 7;
        if (Input.GetKeyDown(KeyCode.Alpha8)) return 8;
        if (Input.GetKeyDown(KeyCode.Alpha9)) return 9;
        return -1;
    }

}
