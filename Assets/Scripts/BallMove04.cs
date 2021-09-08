using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove04 : MonoBehaviour
{
    private PlayerInput04 playerInput = null;

    private Rigidbody2D rigid = null;

    [SerializeField]
    private LineRenderer startAtBall = null;
    [SerializeField]
    private LineRenderer startAtMouse = null;

    private Vector2 currentPosition = Vector2.zero;
    private Vector2 firstClickPosition = Vector2.zero;
    private Vector2 currentDir = Vector2.zero;
    private Vector2 mousePosition = Vector2.zero;
    
    [SerializeField]
    private float ballMovePower = 5f;
    [SerializeField]
    private float maxPower = 10f;
    private float firstPosY = 0f;

    private bool mouseClickDown = false;
    private bool mouseClickUp = false;

    void Start()
    {
        playerInput = GetComponent<PlayerInput04>();
        rigid = GetComponent<Rigidbody2D>();

        firstPosY = transform.position.y;
    }
    private void Update()
    {
        if (playerInput.mouseClickDown)
        {
            mouseClickDown = true;
        }

        if (playerInput.mouseClickUp)
        {
            mouseClickUp = true;
        }

        // rigid.velocity = new Vector2(Mathf.Clamp(rigid.velocity.x, -maxPower, maxPower), Mathf.Clamp(rigid.velocity.y, -maxPower, maxPower));
    }

    void FixedUpdate()
    {
        currentPosition = transform.position;
        mousePosition = playerInput.mousePosition;
        mousePosition = new Vector2(Mathf.Clamp(playerInput.mousePosition.x, -maxPower + firstClickPosition.x, maxPower + firstClickPosition.x), Mathf.Clamp(playerInput.mousePosition.y, -maxPower + firstClickPosition.y, maxPower + firstClickPosition.y));

        if (mouseClickDown)
        {
            ClickDownBall();
        }

        if (mouseClickUp)
        {
            ClickUpBall();
        }

        // Debug.Log(currentDir);

        transform.position = currentPosition;
    }
    public void OnDrawGizmos()
    {
        if (firstClickPosition != Vector2.zero)
        {
            // startAtBall.transform.position = firstClickPosition;

            startAtMouse.SetPosition(0, firstClickPosition);
            startAtMouse.SetPosition(1, mousePosition);

            Vector2 position = firstClickPosition - mousePosition;
            position += currentPosition;

            startAtBall.SetPosition(0, currentPosition);
            startAtBall.SetPosition(1, position);

            // Debug.Log("aaa" + position);
        }
    }
    public void ClickDownBall()
    {
        mouseClickDown = false;

        firstClickPosition = mousePosition;
    }
    public void ClickUpBall()
    {
        mouseClickUp = false;

        Vector2 dir = currentDir;

        dir = firstClickPosition - mousePosition;

        currentDir = dir;

        firstClickPosition = Vector2.zero;
        
        rigid.velocity = Vector2.zero;

        rigid.AddForce(currentDir * ballMovePower);
    }
    public void OnCollisionEnter2D(Collision2D col)
    {
        int layer = 2 << col.gameObject.layer - 1;

        if (layer == LayerMask.GetMask("WALL"))
        {
            Vector2 dir = currentDir;

            dir = new Vector2(-dir.x, dir.y);

            currentDir = dir;

            rigid.velocity = Vector2.zero;

            rigid.AddForce(currentDir * ballMovePower);
        }
        else if (layer == LayerMask.GetMask("UPWALL"))
        {
            Vector2 dir = currentDir;

            dir = new Vector2(dir.x, -dir.y);

            currentDir = dir;

            rigid.velocity = Vector2.zero;

            rigid.AddForce(currentDir * ballMovePower);
        }
        else if (layer == LayerMask.GetMask("GROUND"))
        {
            currentDir = Vector2.zero;
            rigid.velocity = Vector2.zero;

            transform.position = new Vector2(transform.position.x, firstPosY);

            for(int i = 0; i < 2; i++)
            {
                startAtMouse.SetPosition(i, Vector3.zero);
                startAtBall.SetPosition(i, Vector3.zero);
            }
        }
    }
}
