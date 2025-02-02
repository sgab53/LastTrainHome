using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class InitializeMonoBehaviourContextMenu
    {
        [MenuItem("CONTEXT/MonoBehaviour/Initialize MonoBehaviour", false, 0)]
        private static void Initialize(MenuCommand command)
        {
            var target = command.context as MonoBehaviour;

            if (!target)
                return;

            var method = target.GetType().GetMethod(
                "InitializeMonoBehaviour",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (method != null)
                method.Invoke(target, null);
        }

    }
}
