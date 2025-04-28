using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Item))]
public class ItemInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Item item = (Item)target; 
        if(item.type == Item.ItemType.Heal)
        {
            item.healAmount = EditorGUILayout.FloatField("Heal", item.healAmount);
        }
        if (item.type == Item.ItemType.Damage)
        {
            item.damageAmount = EditorGUILayout.FloatField("Damage", item.damageAmount);
        }
        if (item.type == Item.ItemType.Speed)
        {
            item.speedIncrease = EditorGUILayout.FloatField("Speed", item.speedIncrease);
        }
        EditorUtility.SetDirty(item);
    }
}
