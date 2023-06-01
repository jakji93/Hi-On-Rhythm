using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2 : MonoBehaviour
{

    Rigidbody2D bulletRB;
    Vector3 bulletDir;


    // Start is called before the first frame update
    void Start()
    {
        bulletRB = GetComponent<Rigidbody2D>();
        bulletDir = getMouseDir();
        
    }

    // Update is called once per frame
    void Update()
    {
        bulletRB.MovePosition(bulletRB.transform.position + bulletDir);
        
    }

    private Vector3 getMouseDir()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

}
