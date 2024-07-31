

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WakeUp : MonoBehaviour
{
    [Header("Player")]
    public Transform playerTransform;  // �÷��̾��� Ʈ������
    public Transform bedPosition;      // ħ�� ��ġ
    public Rigidbody rb;               // �÷��̾��� ������ٵ�
    public Collider cd;                // �÷��̾��� �ݶ��̴�

    [Header("Blinking Effect")]
    public Image blackScreen;          // UI �̹����� ���Ǵ� ����ũ��
    public float blinkDuration = 0.1f; // �� ������ �ð�
    public int blinkCount = 3;         // �� ������ Ƚ��

    [Header("Transition Settings")]
    public float transitionTime = 5f;  // ȸ�� �ð�
    public float turningHeadDuration = 2f; // �� ������ �� ������ �ð�
    public int headTurnCount = 2;      // �� ������ Ƚ��
    public static bool isWakeUp;

    void Start()
    {
        isWakeUp = false;  
        if (rb != null)
        {
            rb.isKinematic = true; // ȸ�� �� ������ ��ȣ�ۿ� ����
        }
        if (cd != null)
        {
            cd.enabled = false; // ȸ�� �� ������ ��ȣ�ۿ� ����
        }

        // �÷��̾ ħ�� ��ġ�� �̵�
        playerTransform.position = bedPosition.position;
        playerTransform.rotation = bedPosition.rotation;

        // �� ������ ȿ�� ����
        StartCoroutine(BlinkAndWakeUp());
    }

    IEnumerator BlinkAndWakeUp()
    {
        yield return StartCoroutine(BlinkScreen());

        // �÷��̾��� ȸ���� õõ�� X�� �������� -90�� ����
        yield return StartCoroutine(RotatePlayerX());

        // �� ������ ȿ��
        StartCoroutine(LookAround(headTurnCount));
    }

    IEnumerator BlinkScreen()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            // ������ ����: ȭ���� �˰�
            yield return FadeScreen(1f);

            // ��� ���
            yield return new WaitForSeconds(blinkDuration);

            // ������ ��: ȭ���� �������
            yield return FadeScreen(0f);

            // ��� ���
            yield return new WaitForSeconds(blinkDuration);
        }
    }

    IEnumerator FadeScreen(float targetAlpha)
    {
        Color currentColor = blackScreen.color;
        float startAlpha = currentColor.a;
        float elapsedTime = 0f;

        while (elapsedTime < blinkDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / blinkDuration);
            blackScreen.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            yield return null;
        }

        // ���������� ��Ȯ�� ���� �� ����
        blackScreen.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
    }

    IEnumerator RotatePlayerX()
    {
        float elapsedTime = 0f;
        Quaternion startRotation = playerTransform.rotation;

        // ��ǥ ȸ��: ���� ȸ������ X���� �������� -90�� ȸ���� ���ʹϾ�
        Quaternion targetRotation = Quaternion.Euler(startRotation.eulerAngles.x + 90f, startRotation.eulerAngles.y, startRotation.eulerAngles.z);

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            Quaternion newRotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / transitionTime);

            if (rb != null)
            {
                rb.MoveRotation(newRotation); // Rigidbody�� MoveRotation�� ����Ͽ� ȸ��
            }
            else
            {
                playerTransform.rotation = newRotation;
            }

            yield return null; // ���� �����ӱ��� ���
        }

        // ���� ȸ�� ���� �� ������ ��ȣ�ۿ� Ȱ��ȭ
        if (rb != null)
        {
            rb.MoveRotation(targetRotation);
            rb.velocity = Vector3.zero; // ���ʿ��� �̵� ����
            rb.angularVelocity = Vector3.zero; // ���ʿ��� ȸ�� ����
            rb.isKinematic = false; // ������ ��ȣ�ۿ� �ٽ� Ȱ��ȭ
        }
        else
        {
            playerTransform.rotation = targetRotation;
        }

        if (cd != null)
        {
            cd.enabled = true; // Collider �ٽ� Ȱ��ȭ
        }

       // isWakeUp = true;
    }

    IEnumerator LookAround(int turns)
    {
        for (int i = 0; i < turns; i++)
        {
            float elapsedTime = 0f;
            Quaternion initialRotation = playerTransform.rotation;

            // ���� �������� ������ ��ǥ ȸ��
            Quaternion leftRotation = Quaternion.Euler(initialRotation.eulerAngles.x, initialRotation.eulerAngles.y - 50f, initialRotation.eulerAngles.z);
            // ���� ���������� ������ ��ǥ ȸ��
            Quaternion rightRotation = Quaternion.Euler(initialRotation.eulerAngles.x, initialRotation.eulerAngles.y + 50f, initialRotation.eulerAngles.z);

            // ���� �������� ������
            while (elapsedTime < turningHeadDuration / 2)
            {
                elapsedTime += Time.deltaTime;
                Quaternion newRotation = Quaternion.Slerp(initialRotation, leftRotation, elapsedTime / (turningHeadDuration / 2));
                playerTransform.rotation = newRotation;
                yield return null;
            }

            // ���� ���������� ������
            elapsedTime = 0f;
            while (elapsedTime < turningHeadDuration / 2)
            {
                elapsedTime += Time.deltaTime;
                Quaternion newRotation = Quaternion.Slerp(leftRotation, rightRotation, elapsedTime / (turningHeadDuration / 2));
                playerTransform.rotation = newRotation;
                yield return null;
            }

            // ���������� ���� ��ġ�� ����
            elapsedTime = 0f;
            while (elapsedTime < turningHeadDuration / 2)
            {
                elapsedTime += Time.deltaTime;
                Quaternion newRotation = Quaternion.Slerp(rightRotation, initialRotation, elapsedTime / (turningHeadDuration / 2));
                playerTransform.rotation = newRotation;
                yield return null;
            }
        }
        isWakeUp = true;
    }
}



