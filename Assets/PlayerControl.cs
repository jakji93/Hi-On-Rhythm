using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.WSA;

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

    private float playerStam =100f; 
    private float playerDashSpeed = 10f;
    private bool dashPress = false;

    



    // Start is called before the first frame update
    void Start(){
        playerRB = GetComponent<Rigidbody2D>();
        playerAimDir = GetComponentInChildren<playerAim>();

    }

    // Update is called once per frame
    void Update()
    {
        if (playerStam <100)
        {
            playerStam++;
        }

    }

    private void FixedUpdate(){


        if (playerMoveDir != Vector2.zero) 
        {

            int count = playerRB.Cast(playerMoveDir, movementFilter, castCollisions, playerMoveSpeed * Time.fixedDeltaTime + collisionOffset);

            if (count == 0 && !dashPress) {

                MovePlayer(playerRB.position,playerMoveDir,playerMoveSpeed);
            }

            else if (count == 0 && dashPress)
            {
                int count2 = playerRB.Cast(playerMoveDir, movementFilter, castCollisions, playerDashSpeed * Time.fixedDeltaTime + collisionOffset);

                Debug.DrawLine(playerRB.position, playerRB.position + playerMoveDir * playerDashSpeed, Color.red, 4);

                if (count2 == 0)
                {
                    MovePlayer(playerRB.position, playerMoveDir, playerDashSpeed);
                    dashPress = false;
                }
                else
                {

                    RaycastHit2D hitRay = Physics2D.Raycast(playerRB.position, playerMoveDir);

                    Debug.DrawLine(playerRB.position, hitRay.point - playerRB.position * hitRay.fraction, Color.green,4);

                    //playerRB.MovePosition(hitRay.point - playerRB.position.normalized * hitRay.fraction);
                    playerRB.MovePosition(playerRB.position + playerMoveDir * hitRay.fraction *.3f);


                    print(hitRay.fraction);
                    dashPress = false;
                    
                }
            }
            else if (count != 0 && dashPress)
            {
                dashPress = false;
                playerStam = 0;
                print("wall");
            }




        }
        


    
    }

    void OnMove(InputValue playerInput) {
         playerMoveDir = playerInput.Get<Vector2>().normalized;

    }

    void OnSpace(InputValue playerInput)
    {

        if (playerStam == 100)
        {
            dashPress = true;
            playerStam = 0;
        }
        else 
        {
            dashPress = false;
        }

    }

    void MovePlayer(Vector2 curPos, Vector2 moveDir ,float moveSpeed)
    {
        playerRB.MovePosition(curPos + moveDir * moveSpeed * Time.fixedDeltaTime);

    }


    private Vector2 getMouseDir() {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);    
    }



    



}
