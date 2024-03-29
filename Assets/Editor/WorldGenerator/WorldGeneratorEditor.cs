using UnityEditor;

[CustomEditor(typeof(WorldGenerator))]
public class WorldGeneratorEditor : Editor
{
    private bool prefabsFoldout;
    private SerializedProperty groundColliderPrefab;
    private SerializedProperty rowPrefab;
    private SerializedProperty colliderPrefab;
    private SerializedProperty mapObjectPrefab;

    private bool containersFoldout;
    private SerializedProperty groundColliderContainer;
    private SerializedProperty rowContainer;
    private SerializedProperty mapObjectContainer;
    private SerializedProperty colliderContainer;

    private bool settingsFoldout = true;
    private SerializedProperty startLength;
    private SerializedProperty groundSize;
    private SerializedProperty heightOffset;
    private SerializedProperty maxGroundHeight;
    private SerializedProperty z;
    private SerializedProperty generatePosition;
    private SerializedProperty jumpGroundChance;
    private SerializedProperty effectStructureChance;
    private SerializedProperty minStructureOffset;

    private void OnEnable()
    {
        groundColliderPrefab = serializedObject.FindProperty("groundColliderPrefab");
        rowPrefab = serializedObject.FindProperty("rowPrefab");
        colliderPrefab = serializedObject.FindProperty("colliderPrefab");
        mapObjectPrefab = serializedObject.FindProperty("mapObjectPrefab");

        groundColliderContainer = serializedObject.FindProperty("groundColliderContainer");
        rowContainer = serializedObject.FindProperty("rowContainer");
        mapObjectContainer = serializedObject.FindProperty("mapObjectContainer");
        colliderContainer = serializedObject.FindProperty("colliderContainer");

        startLength = serializedObject.FindProperty("StartLength");
        groundSize = serializedObject.FindProperty("GroundSize");
        heightOffset = serializedObject.FindProperty("HeightOffset");
        maxGroundHeight = serializedObject.FindProperty("MaxGroundHeight");
        z = serializedObject.FindProperty("Z");
        generatePosition = serializedObject.FindProperty("GeneratePosition");
        jumpGroundChance = serializedObject.FindProperty("jumpGroundChance");
        effectStructureChance = serializedObject.FindProperty("effectStructureChance");
        minStructureOffset = serializedObject.FindProperty("MinStructureOffset");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        prefabsFoldout = EditorGUILayout.Foldout(prefabsFoldout, "Prefabs");
        if (prefabsFoldout)
        {
            EditorGUI.indentLevel = 1;
            EditorGUILayout.PropertyField(groundColliderPrefab);
            EditorGUILayout.PropertyField(rowPrefab);
            EditorGUILayout.PropertyField(colliderPrefab);
            EditorGUILayout.PropertyField(mapObjectPrefab);
            EditorGUI.indentLevel = 0;
        }

        containersFoldout = EditorGUILayout.Foldout(containersFoldout, "Containers");
        if (containersFoldout)
        {
            EditorGUI.indentLevel = 1;
            EditorGUILayout.PropertyField(groundColliderContainer);
            EditorGUILayout.PropertyField(rowContainer);
            EditorGUILayout.PropertyField(mapObjectContainer);
            EditorGUILayout.PropertyField(colliderContainer);
            EditorGUI.indentLevel = 0;
        }

        settingsFoldout = EditorGUILayout.Foldout(settingsFoldout, "Settings");
        if (settingsFoldout)
        {
            EditorGUI.indentLevel = 1;
            EditorGUILayout.PropertyField(startLength);
            EditorGUILayout.PropertyField(groundSize);
            EditorGUILayout.PropertyField(heightOffset);
            EditorGUILayout.PropertyField(maxGroundHeight);
            EditorGUILayout.PropertyField(z);
            EditorGUILayout.PropertyField(generatePosition);
            EditorGUILayout.PropertyField(jumpGroundChance);
            EditorGUILayout.PropertyField(effectStructureChance);
            EditorGUILayout.PropertyField(minStructureOffset);
            EditorGUI.indentLevel = 0;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
