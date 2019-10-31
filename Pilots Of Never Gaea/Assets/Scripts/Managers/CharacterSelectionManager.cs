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
    public float platformTimeToDissapear, platformLimit;
    private float platformClock;
    public GameObject p1SelectedButton, p2SelectedButton, p1Elements, p2Elements;
    public List<GameObject> shipButtons;
    private bool opening = false;

    private void Start()
    {
        p1SelectedCharacter = Character.nullCharacter;
        p2SelectedCharacter = Character.nullCharacter;
    }

    private void Update()
    {
        if (opening || (p1SelectedCharacter != Character.nullCharacter && p2SelectedCharacter != Character.nullCharacter))
        {
            if (!opening)
            {
                DestroyButtons();
                opening = true;
                Destroy(Camera.main.gameObject);
                if (SelectAction != null)
                    SelectAction(p1SelectedCharacter, p2SelectedCharacter);
            }
            else
            {
                platformClock += Time.deltaTime;
                if (platformClock < platformTimeToDissapear)
                {
                    float speed = platformLimit / platformTimeToDissapear * Time.deltaTime;
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

    private void DestroyButtons()
    {
        foreach (GameObject button in shipButtons)
        {
            Destroy(button);
        }
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
