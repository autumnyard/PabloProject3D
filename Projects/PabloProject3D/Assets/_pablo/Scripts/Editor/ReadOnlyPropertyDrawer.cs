﻿#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace Pablo
{
  [CustomPropertyDrawer(typeof(ShowInInspectorAttribute))]
  public class ShowInInspectorPropertyDrawer : PropertyDrawer
  {
    public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
    {
      return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
      EditorGUI.PropertyField(position, property, label, true);
      GUI.enabled = true;
    }
  }

  [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
  public class ReadOnlyPropertyDrawer : PropertyDrawer
  {
    public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
    {
      return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
      GUI.enabled = false;
      EditorGUI.PropertyField(position, property, label, true);
      GUI.enabled = true;
    }
  }

  [CustomPropertyDrawer(typeof(DisableInPlayModeAttribute))]
  public class DisableInPlayModePropertyDrawer : PropertyDrawer
  {
    public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
    {
      return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
      if (EditorApplication.isPlaying)
      {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
      }
      else
      {
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
      }
    }
  }

  [CustomPropertyDrawer(typeof(DisableInEditorModeAttribute))]
  public class DisableInEditorModePropertyDrawer : PropertyDrawer
  {
    public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
    {
      return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
      if (!EditorApplication.isPlaying)
      {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
      }
      else
      {
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
      }
    }
  }

}
#endif