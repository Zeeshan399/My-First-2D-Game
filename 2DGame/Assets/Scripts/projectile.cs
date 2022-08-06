using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    [SerializeField] private float speed;               //speed of the arrow,provides in inspector
    private bool hit;                                   //true when arrow collides with enemy or wall etc
    private EdgeCollider2D edgecollider;
    private float direction;
    private Animator anim;
    

    private void Awake()
    {

    }

    private void Update()
    {        
        if(hit)
        {
            //this.transform.position = new Vector3(this.transform.position.x, -3.08f, 0);
            //this.transform.position = new Vector3(this.transform.position.x, -2.7f/*collision.transform.position.y*/, 0);
            return;     //update will not run again
        }
        float movementSpeed = speed * Time.deltaTime * direction;       
        transform.Translate(movementSpeed, 0, 0);                       //Arrow translating
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Square" || collision.gameObject.CompareTag("wall") || collision.gameObject.CompareTag("Enemy")) //if arrow hits wall or enemy, hit becomes true...and return in update so translation stops
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Destroy(this.gameObject, 1);
                //this.transform.parent = collision.transform;
                this.transform.position = new Vector3(this.transform.position.x+0.5f, -3.08f, 0);
                
            }
            hit = true;
        }
    }

    public void setDirection(float _direction)
    {       
        direction = _direction;       
        gameObject.SetActive(true);       
        hit = false;      
        //boxcollider.enabled = true;      
        float localScaleX = -transform.localScale.x;       
        if (Mathf.Sign(localScaleX) != _direction)
        {
            localScaleX = -localScaleX;
        }
        transform.localScale = new Vector3(-localScaleX, transform.localScale.y, transform.localScale.z);
    }
}
