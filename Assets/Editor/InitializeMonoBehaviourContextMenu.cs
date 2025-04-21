using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace LTH.Editor
{
    public static class InitializeMonoBehaviourContextMenu
    {
        private const string TargetMethodName = "InitializeMonoBehaviour";

        private static void Initialize(GameObject gameObject)
        {
            var behaviours = gameObject.GetComponents<MonoBehaviour>();
            var len = behaviours.Length;

            for (var i = 0; i < len; ++i)
            {
                Initialize(behaviours[i]);
            }
        }

        private static void Initialize(MonoBehaviour target)
        {
            var method = target.GetType().GetMethod(
                TargetMethodName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (method == null)
                return;

            method.Invoke(target, null);
            EditorUtility.SetDirty(target);
        }

        [MenuItem("CONTEXT/MonoBehaviour/Initialize MonoBehaviour", false, 0)]
        private static void Initialize(MenuCommand command)
        {
            var target = command.context as MonoBehaviour;

            if (!target)
                return;

            Initialize(target);
        }

        [MenuItem("LTH Tools/Initialize All MonoBehaviours")]
        private static void InitializeAll()
        {
            var objects =
                Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            var len = objects.Length;

            for (var i = 0; i < len; ++i)
            {
                Initialize(objects[i]);
            }
        }

        [MenuItem("GameObject/Initialize Selected", false, 0)]
        private static void InitializeSelected()
        {
            var objects = Selection.objects;
            var len = objects.Length;

            for (var i = 0; i < len; ++i)
            {
                if (objects[i] is GameObject obj)
                    Initialize(obj);
            }
        }
    }
}
