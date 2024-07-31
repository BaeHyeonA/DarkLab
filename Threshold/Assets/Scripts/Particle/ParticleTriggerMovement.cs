using System.Collections;
using UnityEngine;

public class ParticleTriggerMovement : MonoBehaviour
{
    public ParticleSystem particleSystem;  // ��ƼŬ �ý��� ����
    public GameObject targetObject;  // Ȱ��ȭ�ϰ� ������ ��� ������Ʈ
    public float startDelay = 3.0f;  // ��ƼŬ ���� ���� �ð�
    public float targetYIncrease = 5.0f;  // Y�� ������
    public float duration = 5.0f;  // Y�� ���� ���� �ð�

    private Vector3 initialPosition;
    private bool isMoving = false;

    void Start()
    {
        if (targetObject != null)
        {
            // ������Ʈ�� ��Ȱ��ȭ ���·� ����
            targetObject.SetActive(false);
            initialPosition = targetObject.transform.position;

            // ���� �ð� �Ŀ� ��ƼŬ �ý��� ���� �� �̵� ����
            Invoke("StartParticleAndMove", startDelay);
        }
    }

    void StartParticleAndMove()
    {
        if (particleSystem != null && targetObject != null)
        {
            particleSystem.Play();
            targetObject.SetActive(true);
            StartCoroutine(MoveObject());
        }
    }

    IEnumerator MoveObject()
    {
        isMoving = true;
        float elapsedTime = 0f;
        Vector3 targetPosition = new Vector3(initialPosition.x, initialPosition.y + targetYIncrease, initialPosition.z);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            targetObject.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            yield return null;
        }

        // Ensure the object reaches the exact target position
        targetObject.transform.position = targetPosition;
        isMoving = false;
    }
}
