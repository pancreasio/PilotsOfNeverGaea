using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlSelectionManager : MonoBehaviour
{
    PlayerInputManager playerInputManager;

    private static GameObject controlSelectionManagerInstance;
    bool isPlayer1Ready;
    bool isPlayer2Ready;
    public GameObject controllerUIEntity;
    List<InputDevice> inputDeviceList = new List<InputDevice>();
    // Start is called before the first frame update



    private void Awake()
    {
        if (controlSelectionManagerInstance == null)
        {
            controlSelectionManagerInstance = this.gameObject;
            DontDestroyOnLoad(controlSelectionManagerInstance);
        }
        else
        {
            Destroy(this.gameObject);
        }

        playerInputManager = GetComponent<PlayerInputManager>();
        InputSystem.onDeviceChange += OnDeviceChange;
        isPlayer1Ready = false;
        isPlayer2Ready = false;
    }
    void Start()
    {

        //inputDeviceList.AddRange(InputSystem.devices);
        foreach (InputDevice i in InputSystem.devices)
        {
            Debug.Log(i.name);
        }
        OnActivate();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnActivate()
    {
        int i = 0;
        PlayerInput p1Input = PlayerInput.Instantiate(controllerUIEntity, i, "Keyboard", -1, Keyboard.current.device);
        p1Input.transform.SetParent(this.transform);
        p1Input.transform.Translate(-p1Input.transform.up * i * 2);


        i++;
        PlayerInput p2Input = PlayerInput.Instantiate(controllerUIEntity, i, "Keyboard Player 2", -1, Keyboard.current.device);
        p2Input.transform.SetParent(this.transform);
        p2Input.transform.Translate(-p2Input.transform.up * i * 2);

        for (int g = 0; g < Gamepad.all.Count; g++)
        {
            i++;
            PlayerInput gamepadInput = PlayerInput.Instantiate(controllerUIEntity, i, "Gamepad", -1, Gamepad.all[g].device);
            Debug.Log(Gamepad.all[g].device.name);
            gamepadInput.transform.SetParent(this.transform);
            gamepadInput.transform.Translate(-gamepadInput.transform.up * i * 2);
        }
    }

    public void OnDeviceChange(InputDevice inputDevice, InputDeviceChange inputDeviceChange)
    {
        switch (inputDeviceChange)
        {
            case InputDeviceChange.Added:
                Debug.Log(inputDevice + " added");
                break;

            case InputDeviceChange.Removed:
                Debug.Log(inputDevice + " removed");
                break;

        }
    }
}
