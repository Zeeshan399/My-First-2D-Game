using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private GameObject arrow;
    private Animator anim;
    private PlayerMovement playerMovement;
    private float coolDowntimer = Mathf.Infinity;
    private float timecount;
    private GameObject cloneArrows;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

   
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && coolDowntimer > attackCooldown && playerMovement.canAttack() && playerMovement.totalArrows > 0)
        {
            Attack();
            --playerMovement.totalArrows;   //Number of arrow decreases with each attack by the player
        }
        else
        {
            //Debug.Log("No More Arrows");           
        }
        coolDowntimer += Time.deltaTime;
    }

    private void Attack()
    {
        anim.SetTrigger("attack");
        coolDowntimer = 0;             
        cloneArrows = Instantiate(arrow);        
        cloneArrows.transform.position = shootingPoint.position;        
        cloneArrows.GetComponent<projectile>().setDirection(Mathf.Sign(transform.localScale.x));
        Destroy(cloneArrows, 500);        //will destroy the arrow after 5 seconds
    }

}
