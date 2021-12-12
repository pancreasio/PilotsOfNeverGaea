using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlSelectionManager : MonoBehaviour
{
    PlayerInputManager playerInputManager;

    public static GameObject controlSelectionManagerInstance;
    public delegate void ControlsSetDelegate(ControllerDeviceUI p1Device, ControllerDeviceUI p2Device);
    public ControlsSetDelegate controlsSetAction;
    public GameManager.ButtonAction OnActivateAction;
    bool isPlayer1Ready;
    bool isPlayer2Ready;

    private int p1DeviceID = 0;
    private int p2DeviceID = 0;

    bool isActive;

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

    public TMPro.TextMeshProUGUI p1ReadyText;
    public TMPro.TextMeshProUGUI p2ReadyText;

    public GameObject controlSettingsCanvas;


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
        isActive = false;
        controlSettingsCanvas.SetActive(false);
    }
    void Start()
    {
        //OnActivate();
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
        if (p1AssignedController == controller.gameObject && !isMovingLeft && !isPlayer1Ready)
        {
            p1AssignedController = null;
            ReturnControllerToList(controller.gameObject);
        }
        else
        if (p2AssignedController == controller.gameObject && isMovingLeft && !isPlayer2Ready)
        {
            p2AssignedController = null;
            ReturnControllerToList(controller.gameObject);
        }
    }

    public void ConfirmSelection(ControllerDeviceUI controller)
    {
        if (controller.gameObject == p1AssignedController)
        {
            isPlayer1Ready = true;
            ModifyReadyText(p1ReadyText, true);
            CheckPlayersReady();
        }

        if (controller.gameObject == p2AssignedController)
        {
            isPlayer2Ready = true;
            ModifyReadyText(p2ReadyText, true);
            CheckPlayersReady();
        }
    }

    public void CancelSelection(ControllerDeviceUI controller)
    {
        if (controller.gameObject == p1AssignedController && isPlayer1Ready)
        {
            isPlayer1Ready = false;
            ModifyReadyText(p1ReadyText, false);
        }

        if (controller.gameObject == p2AssignedController && isPlayer2Ready)
        {
            isPlayer2Ready = false;
            ModifyReadyText(p2ReadyText, false);
        }
    }

    public void DeviceLost(ControllerDeviceUI controller)
    {
        int deviceID = controller.assignedDeviceID;
        if (isActive)
        {
            if (p1AssignedController != null && deviceID == p1AssignedController.GetComponent<ControllerDeviceUI>().assignedDeviceID)
            {
                //Destroy(p1AssignedController);
                p1AssignedController = null;
                isPlayer1Ready = false;
                ModifyReadyText(p1ReadyText, false);
            }
            else
                if (p2AssignedController != null && deviceID == p2AssignedController.GetComponent<ControllerDeviceUI>().assignedDeviceID)
            {
                //Destroy(p2AssignedController);
                p2AssignedController = null;
                isPlayer2Ready = false;
                ModifyReadyText(p2ReadyText, false);
            }
            else
                for (int i = 0; i < playerInputList.Count; i++)
                {
                    if (playerInputList[i].GetComponent<ControllerDeviceUI>().assignedDeviceID == deviceID)
                    {
                        //Destroy(playerInputList[i].gameObject);
                        playerInputList.Remove(playerInputList[i]);
                        OrderDeviceList();
                        i = playerInputList.Count;
                    }
                }
        }
    }

    private void ModifyReadyText(TMPro.TextMeshProUGUI readyText, bool isConfirming)
    {
        if (isConfirming)
        {
            readyText.text = "Ready!";
            readyText.fontSize = 40;
        }
        else
        {
            readyText.text = "Ready?";
            readyText.fontSize = 36;
        }
    }

    private void CheckPlayersReady()
    {
        if (isPlayer1Ready && isPlayer2Ready)
        {
            if (controlsSetAction != null)
            {
                controlsSetAction.Invoke(p1AssignedController.GetComponent<ControllerDeviceUI>(), p2AssignedController.GetComponent<ControllerDeviceUI>());
                p1DeviceID = p1AssignedController.GetComponent<ControllerDeviceUI>().assignedDeviceID;
                p2DeviceID = p2AssignedController.GetComponent<ControllerDeviceUI>().assignedDeviceID;
                Destroy(p1AssignedController);
                Destroy(p2AssignedController);
                ModifyReadyText(p1ReadyText, false);
                ModifyReadyText(p2ReadyText, false);
                isPlayer1Ready = false;
                isPlayer2Ready = false;
                foreach (GameObject inputObject in playerInputList)
                {
                    Destroy(inputObject);
                }
                controlSettingsCanvas.SetActive(false);
                isActive = false;
            }
        }
    }

    public void OnActivate()
    {
        if(OnActivateAction!=null)
            OnActivateAction.Invoke();

        isActive = true;
        controlSettingsCanvas.SetActive(true);
        playerInputList.Clear();

        int i = 0;
        PlayerInput p1Input = PlayerInput.Instantiate(controllerUIEntity, i, "Keyboard", -1, Keyboard.current.device);
        p1Input.transform.SetParent(controlSettingsCanvas.transform);
        p1Input.transform.SetPositionAndRotation(deviceListUIPosition.position - (Vector3.up * i * deviceUIOffset), Quaternion.identity);
        p1Input.transform.GetComponent<SpriteRenderer>().sprite = WASDSprite;
        p1Input.transform.GetComponent<ControllerDeviceUI>().assignedDevice = Keyboard.current.device;
        p1Input.transform.GetComponent<ControllerDeviceUI>().assignedScheme = p1Input.currentControlScheme;
        p1Input.transform.GetComponent<ControllerDeviceUI>().assignedDeviceID = Keyboard.current.deviceId;

        i++;
        PlayerInput p2Input = PlayerInput.Instantiate(controllerUIEntity, i, "Keyboard Player 2", -1, Keyboard.current.device);
        p2Input.transform.SetParent(controlSettingsCanvas.transform);
        p2Input.transform.SetPositionAndRotation(deviceListUIPosition.position - (Vector3.up * i * deviceUIOffset), Quaternion.identity);
        p2Input.transform.GetComponent<SpriteRenderer>().sprite = arrowKeysSprite;
        p2Input.transform.GetComponent<ControllerDeviceUI>().assignedDevice = Keyboard.current.device;
        p2Input.transform.GetComponent<ControllerDeviceUI>().assignedScheme = p2Input.currentControlScheme;
        p2Input.transform.GetComponent<ControllerDeviceUI>().assignedDeviceID = Keyboard.current.deviceId;

        playerInputList.Add(p1Input.gameObject);
        playerInputList.Add(p2Input.gameObject);


        for (int g = 0; g < Gamepad.all.Count; g++)
        {
            i++;
            PlayerInput gamepadInput = PlayerInput.Instantiate(controllerUIEntity, i, "Gamepad", -1, Gamepad.all[g].device);
            gamepadInput.transform.SetParent(controlSettingsCanvas.transform);
            gamepadInput.transform.SetPositionAndRotation(deviceListUIPosition.position - (Vector3.up * i * deviceUIOffset), Quaternion.identity);
            gamepadInput.transform.GetComponent<SpriteRenderer>().sprite = gamepadSprite;
            gamepadInput.transform.GetComponent<ControllerDeviceUI>().assignedDevice = Gamepad.all[g].device;
            gamepadInput.transform.GetComponent<ControllerDeviceUI>().assignedScheme = "Gamepad";
            gamepadInput.transform.GetComponent<ControllerDeviceUI>().assignedDeviceID = Gamepad.all[g].device.deviceId;
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
        OrderDeviceList();
    }

    private void OrderDeviceList()
    {
        int i = 0;
        foreach (GameObject controllerObject in playerInputList)
        {
            controllerObject.transform.SetPositionAndRotation(deviceListUIPosition.transform.position - Vector3.up * deviceUIOffset * i, Quaternion.identity);
            i++;
        }
    }

    private void TryDealingWithUnityControllerBug()
    {

    }
    public void OnDeviceChange(InputDevice inputDevice, InputDeviceChange inputDeviceChange)
    {
        if (isActive && inputDeviceChange == InputDeviceChange.Added)
        {
            for (int i = 0; i < Gamepad.all.Count; i++)
            {
                if (inputDevice == Gamepad.all[i].device)
                {
                    PlayerInput gamepadInput = PlayerInput.Instantiate(controllerUIEntity, playerInputList.Count + 1, "Gamepad", -1, inputDevice);
                    gamepadInput.transform.SetParent(controlSettingsCanvas.transform);
                    gamepadInput.transform.GetComponent<SpriteRenderer>().sprite = gamepadSprite;
                    gamepadInput.transform.GetComponent<ControllerDeviceUI>().assignedDevice = inputDevice;
                    gamepadInput.transform.GetComponent<ControllerDeviceUI>().assignedScheme = "Gamepad";
                    gamepadInput.transform.GetComponent<ControllerDeviceUI>().assignedDeviceID = inputDevice.deviceId;
                    playerInputList.Add(gamepadInput.gameObject);
                    OrderDeviceList();
                    i = Gamepad.all.Count;
                }
            }
        }

        if(!isActive && inputDeviceChange == InputDeviceChange.Removed && (inputDevice.deviceId == p1DeviceID || inputDevice.deviceId == p2DeviceID))
        {
            OnActivate();
        }
    }

    private void InstantiateGamepad()
    {

    }
}
