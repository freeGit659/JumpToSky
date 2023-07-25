using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb2D;
    Animator animator;
    [SerializeField] GameObject bar;
    [SerializeField] JumpForceBar jumpForceBar;
    private float reboundForce;
    private bool isSpace;
    private bool jumpCancelled;
    private bool isGround;
    public float jumpForce;
    public bool isJump;
    private int isFacingRight;
    private float jumpDirection;
    public float rotationSpeed;
    public float gravityScale = 5;
    public float fallGravityScale = 15;
    public float cancelRate;
    private float buttonPressedTime;
    private float buttonPressedTimeSet;
    public float jumpPos;
    public float playerPos;
    Vector3 lastvelocity;
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isFacingRight = 1;
        jumpForce = 15;
        reboundForce = 3;
        buttonPressedTimeSet = 1.5f;
        jumpDirection = 1;
        isSpace = false;
        bar.SetActive(false);
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") > 0) jumpDirection = 1;
        else if(Input.GetAxis("Horizontal") < 0) jumpDirection = -1;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bar.SetActive(true);
            isJump = true;
            isSpace = true;
        } 
        if(Input.touchCount > 0)
        {
            bar.SetActive(true);
            isJump = true;
            if (Input.GetTouch(0).position.x > 700)
            {
                jumpDirection = 1;
            }
            else jumpDirection = -1;
        }
        flip();
        if (rb2D.velocity.y> 0)
        {
            rb2D.gravityScale = gravityScale;
        }
        else
        {
            rb2D.gravityScale = fallGravityScale;
        }
        if (isJump && isGround)
        {
            animator.SetBool("IsPut", true);
            buttonPressedTime += Time.deltaTime;
            if (buttonPressedTime >= buttonPressedTimeSet) buttonPressedTime = buttonPressedTimeSet;
            if (Input.GetKeyUp(KeyCode.Space) || (Input.touchCount <= 0 && !isSpace))
            {
                //cancel the Jump
                rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                rb2D.velocity = new Vector2((jumpForce/3) * jumpDirection * buttonPressedTime/buttonPressedTimeSet, jumpForce * buttonPressedTime/buttonPressedTimeSet);
                buttonPressedTime= 0;
                animator.SetBool("IsPut", false); 
                animator.SetBool("IsGround", false);
                isJump = false;
                isSpace = false;
                bar.SetActive(false);

            }
            //if (rb2D.velocity.y < 0)
            //{
            //    rb2D.gravityScale = fallGravityScale;
            //    isJump = false;
            //}
        }
        jumpForceBar.UpdateBar(buttonPressedTime, buttonPressedTimeSet);

        lastvelocity = rb2D.velocity;
    }
    void FixedUpdate()
    {
        //if(jumpCancelled && isJump && rb2D.velocity.y > 0)
        //{
        //    rb2D.AddForce(Vector2.down * cancelRate);
        //}
        //if (isGround)
        //{
        //    transform.rotation = Quaternion.identity;
        //    return;
        //}
        //if (isJump != 0) transform.rotation = Quaternion.Euler(0,0,rb2D.velocity.y * rotationSpeed * isJump*-1f);
        //else transform.rotation = Quaternion.Euler(0, 0, rb2D.velocity.y * rotationSpeed);
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            animator.SetBool("IsGround", true);
            animator.SetBool("IsColWall", false);
        }
        if (col.gameObject.CompareTag("Wall"))
        {
            animator.SetBool("IsColWall", true);
            var speed = lastvelocity.magnitude;
            var direction = Vector3.Reflect(lastvelocity.normalized, col.contacts[0].normal);

            rb2D.velocity = direction * Mathf.Max(speed*0.5f, 0f);
        }
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground")) isGround = false;
    }
    public void flip()
    {
        if (isFacingRight ==1 && jumpDirection == -1 || isFacingRight == -1 && jumpDirection == 1)
        {
            isFacingRight = -1*isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x = scale.x * -1;
            transform.localScale = scale;
        }
    }
}
