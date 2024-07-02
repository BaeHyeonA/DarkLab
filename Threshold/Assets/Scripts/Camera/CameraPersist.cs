using UnityEngine;
using Cinemachine; // Cinemachine�� ����ϱ� ���� �߰�

public class CameraPersist : MonoBehaviour
{
    public GameObject virtualCamera;
    void Awake()
    {
        // ������ ������Ʈ�� ���� �� ������ �ʵ��� üũ
        int numOfCameras = FindObjectsOfType<CameraPersist>().Length;
        if (numOfCameras > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);

            // ���� ī�޶� �ڽ� ������Ʈ�� �ִ� ��쿡 ���� ó��
           
                DontDestroyOnLoad(virtualCamera);
            
        }
    }
}
