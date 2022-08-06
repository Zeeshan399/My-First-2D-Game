using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController cam;
    [SerializeField] private GameObject wall;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (collision.transform.position.x < transform.position.x)
            {
                cam.MoveToNewRoom(nextRoom);
            }
            else
            {
                cam.MoveToNewRoom(previousRoom);
            }
            StartCoroutine(setActiveWall());
            
        }
    }
    IEnumerator setActiveWall()
    {
        yield return new WaitForSeconds(1.0f);
        wall.SetActive(true);
    }
    
}
