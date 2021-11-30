using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlSelectionManager : MonoBehaviour
{
    PlayerInputManager playerInputManager;

    public static GameObject controlSelectionManagerInstance;
    bool isPlayer1Ready;
    bool isPlayer2Ready;

    public float deviceUIOffset = 1f;
    public Sprite WASDSprite;
    public Sprite arrowKeysSprite;
    public Sprite gamepadSprite;
    public GameObject controllerUIEntity;
    public Transform p1UIPosition;
    public Transform p2UIPosition;
    public Transform deviceListUIPosition;
    List<GameObject> playerInputList = new List<GameObject>();
    GameObject p1AssignedController;
    GameObject p2AssignedController;
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
        OnActivate();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveUIController(ControllerDeviceUI controller, bool isMovingLeft)
    {
        if (playerInputList.Contains(controller.gameObject))
        {
            if (isMovingLeft)
            {
                if (p1AssignedController == null)
                {
                    AssignPlayerController(controller.gameObject, true);
                    playerInputList.Remove(controller.gameObject);
                }
            }

            else
                if (p2AssignedController == null)
            {
                AssignPlayerController(controller.gameObject, false);
                playerInputList.Remove(controller.gameObject);
            }
        }
        else
        if (p1AssignedController == controller.gameObject && !isMovingLeft)
        {
            p1AssignedController = null;
            ReturnControllerToList(controller.gameObject);
            Debug.Log("should return");
        }
        else
        if (p2AssignedController == controller.gameObject && isMovingLeft)
        {
            p2AssignedController = null;
            ReturnControllerToList(controller.gameObject);
            Debug.Log("should return");
        }
    }

    public void OnActivate()
    {
        int i = 0;
        PlayerInput p1Input = PlayerInput.Instantiate(controllerUIEntity, i, "Keyboard", -1, Keyboard.current.device);
        p1Input.transform.SetParent(this.transform);
        p1Input.transform.SetPositionAndRotation(deviceListUIPosition.position - (Vector3.up * i * deviceUIOffset),Quaternion.identity);
        p1Input.transform.GetComponent<SpriteRenderer>().sprite = WASDSprite;
        p1Input.transform.GetComponent<ControllerDeviceUI>().assignedDevice = Keyboard.current.device;
        p1Input.transform.GetComponent<ControllerDeviceUI>().assignedScheme = p1Input.currentControlScheme;

        i++;
        PlayerInput p2Input = PlayerInput.Instantiate(controllerUIEntity, i, "Keyboard Player 2", -1, Keyboard.current.device);
        p2Input.transform.SetParent(this.transform);
        p2Input.transform.SetPositionAndRotation(deviceListUIPosition.position - (Vector3.up * i * deviceUIOffset),Quaternion.identity);
        p2Input.transform.GetComponent<SpriteRenderer>().sprite = arrowKeysSprite;
        p2Input.transform.GetComponent<ControllerDeviceUI>().assignedDevice = Keyboard.current.device;
        p2Input.transform.GetComponent<ControllerDeviceUI>().assignedScheme = p2Input.currentControlScheme;

        playerInputList.Add(p1Input.gameObject);
        playerInputList.Add(p2Input.gameObject);


        for (int g = 0; g < Gamepad.all.Count; g++)
        {
            i++;
            PlayerInput gamepadInput = PlayerInput.Instantiate(controllerUIEntity, i, "Gamepad", -1, Gamepad.all[g].device);
            gamepadInput.transform.SetParent(this.transform);
            gamepadInput.transform.SetPositionAndRotation(deviceListUIPosition.position - (Vector3.up * i * deviceUIOffset),Quaternion.identity);
            gamepadInput.transform.GetComponent<SpriteRenderer>().sprite = gamepadSprite;
            gamepadInput.transform.GetComponent<ControllerDeviceUI>().assignedDevice = Gamepad.all[g].device;
            gamepadInput.transform.GetComponent<ControllerDeviceUI>().assignedScheme = "Gamepad";
            playerInputList.Add(gamepadInput.gameObject);
        }
    }


    private void AssignPlayerController(GameObject UIController, bool isPlayer1)
    {
        if (isPlayer1)
        {
            UIController.transform.SetPositionAndRotation(p1UIPosition.position, Quaternion.identity);
            p1AssignedController = UIController;
        }
        else
        {
            UIController.transform.SetPositionAndRotation(p2UIPosition.position, Quaternion.identity);
            p2AssignedController = UIController;
        }
    }

    private void ReturnControllerToList(GameObject UIController)
    {
        playerInputList.Add(UIController);
        int i=0;
        foreach (GameObject controllerObject in playerInputList)
        {
            controllerObject.transform.SetPositionAndRotation(deviceListUIPosition.transform.position - Vector3.up * deviceUIOffset * i, Quaternion.identity);
            i++;
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
