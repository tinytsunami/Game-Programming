﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 10.0f;
    public float jumpForce = 800.0f;
    
    private float movingSpeed;
    private bool jump;
    //private Collision coll;

    bool facingLeft;
    Transform groundCheck;
    float groundRadius;
    bool onGround;
    public LayerMask groundLayer;

    Animator anim;

    Scene currentScene;

    public bool Event;

    string sceneName;
    public float nextx;

    bool SceneChangeFlag;
    GameController Game;

    static PlayerController instance;

    void Awake()
    {
        anim = GetComponent<Animator>();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (this != instance)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //coll = GetComponent<Collision>();
        currentScene = SceneManager.GetActiveScene ();
        Game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        facingLeft = true;
        onGround = false;
        groundCheck = transform.Find("GroundCheck");
        groundRadius = 0.01f;
        Event = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1 ||
           SceneManager.GetActiveScene().buildIndex == 0 ||
           SceneManager.GetActiveScene().buildIndex == 8)
        {
            Destroy(this.gameObject);
        }
        if(Input.GetButtonDown("Jump")){
            jump = true;
        }
        if (Game.Moveable())
        {
            movingSpeed = Input.GetAxis("Horizontal");
        }
        else
        {
            movingSpeed = 0;
            jump = false;
        }
    }

    void FixedUpdate()
    {
        Move(movingSpeed, jump);
            
        jump = false;
        onGround = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
//        Debug.Log(Event);
    }

    void Move(float MovingSpeed, bool jump)
    {
        if(onGround && jump)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, jumpForce));
        }
        else
        {
            anim.SetFloat("Speed", Mathf.Abs(movingSpeed));

            GetComponent<Rigidbody2D>().velocity = new Vector2(movingSpeed*maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

            if (movingSpeed > 0 && facingLeft || movingSpeed < 0 && !facingLeft) Flip ();
        }
    }

    void Flip()
    {
        facingLeft = !facingLeft;

		Vector3 characterScale = transform.localScale;
		characterScale.x *= -1;
		transform.localScale = characterScale;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }
}
