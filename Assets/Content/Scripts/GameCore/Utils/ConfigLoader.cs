using UnityEngine;

namespace Content.Scripts.GameCore.Utils
{
    public class ConfigLoader
    {
        public T LoadConfig<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }
    }
}
