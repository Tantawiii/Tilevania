using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] GameObject fire;
    [SerializeField] Transform magicFire;
    Vector2 moveInput;
    Rigidbody2D myrigidBody;
    SpriteRenderer mySprite;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    private CinemachineImpulseSource _myImpulseSource;


    float gravitymain;
    float jumpDistance;

    bool isAlive = true;
    void Start()
    {
        myrigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySprite = GetComponent<SpriteRenderer>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravitymain = myrigidBody.gravityScale;
        _myImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAlive){
            myrigidBody.simulated=false;
            return;
        }
        Run();
        FlipSpirte();
        ClimbLadder();
        Die();
    }

    void OnMove(InputValue value){
        if(!isAlive){
            myrigidBody.simulated=false;
            return;
        }
        moveInput = value.Get<Vector2>();
    }

    void OnFire(InputValue value){
        if(!isAlive){
            myrigidBody.simulated=false;
            return;
        }
        if(value.isPressed){
            myAnimator.SetBool("isShooting", true);
            Instantiate(fire, magicFire.position, transform.rotation);  
        }
    }

    void Run(){
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myrigidBody.velocity.y);
        myrigidBody.velocity = playerVelocity;
        bool hasHorizontalMovement = Mathf.Abs(myrigidBody.velocity.x) > Mathf.Epsilon;
        if(hasHorizontalMovement){
            myAnimator.SetBool("isRunning", true);
            myAnimator.SetBool("isShooting", false);
            myAnimator.SetBool("isDashing", false);
            myAnimator.SetBool("isJumping", false);
        }
        else{
            myAnimator.SetBool("isRunning", false);
        }
    }

    void OnJump(InputValue value){
        if(!isAlive){
            myrigidBody.simulated=false;
            return;
        }
        if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))){
            return;
        }
        if(value.isPressed){
            myrigidBody.velocity += new Vector2(myrigidBody.velocity.x, jumpSpeed);
            myAnimator.SetBool("isShooting", false);
            myAnimator.SetBool("isDashing", false);
            myAnimator.SetBool("isJumping", true);
        }
    }

    void OnSlideDash(InputValue value){
        myrigidBody.velocity += new Vector2(moveInput.x * dashSpeed, myrigidBody.velocity.y);
        myAnimator.SetBool("isJumping", false);
        myAnimator.SetBool("isShooting", false);
        myAnimator.SetBool("isDashing", true);
    }

    void FlipSpirte(){
        bool hasHorizontalMovement = Mathf.Abs(myrigidBody.velocity.x) > Mathf.Epsilon;
        if (hasHorizontalMovement)
            {
                mySprite.flipX = (Mathf.Sign(myrigidBody.velocity.x)) < 0;
            }
    }

    void ClimbLadder(){
        if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))){
            myrigidBody.gravityScale = gravitymain;
            myAnimator.SetBool("isClimbing", false);
            return;
        }
        Vector2 climbVelocity = new Vector2(myrigidBody.velocity.x, moveInput.y * climbSpeed);
        myrigidBody.velocity = climbVelocity;
        myrigidBody.gravityScale = 0f;
        bool hasVerticalMovement = Mathf.Abs(myrigidBody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isShooting", false);
        myAnimator.SetBool("isClimbing", hasVerticalMovement);
    }
    
    void Die(){
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Slimey", "Hazards"))){
            isAlive = false;
            _myImpulseSource.GenerateImpulse(1);
            myAnimator.SetTrigger("Dying");
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
