using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator anim;
    Rigidbody rb;
    SpriteRenderer sprite, weaponSprite;
    [SerializeField] float xInput;
    [SerializeField] float yInput;
    [SerializeField] float moveSpeed;
    [SerializeField] bool facingRight = true;
    AttackHandler atk;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        sprite = GetComponent<SpriteRenderer>();
        weaponSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        atk = GetComponentInChildren<AttackHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        if (xInput != 0 || yInput != 0) { 
            rb.velocity = xInput!=0 && yInput!=0 ? new Vector3(xInput * (moveSpeed*.8f), 0,  yInput * (moveSpeed*.8f)) : new Vector3(xInput*moveSpeed, 0, yInput*moveSpeed); 
            anim.SetBool("isWalking", true); 
        } else {
            rb.velocity = new Vector2(0, 0);
            anim.SetBool("isWalking", false); 
        }



        if(xInput < 0 && facingRight == true) { 
            transform.localEulerAngles = new Vector3(0,180,0); facingRight = false;
            atk.flipHitCheckOffset();
        }
        if(xInput > 0 && facingRight == false){ 
            transform.localEulerAngles = new Vector3(0, 0, 0); facingRight = true;
            atk.flipHitCheckOffset();
        }

        sprite.sortingOrder = Mathf.FloorToInt(transform.position.z * -100);
        weaponSprite.sortingOrder = sprite.sortingOrder + 1;
    }
}
