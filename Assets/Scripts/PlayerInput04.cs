using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput04 : MonoBehaviour
{
    public Vector2 mousePosition { get; set; }
    public bool mouseClickDown { get; set; }
    public bool mouseClickUp {get; set;}

    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MouseCheck();
    }

    private void MouseCheck()
    {
        mouseClickDown = Input.GetMouseButtonDown(0);
        mouseClickUp = Input.GetMouseButtonUp(0);
    }
}
