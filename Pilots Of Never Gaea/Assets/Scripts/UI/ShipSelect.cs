using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipSelect : MonoBehaviour
{
    public float shipOffset, scrollTime, cancelTime;
    public int shipAmmount, firstShip;

    public bool isPlayer1;
    private bool isMoving = false, isSelecting = false;
    private bool shouldCancel = false;
    private int currentSelected;
    public List<Animator> shipList;
    public static GameManager.StartDuelFunction SelectAction;
    public static GameManager.SceneChange FadeAction;
    public CharacterSelectionManager characterSelectionManager;

    public CharacterSelectionManager.Character selectedShip;

    public GameManager.ButtonAction OnDeviceLostAction;

    private void Awake()
    {
        selectedShip = CharacterSelectionManager.Character.none;
        currentSelected = firstShip;

        if (!isPlayer1)
        {
            foreach (Animator ship in shipList)
            {
                ship.SetTrigger("RIGHT");
            }
        }
    }

    private void Start()
    {
        shipList[currentSelected].SetTrigger("HIGHLIGHTED");
    }

    private void Update()
    {

    }

    //private void Selecthip(int shipIndex)
    //{
    //    selectedShip = (CharacterSelectionManager.Character)(shipIndex + 1);
    //    if (isPlayer1)
    //        characterSelectionManager.p1SelectedCharacter = selectedShip;
    //    else
    //        characterSelectionManager.p2SelectedCharacter = selectedShip;

    //}

    public void TryMoving(InputAction.CallbackContext context)
    {
        if (selectedShip == CharacterSelectionManager.Character.none)
        {
            if (!isMoving)
            {
                if (context.ReadValue<Vector2>().y != 0)
                    StartCoroutine(Scroll(context.ReadValue<Vector2>().y));
            }
        }
    }

    public void TryAction(InputAction.CallbackContext context)
    {
        if (selectedShip == CharacterSelectionManager.Character.none)
        {
            if (!isMoving)
            {
                if (context.performed)
                {
                    shipList[currentSelected].SetTrigger("SELECTED");
                    foreach (Animator ship in shipList)
                    {
                        if (ship != shipList[currentSelected])
                            ship.SetTrigger("NOT_SELECTED");
                    }

                    shouldCancel = false;
                    StartCoroutine(SelectShip(currentSelected));
                }
            }
        }
    }

    public void TryCancel(InputAction.CallbackContext context)
    {

            if (isSelecting || selectedShip != CharacterSelectionManager.Character.none)
            {
                shouldCancel = true;
                foreach (Animator ship in shipList)
                {
                    if (ship != shipList[currentSelected])
                        ship.SetTrigger("IDLE");
                }
                selectedShip = CharacterSelectionManager.Character.none;
                if (isPlayer1)
                    characterSelectionManager.p1SelectedCharacter = selectedShip;
                else
                    characterSelectionManager.p2SelectedCharacter = selectedShip;
            }

    }

    private void OnDeviceLost()
    {
        if(OnDeviceLostAction!=null)
            OnDeviceLostAction.Invoke();
    }

    IEnumerator SelectShip(int shipIndex)
    {
        float selectClock = 0f;
        isSelecting = true;
        if (isPlayer1)
            AkSoundEngine.PostEvent("sfx_ui_ok_p1", gameObject);
        else
            AkSoundEngine.PostEvent("sfx_ui_ok_p2", gameObject);
        while (selectClock < cancelTime && !shouldCancel)
        {
            selectClock += Time.deltaTime;
            yield return null;
        }
        if (!shouldCancel)
        {
            selectedShip = (CharacterSelectionManager.Character)(shipIndex + 1);
            if (isPlayer1)
                characterSelectionManager.p1SelectedCharacter = selectedShip;
            else
                characterSelectionManager.p2SelectedCharacter = selectedShip;
        }
        else
        {
            foreach (Animator ship in shipList)
            {
                if (ship != shipList[currentSelected])
                    ship.SetTrigger("IDLE");
            }
        }
        isSelecting = false;
    }

    IEnumerator Scroll(float moveDirection)
    {
        if (!(currentSelected == 0 && moveDirection > 0 || currentSelected == shipAmmount - 1 && moveDirection < 0))
        {
            shipList[currentSelected].SetTrigger("IDLE");
            isMoving = true;
            float scrollClock = 0f;
            if (isPlayer1)
                AkSoundEngine.PostEvent("sfx_ui_selectship_p1", gameObject);
            else
                AkSoundEngine.PostEvent("sfx_ui_selectship_p2", gameObject);
            while (scrollClock < scrollTime)
            {
                scrollClock += Time.deltaTime;
                transform.Translate(0f, -moveDirection * Time.deltaTime * shipOffset / scrollTime, 0f);
                yield return null;
            }
            currentSelected -= Mathf.FloorToInt(moveDirection);
            shipList[currentSelected].SetTrigger("HIGHLIGHTED");
            isMoving = false;
        }
    }
}