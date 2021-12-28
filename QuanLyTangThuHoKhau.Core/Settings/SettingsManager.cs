using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuanLyTangThuHoKhau.Core.Settings.Constants;

namespace QuanLyTangThuHoKhau.Core.Settings
{
    //Tao trinh quan ly cac cai dat: https://gist.github.com/aopell/fbb159cd6090cbb6dca861acc32476af
    //Fix loi class ReaderWriterLock: https://stackoverflow.com/questions/9904142/c-sharp-readerwriterlockslim-best-practice-to-avoid-recursion
    public class SettingsManager : ISettingsManager
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string SettingsPath { get; }
        private ReaderWriterLockSlim rwLock { get; }

        private JObject settings;

        public SettingsManager()
        {
            rwLock = new ReaderWriterLockSlim();
            SettingsPath = SettingPaths.DEFAULT_SETTINGS_PATH;
            LoadSettings();
        }

        public SettingsManager(string settingsPath)
        {
            rwLock = new ReaderWriterLockSlim();
            SettingsPath = settingsPath;
            LoadSettings();
        }

        /// <summary>
        /// Creates a JSON file for settings at <see cref="SettingsPath"/>
        /// </summary>
        private void CreateSettingsFile()
        {
            try
            {
                if (!File.Exists(SettingsPath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath));
                    File.Create(SettingsPath).Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException($"The provided {nameof(SettingsPath)} was invalid", ex);
            }
        }

        /// <summary>
        /// Loads the contents of the settings file into <see cref="settings"/>
        /// </summary>
        private void LoadSettings()
        {
            if (settings == null)
            {
                rwLock.EnterWriteLock();

                try
                {
                    CreateSettingsFile();
                    string fileContents = File.ReadAllText(SettingsPath);
                    settings = string.IsNullOrWhiteSpace(fileContents) ? new JObject() : JObject.Parse(fileContents);
                }
                finally
                {
                    rwLock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Add a new setting or update existing setting with the same name
        /// </summary>
        /// <param name="setting">Setting name</param>
        /// <param name="value">Setting value</param>
        public void AddSetting(string setting, object value)
        {
            LoadSettings();

            rwLock.EnterWriteLock();

            try
            {
                if (settings[setting] == null && value != null)
                    settings.Add(setting, JToken.FromObject(value));
                else if (value == null && settings[setting] != null)
                    settings[setting] = null;
                else if (value != null)
                    settings[setting] = JToken.FromObject(value);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Adds a batch of new settings, replacing already existing values with the same name
        /// </summary>
        /// <param name="settings"></param>
        public void AddSettings(Dictionary<string, object> settings)
        {
            foreach (var setting in settings)
            {
                AddSetting(setting.Key, setting.Value);
            }
        }

        /// <summary>
        /// Gets the value of the setting with the provided name as type <c>T</c>
        /// </summary>
        /// <typeparam name="T">Result type</typeparam>
        /// <param name="setting">Setting name</param>
        /// <param name="result">Result value</param>
        /// <returns>True when setting was successfully found, false when setting is not found</returns>
        public bool GetSetting<T>(string setting, out T result)
        {
            result = default(T);

            try
            {
                LoadSettings();

                rwLock.EnterReadLock();

                try
                {
                    if (settings[setting] == null) return false;

                    result = settings[setting].ToObject<T>();
                }
                finally
                {
                    rwLock.ExitReadLock();
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);

                try
                {
                    rwLock.EnterReadLock();

                    try
                    {
                        result = settings[setting].Value<T>();
                    }
                    finally
                    {
                        rwLock.ExitReadLock();
                    }

                    return true;
                }
                catch (Exception ex2)
                {
                    Log.Error(ex2);
                    return false;
                }
            }
        }

        /// <summary>
        /// Returns a dictionary of all settings
        /// </summary>
        /// <returns></returns>
        public bool GetSettings(Dictionary<string, object> values) => GetSettings(out values);

        /// <summary>
        /// Returns a dictionary of all settings, where all settings are of the same type
        /// </summary>
        /// <typeparam name="TValue">The type of all of the settings</typeparam>
        /// <returns></returns>
        public bool GetSettings<TValue>(out Dictionary<string, TValue> values)
        {
            try
            {
                LoadSettings();

                rwLock.EnterReadLock();

                try
                {
                    values = settings.ToObject<Dictionary<string, TValue>>();
                    return true;
                }
                finally
                {
                    rwLock.ExitReadLock();
                }
            }
            catch
            {
                values = null;
                return false;
            }
        }

        /// <summary>
        /// Saves all queued settings to <see cref="SettingsPath"/>
        /// </summary>
        public void SaveSettings()
        {
            rwLock.EnterWriteLock();

            try
            {
                File.WriteAllText(SettingsPath, settings.ToString(Formatting.Indented));
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }


        /// <summary>
        /// Completely delete the settings file at <see cref="SettingsPath"/>. This action is irreversable.
        /// </summary>
        public void DeleteSettings()
        {
            rwLock.EnterWriteLock();

            try
            {
                File.Delete(SettingsPath);
                settings = null;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }
    }
}