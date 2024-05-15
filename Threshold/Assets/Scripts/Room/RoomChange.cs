using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChange : MonoBehaviour
{
    public GameObject[] As;
    public GameObject[] Bs;
    public GameObject[] Cs;
    public GameObject[] Ds;

    [SerializeField]private int stage;
    public List<GameObject> curObjects = new List<GameObject>();
    public List<GameObject> setObjects = new List<GameObject>();
    public bool[] isSelectObject = new bool[4] { false, false, false, false };
    // Start is called before the first frame update
    void Start()
    {
        setObjects = RandomSelect();
        Copy();
        stage = 1;
        



    }
    void Copy() 
    {
        for (int i = 0; i < setObjects.Count; i++) 
        {
            curObjects.Add(setObjects[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RoomStage(curObjects);  // �����̽��ٸ� ������ �� �Լ� ����
        }
    }

    private List<GameObject> RandomSelect()
    {
        List<GameObject> curObjects = new List<GameObject>();
        int num = UnityEngine.Random.Range(0, 2);
        As[num].SetActive(true);
        curObjects.Add(As[num]);    
        Bs[num].SetActive(true);
        curObjects.Add(Bs[num]);   
        Cs[num].SetActive(true);
        curObjects.Add(Cs[num]);
        Ds[num].SetActive(true);
        curObjects.Add(Ds[num]);
        return curObjects;
       
    }
    bool isAllSelect() 
    {
        for(int i = 0; i < isSelectObject.Length;i++) 
        {
            if (isSelectObject[i] == false) return false;
        }
        return true;

    }

    private void ResetBool ()
    {
        for (int i = 0; i < isSelectObject.Length; i++)
        {
            isSelectObject[i] = false;
        }


    }


    void RoomStage(List<GameObject> gameObjects) 
    {
        if (!isAllSelect())
        {
            int num = Random.Range(0, gameObjects.Count);
            if(isSelectObject[num] == true) 
            {
                RoomStage(curObjects);
            }
            if (stage == 1)
            {
                gameObjects[num].transform.GetChild(0).gameObject.SetActive(false);
                gameObjects[num].transform.GetChild(1).gameObject.SetActive(true);
                Debug.Log("1�ܰ� �Ծ��");
            }
            if (stage == 2)
            {
                gameObjects[num].transform.GetChild(1).gameObject.SetActive(false);
                gameObjects[num].transform.GetChild(2).gameObject.SetActive(true);
            }
            isSelectObject[num] = true;
        }
        else 
        {
            Debug.Log("���� �����ϴ�.");
            if (stage != 2)
            {
                stage++;
                Debug.Log("���������� �����մϴ�.");
                ResetBool();
                RoomStage(curObjects);
            }
            else 
            {
                Debug.Log("�ְ� �ܰ��Դϴ�!");
            }
        }

        //gameObjects.Remove(As)

    }
}
