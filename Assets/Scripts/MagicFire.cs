using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicFire : MonoBehaviour
{
    [SerializeField] float fireSpeed = 15f;
    float xSpeed;
    Rigidbody2D myRigidBody;
    SpriteRenderer playerSprite;
    PlayerMovement player;
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        playerSprite = player.GetComponent<SpriteRenderer>();
        xSpeed = player.transform.localScale.x * fireSpeed;
        if(playerSprite.flipX){
            xSpeed = -xSpeed;
            Vector3 localScale = transform.localScale;
            localScale.x = -localScale.x;
            transform.localScale = localScale;
        }    
    }

    void Update()
    {
        myRigidBody.velocity = new Vector2(xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy"){
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Destroy(gameObject);
    }
}
