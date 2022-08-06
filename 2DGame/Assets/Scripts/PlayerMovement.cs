using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float jumppower;
    [SerializeField] private LayerMask groundlayer;
    [SerializeField] private LayerMask Walllayer;
    [SerializeField] private GameObject Enemy1;
    [SerializeField] private Enemy1 enemy;
    private Rigidbody2D body;
    private Animator anim;
    private bool check_if_player_dead = false;
    private bool isDead = false;
    private BoxCollider2D boxcollider;
    private float walljumpcooldown;
    private float horizontal_input;
    public int totalArrows;
    private GameObject cloneEnemies;
    private int i = 0;
    private void Awake()
    {
        //Grab references for rigid body and animator from the game object(player here)
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxcollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //for left-right movement
        horizontal_input = Input.GetAxis("Horizontal");

        //flip player when move left-right
        if (horizontal_input > 0.01f)
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
        else if (horizontal_input < -0.01f)
        {
            transform.localScale = new Vector3(-2, 2, 2);
        }


        //set animator parameters(for running, idle and death
        anim.SetBool("run", horizontal_input != 0 && IsGrounded());
        anim.SetBool("grounded", IsGrounded());
        if (isDead == true)
        {   
            
            speed = 0;
            
            //transform.position = new Vector3(transform.position.x, -2.9f, transform.position.z);
            anim.SetBool("isDead", isDead);
            //Destroy(this.GetComponent<BoxCollider2D>());
            isDead = false;
            //transform.position = new Vector3(transform.position.x, -2.9f, transform.position.z);
            Destroy(this.gameObject, 5f);
            StartCoroutine(generateEnemies());
        }

        //Wall jump logic
        if (walljumpcooldown > 0.2f)
        {
            body.velocity = new Vector2(horizontal_input * speed, body.velocity.y);
            if (OnWall() && !IsGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
            {
                body.gravityScale = 3f;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                jump();
            }
        }
        else
        {
            walljumpcooldown += Time.deltaTime;
        }

    }

    //Player gets random number of arrows at start of the game
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("GetArrows"))
        {
            totalArrows = Random.Range(1, 11);
            if(totalArrows % 2 != 0)
            {
                totalArrows++;
            }
            //To generate enemies(Enemy1)half than total arrows...each after 5 seconds
            totalArrows = 8;
            if(totalArrows == 14 || totalArrows == 16) { i = -1; }
            if(totalArrows == 18 || totalArrows == 20) { i = -2; }
            Debug.Log(totalArrows);
            StartCoroutine(generateEnemies());
            collision.gameObject.SetActive(false);
        }
    }

    //Player dies when collides with enemy
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && check_if_player_dead == false)
        {
            Debug.Log("Collision run once");
            body.constraints = RigidbodyConstraints2D.FreezePositionY;      //so the player dont fall to the ground
            //Destroy(this.GetComponent<BoxCollider2D>());
            isDead = true;
            check_if_player_dead = true;
        }
    }

    private void jump()
    {

        if (IsGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumppower);
            StartCoroutine(jumpagain());
        }
        else if (OnWall() && !IsGrounded())
        {
            walljumpcooldown = 0;
            body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
        }

    }

    //For second jump (Double Jump)
    IEnumerator jumpagain()
    {
        yield return new WaitForSeconds(0.2f);
        if (Input.GetKey(KeyCode.Space))
        {
            body.velocity = new Vector2(body.velocity.x, jumppower);
        }
    }

    //To check if the player is grounded...checking with the help of raycast(BoxCast)
    private bool IsGrounded()
    {
        RaycastHit2D raycasthit = Physics2D.BoxCast(boxcollider.bounds.center, boxcollider.bounds.size, 0, Vector2.down, 0.1f, groundlayer);
        return raycasthit.collider != null;
    }

    //To check if the player is on the wall...checking with the help of raycast(BoxCast)
    private bool OnWall()
    {
        RaycastHit2D raycasthit = Physics2D.BoxCast(boxcollider.bounds.center, boxcollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, Walllayer);
        return raycasthit.collider != null;
    }

    public bool canAttack()
    {
        return horizontal_input == 0 && IsGrounded() && !OnWall();
    }

    IEnumerator generateEnemies()
    {
        //if (i <= (totalArrows / 2))
        //{
        //    Debug.Log("Value of i = "+i);
        //    yield return new WaitForSeconds(2.5f);
        //    //cloneEnemies = Instantiate(Enemy1, new Vector3(-1.9f, 0f, 0f), transform.rotation) as GameObject;   //with a specific spawnpoint
        //    cloneEnemies = Instantiate(Enemy1);         //With a spawnpoint of standard Enemy1
        //    cloneEnemies.SetActive(true);
        //    i++;
        //    StartCoroutine(generateEnemies());
        //}
        //else
        //{

        //}
        if (check_if_player_dead == false)
        {
            for (; i < (totalArrows / 2); i++)
            {
                yield return new WaitForSeconds(2.5f);
                //cloneEnemies = Instantiate(Enemy1, new Vector3(-1.9f, 0f, 0f), transform.rotation) as GameObject;   //with a specific spawnpoint
                cloneEnemies = Instantiate(Enemy1);         //With a spawnpoint of standard Enemy1
                cloneEnemies.SetActive(true);
            }
        }
        //To lower the y transform of player when he dies...for smooth death animation..otherwise he stays a little above the ground when dies
        else
        {
            yield return new WaitForSeconds(1.5f);
            transform.position = new Vector3(transform.position.x, -2.9f, transform.position.z);
            //enemy.isAttack = false;
            //enemy.isWalking = true;
        }
    }
}
