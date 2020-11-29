using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer renderer;
    [SerializeField] float xInput;
    [SerializeField] float yInput;
    [SerializeField] float moveSpeed;
    bool facingRight = true;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        if (xInput != 0|| yInput != 0) { 
            rb.velocity = new Vector2(xInput*moveSpeed, yInput*moveSpeed); 
            anim.SetBool("isWalking", true); 
        } else {
            rb.velocity = new Vector2(0, 0);
            anim.SetBool("isWalking", false); 
        }

        if(xInput < 0 && facingRight == true) { transform.eulerAngles = new Vector3(0,180,0); facingRight = false; }
        if(xInput > 0 && facingRight == false){ transform.eulerAngles = new Vector3(0, 0, 0); facingRight = true; }

        renderer.sortingOrder = Mathf.FloorToInt(transform.position.y * -100);
    }
}
