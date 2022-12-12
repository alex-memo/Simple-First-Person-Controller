using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool isGrounded => Physics.CheckSphere(groundCheck.position, .2f, groundLayer);
    [SerializeField] private LayerMask groundLayer;
    
    private CharacterController controller=>GetComponent<CharacterController>();
    [SerializeField] private float speed=9;
    [SerializeField] private float jumpHeight = 6;
    [SerializeField] private float gravity = -30;
    
    [SerializeField][Range(0, .5f)] private float moveSmoothTime = .3f;

    [SerializeField] private Transform groundCheck;

    private Vector2 currentDir;
    private Vector2 currentDirVelocity;
    private float velocityY;

    private void Update()
    {
        move();
    }
    private void move()
    {
        Vector2 _targetDir = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, _targetDir, ref currentDirVelocity, moveSmoothTime);
        velocityY += gravity * 2f * Time.deltaTime;
        Vector3 _velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * speed + Vector3.up * velocityY;
        controller.Move(_velocity*Time.deltaTime);
        if(isGrounded&&Input.GetButtonDown("Jump"))
        {
            velocityY = Mathf.Sqrt(jumpHeight*-2f*gravity);
        }
        if (!isGrounded && controller.velocity.y < 1)
        {
            velocityY = -8;
        }
    }
}
