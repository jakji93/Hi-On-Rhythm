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

   [SerializeField] private GameObject bullet;



    // Start is called before the first frame update
    void Start()
    {
        //bulletRB = GetComponent<Rigidbody2D>();
        var newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        newBullet.transform.up = getMouseDir();
        Destroy(this.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        //bulletRB.MovePosition(bulletRB.transform.position + bulletDir * bulletSpeed * Time.deltaTime);

        if (curDur <= atk1Dur)
        {
            curDur++;
        }
        else
        {
            //Destroy(this.gameObject);
        }

    }

    private Vector3 getMouseDir()
    {
        //Vector3 mousePos = Input.mousePosition;
        //mousePos.z = Camera.main.nearClipPlane;
        //return Camera.main.ScreenToWorldPoint(mousePos);

        Vector2 playerPos;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        playerPos = PlayerControl.Instance.transform.position;

        Vector3 aimDir =  mousePos - (Vector2)transform.position;

        return (aimDir.normalized);

       
    }

}
