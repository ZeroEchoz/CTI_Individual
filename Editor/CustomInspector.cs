using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UpgradesManager))]
[CanEditMultipleObjects]
public class CustomInspector : Editor {

    public override void OnInspectorGUI()
    {
        //inherit the Inspector GUI of the script
        DrawDefaultInspector();

        //get the upgrades script
        UpgradesManager upgradeScript = (UpgradesManager)target;

        //if object activation is selected, show and enable these values, otherwise, show the other value
        if (upgradeScript.upgradableParameters == UpgradesManager.PlayerParameters.ObjectActivation)
        {
            upgradeScript.objectToActivate = (GameObject)EditorGUILayout.ObjectField("Object To Activate", upgradeScript.objectToActivate, typeof(GameObject), true);
            upgradeScript.objectActivateDescription = EditorGUILayout.TextField("Object Activate Description", upgradeScript.objectActivateDescription);
        } else
        {
            upgradeScript.upgradeValue = EditorGUILayout.FloatField("Upgrade Value", upgradeScript.upgradeValue);
        }
    }
}
