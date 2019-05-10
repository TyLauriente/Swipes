using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 
 * This Class is a Game Manager for the prototype of SR_swipes
 * 
 * It's purpose is to give an idea as to what the project will be like
 * when it is complete.
 * 
 * 
 * Authored by Ty Lauriente
 * 05/08/2019
 * 
 * */

public class GameManager : MonoBehaviour
{
    // References to background 1 and 2 and SR_swipe
    public SpriteRenderer SR_background1;
    public SpriteRenderer SR_background2;
    public SpriteRenderer SR_swipe;


    // Used to switch between backgrounds
    SpriteRenderer SR_background;


    // While the player has their finger held down (or mouse)
    bool isDragging = false;

    // Variables to keep track of where player first touches and where it moves
    Vector2 startPosition, deltaPosition;

    // Varialbes to keep track of background speed
    const float verticalStartSpeed = 7.0f;
    const float horizontalStartSpeed = verticalStartSpeed * 0.4f;
    const float speedIncreaseRate = 0.25f;
    float verticalSpeedMultiplier = verticalStartSpeed;
    float horizontalSpeedMultiplier = horizontalStartSpeed;

    // Used to convert touch position to local space
    float width;
    float height;

    // Utility variables
    Random rand = new Random();


    // Direction for SR_swipe Image
    Direction direction;

    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    // Initialization -----------------------------------------------------------------------------------------------
    void Start()
    {
        width = (float)Screen.width * 0.5f;
        height = (float)Screen.height * 0.5f;

        GenerateRandomDirection();


        SR_background = SR_background1;
        SR_background2.transform.position = new Vector3(0.0f, 0.0f, -1.0f);
    }

    // GameLoop ----------------------------------------------------------------------------------------------------
    void Update()
    {
        verticalSpeedMultiplier += verticalSpeedMultiplier* Time.deltaTime;
        horizontalSpeedMultiplier += horizontalSpeedMultiplier * Time.deltaTime;

        OnMousePress();

        OnMouseRelease();
    }


    // RandomDirections --------------------------------------------------------------------------------------------

    private void GenerateRandomDirection()
    {
        direction = (Direction)Random.Range(0, 4);
        SR_swipe.transform.rotation = Quaternion.identity;

        if (direction == Direction.Up)
        {
            SR_swipe.transform.Rotate(new Vector3(0.0f, 0.0f, 0.0f));
        }
        else if (direction == Direction.Down)
        {
            SR_swipe.transform.Rotate(new Vector3(0.0f, 0.0f, 180.0f));
        }
        else if (direction == Direction.Left)
        {
            SR_swipe.transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));
        }
        else
        {
            SR_swipe.transform.Rotate(new Vector3(0.0f, 0.0f, 270.0f));
        }
    }

    // MousePressResponse -----------------------------------------------------------------------------------------

    private void OnMousePress()
    {
        if (Input.GetMouseButtonDown(0) || isDragging)
        {
            Vector2 pos = Input.mousePosition;

            pos.x = ((pos.x - width) / width) * horizontalSpeedMultiplier;
            pos.y = ((pos.y - height) / height) * verticalSpeedMultiplier;

            if (!isDragging)
            {
                startPosition = pos;
            }

            pos -= startPosition;

            if ((direction == Direction.Left && pos.x < 0.0f) || (direction == Direction.Right && pos.x > 0.0f))
            {
                SR_background.transform.position = new Vector3(pos.x, 0.0f, SR_background.transform.position.z);
            }

            if ((direction == Direction.Up && pos.y > 0.0f) || (direction == Direction.Down && pos.y < 0.0f))
            {
                SR_background.transform.position = new Vector3(0.0f, pos.y, SR_background.transform.position.z);
            }

            isDragging = true;
        }
        SR_swipe.transform.position = new Vector3(SR_background.transform.position.x,
            SR_background.transform.position.y, SR_swipe.transform.position.z);
    }

    // MouseReleaseResponse --------------------------------------------------------------------------------------

    private void OnMouseRelease()
    {
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            SR_swipe.transform.rotation = Quaternion.identity;
            SR_swipe.transform.position = new Vector3(0.0f, 0.0f, SR_swipe.transform.position.z);
            SR_background.transform.position = new Vector3(0.0f, 0.0f, SR_background.transform.position.z);

            GenerateRandomDirection();

            if (SR_background == SR_background1)
            {
                SR_background1.transform.position = new Vector3(0.0f, 0.0f, -1.0f);
                SR_background = SR_background2;
                SR_background2.transform.position = new Vector3(0.0f, 0.0f, -2.0f);
            }
            else
            {
                SR_background2.transform.position = new Vector3(0.0f, 0.0f, -1.0f);
                SR_background = SR_background1;
                SR_background1.transform.position = new Vector3(0.0f, 0.0f, -2.0f);
            }


            verticalSpeedMultiplier = verticalStartSpeed;
            horizontalSpeedMultiplier = horizontalStartSpeed;
        }
    }







    // End of File
}