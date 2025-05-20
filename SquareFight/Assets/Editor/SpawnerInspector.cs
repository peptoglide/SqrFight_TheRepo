using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Spawner))]
public class SpawnerInspector : Editor
{
    private float newProbabilityTotal = 100f;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        float sum_probability = 0f;
        Spawner spawner = (Spawner)target;

        foreach (SpawnObj item in spawner.spawns)
        {
            sum_probability += item.probability;
        }
        EditorGUILayout.LabelField($"Probability sum: {sum_probability}");

        newProbabilityTotal = EditorGUILayout.FloatField("New Total", newProbabilityTotal);

        if (GUILayout.Button("Shrink"))
        {
            ShrinkTo(sum_probability, newProbabilityTotal);
        }
    }

    void ShrinkTo(float currentSum, float newSum)
    {
        Spawner spawner = (Spawner)target;
        foreach (SpawnObj item in spawner.spawns)
        {
            item.probability = item.probability / currentSum * newSum;
        }
    }
}
