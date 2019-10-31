using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
    public enum Character
    {
        nullCharacter, raildrive, magstream, kunst
    }
    public static GameManager.StartDuelFunction SelectAction;
    public static GameManager.SceneChange FadeAction;
    private Character p1SelectedCharacter, p2SelectedCharacter;
    public float platformTime, platformLimit;
    private float platformClock;
    private Vector3 leftPlatformStartingPosition, rightPlatformStartingPosition;
    public GameObject p1Elements, p2Elements;
    public AutoScroll p1Scroll, p2Scroll;
    public List<GameObject> shipButtons;
    private bool opening = false, closing = true;

    private void Start()
    {
        p1SelectedCharacter = Character.nullCharacter;
        p2SelectedCharacter = Character.nullCharacter;
        leftPlatformStartingPosition = p1Elements.transform.position;
        rightPlatformStartingPosition = p2Elements.transform.position;
        p1Elements.transform.Translate(-transform.right * platformLimit);
        p2Elements.transform.Translate(transform.right * platformLimit);
        platformClock = 0f;
        SetActiveButtons(false);
    }

    private void Update()
    {
        if (closing)
        {
            if (platformClock < platformTime)
            {
                platformClock += Time.deltaTime;
                float speed = platformLimit / platformTime * Time.deltaTime;
                p1Elements.transform.Translate(transform.right * speed);
                p2Elements.transform.Translate(-transform.right * speed);
            }
            else
            {
                p1Elements.transform.position = leftPlatformStartingPosition;
                p2Elements.transform.position = rightPlatformStartingPosition;
                SetActiveButtons(true);
                closing = false;
            }

        }
        else
        {
            if (opening || (p1SelectedCharacter != Character.nullCharacter && p2SelectedCharacter != Character.nullCharacter))
            {
                if (!opening)
                {
                    platformClock = 0f;
                    DestroyButtons();
                    opening = true;
                    Destroy(Camera.main.gameObject);
                    if (SelectAction != null)
                        SelectAction(p1SelectedCharacter, p2SelectedCharacter);
                }
                else
                {
                    platformClock += Time.deltaTime;
                    if (platformClock < platformTime)
                    {
                        float speed = platformLimit / platformTime * Time.deltaTime;
                        p1Elements.transform.Translate(-transform.right * speed);
                        p2Elements.transform.Translate(transform.right * speed);
                    }
                    else
                    {
                        if (FadeAction != null)
                            FadeAction(1);
                    }
                }
            }
        }
    }

    private void DestroyButtons()
    {
        foreach (GameObject button in shipButtons)
        {
            Destroy(button);
        }
    }

    private void SetActiveButtons(bool active)
    {
        foreach (GameObject button in shipButtons)
        {
            button.SetActive(active);
        }
        p1Scroll.enabled = active;
        p2Scroll.enabled = active;
    }

    public void P1SelectCharacter(int character)
    {
        Character selectedCharacter = (Character)character;
        p1SelectedCharacter = selectedCharacter;
    }

    public void P2SelectCharacter(int character)
    {
        Character selectedCharacter = (Character)character;
        p2SelectedCharacter = selectedCharacter;
    }
}
