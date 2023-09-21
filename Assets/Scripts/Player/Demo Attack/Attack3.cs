using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack3 : MonoBehaviour
{
   [SerializeField] private GameObject bullet;

   // Start is called before the first frame update
   void Start()
   {
      var newBullet = Instantiate(bullet, transform.position, transform.rotation);
      var newBullet1 = Instantiate(bullet, transform.position, transform.rotation * Quaternion.Euler(0f, 0f, 90f));
      var newBullet2 = Instantiate(bullet, transform.position, transform.rotation * Quaternion.Euler(0f, 0f, 180f));
      var newBullet3 = Instantiate(bullet, transform.position, transform.rotation * Quaternion.Euler(0f, 0f, 270f));
      Destroy(this.gameObject);
   }
}
