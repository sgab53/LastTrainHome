using UnityEngine;
using UnityEngine.UIElements;

namespace LTH.UI
{
    public static class VisualElementExtensions
    {
        public static void LoadVisualTreeAsset<T>(this T self, string name = null) where T : VisualElement
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;

            Resources.Load<VisualTreeAsset>(name).CloneTree(self);
        }
    }
}