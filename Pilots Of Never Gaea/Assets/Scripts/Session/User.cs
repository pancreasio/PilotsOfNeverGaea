using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class User : MonoBehaviour
{
    public CharacterSelectionManager.Character selectedCharacter;
    public int playerNumber;
    public Palette currentPalette;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Keyboard", devices: Keyboard.current);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetAsPlayer(int newPlayerNumber)
    {
        if(0 < newPlayerNumber && playerNumber < 3)
            playerNumber = newPlayerNumber;
    }
}
