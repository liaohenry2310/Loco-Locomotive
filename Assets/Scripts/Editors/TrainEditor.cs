using UnityEditor;
using GamePlay;
using UnityEngine;

[CustomEditor(typeof(Train))]
public class TrainEditor : Editor
{
    private TrainData _trainData;
    private void OnEnable()
    {
        _trainData = serializedObject.FindProperty("_trainData").objectReferenceValue as TrainData;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        GUILayout.Label("Train properties:");

        _trainData.MaxHealth = EditorGUILayout.FloatField("Max Health", _trainData.MaxHealth);
        _trainData.MaxFuel = EditorGUILayout.FloatField("Max Fuel", _trainData.MaxFuel);
        _trainData.FuelRate = EditorGUILayout.FloatField("Fuel Rate", _trainData.FuelRate);
        serializedObject.ApplyModifiedProperties();
    }

}
