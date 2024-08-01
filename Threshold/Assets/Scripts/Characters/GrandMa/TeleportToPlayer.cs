using UnityEngine;
using System.Collections;

public class TeleportToPlayer : MonoBehaviour
{
    public float moveDistance = 10f;  // �̵��� �Ÿ�
    public float moveSpeed = 1f;      // �̵� �ӵ�
    public float lookAtDuration = 1f; // �÷��̾ �ٶ󺸴� �ð�
    public float disappearDelay = 2f; // ������� �� ���� �ð�
    public Transform playerTransform; // �÷��̾��� ��ġ ����
    public float teleportOffset = 2f; // �÷��̾� �տ��� ������ �Ÿ�
    public float teleportHeightOffset = 0f; // Y�� ��ġ ����
    public float fadeDuration = 1f;   // ����ȭ �ð�

    public AudioClip moveSound;       // �̵� �� ���� Ŭ��
    public AudioClip teleportSound;   // �����̵� �� ���� Ŭ��
    private AudioSource audioSource;  // ����� �ҽ�

    private Vector3 initialPosition;  // �ʱ� ��ġ
    private Vector3 targetPosition;   // ��ǥ ��ġ
    private Renderer[] childRenderers;

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + transform.forward * moveDistance;

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

        // AudioSource ������Ʈ �߰�
        audioSource = gameObject.AddComponent<AudioSource>();

        StartCoroutine(MoveAndTeleport());
    }

    IEnumerator MoveAndTeleport()
    {
        // ��ǥ ��ġ�� �̵�
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            Vector3 currentPosition = transform.position;
            currentPosition = Vector3.MoveTowards(currentPosition, new Vector3(targetPosition.x, currentPosition.y, targetPosition.z), moveSpeed * Time.deltaTime);
            transform.position = currentPosition;
            yield return null;
        }

        // �÷��̾ �ٶ󺸱�
        yield return new WaitForSeconds(lookAtDuration);

        Vector3 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.y = 0; // �������θ� ȸ���ϰ� ��
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 360f);
            yield return null;
        }

        // �����̵� ���� ��� (���� �� ���� ���)
        PlaySound(teleportSound);

        // �÷��̾� �տ� �����̵�
        Vector3 teleportPosition = playerTransform.position + playerTransform.forward * teleportOffset;
        teleportPosition.y = playerTransform.position.y + teleportHeightOffset; // Y�� ��ġ ����
        transform.position = teleportPosition;

        // �����
        yield return StartCoroutine(FadeOutAndDisappear(disappearDelay));
    }


    IEnumerator FadeOutAndDisappear(float delay)
    {
        yield return new WaitForSeconds(delay); // ������� �� ���� �ð�

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

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

        // ������Ʈ�� ��Ȱ��ȭ�ϰų� �ı�
        gameObject.SetActive(false);
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
