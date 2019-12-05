using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leytenant : Palette
{
    public float speedReduction, recieveTime, holdTime, cannonRotationSpeed;
    private bool shooting, recieving;
    public GameObject cannon, openClaw, closedClaw;
    public Transform cannonVector;

    protected override void Start()
    {
        base.Start();
        recieving = false;
        shooting = false;
    }

    protected override void Update()
    {
        base.Update();

        if (power && action)
            StartCoroutine(Artillery());
    }

    private IEnumerator Artillery()
    {
        power = false;
        action = false;
        charges -= chargesRequired;
        UpdateCharges(charges);
        speed = speed / speedReduction;
        float recieveClock = 0f;
        recieving = true;
        openClaw.SetActive(true);
        while (recieveClock < recieveTime && recieving)
        {
            recieveClock += Time.deltaTime;
            yield return null;
        }
        recieving = false;
        openClaw.SetActive(false);
        speed *= speedReduction;
    }

    private IEnumerator Arm(Ball ball)
    {
        recieving = false;
        Vector2 armPosition = transform.position;
        float holdClock = 0f, rotation = 0f;
        cannon.SetActive(true);
        closedClaw.SetActive(true);
        cannon.transform.rotation = Quaternion.identity;
        if (!isPlayer1)
        {
            cannon.transform.Rotate(Vector3.back, 180f);
        }
        while (holdClock < holdTime)
        {
            holdClock += Time.deltaTime;
            transform.position = new Vector2(transform.position.x, armPosition.y);
            ball.transform.position = transform.position;
            rotation = 0f;
            rotation = cannonRotationSpeed * -moveSpeed;

            cannon.transform.Rotate(Vector3.back, rotation * Time.deltaTime);
            yield return null;
        }
        closedClaw.SetActive(false);
        ball.InitialKick(cannonVector.position - cannon.transform.position);
        ball.ArtilleryShot();
        cannon.SetActive(false);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (!recieving && !shooting)
            base.OnCollisionEnter2D(collision);
    }

    public void CatchBall(Ball ball)
    {
        StartCoroutine(Arm(ball));
    }
}
