using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BridgeManager : MonoBehaviour
{

    #region ControlsButton
    [SerializeField] private GameObject controlPanel;

    public void OnControlsButtonPress()
    {
        controlPanel.SetActive(!controlPanel.activeSelf);
    }
    #endregion

    [SerializeField] private GameObject bSlider;

    #region BridgePrefab GameObjects

  [SerializeField]  private int bridgeObjectsAdded = 0;

    [SerializeField] private int extensivesPerMiddle = 8;

    [SerializeField] private Transform initialBridgeSpawnSlot=null;

    [Header("Bridge Objects")]

    [Tooltip("This is for any variation of the BridgeStartParent prefab")]
    [SerializeField] private GameObject bridgeStartObject = null;

    [Tooltip("This is for any variation of the BridgeExtensiveParent prefab")]
    [SerializeField] private GameObject bridgeExtensiveObject = null;

    [Tooltip("This is for any variation of the BridgeExtensiveParent prefab")]
    [SerializeField] private GameObject bridgeMiddleObject = null;

    [Tooltip("This is for any variation of the BridgeEndParent prefab")]
    [SerializeField] private GameObject bridgeEndObject = null;
    #endregion

    [Header("UI")]
    [SerializeField] private TMP_Text extensivePiecesText;
    [SerializeField] private TMP_Text middlePiecesText;


    #region singleton
    public static BridgeManager Instance;

    private void Awake()
    {
        Instance = this;
        SpawnBridgePieces(true);

    }

    #endregion


    /// <summary>
    /// 
    /// </summary>
    /// <param name="isInitial">isInitial is only true in the Start function</param>
    public void SpawnBridgePieces(bool isInitial)
    {
        //Gets a reference to our newly spawned bridgeStart Spawns the start of the bridge at the initial spawn's transform.position
        GameObject _bridgeStart = Instantiate(bridgeStartObject, initialBridgeSpawnSlot.position, Quaternion.Euler(Vector3.zero));

        //Sets the piece to parent the initialBridgeSpawnSlot Transform
        _bridgeStart.transform.SetParent(initialBridgeSpawnSlot);

        GameObject lastGameObject = _bridgeStart;

        //Gets the spawn point for the end of the bridge
        Vector3 _bridgeSpawnSlot = lastGameObject.GetComponent<BridgePiece>().GetSpawnSlot();

        //These variables control how many middle pieces there are and how many extensive pieces to add
        int middlePiecesToSpawn = bridgeObjectsAdded/8;
        int _bridgeObjectsAdded = bridgeObjectsAdded - (middlePiecesToSpawn * 8);

        //Updates the text using ExtensivePieces (_bridgeObjectsAdded), then middlePieces (middlePiecesToSpawn)
        UpdateExtensivePiecesText(_bridgeObjectsAdded, middlePiecesToSpawn);

        //If this isn't the initial bridge then there is nothing in the middle of the bridge
        if (!isInitial)
        {

            if (_bridgeObjectsAdded > 0)
            {
                //
                //On this half of the if statement, there are both extensive and middle bridge pieces
                //
                for (int i = 0; i < _bridgeObjectsAdded; i++)
                {
                    //Checks if it's the middle of the extensive pieces and that there are middle pieces to spawn
                    if (_bridgeObjectsAdded / 2 == i && middlePiecesToSpawn > 0)
                    {
                        for (int x = 0; x < middlePiecesToSpawn; x++)
                        {
                            //This places the middle pieces
                            GameObject newBridgeMiddle = Instantiate(bridgeMiddleObject, _bridgeSpawnSlot, Quaternion.Euler(Vector3.zero));

                            newBridgeMiddle.transform.SetParent(initialBridgeSpawnSlot);

                            _bridgeSpawnSlot = newBridgeMiddle.GetComponent<BridgePiece>().GetSpawnSlot();
                        }
                    }

                    //This places the extensive pieces
                    GameObject newBridgeExtensive = Instantiate(bridgeExtensiveObject, _bridgeSpawnSlot, Quaternion.Euler(Vector3.zero));

                    newBridgeExtensive.transform.SetParent(initialBridgeSpawnSlot);

                    _bridgeSpawnSlot = newBridgeExtensive.GetComponent<BridgePiece>().GetSpawnSlot();
                }
            }
            else
            {
                //
                //On this half there are only bridge pieces
                //
                for (int x = 0; x < middlePiecesToSpawn; x++)
                {
                    //This places the middle pieces
                    GameObject newBridgeMiddle = Instantiate(bridgeMiddleObject, _bridgeSpawnSlot, Quaternion.Euler(Vector3.zero));

                    newBridgeMiddle.transform.SetParent(initialBridgeSpawnSlot);

                    _bridgeSpawnSlot = newBridgeMiddle.GetComponent<BridgePiece>().GetSpawnSlot();
                }

            }
        }


       
        //Spawns the end of the bridge, gets a reference to the new end of the bridge GameObject for expandability
        GameObject _bridgeEnd = Instantiate(bridgeEndObject, _bridgeSpawnSlot, Quaternion.Euler(Vector3.zero));
        _bridgeEnd.transform.SetParent(initialBridgeSpawnSlot);

    }

    public void DestroyAllBridgePieces()
    {
        foreach(Transform _child in initialBridgeSpawnSlot)
        {
            Destroy(_child.gameObject);
        }
    }
    /// <summary>
    /// This is the function where the magic happens
    /// </summary>
    /// <param name="isAdding">When isAdding is true, it increases the piece amount, if it's false it decreases the piece amount</param>
    public void UpdateBridgePieceAmount(bool isAdding)
    {
        //You can't remove a piece if there are no extra objects added
        if(!isAdding && bridgeObjectsAdded == 0) { return; }

        DestroyAllBridgePieces();

        print($"UpdateBridgePieceAmount is being called to {(isAdding ? "add" : "subtract" )}");

        //Clears the parent containing all the bridge pieces

        bridgeObjectsAdded = isAdding ? bridgeObjectsAdded+1 : bridgeObjectsAdded-1;
        SpawnBridgePieces(false);
    }

    private GameObject ObjectToSpawn()
    {
        GameObject objectToReturn = null;

        


        return objectToReturn;
    }

    private void UpdateExtensivePiecesText(int _extensivePieces, int _middlePieces)
    {
        extensivePiecesText.text = $"Extensive Pieces: {_extensivePieces}";
        middlePiecesText.text = $"Middle Pieces: {_middlePieces}";
    }

    public GameObject GetLastBridgePiece()
    {
        return initialBridgeSpawnSlot.transform.GetChild(initialBridgeSpawnSlot.childCount-1).gameObject;
    }
    
    public void ReturnSliderB()
    {
       bSlider.transform.position = GetLastBridgePiece().GetComponent<BridgePiece>().GetSpawnSlot();
    }
}
