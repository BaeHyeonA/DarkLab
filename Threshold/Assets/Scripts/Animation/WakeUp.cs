using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WakeUp : MonoBehaviour
{
    [Header("Player")]
    public Transform playerTransform;  // �÷��̾��� Ʈ������
    public Transform bedPosition;      // ħ�� ��ġ
    public Rigidbody rb;               // �÷��̾��� ������ٵ�
    public Collider cd;
    [Header("Transition Settings")]
    public float transitionTime = 5f;  // ȸ�� �ð�

    void Start()
    {
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

        // �÷��̾��� ȸ���� õõ�� X�� �������� -90�� ����
        StartCoroutine(RotatePlayerX());
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
            cd.enabled = true; // ȸ�� �� ������ ��ȣ�ۿ� ����
            var rb = cd.AddComponent<Rigidbody>();
            
        }
    }
}
