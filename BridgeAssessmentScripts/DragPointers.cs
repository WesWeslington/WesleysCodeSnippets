using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragPointers : MonoBehaviour
{

    [SerializeField] private Renderer renderer;


    [Header("Pointer Colors")]
    [SerializeField] private Color normalColor;
    [SerializeField] private Color hoveredColor;
    [SerializeField] private Color selectedColor;

    //These variables are for detecting whether or not to add things
    private float startingX;
    private float nextTargetX;
    private float previousTargetX;

    [Header("Pointer Settings")]
    [SerializeField] private bool isSliderB = false;
    [SerializeField]  private float distanceX=2f;
    [SerializeField] private float movementSpeed = 0.5f;

    private void Start()
    {

        if (isSliderB)
            BridgeManager.Instance.ReturnSliderB();

        startingX = transform.position.x;
        nextTargetX = startingX + distanceX;
        previousTargetX = startingX - distanceX;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseEnter()
    {
        renderer.material.color = hoveredColor;
    }

    private void OnMouseExit()
    {
        renderer.material.color = normalColor;
    }

    private void OnMouseDrag()
    {
        float mouseInput = Input.GetAxis("Mouse X");

        Vector3 currentPosition = transform.position;


        renderer.material.color = selectedColor;
      
            transform.position =  new Vector3(currentPosition.x + (mouseInput*movementSpeed*-1), currentPosition.y, currentPosition.z);

        if(mouseInput!=0)
        CheckForAdding();

   
    }

    private void OnMouseUp()
    {
        renderer.material.color = normalColor;

        //If it's sliderB, when released, the slider will go to the end of the bridge
        if (isSliderB)
         BridgeManager.Instance.ReturnSliderB();
        ResetSliderBTargetX();

    }

    private void CheckForAdding()
    {
        bool makesChange = false;
        bool _isAdding = false;

        if (!isSliderB)
        {
            //
            //These if statements are only for the A slider, since the B slider works the opposite way
            //
            if (transform.position.x >= nextTargetX)
            {
                makesChange = true;
                _isAdding = true;

                UpdateXDistances(true);
            }

            if (transform.position.x <= previousTargetX)
            {
                makesChange = true;
                _isAdding = false;

                UpdateXDistances(false);

            }
        }
        else
        //
        //These if statements are only for the B slider, since the A slider works the opposite way
        //
        {
            if (transform.position.x >= nextTargetX)
            {
                makesChange = true;
                _isAdding = false;

                UpdateXDistances(true);
            }

            if (transform.position.x <= previousTargetX)
            {
                makesChange = true;
                _isAdding = true;

                UpdateXDistances(false);

            }
        }

        //If changes need to be made then this if statement does it according to the direction and slider
        if (makesChange)
        {

            BridgeManager.Instance.UpdateBridgePieceAmount(_isAdding);

            if (!isSliderB)
                BridgeManager.Instance.ReturnSliderB();
            ResetSliderBTargetX();
        }


        
    }

    //Updates the distance targets based off the direction of the sliders movement
    public void UpdateXDistances(bool isPositive)
    {

      
            nextTargetX = isPositive ? nextTargetX+distanceX:nextTargetX- distanceX;
            previousTargetX = isPositive ? previousTargetX+distanceX: previousTargetX-distanceX;
    
    }

    //This is needed to encapsulate the targetX to avoid buggy movement 
    public void ResetSliderBTargetX()
    {
        nextTargetX = transform.position.x + distanceX;
        previousTargetX = transform.position.x - distanceX;

    }
}
