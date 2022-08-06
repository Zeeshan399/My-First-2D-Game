using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    [SerializeField] private float speed;
    private bool isDead = false;
    public bool isAttack = false;
    public bool isWalking = true;
    private Animator anim;
    [SerializeField] private GameObject player;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if(!player)
        {
            isWalking = true;
            anim.SetBool("isWalking", isWalking);
            Debug.Log("Enemy Dead ab false hojani chahye condition"+isAttack);
        }
        
        if (isAttack == false)
        {
            if (transform.localScale == new Vector3(2, 2, 2))
            {
                transform.Translate((speed * Time.deltaTime), 0, 0);
            }
            else
            {
                transform.Translate(-(speed * Time.deltaTime), 0, 0);
            }
        }
        anim.SetBool("isDead", isDead);
        anim.SetBool("isAttack", isAttack);
        
        if(isDead == true)
        {
            speed = 0;
            Destroy(this.GetComponent<BoxCollider2D>());
            Destroy(this.gameObject, 3);            
        }


        //The enemy follows the player
        if (transform.position.x > player.transform.position.x)
        {
            transform.localScale = new Vector3(-2, 2, 2);
        }
        else
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            isDead = true;
        }
        if(collision.gameObject.CompareTag("Player"))
        {
            isAttack = true;
            transform.Translate(0, 0, 0);
        }    
        if(collision.gameObject.CompareTag("wall"))
        {
            transform.localScale = new Vector3(-transform.localScale.x, 2, 2);
        }
    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        isAttack = false;
    //        //this.GetComponent<Rigidbody>().AddForce(speed * Time.deltaTime, 0, 0);
    //        //transform.Translate((speed * Time.deltaTime), 0, 0);
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("door"))
    //    {
    //        transform.localScale = new Vector3(2, 2, 2);
    //    }
    //}
}
