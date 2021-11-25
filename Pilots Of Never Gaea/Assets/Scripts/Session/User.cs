using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class User : MonoBehaviour
{
    public CharacterSelectionManager.Character selectedCharacter;
    public int playerNumber;
    public InputDevice currentDevice;
    public Palette currentPalette;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MovePalette (InputAction.CallbackContext inputVector)
    {
        Debug.Log(inputVector.ReadValue<Vector2>());
    }
}
