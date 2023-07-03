using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2 : MonoBehaviour
{
   [SerializeField] private GameObject bullet;



    // Start is called before the first frame update
    void Start()
    {
        var newBullet = Instantiate(bullet, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
