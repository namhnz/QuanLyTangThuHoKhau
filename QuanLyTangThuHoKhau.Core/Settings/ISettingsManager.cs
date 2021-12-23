using System.Collections.Generic;

namespace QuanLyTangThuHoKhau.Core.Settings
{
    public interface ISettingsManager
    {
        public void AddSetting(string setting, object value);

        public void AddSettings(Dictionary<string, object> settings);

        public bool GetSetting<T>(string setting, out T result);

        public bool GetSettings(Dictionary<string, object> values) => GetSettings(out values);

        public bool GetSettings<TValue>(out Dictionary<string, TValue> values);

        public void SaveSettings();

        public void DeleteSettings();
    }
}