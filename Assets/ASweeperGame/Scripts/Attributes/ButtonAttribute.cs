using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[AttributeUsage(AttributeTargets.Method)]
public class ButtonAttribute : Attribute { }

#if UNITY_EDITOR

// Works for MonoBehaviours
[CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
public class ButtonDrawerMono : ButtonDrawerBase { }

// Works for ScriptableObjects
[CustomEditor(typeof(ScriptableObject), true), CanEditMultipleObjects]
public class ButtonDrawerSO : ButtonDrawerBase { }

public abstract class ButtonDrawerBase : Editor
{
    private readonly Dictionary<string, object> _paramCache = new();

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var targetType = target.GetType();
        var methods = targetType.GetMethods(
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Instance |
            BindingFlags.Static);

        foreach (var method in methods)
        {
            if (method.GetCustomAttribute<ButtonAttribute>() == null)
                continue;

            var parameters = method.GetParameters();
            string methodName = ObjectNames.NicifyVariableName(method.Name);

            if (parameters.Length == 0)
            {
                if (GUILayout.Button(methodName))
                    method.Invoke(method.IsStatic ? null : target, null);
            }
            else if (parameters.Length == 1)
            {
                var param = parameters[0];
                string key = $"{target.GetInstanceID()}.{method.Name}";

                if (!_paramCache.ContainsKey(key))
                {
                    if (param.ParameterType == typeof(int)) _paramCache[key] = 0;
                    else if (param.ParameterType == typeof(float)) _paramCache[key] = 0f;
                    else if (param.ParameterType == typeof(bool)) _paramCache[key] = false;
                    else if (typeof(UnityEngine.Object).IsAssignableFrom(param.ParameterType))
                        _paramCache[key] = null;
                }

                // ---- BOOL ----
                if (param.ParameterType == typeof(bool))
                {
                    _paramCache[key] = EditorGUILayout.Toggle(methodName, (bool)_paramCache[key]);

                    if (GUILayout.Button(methodName))
                        method.Invoke(method.IsStatic ? null : target, new object[] { (bool)_paramCache[key] });
                }

                // ---- INT ----
                else if (param.ParameterType == typeof(int))
                {
                    _paramCache[key] = EditorGUILayout.IntField(methodName, (int)_paramCache[key]);

                    if (GUILayout.Button(methodName))
                        method.Invoke(method.IsStatic ? null : target, new object[] { (int)_paramCache[key] });
                }

                // ---- FLOAT ----
                else if (param.ParameterType == typeof(float))
                {
                    _paramCache[key] = EditorGUILayout.FloatField(methodName, (float)_paramCache[key]);

                    if (GUILayout.Button(methodName))
                        method.Invoke(method.IsStatic ? null : target, new object[] { (float)_paramCache[key] });
                }

                // ---- UNITY OBJECT (MonoBehaviour, ScriptableObject, etc) ----
                else if (typeof(UnityEngine.Object).IsAssignableFrom(param.ParameterType))
                {
                    _paramCache[key] = EditorGUILayout.ObjectField(
                        methodName,
                        _paramCache[key] as UnityEngine.Object,
                        param.ParameterType,
                        true
                    );

                    if (GUILayout.Button(methodName))
                        method.Invoke(method.IsStatic ? null : target, new object[] { _paramCache[key] });
                }
            }
        }
    }
}

#endif