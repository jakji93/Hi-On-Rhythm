using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerControl : MonoBehaviour
{
    public float playerMoveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    Vector2 playerMoveDir;
    Rigidbody2D playerRB;
    List<RaycastHit2D> castCollisions= new List<RaycastHit2D>();
    Vector2 playerMouseDir;
    private playerAim playerAimDir;




    // Start is called before the first frame update
    void Start(){
        playerRB = GetComponent<Rigidbody2D>();
        playerAimDir = GetComponentInChildren<playerAim>();
    }

    // Update is called once per frame
    void Update()
    {
       ////
    }



    private void FixedUpdate(){

        if (playerMoveDir != Vector2.zero) {
            int count = playerRB.Cast(playerMoveDir, movementFilter, castCollisions, playerMoveSpeed * Time.fixedDeltaTime + collisionOffset);

            if (count == 0) { 
            
                playerRB.MovePosition(playerRB.position + playerMoveDir * playerMoveSpeed * Time.fixedDeltaTime);
            }
        }

        //playerMouseDir = getMouseDir();
        //playerAimDir.pointerPos = playerMouseDir;
       

    }

    void OnMove(InputValue playerInput) {
    playerMoveDir = playerInput.Get<Vector2>();

    }

    private Vector2 getMouseDir() {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);    
    }



}
