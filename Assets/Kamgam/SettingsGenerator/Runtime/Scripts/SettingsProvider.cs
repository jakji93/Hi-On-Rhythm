using UnityEngine;
using UnityEngine.Serialization;

namespace Kamgam.SettingsGenerator
{
    /// <summary>
    /// The Settings Provider creates an instance of the Settings object
    /// and keeps a reference to it so other objects can come and ask
    /// for it at any time.
    /// <br /><br />
    /// It also handles resetting objects before play mode
    /// if Domain-Reload is disabled (via IResetBeforeDomainReload).
    /// </summary>
    [CreateAssetMenu(fileName = "SettingsProvider", menuName = "SettingsGenerator/SettingsProvider", order = 1)]
    public class SettingsProvider : ScriptableObject
#if UNITY_EDITOR
        , IResetBeforeDomainReload
#endif
    {
        /// <summary>
        /// Hold a reference to the last used SettingsProvider.<br />
        /// You should NOT build your code upon this, it may be null (especially before initialization).<br />
        /// However it can be very handy if you know that you are only using one single provider and you need to fetch it quickly.
        /// </summary>
        public static SettingsProvider LastUsedSettingsProvider;

        [SerializeField, Tooltip("The player prefs key under which your settings will be saved.")]
        protected string playerPrefsKey;

        [Tooltip("The default settings asset.\nYou can leave this empty if you define all your settings via script.")]
        [FormerlySerializedAs("Default")]
        public Settings SettingsAsset;

        protected Settings _settings;
        public Settings Settings
        {
            get
            {
                LastUsedSettingsProvider = this;

#if UNITY_EDITOR
                if (!UnityEditor.EditorApplication.isPlaying)
                    return null;
#endif

                if (_settings == null)
                {
                    // Create a new
                    if (SettingsAsset == null)
                    {
                        // Create a new instance from code.
                        _settings = ScriptableObject.CreateInstance<Settings>();
                    }
                    else
                    {
                        // Create from asset.
                        _settings = ScriptableObject.Instantiate(SettingsAsset);
                    }

                    // Make a global backup copy of the current quality level (we use this in the Connections to restore it later).
                    QualityPresets.AddCurrentLevel();

                    // Load user settings from storage
                    Settings.Load(playerPrefsKey);

                    // Register to setting changed for auto-loading
                    Settings.OnSettingChanged += onSettingChanged;
                }

                return _settings;
            }
        }

        /// <summary>
        /// If turned on then for each change in a setting a save will be SCHEDULED. If for AutoSaveWaitTimeInSec after the last change no further change happens then it will save.
        /// </summary>
        [Tooltip("If turned on then for each change in a setting a save will be SCHEDULED. If for AutoSaveWaitTimeInSec after the last change no further change happens then it will save.")]
        public bool AutoSave = false;

        /// <summary>
        /// Only used if AutoSave is turned on. If for AutoSaveWaitTimeInSec after the last change no further change happens then it will save.
        /// </summary>
        [Tooltip("Only used if AutoSave is turned on. If for AutoSaveWaitTimeInSec after the last change no further change happens then it will save.")]
        public float AutoSaveWaitTimeInSec = 1.0f;

        /// <summary>
        /// Use this to check whether or not the settings have loaded.
        /// </summary>
        public bool HasSettings()
        {
            return _settings != null;
        }

        public void OnEnable()
        {
            if (string.IsNullOrEmpty(playerPrefsKey))
            {
                playerPrefsKey = "SGSettings";
            }
        }

#if UNITY_EDITOR
        // Domain Reload handling
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        protected static void onResetBeforePlayMode()
        {
            DomainReloadUtils.CallOnResetOnAssets(typeof(SettingsProvider));
        }

        public void ResetBeforePlayMode()
        {
            _settings = null;
        }
#endif

        public void Reset()
        {
            if(Settings != null)
                Settings.Reset();
        }

        public void Reset(params string[] ids)
        {
            Settings.Reset(ids);
        }

        public void ResetGroups(params string[] groups)
        {
            Settings.ResetGroups(groups);
        }

        public void ResetGroup(string group)
        {
            Settings.ResetGroups(group);
        }

        public void Apply()
        {
            Settings.Apply();
        }


        // Load & Save

        public void Load()
        {
            if (_settings == null)
            {
                // At the very first load this will be executed.

                // Accessing the "Settings" getter for the very first time
                // causes a load automatically, thus we do not need to load
                // anything here.
                Settings.RefreshRegisteredResolvers();
            }
            else
            {
                // Pull values from connections to initialize the default values.
                Settings.PullFromConnections();

                // Load user settings from storage
                // Also triggers resolver updates (aka Settings.RefreshRegisteredResolvers())
                Settings.Load(playerPrefsKey);
            }
        }

        public void ResetToLastSave()
        {
            // Load user settings from storage
            // Also triggers resolver updates (aka Settings.RefreshRegisteredResolvers())
            Settings.Load(playerPrefsKey);
        }

        public void Save()
        {
            Settings.Save(playerPrefsKey);
        }

        public void Delete()
        {
            if (Settings != null)
            {
                Settings.Delete(playerPrefsKey);
            }
            else
            {
                // This only exists to support deleting in Editor if not in play mode.
                // Notice the static use of Settings.
                Settings.DeletePlayerPrefs(playerPrefsKey);
            }
        }

        // Auto Save

        protected void onSettingChanged(ISetting setting)
        {
            if (AutoSave)
            {
                ScheduleAutoSave(AutoSaveWaitTimeInSec);
            }
        }

        [System.NonSerialized]
        protected float _autoSaveTime = -1f;

        /// <summary>
        /// If for AutoSaveWaitTimeInSec after the last change no further change happens then it will save.
        /// </summary>
        public void ScheduleAutoSave(float autoSaveWaitTimeInSec)
        {
            if (_autoSaveTime < 0f)
            {
                _autoSaveTime = Time.realtimeSinceStartup + autoSaveWaitTimeInSec;
                scheduleAutoSaveAsync();
            }
            else
            {
                _autoSaveTime = Time.realtimeSinceStartup + autoSaveWaitTimeInSec;
            }
        }

        protected async void scheduleAutoSaveAsync()
        {
            // Wait for the timer to run out.
            float deltaTime = _autoSaveTime - Time.realtimeSinceStartup;
            while (deltaTime > 0)
            {
                await System.Threading.Tasks.Task.Delay(Mathf.RoundToInt(deltaTime * 1000) + 50);
                deltaTime = _autoSaveTime - Time.realtimeSinceStartup;
            }

            Save();
            _autoSaveTime = -1f;
        }
    }
}
