using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack1 : MonoBehaviour
{
   [SerializeField] private Collider2D spawnArea;
   [SerializeField] private GameObject swordFall;
   [SerializeField] private int minSwordserFall;
   [SerializeField] private int maxSwordserFall;
   [SerializeField] private float interval;
   [SerializeField] private float lifeTime;

   private float timer = 0f;

   private void Start()
   {
      transform.parent = null;
      transform.position = Vector3.zero;
      timer = interval;
      Destroy(gameObject, lifeTime);
   }

   private void Update()
   {
      timer += Time.deltaTime;
      if (timer > interval) {
         timer = 0f;
         int numOfSpawn = Random.Range(minSwordserFall, maxSwordserFall);
         for(int i = 0; i < numOfSpawn; i++) {
            var position = GetRandomPointInCollider(spawnArea);
            Instantiate(swordFall, position, Quaternion.identity);
         }
      }
   }

   private Vector2 GetRandomPointInCollider(Collider2D collider, float offset = 1f)
   {
      Bounds bounds = collider.bounds;
      Vector2 minBounds = new Vector2(bounds.min.x + offset, bounds.min.y + offset);
      Vector2 maxBounds = new Vector2(bounds.max.x - offset, bounds.max.y - offset);

      float randomX = Random.Range(minBounds.x, maxBounds.x);
      float randomY = Random.Range(minBounds.y, maxBounds.y);

      return new Vector2(randomX, randomY);
   }
}
