using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;
    private Camera cam;
    private CapsuleCollider cap;
    private Rigidbody rigid;

    [SerializeField] private Transform cameraTrs;
    [SerializeField] private Transform playerTrs;
    [SerializeField] private Transform bulletTrs;

    private Vector3 moveDir;
    private float gravity = 9.81f;
    private float verticalVelocity;
    private bool isJump = false;
    private bool isGround = false;
    private bool isRun = false;
    public bool hasWeapon = false;

    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float runSpeed;

    [SerializeField] GameObject weapon1;
    [SerializeField] GameObject bullet;


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
        jumping();
        checkMouseLock();
        swap();
    }

    private void moving()
    {
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRun = true;
            anim.SetBool("isRun", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRun = false;
            anim.SetBool("isRun", false);
        }

        if (Input.GetKey(KeyCode.LeftShift) && hasWeapon == true)
        {
            anim.SetBool("isRifleRun", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            anim.SetBool("isRifleRun", false);
        }

        if (isJump == false)
        {
            transform.position += moveDir * moveSpeed * (isRun ? runSpeed : 1f) * Time.deltaTime;
        }

        anim.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        anim.SetFloat("Vertical", Input.GetAxis("Vertical"));
    }

    private void checkGround()
    {
        if (rigid.velocity.y <= 0)
        {
            isGround = Physics.Raycast(cap.bounds.center, Vector3.down,
                cap.height * 0.55f, LayerMask.GetMask("Ground"));
        }
        else 
        {
            isGround = false;
        }
    }
    private void jumping()
    {
        if (isGround == true)
        {
            isJump = false;
            verticalVelocity = 0f;
        }

        if(Input.GetKeyDown(KeyCode.Space) && isJump == false)
        {
            verticalVelocity = jumpForce;
            anim.SetTrigger("doJump");
            isJump = true;
        }
        else if(isJump == true) 
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        rigid.velocity = new Vector3(0f, verticalVelocity, 0f);
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


    public void swap()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            hasWeapon = true;
            anim.SetBool("hasWeapon", true);

        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            hasWeapon = false;
            anim.SetBool("hasWeapon", false);
        }
    }

    private void shoot()
    {
        if (Input.GetMouseButtonDown(0) && hasWeapon == true)
        {
            GameObject flame = GameObject.Find("MuzzleFlame");
            flame.SetActive(true);
        }
    }

}
