using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeUpPlayer : MonoBehaviour
{
    [SerializeField] float timeBeforeCanMove = 2.0f;
    [SerializeField] float fadeTime = 3.0f;

    private bool isTriggered = false;
    private bool camHasStopped = false;

    CameraController camController;
    Animator playerAnimator;

    private float originalPlayerSpeed;
    private float originalPlayerRotationSpeed;
    private float fadeSmoothness;

    private void Awake()
    {
        fadeSmoothness = 1.0f / 30.0f;
    }

    private void Start()
    {
        originalPlayerSpeed = PlayerMovement.MyPlayer.groundSettings.speed;
        originalPlayerRotationSpeed = PlayerMovement.MyPlayer.groundSettings.rotationLerpSpeed;
        camController = Camera.main.GetComponent<CameraController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggered == false && other.gameObject.CompareTag("Player"))
        {
            isTriggered = true;
            playerAnimator = other.GetComponent<Animator>();
            playerAnimator.SetTrigger("WakeUp");

            DisablePlayerMovement();
            if (camController != null && camHasStopped == false)
            {
                camHasStopped = true;
                camController.SwitchSensitivity();
            }

            StartCoroutine(WaitBeforeFadeMovement(timeBeforeCanMove));
        }
    }

    private void DisablePlayerMovement()
    {
        PlayerMovement.MyPlayer.groundSettings.speed = 0.0f;
        PlayerMovement.MyPlayer.abilities.jumpDisabled = true;
        PlayerMovement.MyPlayer.groundSettings.rotationLerpSpeed = 0.0f;
    }

    private IEnumerator WaitBeforeFadeMovement(float time)
    {
        yield return new WaitForSeconds(time);
        playerAnimator.SetBool("IsAwake", true);

        if (camController != null && camHasStopped == true)
        {
            camController.SwitchSensitivityBack();
        }

        StartCoroutine(PlayerMovementFadeIn());
    }

    IEnumerator PlayerMovementFadeIn()
    {
        bool hasFaded = false;
        float delta = 0;

        while (hasFaded == false)
        {
            PlayerMovement.MyPlayer.groundSettings.speed = Mathf.Lerp(0.0f, originalPlayerSpeed, delta);
            PlayerMovement.MyPlayer.groundSettings.rotationLerpSpeed = Mathf.Lerp(0.0f, originalPlayerRotationSpeed, delta);

            delta += 1 / fadeTime * fadeSmoothness;

            if (delta > 0.5f)
            {
                PlayerMovement.MyPlayer.abilities.jumpDisabled = false;
            }

            if (delta >= 1.0f + (1 / fadeTime * fadeSmoothness) * 2.0f)
            {
                hasFaded = true;
            }
            yield return new WaitForSeconds(fadeSmoothness);
        }
    }
}