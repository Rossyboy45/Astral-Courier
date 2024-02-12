using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHMove : MonoBehaviour
{
    [SerializeField] Transform camPos;

    // Update is called once per frame
    void Update()
    {
       //moves the camera holder position as to not mess with rigid body
       transform.position = camPos.position;
    }
}
