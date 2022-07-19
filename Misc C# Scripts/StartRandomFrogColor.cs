using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// This script should only be used for updating a frog model's color at the beginning of the game
/// 
/// </summary>

public class StartRandomFrogColor : MonoBehaviour
{

    [Tooltip("This Skinned Mesh Renderer component will be the only Mesh updates by this script")]
    [SerializeField] private SkinnedMeshRenderer skinnedMesh;


    void Start()
    {
        int maxPlayerColors = PlayerManager.instance.playerColors.Count;
        int colorInt = Random.Range(0,maxPlayerColors);

        UpdateColors(colorInt);
    }

    public void UpdateColors(int colorInt)
    {
        //Grabs the list of frog colors from the PlayerManager singleton
        List<Color> frogColors = PlayerManager.instance.playerColors;

        //Creates temporary materials to reference to avoid changing the material for all Mesh's 
        Material _newBaseMat = Instantiate(skinnedMesh.materials[1]);
        Material _newSecondaryMat = Instantiate(skinnedMesh.materials[4]);

        //Changes the color of the reference material in the Toon shader that the material uses
        _newBaseMat.SetColor("_BaseColor", frogColors[colorInt]);
        _newSecondaryMat.SetColor("_BaseColor", frogColors[colorInt] * Color.gray);

        //Copies the properties from the reference materials into the respective material on the Frog_Mesh Skinned Mesh Renderer
        skinnedMesh.materials[1].CopyPropertiesFromMaterial(_newBaseMat);
        skinnedMesh.materials[4].CopyPropertiesFromMaterial(_newSecondaryMat);

        //Destroys the instantiated reference materials that are no longer used
        Destroy(_newBaseMat);
        Destroy(_newSecondaryMat);

    }



}
