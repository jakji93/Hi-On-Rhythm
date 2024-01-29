using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class NoteSpawner : EditorWindow
{
   private GameObject notePrefabToSpawn;
   private GameObject parentObject;
   private int distanceBetweenNote = 120;
   private float initialDistance = 0f;
   private int[] dataList = new int[2400];
   private Vector2 scrollPosition = Vector2.zero;

   [MenuItem("Tools/Note Spawner")]
   public static void ShowWindow()
   {
      GetWindow<NoteSpawner>("Note Spawner");
   }

   private void OnGUI()
   {
      notePrefabToSpawn = EditorGUILayout.ObjectField("Note to Spawn:", notePrefabToSpawn, typeof(GameObject), false) as GameObject;
      parentObject = EditorGUILayout.ObjectField("Parent Object:", parentObject, typeof(GameObject), true) as GameObject;
      distanceBetweenNote = EditorGUILayout.IntField("Distance between Note", distanceBetweenNote);
      initialDistance = EditorGUILayout.FloatField("Initial delay", initialDistance);
      scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
      var noteCount = 0;
      var buttonID = "";
      for (int i = 0; i < dataList.Length; i++) {
         if (i == 0) {
            EditorGUILayout.LabelField("0");
            EditorGUILayout.Separator();

            Handles.color = Color.gray;
            Handles.DrawLine(new Vector2(10, GUILayoutUtility.GetLastRect().yMax), new Vector2(position.width - 10, GUILayoutUtility.GetLastRect().yMax));
         }
         if (i % 16 == 0 && i > 0) {
            int number = i / 16;
            EditorGUILayout.LabelField(number.ToString());
         }
         if (i % 4 == 0 && i > 0) {
            EditorGUILayout.Separator();

            Handles.color = Color.gray;
            Handles.DrawLine(new Vector2(10, GUILayoutUtility.GetLastRect().yMax), new Vector2(position.width - 10, GUILayoutUtility.GetLastRect().yMax));
         }
         Color originalBackgroundColor = GUI.backgroundColor;
         if (dataList[i] == 1) {
            noteCount++;
            GUI.backgroundColor = Color.green;
            buttonID = noteCount.ToSafeString();
         }
         if (GUILayout.Button(buttonID, GUILayout.Height(30))) {
            dataList[i] = dataList[i] == 0 ? 1 : 0;
         }
         GUI.backgroundColor = originalBackgroundColor;
         buttonID = "";
      }

      EditorGUILayout.EndScrollView();
      GUILayout.Space(20);

      GUILayout.BeginHorizontal();
      if (GUILayout.Button("Create Notes", GUILayout.Height(30))) {
         DeleteSpawnedObjects();
         SpawnNotes();
      }
      if (GUILayout.Button("Reset Notes", GUILayout.Height(30))) {
         for (int i = 0; i < dataList.Length; i++) {
            dataList[i] = 0;
         }
      }
      if (GUILayout.Button("Scan Notes", GUILayout.Height(30))) {
         ScanNotes();
      }
      GUILayout.EndHorizontal();
   }

   private void SpawnNotes()
   {
      for (int i = 0; i < dataList.Length; i++) {
         if (dataList[i] == 1) {
            GameObject newObject = PrefabUtility.InstantiatePrefab(notePrefabToSpawn) as GameObject;
            newObject.transform.SetParent(parentObject.transform);
            newObject.transform.localPosition = new Vector3(initialDistance + (i * (float)distanceBetweenNote), 0f, 0f);
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

   private void ScanNotes()
   {
      for (int i = 0; i < dataList.Length; i++) {
         dataList[i] = 0;
      }
      for (int i = 0; i < parentObject.transform.childCount; ++i) {
         var position = parentObject.transform.GetChild(i).position;
         var index = Mathf.FloorToInt((position.x - initialDistance) / distanceBetweenNote);
         dataList[index] = 1;
      }
   }
}