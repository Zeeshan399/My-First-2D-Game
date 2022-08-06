using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed;
    private float CurrentPosX; //to tell the camera to which position to go
    private Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(CurrentPosX, transform.position.y,
                                                transform.position.z),ref velocity, speed);
    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        CurrentPosX = _newRoom.position.x;
    }
}
