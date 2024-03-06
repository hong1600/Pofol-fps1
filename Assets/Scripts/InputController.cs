using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private GameObject gameobj;
    private Animator anim;
    private Rigidbody rigid;
    private CapsuleCollider cap;
    private Camera cam;

    [SerializeField] private Transform cameraTrs;
    [SerializeField] private Transform playerTrs;

    private Vector3 moveDir;

    [SerializeField] private float gravity = 9.81f;
    private float verticalVelocity = 0f;
    [SerializeField] private bool isGround;
    [SerializeField] private bool isJump;

    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;


    private void Awake()
    {
        cam = Camera.main;
        cap = GetComponent<CapsuleCollider>();
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        moving();
        checkGround();
        checkGravity();
        jumping();
        checkMouseLock();
    }

    private void moving()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        moveDir = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        
        transform.position += moveDir * Time.deltaTime * moveSpeed;

        anim.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        anim.SetFloat("Vertical", Input.GetAxis("Vertical"));
    }

    private void checkGround()
    {
        if (isGround = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit,
            cap.height * 0.65f, LayerMask.GetMask("Ground")))
        {
            isGround = true;
        }
        else isGround = false;
    }

    private void checkGravity()
    {
        if (isGround == false)
        {
            verticalVelocity = 0f;
        }
        else if (isGround == true)
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        if (isJump)
        {
            isJump = false;
            rigid.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }

        rigid.velocity = new Vector3(rigid.velocity.x, verticalVelocity, rigid.velocity.z);
    }

    private void jumping()
    {
        if (isGround == false && Input.GetKeyDown(KeyCode.Space))
        {
            isJump = true;
        }
    }

    private void checkMouseLock()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }


}
