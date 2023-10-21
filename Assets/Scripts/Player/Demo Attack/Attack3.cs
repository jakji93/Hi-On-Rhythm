using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack3 : MonoBehaviour
{
   [SerializeField] private GameObject bullet;

   // Start is called before the first frame update
   void Start()
   {
      Instantiate(bullet, transform.position, transform.rotation);
      Instantiate(bullet, transform.position, transform.rotation * Quaternion.Euler(0f, 0f, 5f));
      Instantiate(bullet, transform.position, transform.rotation * Quaternion.Euler(0f, 0f, 15f));
      Instantiate(bullet, transform.position, transform.rotation * Quaternion.Euler(0f, 0f, 355f));
      Instantiate(bullet, transform.position, transform.rotation * Quaternion.Euler(0f, 0f, 345f));
      Instantiate(bullet, transform.position, transform.rotation * Quaternion.Euler(0f, 0f, 10f));
      Instantiate(bullet, transform.position, transform.rotation * Quaternion.Euler(0f, 0f, 350f));
      //Instantiate(bullet, transform.position, transform.rotation * Quaternion.Euler(0f, 0f, 315f));
      Destroy(this.gameObject);
   }
}
