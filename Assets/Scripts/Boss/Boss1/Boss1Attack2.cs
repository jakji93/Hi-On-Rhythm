using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Attack2 : MonoBehaviour
{
   [SerializeField] private GameObject projectile;
   [SerializeField] private float offset = 0;
   [Min(1)]
   [SerializeField] int numOfProjectile = 1;
   [SerializeField] float rotatingSpeed;

   private void Start()
   {
      var degreeInBtw = (float)360 / numOfProjectile;
      for (int i = 0; i < numOfProjectile; i++) {
         var degreeOfProj = degreeInBtw * i + offset;
         var quart = Quaternion.Euler(0, 0, degreeOfProj);
         Instantiate(projectile, transform.position, quart, transform);
      }
   }

   private void Update()
   {
      var curRotation = transform.rotation.eulerAngles;
      curRotation.z = curRotation.z + rotatingSpeed * Time.deltaTime;
      transform.rotation = Quaternion.Euler(curRotation);
   }

   private void LateUpdate()
   {
      if (transform.childCount == 0) {
         Destroy(gameObject);
      }
   }
}
