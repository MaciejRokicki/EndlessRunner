using UnityEditor;

[CustomEditor(typeof(StructureTrigger))]
public class StructureTriggerEditor : Editor
{
    private SerializedProperty strategyType;
    private SerializedProperty movementSpeed;

    void OnEnable()
    {
        strategyType = serializedObject.FindProperty("StrategyType");

        movementSpeed = serializedObject.FindProperty("MovementSpeed");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(strategyType);
        switch(strategyType.enumValueIndex)
        {
            case (int)StructureTriggerStrategyType.MovementSpeedStrategy:
                EditorGUILayout.PropertyField(movementSpeed);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
