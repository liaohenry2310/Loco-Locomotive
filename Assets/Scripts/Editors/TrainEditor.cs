using UnityEditor;
using GamePlay;
using UnityEngine;

[CustomEditor(typeof(Train))]
public class TrainEditor : Editor
{
    [HideInInspector, SerializeField] private TrainData _trainData;

    private void OnEnable()
    {
        _trainData = serializedObject.FindProperty("_trainData").objectReferenceValue as TrainData;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        GUIStyle labelStyle = new GUIStyle(EditorStyles.label)
        {
            fontStyle = FontStyle.BoldAndItalic,
            fontSize = 20
        };
        labelStyle.normal.textColor = Color.white;
        GUILayout.Label("Train properties:", labelStyle);

        _trainData.MaxHealth = EditorGUILayout.FloatField("Max Health", _trainData.MaxHealth);
        _trainData.MaxFuel = EditorGUILayout.FloatField("Max Fuel", _trainData.MaxFuel);
        _trainData.FuelRate = EditorGUILayout.FloatField("Fuel Rate", _trainData.FuelRate);
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(_trainData);
    }

}
