using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace QuanLyTangThuHoKhau.Core.Ultis
{
    //https://stackoverflow.com/a/129395/7182661

    public static class ObjectCloneExtensionMethod
    {
        public static T DeepClone<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
    }
}