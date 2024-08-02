
/*using System.Collections;
using UnityEngine;

public class GrandMaMove : MonoBehaviour
{
    public float moveDistance = 10f;  // �̵��� �Ÿ�
    public float moveSpeed = 1f;      // �̵� �ӵ�
    public float fadeDelay = 2f;      // ����ȭ �� ���� �ð�
    public float fadeDuration = 1f;   // ����ȭ �ð�
    public Transform playerTransform; // �÷��̾��� ��ġ ����
    public float chaseSpeed = 5f;     // �÷��̾ �Ѵ� �ӵ�
    public float rotationSpeed = 360f; // ȸ�� �ӵ� (��/��)
    public GameObject playerCamera;       // �÷��̾��� ī�޶� ����

    public static bool isGrandEvent;

    public AudioSource audioSource;  // ���� ����� �ҽ�
    public AudioClip moveSoundClip;  // �̵� �� �Ҹ�
    public AudioClip chaseSoundClip; // �߰� �� �Ҹ�
    public float soundDelay = 1f;     // �̵� ���� ��� ���� �ð�

    private Vector3 initialPosition;  // �ʱ� ��ġ
    private Vector3 targetPosition;   // ��ǥ ��ġ
    private bool moving = false;      // �̵� �� ����
    private Renderer[] childRenderers;

    void Start()
    {
        isGrandEvent = true;
        initialPosition = transform.position;
        targetPosition = initialPosition + transform.forward * moveDistance;
        moving = true;  // ���� �� �̵��� Ȱ��ȭ

        // ��� �ڽ��� Renderer ������Ʈ ��������
        childRenderers = GetComponentsInChildren<Renderer>();

        // �� �ڽ��� Material ���� ����
        foreach (Renderer renderer in childRenderers)
        {
            Material material = renderer.material;
            material.SetOverrideTag("RenderType", "Transparent");
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.renderQueue = 3000;
        }

        StartCoroutine(PlayMoveSound());
    }

    IEnumerator PlayMoveSound()
    {
        yield return new WaitForSeconds(soundDelay); // ���� �ð� ���� ��
        audioSource.clip = moveSoundClip;
        audioSource.Play(); // �̵� ���� ���
    }

    void Update()
    {
        if (moving)
        {
            // ��ǥ ��ġ�� ������ ������ ������Ʈ �̵� (Y�� �̵� ����)
            Vector3 currentPosition = transform.position;
            currentPosition = Vector3.MoveTowards(currentPosition, new Vector3(targetPosition.x, currentPosition.y, targetPosition.z), moveSpeed * Time.deltaTime);
            transform.position = currentPosition;

            // �÷��̾��� ī�޶� �ҸӴϸ� �ٶ󺸵��� ����
            if (playerCamera != null)
            {
                Vector3 directionToGrandma = transform.position - playerCamera.transform.position;
                Quaternion lookRotation = Quaternion.LookRotation(directionToGrandma);
                playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }

            // ��ǥ ��ġ�� �����ߴ��� Ȯ��
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isGrandEvent = false;
                moving = false; // �̵� �Ϸ�
                audioSource.Stop(); // �̵� ���� ����
                StartCoroutine(FaceAndChasePlayer());
            }
        }
    }

    IEnumerator FaceAndChasePlayer()
    {
        // ��� ��� �� �÷��̾ �ٶ󺸵��� ȸ�� (������Ʈ�� Z���� �÷��̾� �������� ����)
        yield return new WaitForSeconds(fadeDelay);

        Vector3 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.y = 0; // Y�� ȸ���� �����Ͽ� �������θ� ȸ���ϰ� ��
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer); // Z���� �÷��̾� ������

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        // 1�� ��� �� �߰� ���� ���
        yield return new WaitForSeconds(1f);
        audioSource.clip = chaseSoundClip;
        audioSource.Play();

        // �÷��̾ ���� �̵� �� ����ȭ ����
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            // �÷��̾� ������ �̵� (Y�� �̵� ����)
            Vector3 newPosition = Vector3.MoveTowards(transform.position, new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z), chaseSpeed * Time.deltaTime);
            transform.position = newPosition;

            // ����ȭ
            foreach (Renderer renderer in childRenderers)
            {
                Material material = renderer.material;
                Color originalColor = material.color;
                float alpha = Mathf.Lerp(originalColor.a, 0, elapsedTime / fadeDuration);
                material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            }

            yield return null;
        }

        // ���������� ������ �����ϰ� ����
        foreach (Renderer renderer in childRenderers)
        {
            Material material = renderer.material;
            Color originalColor = material.color;
            material.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        }
    }
}*/

using System.Collections;
using UnityEngine;

