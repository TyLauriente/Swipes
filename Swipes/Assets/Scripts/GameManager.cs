using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // References to Win and Lose sound effects and Game song
    public AudioSource AS_gameSound;
    public AudioSource AS_winSound;
    public AudioSource AS_loseSound;

    // Android native audio plugin variables
    int winSoundId;
    int loseSoundId;
    int winStreamId;
    int loseStreamId;


    // Can Play Audio
    bool canPlay = true;

    // Used to switch between backgrounds
    SpriteRenderer SR_background;


    // While the player has their finger held down (or mouse)
    bool isDragging = false;

    // Variables to keep track of where player first touches and where it moves
    Vector2 startPosition, deltaPosition;

    // Varialbes to keep track of background speed
    const float verticalSpeed = 7.0f;
    const float horizontalSpeed = verticalSpeed * 0.5f;

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

        SR_background = SR_background1;
        SR_background2.transform.position = new Vector3(0.0f, 0.0f, -1.0f);

        AndroidNativeAudio.makePool(2);
        winSoundId = AndroidNativeAudio.load("WinSound.wav");
        loseSoundId = AndroidNativeAudio.load("LoseSound.wav");
    }

    // GameLoop ----------------------------------------------------------------------------------------------------
    void Update()
    {
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
        else // Right
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

            pos.x = ((pos.x - width) / width) * horizontalSpeed;
            pos.y = ((pos.y - height) / height) * verticalSpeed;

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

            CheckCorrectSwipe();
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
            CheckCorrectSwipe();
            Invoke("EnableCanPlay", 0.1f);

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
        }
    }


    private void CheckCorrectSwipe()
    {
        if (canPlay)
        {
            bool win = false;
            if (direction == Direction.Up)
            {
                if (SR_background.transform.position.y > 0.2f)
                {
                    win = true;
                }
            }
            else if (direction == Direction.Down)
            {
                if (SR_background.transform.position.y < -0.2f)
                {
                    win = true;
                }
            }
            else if (direction == Direction.Left)
            {
                if (SR_background.transform.position.x < -0.2f)
                {
                    win = true;
                }
            }
            else // Right
            {
                if (SR_background.transform.position.x > 0.2f)
                {
                    win = true;
                }
            }

            if (isDragging && win)
            {
                //AS_winSound.PlayOneShot(AS_winSound.clip);
                winStreamId = AndroidNativeAudio.play(winSoundId);
                canPlay = false;
            }
            else if (!isDragging)
            {
                //AS_loseSound.PlayOneShot(AS_loseSound.clip);
                loseStreamId = AndroidNativeAudio.play(loseSoundId);
                canPlay = false;
            }
        }
    }
    private void EnableCanPlay()
    {
        canPlay = true;
    }

    private void OnApplicationQuit()
    {
        AndroidNativeAudio.unload(winSoundId);
        AndroidNativeAudio.unload(loseSoundId);
        AndroidNativeAudio.releasePool();
    }




    // End of File
}