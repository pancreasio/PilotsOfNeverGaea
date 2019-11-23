using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSelect : MonoBehaviour
{
    public float shipOffset, scrollTime, cancelTime;
    public int shipAmmount, firstShip;
    private float direction;
    public bool isPlayer1;
    private bool isMoving = false, selectButton;
    private int currentSelected;
    public List<GameObject> shipList;
    public static GameManager.StartDuelFunction SelectAction;
    public static GameManager.SceneChange FadeAction;
    public CharacterSelectionManager characterSelectionManager;

    public CharacterSelectionManager.Character selectedShip;

    private void Awake()
    {
        selectedShip = CharacterSelectionManager.Character.none;
        currentSelected = firstShip;
        selectButton = false;
        direction = 0f;
        if (!isPlayer1)
        {
            foreach (GameObject ship in shipList)
            {
                ship.GetComponent<Animator>().SetTrigger("RIGHT");
            }
        }
    }

    private void Start()
    {
        //foreach (GameObject ship in shipList)
        //{
        //    ship.GetComponent<Animator>().SetTrigger("IDLE");
        //}
        shipList[currentSelected].GetComponent<Animator>().SetTrigger("HIGHLIGHTED");
    }

    private void Update()
    {
        if (selectedShip == CharacterSelectionManager.Character.none)
        {
            GetScrollInput();
            if (!isMoving)
            {
                if (direction != 0)
                    StartCoroutine(Scroll(direction));
                else
                {
                    if (selectButton)
                    {
                        shipList[currentSelected].GetComponent<Animator>().SetTrigger("SELECTED");
                        foreach (GameObject ship in shipList)
                        {
                            if (ship != shipList[currentSelected])
                                ship.GetComponent<Animator>().SetTrigger("NOT_SELECTED");
                        }
                        StartCoroutine(SelectShip(currentSelected));
                    }
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                foreach (GameObject ship in shipList)
                {
                    if (ship != shipList[currentSelected])
                        ship.GetComponent<Animator>().SetTrigger("IDLE");
                }
                selectedShip = CharacterSelectionManager.Character.none;
                if (isPlayer1)
                    characterSelectionManager.p1SelectedCharacter = selectedShip;
                else
                    characterSelectionManager.p2SelectedCharacter = selectedShip;
            }
        }
    }

    private void Selecthip(int shipIndex)
    {
        selectedShip = (CharacterSelectionManager.Character)(shipIndex + 1);
        if (isPlayer1)
            characterSelectionManager.p1SelectedCharacter = selectedShip;
        else
            characterSelectionManager.p2SelectedCharacter = selectedShip;

    }

    IEnumerator SelectShip(int shipIndex)
    {
        float selectClock = 0f;
        bool canceled = false;
        while (selectClock < cancelTime && !canceled)
        {
            selectClock += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Escape))
                canceled = true;

            yield return null;
        }
        if (!canceled)
        {
            selectedShip = (CharacterSelectionManager.Character)(shipIndex + 1);
            if (isPlayer1)
                characterSelectionManager.p1SelectedCharacter = selectedShip;
            else
                characterSelectionManager.p2SelectedCharacter = selectedShip;
        }
        else
        {
            foreach (GameObject ship in shipList)
            {
                if (ship != shipList[currentSelected])
                    ship.GetComponent<Animator>().SetTrigger("IDLE");
            }
        }
    }

    IEnumerator Scroll(float moveDirection)
    {
        if (!(currentSelected == 0 && direction > 0 || currentSelected == shipAmmount - 1 && direction < 0))
        {
            shipList[currentSelected].GetComponent<Animator>().SetTrigger("IDLE");
            isMoving = true;
            float scrollClock = 0f;
            while (scrollClock < scrollTime)
            {
                scrollClock += Time.deltaTime;
                transform.Translate(0f, -moveDirection * Time.deltaTime * shipOffset / scrollTime, 0f);
                yield return null;
            }
            currentSelected -= Mathf.FloorToInt(moveDirection);
            shipList[currentSelected].GetComponent<Animator>().SetTrigger("HIGHLIGHTED");
            isMoving = false;
        }
    }

    private void GetScrollInput()
    {
        if (isPlayer1)
        {
            direction = Input.GetAxisRaw("P1Vertical");
            selectButton = Input.GetKeyDown(KeyCode.Space);
        }
        else
        {
            direction = Input.GetAxisRaw("P2Vertical");
            selectButton = Input.GetKeyDown(KeyCode.RightControl);
        }
    }
}
