using UnityEngine;
using System.Collections;

public class GrandMaMove : MonoBehaviour
{
    public float moveDistance = 10f;  // �̵��� �Ÿ�
    public float moveSpeed = 1f;      // �̵� �ӵ�
    public float fadeDelay = 2f;      // ����ȭ �� ���� �ð�
    public float fadeDuration = 1f;   // ����ȭ �ð�

    private Vector3 initialPosition;  // �ʱ� ��ġ
    private Vector3 targetPosition;   // ��ǥ ��ġ
    private bool moving = false;      // �̵� �� ����
    private Renderer[] childRenderers;

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = -(initialPosition + transform.forward * moveDistance);
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
            material.DisableKeyword("_ALPHABLEND_ON");
            material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
        }
    }

    void Update()
    {
        if (moving)
        {
            // ������Ʈ�� ��ǥ ��ġ�� �̵�
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // ��ǥ ��ġ�� �����ߴ��� Ȯ��
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                moving = false; // �̵� �Ϸ�
                StartCoroutine(FadeOut(fadeDelay, fadeDuration)); // ���� �ð� �� ����ȭ ����
            }
        }
    }

    IEnumerator FadeOut(float delay, float duration)
    {
        yield return new WaitForSeconds(delay); // ����ȭ �� ���� �ð�

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            foreach (Renderer renderer in childRenderers)
            {
                Material material = renderer.material;
                Color originalColor = material.color;
                float alpha = Mathf.Lerp(originalColor.a, 0, elapsedTime / duration);
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
