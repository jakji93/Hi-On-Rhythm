using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2 : MonoBehaviour
{

    private Rigidbody2D bulletRB;
    private Vector3 bulletDir;
    private float curDur;
    [SerializeField] float bulletSpeed = 30f;
    [SerializeField] private float atk1Dur = 200f;



    // Start is called before the first frame update
    void Start()
    {
        bulletRB = GetComponent<Rigidbody2D>();
        bulletDir = getMouseDir();
        curDur = 0;

    }

    // Update is called once per frame
    void Update()
    {
        bulletRB.MovePosition(bulletRB.transform.position + bulletDir * bulletSpeed * Time.deltaTime);

        if (curDur <= atk1Dur)
        {
            curDur++;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    private Vector3 getMouseDir()
    {
        //Vector3 mousePos = Input.mousePosition;
        //mousePos.z = Camera.main.nearClipPlane;
        //return Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 playerPos;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

        Vector3 aimDir =  mousePos - playerPos;

        return (aimDir.normalized);

       
    }

}
