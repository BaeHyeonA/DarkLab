using UnityEngine;

public class WaterToBloodEffect : MonoBehaviour
{
    public ParticleSystem waterToBloodParticles;
    public Material wallMaterial;
    public float transitionTime = 5.0f;
    private float elapsedTime = 0.0f;
    private ParticleSystem.MainModule mainModule;
    private ParticleSystem.EmissionModule emissionModule;
    private ParticleSystem.ColorOverLifetimeModule colorModule;
    private Gradient grad;

    void Start()
    {
        mainModule = waterToBloodParticles.main;
        emissionModule = waterToBloodParticles.emission;
        colorModule = waterToBloodParticles.colorOverLifetime;

        // �ʱ� �� ���� ����
        grad = new Gradient();
        grad.SetKeys(new GradientColorKey[]
        {
            new GradientColorKey(new Color(0.5f, 0.5f, 1f, 1f), 0.0f),
            new GradientColorKey(Color.red, 1.0f)
        }, new GradientAlphaKey[]
        {
            new GradientAlphaKey(1.0f, 0.0f),
            new GradientAlphaKey(1.0f, 1.0f)
        });
        colorModule.color = grad;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime < transitionTime)
        {
            float t = elapsedTime / transitionTime;

            // ��ƼŬ ũ��, �ӵ�, ����, ���� �ӵ� ����
            mainModule.startSize = Mathf.Lerp(0.05f, 0.3f, t);
            mainModule.startSpeed = Mathf.Lerp(1f, 5f, t);
            mainModule.startLifetime = Mathf.Lerp(0.5f, 3f, t);
            emissionModule.rateOverTime = Mathf.Lerp(20f, 100f, t);

            // �� �ؽ�ó�� ���� ��ȭ
            wallMaterial.SetColor("_FlowColor", Color.Lerp(new Color(0.5f, 0.5f, 1f, 1f), Color.red, t));
        }
    }

    public void StartLeakChange()
    {
        // ��ũ��Ʈ�� ����Ͽ� ������� �Ƿ� ���ϴ� �̺�Ʈ�� ����
        elapsedTime = 0.0f;
    }
}