public class GrandMaMove : MonoBehaviour
{
    public float moveDistance = 10f;  // �̵��� �Ÿ�
    public float moveSpeed = 1f;      // �̵� �ӵ�
    public float fadeDelay = 2f;      // ����ȭ �� ���� �ð�
    public float fadeDuration = 1f;   // ����ȭ �ð�
    public Transform playerTransform; // �÷��̾��� ��ġ ����
    public float chaseSpeed = 5f;     // �÷��̾ �Ѵ� �ӵ�
    public float rotationSpeed = 360f; // ȸ�� �ӵ� (��/��)
    public GameObject playerCamera;   // �÷��̾��� ī�޶� ����

    public static bool isGrandEvent;

    public AudioSource audioSource;  // ���� ����� �ҽ�
    public AudioClip moveSoundClip;  // �̵� �� �Ҹ�
    public AudioClip chaseSoundClip; // �߰� �� �Ҹ�
    public float soundDelay = 1f;    // �̵� ���� ��� ���� �ð�

    private Vector3 initialPosition;  // �ʱ� ��ġ
    private Vector3 targetPosition;   // ��ǥ ��ġ
    private bool moving = false;      // �̵� �� ����
    private Renderer[] childRenderers;

    void Start()
    {
        // ��� �ڽ��� Renderer ������Ʈ ��������
        childRenderers = GetComponentsInChildren<Renderer>();

        // �� �ڽ��� Material ���� ����
        foreach (Renderer renderer in childRenderers)
        {
            Material material = renderer.material;
            material.SetOverrideTag("RenderType", "Transparent");
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.renderQueue = 3000;
        }
    }

    public void StartGrandmaEvent()
    {
        isGrandEvent = true;
        initialPosition = transform.position;
        targetPosition = initialPosition + transform.forward * moveDistance;
        StartCoroutine(GrandmaEventSequence());
    }

    

    IEnumerator GrandmaEventSequence()
    {
        moving = true;
        yield return StartCoroutine(PlayMoveSound());

        while (moving)
        {
            // ��ǥ ��ġ�� ������ ������ ������Ʈ �̵� (Y�� �̵� ����)
            Vector3 currentPosition = transform.position;
            currentPosition = Vector3.MoveTowards(currentPosition, new Vector3(targetPosition.x, currentPosition.y, targetPosition.z), moveSpeed * Time.deltaTime);
            transform.position = currentPosition;

            // �÷��̾��� ī�޶� �ҸӴϸ� �ٶ󺸵��� ����
            if (playerCamera != null)
            {
                Vector3 directionToGrandma = transform.position - playerCamera.transform.position;
                Quaternion lookRotation = Quaternion.LookRotation(directionToGrandma);
                playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }

            // ��ǥ ��ġ�� �����ߴ��� Ȯ��
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isGrandEvent = false;
                moving = false; // �̵� �Ϸ�
                audioSource.Stop(); // �̵� ���� ����
            }

            yield return null;
        }

        yield return StartCoroutine(FaceAndChasePlayer());
    }

    IEnumerator PlayMoveSound()
    {
        yield return new WaitForSeconds(soundDelay); // ���� �ð� ���� ��
        audioSource.clip = moveSoundClip;
        audioSource.Play(); // �̵� ���� ���
    }

    IEnumerator FaceAndChasePlayer()
    {
        // ��� ��� �� �÷��̾ �ٶ󺸵��� ȸ�� (������Ʈ�� Z���� �÷��̾� �������� ����)
        yield return new WaitForSeconds(fadeDelay);

        Vector3 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.y = 0; // Y�� ȸ���� �����Ͽ� �������θ� ȸ���ϰ� ��
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer); // Z���� �÷��̾� ������

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        // 1�� ��� �� �߰� ���� ���
        yield return new WaitForSeconds(1f);
        audioSource.clip = chaseSoundClip;
        audioSource.Play();

        // �÷��̾ ���� �̵� �� ����ȭ ����
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            // �÷��̾� ������ �̵� (Y�� �̵� ����)
            Vector3 newPosition = Vector3.MoveTowards(transform.position, new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z), chaseSpeed * Time.deltaTime);
            transform.position = newPosition;

            // ����ȭ
            foreach (Renderer renderer in childRenderers)
            {
                Material material = renderer.material;
                Color originalColor = material.color;
                float alpha = Mathf.Lerp(originalColor.a, 0, elapsedTime / fadeDuration);
                material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            }

            yield return null;
        }

        // ���������� ������ �����ϰ� ����
        foreach (Renderer renderer in childRenderers)
        {
            Material material = renderer.material;
            Color originalColor = material.color;
            material.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        }
    }
}


