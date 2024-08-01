using System.Collections;
using UnityEngine;

public class BedEvent : MonoBehaviour
{
    public Transform childObject;      // �ڽ� ������Ʈ
    public Transform parentObject;     // �θ� ������Ʈ
    public Transform specialObject;    // Ư�� ������Ʈ
    public AudioSource audioSource;    // ���� ����� �ҽ�
    public AudioClip shakeClip;        // ��鸲 �Ҹ� Ŭ��
    public AudioClip specialClip;      // Ư�� ������Ʈ �Ҹ� Ŭ��
    public float shakeAmount = 0.1f;   // ��鸲�� ����
    public float shakeSpeed = 5f;      // ��鸲 �ӵ�
    public float shakeDuration = 1f;   // ��鸲 ���� �ð�
    public float shakeInterval = 2f;   // ��鸲 �ֱ�
    public float rotationSpeed = 90f;  // �θ� ������Ʈ�� ȸ�� �ӵ�
    public float targetRotationZ = 90f; // ��ǥ ȸ���� (Z��)
    public float specialObjectDelay = 1f; // Ư�� ������Ʈ�� ��Ÿ���� ���� �ð�
    public bool triggerEvent = false;  // �̺�Ʈ�� ������ ���� ����

    private Vector3 originalChildPosition;
    private Quaternion originalParentRotation; // �θ� ������Ʈ�� ���� ȸ��
    private bool rotationComplete = false; // ȸ�� �Ϸ� ���� üũ
    private bool shaking = true; // �ڽ� ������Ʈ�� ��鸮�� �ִ��� ����

    private void Start()
    {
        originalChildPosition = childObject.localPosition;
        originalParentRotation = parentObject.localRotation;
        StartCoroutine(ShakeRoutine());
    }

    private void Update()
    {
        if (triggerEvent)
        {
            if (!rotationComplete)
            {
                // �θ� ������Ʈ�� ȸ��
                Quaternion targetRotation = Quaternion.Euler(parentObject.localEulerAngles.x, parentObject.localEulerAngles.y, targetRotationZ);
                parentObject.localRotation = Quaternion.RotateTowards(parentObject.localRotation, targetRotation, rotationSpeed * Time.deltaTime);

                // ��ǥ ȸ���� �����ߴ��� üũ
                if (Quaternion.Angle(parentObject.localRotation, targetRotation) < 0.1f)
                {
                    rotationComplete = true; // ȸ�� �Ϸ� ǥ��
                    shaking = false; // ��鸲 ����
                    StartCoroutine(ActivateSpecialObject());
                }
            }
        }
    }

    IEnumerator ShakeRoutine()
    {
        while (shaking)
        {
            // ���� �ð� ���
            yield return new WaitForSeconds(shakeInterval);

            // ��鸲 ȿ�� ����
            float elapsedTime = 0f;
            audioSource.clip = shakeClip;
            audioSource.Play();

            while (elapsedTime < shakeDuration)
            {
                elapsedTime += Time.deltaTime;
                Vector3 shakePosition = originalChildPosition + Random.insideUnitSphere * shakeAmount;
                childObject.localPosition = Vector3.Lerp(childObject.localPosition, shakePosition, Time.deltaTime * shakeSpeed);
                yield return null;
            }

            // ��鸲 ȿ�� ����
            audioSource.Stop();

            // ���� ��ġ�� ����
            childObject.localPosition = originalChildPosition;
        }
    }

    IEnumerator ActivateSpecialObject()
    {
        yield return new WaitForSeconds(specialObjectDelay); // Ư�� ������Ʈ Ȱ��ȭ �� ���� �ð�

        if (specialObject != null && !specialObject.gameObject.activeSelf)
        {
            specialObject.gameObject.SetActive(true);
            audioSource.clip = specialClip;
            audioSource.Play();
        }

        // ȿ�� ���� �� ���� ���·� ����
        yield return new WaitForSeconds(specialObjectDelay);
        ResetEvent();
    }

    private void ResetEvent()
    {
        // �θ� ������Ʈ ȸ���� �������
        parentObject.localRotation = originalParentRotation;

        // �ڽ� ������Ʈ ��ġ �ʱ�ȭ
        childObject.localPosition = originalChildPosition;

        // ���� ����
        audioSource.Stop();

        // Ư�� ������Ʈ ��Ȱ��ȭ
        if (specialObject != null)
        {
            specialObject.gameObject.SetActive(false);
        }

        // ���� �ʱ�ȭ
        rotationComplete = false;
        shaking = true;
        triggerEvent = false;
    }
}
