using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   [SerializeField] private CharacterController cControler;
   [SerializeField] private Vector3 playerVelocity;
   [SerializeField] private bool groundedPlayer;
   [SerializeField] private float playerMinSpeed = 2.0f;
   [SerializeField] private float playerMaxSpeed = 10.0f;
   [SerializeField] private float jumpHeight = 1.0f;
   [SerializeField] private float gravityValue = -9.81f;
   
   private void Start()
   {
       cControler = GetComponent<CharacterController>();
   }

   private void Update()
   {
        groundedPlayer = cControler.isGrounded;
         if (groundedPlayer && playerVelocity.y < 0)
         {
              playerVelocity.y = 0f;
         }
    
         Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
         //lerp between playerMinSpeed and playerMaxSpeed speed based in the time the player is moving.
         // Todo: change the lerp to a more realistic movement.
         cControler.Move(move * Time.deltaTime * Mathf.Lerp(playerMinSpeed,playerMaxSpeed,move.magnitude));

         if (move != Vector3.zero)
         {
              gameObject.transform.forward = move;
         }
    
         // Changes the height position of the player..
         if (Input.GetButtonDown("Jump") && groundedPlayer)
         {
              playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
         }
    
         playerVelocity.y += gravityValue * Time.deltaTime;
         cControler.Move(playerVelocity * Time.deltaTime);
   }
}
