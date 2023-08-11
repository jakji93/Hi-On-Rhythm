using UnityEditor;
using UnityEngine;

public class BarSpawner : EditorWindow
{
   private GameObject prefabToSpawn;
   private int numberOfObjects = 5; // Default number of objects to spawn
   private GameObject parentObject;
   private int distance = 480;

   [MenuItem("Tools/Bar Spawner")]
   public static void ShowWindow()
   {
      GetWindow(typeof(BarSpawner));
   }

   private void OnGUI()
   {
      GUILayout.Label("Bar Spawner", EditorStyles.boldLabel);

      prefabToSpawn = EditorGUILayout.ObjectField("Bar to Spawn:", prefabToSpawn, typeof(GameObject), false) as GameObject;
      numberOfObjects = EditorGUILayout.IntField("Number of Objects:", numberOfObjects);
      parentObject = EditorGUILayout.ObjectField("Parent Object:", parentObject, typeof(GameObject), true) as GameObject;
      distance = EditorGUILayout.IntField("Distance between", distance);

      if (GUILayout.Button("Spawn Objects")) {
         DeleteSpawnedObjects();
         SpawnObjects();
      }
   }

   private void SpawnObjects()
   {
      if (prefabToSpawn == null) {
         Debug.LogWarning("Prefab not selected.");
         return;
      }

      GameObject parent = parentObject;
      if (parent == null) {
         parent = new GameObject("SpawnedObjects");
         Undo.RegisterCreatedObjectUndo(parent, "Created Parent Object");
      }

      for (int i = 0; i < numberOfObjects; i++) {
         GameObject newObject = PrefabUtility.InstantiatePrefab(prefabToSpawn) as GameObject;
         newObject.transform.SetParent(parent.transform);
         newObject.transform.localPosition = new Vector3(i * (float)distance, 0f, 0f);
         Undo.RegisterCreatedObjectUndo(newObject, "Spawned Object");
      }
   }

   private void DeleteSpawnedObjects()
   {
      if (parentObject != null) {
         for (int i = parentObject.transform.childCount - 1; i >= 0; i--) {
            Undo.DestroyObjectImmediate(parentObject.transform.GetChild(i).gameObject);
         }
      }
   }
}
