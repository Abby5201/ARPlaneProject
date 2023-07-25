using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;

[CanEditMultipleObjects,CustomEditor(typeof(ExToggle),true)]
public class ExToggleEditor : ToggleEditor
{
    private SerializedProperty textNormalColor;
    private SerializedProperty textSelectedColor;
    private SerializedProperty useImageColor;
    private SerializedProperty imageSelectColor;

    public Color color;

    [MenuItem("GameObject/UI/ExToggle")]
    static void Create()
    {
        GameObject go = new GameObject("ExToggle",typeof(Image),typeof(ExToggle));
        go.GetComponent<Image>().type = Image.Type.Simple;
        if (Selection.activeGameObject is GameObject o)
        {
            go.transform.SetParent(o.transform);
            go.transform.localPosition = Vector3.zero;
        }
        GameObject graphic = new GameObject("Checkmark", typeof(Image));
        graphic.transform.SetParent(go.transform, false);
        GameObject text = new GameObject("Text", typeof(Text));
        text.transform.SetParent(go.transform,false);
        EditorUtility.SetDirty(go);
    }

    private void OnEnable()
    {
        base.OnEnable();
        textNormalColor = serializedObject.FindProperty("textNormalColor");
        textSelectedColor = serializedObject.FindProperty("textSelectColor");
        useImageColor = serializedObject.FindProperty("useImageColor");
        imageSelectColor = serializedObject.FindProperty("imageSelectColor");
       //color = EditorGUILayout.ColorField(Color.white);
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        this.serializedObject.Update();
        EditorGUILayout.PropertyField(useImageColor);
        EditorGUILayout.PropertyField(textNormalColor);
        EditorGUILayout.PropertyField(textSelectedColor);
        EditorGUILayout.PropertyField(imageSelectColor);
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }
}
