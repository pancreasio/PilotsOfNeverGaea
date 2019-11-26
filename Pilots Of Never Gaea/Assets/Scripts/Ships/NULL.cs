using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NULL : Palette
{
    private Animator animator;
    public LevelManager.UpdatePower UpdateGlitchPower;
    public LevelManager.PaletteAction GlitchAction;
    public GameObject glitchPrefab;
    public float powerIncrease;
    private float powerAmmount;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        powerAmmount = 0f;
    }

    protected override void Update()
    {
        if(!action)
        base.Update();

        if (left)
            action = Input.GetKey(KeyCode.Space);
        else
            action = Input.GetKey(KeyCode.RightControl);

        animator.SetBool("ACTION", action);

        if (action)
            powerAmmount += powerIncrease * Time.deltaTime;

        if (UpdateGlitchPower != null)
            UpdateGlitchPower(powerAmmount);

        if (powerAmmount >= 1f)
        {
            Glitch();
        }
    }

    private void Glitch()
    {
        if (GlitchAction != null)
            GlitchAction();

        if (glitchPrefab != null)
            Instantiate(glitchPrefab, Vector2.zero, Quaternion.identity);
    }

    public override void ResetPalette()
    {
        base.ResetPalette();
        powerAmmount = 0f;
    }
}