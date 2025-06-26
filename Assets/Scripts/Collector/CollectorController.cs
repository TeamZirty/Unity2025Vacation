using UnityEngine;

public class CollectorController : MonoBehaviour
{
    public float maxDistanceFromPlayer = 5f; // �÷��̾�κ��� �ִ� �Ÿ�
    private GameObject player; // �÷��̾� ������Ʈ
    private LineRenderer lineRenderer; // ���콺���� ���� �׸� LineRenderer

    void Start()
    {
        // �÷��̾� ������Ʈ�� �±׷� ã�ų�, �̸� �Ҵ�
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player ������Ʈ�� 'Player' �±׸� �������ּ���!");
            enabled = false; // �÷��̾ ã�� ���ϸ� ��ũ��Ʈ ��Ȱ��ȭ
            return;
        }

        // LineRenderer ������Ʈ �������� (������ ������Ʈ�� �߰� �ʿ�)
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("������ ������Ʈ�� LineRenderer ������Ʈ�� �߰����ּ���!");
            enabled = false;
        }

        // LineRenderer ���� (���ϴ� ����, �β� ������ �ν����Ϳ��� ���� ����)
        lineRenderer.positionCount = 2; // �������� ���� 2��
    }

    void Update()
    {
        if (player == null || lineRenderer == null) return;

        // 1. ���콺 ���� ��ǥ ��������
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 2. �÷��̾� ��ġ
        Vector2 playerPos = player.transform.position;

        // 3. �÷��̾�� ���콺������ ���� ����
        Vector2 direction = (mouseWorldPos - playerPos).normalized;

        // 4. ������ ��ġ ������Ʈ
        // �÷��̾�κ��� ���콺 �������� �ִ� �Ÿ������� �̵�
        // �� �κ��� �����Ͽ� ���콺�� �־����� �����Ⱑ ���󰡴� �ӵ� ���� ������ �� �ֽ��ϴ�.
        // ���⼭�� ���콺 ��ġ�� �÷��̾�κ��� ���� �Ÿ��� �Ѿ�� �� �Ÿ������� �÷��͸� ��ġ�մϴ�.
        float distance = Vector2.Distance(playerPos, mouseWorldPos);
        Vector2 targetCollectorPos = playerPos + direction * Mathf.Min(distance, maxDistanceFromPlayer);
        transform.position = targetCollectorPos;

        // 5. LineRenderer ������Ʈ (�÷��̾�� ��������� �� �׸���)
        // �Ǵ� �÷��̾�� ���콺���� ���� �׸� ���� �ֽ��ϴ�. (��ȹ�� ���� ����)
        lineRenderer.SetPosition(0, playerPos);
        lineRenderer.SetPosition(1, transform.position); // ������ ��ġ
        // lineRenderer.SetPosition(1, mouseWorldPos); // ���콺 ��ġ���� �׸� ���
    }

    // �浹 ó�� (Collider2D�� Rigidbody2D �ʿ�)
    void OnTriggerEnter2D(Collider2D other)
    {
        // ���� ������Ʈ�� �浹
        if (other.CompareTag("Collectible"))
        {
            // ���� ȹ��, ������Ʈ �ı� �� ó��
            Debug.Log("������Ʈ ȹ��!");
            GameManager.Instance?.AddScore(1); // GameManager�� ������ٸ�
            Destroy(other.gameObject);
        }
        // ��ֹ��� �浹
        else if (other.CompareTag("Obstacle"))
        {
            // ���� ���� ó��
            Debug.Log("��ֹ� �浹! ���� ����!");
            GameManager.Instance?.GameOver(); // GameManager�� ������ٸ�
        }
    }
}