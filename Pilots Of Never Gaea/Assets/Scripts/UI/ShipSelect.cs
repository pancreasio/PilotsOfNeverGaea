using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSelect : MonoBehaviour
{
    public float shipOffset, scrollTime;
    public int shipAmmount, firstShip;
    private float direction;
    public bool isPlayer1;
    private bool isMoving = false, selectButton;
    private int currentSelected;

    public enum Ship
    {
        none,
        Raildrive,
        Magstream,
        Kunst,
        Knockout,
        Djinn
    }
    public Ship selectedShip;

    private void Start()
    {
        selectedShip = Ship.none;
        currentSelected = firstShip;
        selectButton = false;
        direction = 0f;
    }

    private void Update()
    {
        if (selectedShip == Ship.none)
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
                        SelectShip(currentSelected);
                    }
                }
            }
        }
    }

    private void SelectShip(int shipIndex)
    {
        selectedShip = (Ship)(shipIndex +1);
    }

    IEnumerator Scroll(float moveDirection)
    {
        if (!(currentSelected == 0 && direction > 0 || currentSelected == shipAmmount - 1 && direction < 0))
        {
            isMoving = true;
            float scrollClock = 0f;
            while (scrollClock < scrollTime)
            {
                scrollClock += Time.deltaTime;
                transform.Translate(0f, -moveDirection * Time.deltaTime * shipOffset / scrollTime, 0f);
                yield return null;
            }
            currentSelected -= Mathf.FloorToInt(moveDirection);
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
