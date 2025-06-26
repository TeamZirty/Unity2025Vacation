using UnityEngine;

public class Rotator : MonoBehaviour
{
    private Camera camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dirVec = mousePos - (Vector2)transform.position; //���콺�� �ٶ󺸴� ������ ��Ÿ���� ����

        transform.up = (Vector3)dirVec.normalized;

    }
}
