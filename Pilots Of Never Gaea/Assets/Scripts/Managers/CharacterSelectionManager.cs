﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
    public enum Character
    {
        none,
        raildrive,
        magstream,
        kunst,
        knockout,
        djinn,
        Leytenant,
        NULL,
    }
    public static GameManager.StartDuelFunction SelectAction;
    public static GameManager.SceneChange FadeAction;
    public static GameManager.ButtonAction ExitButtonAction;
    public Character p1SelectedCharacter, p2SelectedCharacter;
    public float platformTime, platformLimit, cancelTime;
    private float platformClock, exitClock;
    private Vector3 leftPlatformStartingPosition, rightPlatformStartingPosition;
    public GameObject p1Elements, p2Elements, background;
    private bool openingPlatforms = false, closingPlatforms = true;

    private void Start()
    {
        AkSoundEngine.PostEvent("music_select_ship", gameObject);
        //AkSoundEngine.PostEvent("sfx_ocdoors", gameObject);
        p1SelectedCharacter = Character.none;
        p2SelectedCharacter = Character.none;
        leftPlatformStartingPosition = p1Elements.transform.position;
        rightPlatformStartingPosition = p2Elements.transform.position;
        p1Elements.transform.Translate(-transform.right * platformLimit);
        p2Elements.transform.Translate(transform.right * platformLimit);
        platformClock = 0f;
        exitClock = 0f;
    }

    private void Update()
    {
        if (closingPlatforms)
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
                closingPlatforms = false;
            }

        }
        else
        {
            if (openingPlatforms || (p1SelectedCharacter != Character.none && p2SelectedCharacter != Character.none))
            {
                if (!openingPlatforms)
                {
                    platformClock = 0f;
                    openingPlatforms = true;
                    //AkSoundEngine.PostEvent("sfx_ocdoors", gameObject);
                    background.SetActive(false);
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
            else
            {
                if (Input.GetKey(KeyCode.Escape))
                    exitClock += Time.deltaTime;
                else
                    exitClock = 0f;

                if (exitClock > cancelTime)
                {
                    if (ExitButtonAction != null)
                        ExitButtonAction();
                }
            }
        }
    }
}
