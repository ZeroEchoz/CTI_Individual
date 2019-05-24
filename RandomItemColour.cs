using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemColour : MonoBehaviour {

    public Material[] objectMaterial;

    int index;

    private void OnEnable()
    {
        index = Random.Range(0, objectMaterial.Length);
        this.GetComponent<Renderer>().material = objectMaterial[index];
    }

}
