using UnityEditor;
using UnityEngine;

public class AudioWaveformEditor : EditorWindow
{
    private static AudioSource audioSource;
    private bool isPlaying;
    private float currentTime = 0f;
    private bool isScrubbing = false;

    [MenuItem("Window/Audio Waveform Editor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<AudioWaveformEditor>("Audio Waveform Editor");
    }

    private void OnEnable()
    {
        if (audioSource == null)
        {
            audioSource = EditorUtility.CreateGameObjectWithHideFlags("Audio Source", HideFlags.HideAndDontSave, typeof(AudioSource)).AddComponent<AudioSource>();
        }

        isPlaying = false;
        currentTime = 0f;
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Audio Waveform");

        AudioClip audioClip = (AudioClip)EditorGUILayout.ObjectField("Audio Clip", null, typeof(AudioClip), false);

        if (audioClip != null)
        {
            float[] audioData = new float[audioClip.samples];
            audioClip.GetData(audioData, 0);

            const float height = 50f;
            const float yOffset = 70f;
            const float step = 1f;
            float duration = audioClip.length;

            Handles.color = Color.green;
            Vector3 startPoint = new Vector3(0, yOffset, 0);
            for (int i = 1; i < audioData.Length; i++)
            {
                Vector3 start = new Vector3((i - 1) * step, audioData[i - 1] * height + yOffset, 0);
                Vector3 end = new Vector3(i * step, audioData[i] * height + yOffset, 0);
                Handles.DrawLine(start, end);
            }

            EditorGUILayout.Space();

            // Time scale
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(EditorGUIUtility.labelWidth);
            float newTime = EditorGUILayout.Slider(currentTime, 0f, duration);
            if (newTime != currentTime)
            {
                currentTime = newTime;
                if (!isScrubbing)
                {
                    audioSource.time = currentTime;
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(EditorGUIUtility.labelWidth);

            if (GUILayout.Button(isPlaying ? "Pause" : "Play"))
            {
                if (isPlaying)
                {
                    isPlaying = false;
                    audioSource.Pause();
                }
                else
                {
                    isPlaying = true;
                    audioSource.clip = audioClip;
                    audioSource.time = currentTime;
                    audioSource.Play();
                }
            }

            if (GUILayout.Button("Stop"))
            {
                isPlaying = false;
                audioSource.Stop();
                currentTime = 0f;
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    private void Update()
    {
        if (isPlaying && !isScrubbing)
        {
            currentTime = audioSource.time;
            Repaint();
        }
    }

    private void OnDisable()
    {
        // Do not destroy the audioSource here
        // Since it's static, it will persist across different instances of the editor window
    }
}
