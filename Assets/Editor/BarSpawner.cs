using UnityEditor;
using UnityEngine;

public class BarSpawner : EditorWindow
{
   private GameObject prefabToSpawn;
   private int numberOfColumn = 5; // Default number of objects to spawn
   private int numberOfRow = 5;
   private GameObject parentObject;
   private int distanceBetweenColumn = 480;
   private int distanceBetweenRow = 480;

   [MenuItem("Tools/Grid Spawner")]
   public static void ShowWindow()
   {
      GetWindow(typeof(BarSpawner));
   }

   private void OnGUI()
   {
      GUILayout.Label("Bar Spawner", EditorStyles.boldLabel);

      prefabToSpawn = EditorGUILayout.ObjectField("Bar to Spawn:", prefabToSpawn, typeof(GameObject), false) as GameObject;
      numberOfColumn = EditorGUILayout.IntField("Number of Columns:", numberOfColumn);
      numberOfRow = EditorGUILayout.IntField("Number of Rows:", numberOfRow);
      parentObject = EditorGUILayout.ObjectField("Parent Object:", parentObject, typeof(GameObject), true) as GameObject;
      distanceBetweenColumn = EditorGUILayout.IntField("Distance between Column", distanceBetweenColumn);
      distanceBetweenRow = EditorGUILayout.IntField("Distance between Row", distanceBetweenRow);

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

      for (int j = 0; j < numberOfRow; j++) {
         for (int i = 0; i < numberOfColumn; i++) {
            GameObject newObject = PrefabUtility.InstantiatePrefab(prefabToSpawn) as GameObject;
            newObject.transform.SetParent(parent.transform);
            newObject.transform.localPosition = new Vector3(i * (float)distanceBetweenColumn, j * (float)distanceBetweenRow, 0f);
            Undo.RegisterCreatedObjectUndo(newObject, "Spawned Object");
         }
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
