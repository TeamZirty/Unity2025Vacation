using UnityEngine;

public class CollectorController : MonoBehaviour
{
    public float maxDistanceFromPlayer = 5f; // 플레이어로부터 최대 거리
    private GameObject player; // 플레이어 오브젝트
    private LineRenderer lineRenderer; // 마우스까지 선을 그릴 LineRenderer

    void Start()
    {
        // 플레이어 오브젝트를 태그로 찾거나, 미리 할당
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player 오브젝트에 'Player' 태그를 설정해주세요!");
            enabled = false; // 플레이어를 찾지 못하면 스크립트 비활성화
            return;
        }

        // LineRenderer 컴포넌트 가져오기 (수집기 오브젝트에 추가 필요)
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("수집기 오브젝트에 LineRenderer 컴포넌트를 추가해주세요!");
            enabled = false;
        }

        // LineRenderer 설정 (원하는 색상, 두께 등으로 인스펙터에서 조정 가능)
        lineRenderer.positionCount = 2; // 시작점과 끝점 2개
    }

    void Update()
    {
        if (player == null || lineRenderer == null) return;

        // 1. 마우스 월드 좌표 가져오기
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 2. 플레이어 위치
        Vector2 playerPos = player.transform.position;

        // 3. 플레이어에서 마우스까지의 방향 벡터
        Vector2 direction = (mouseWorldPos - playerPos).normalized;

        // 4. 수집기 위치 업데이트
        // 플레이어로부터 마우스 방향으로 최대 거리까지만 이동
        // 이 부분을 조절하여 마우스가 멀어지면 수집기가 따라가는 속도 등을 조절할 수 있습니다.
        // 여기서는 마우스 위치가 플레이어로부터 일정 거리를 넘어가면 그 거리까지만 컬렉터를 배치합니다.
        float distance = Vector2.Distance(playerPos, mouseWorldPos);
        Vector2 targetCollectorPos = playerPos + direction * Mathf.Min(distance, maxDistanceFromPlayer);
        transform.position = targetCollectorPos;

        // 5. LineRenderer 업데이트 (플레이어에서 수집기까지 선 그리기)
        // 또는 플레이어에서 마우스까지 선을 그릴 수도 있습니다. (기획에 따라 선택)
        lineRenderer.SetPosition(0, playerPos);
        lineRenderer.SetPosition(1, transform.position); // 수집기 위치
        // lineRenderer.SetPosition(1, mouseWorldPos); // 마우스 위치까지 그릴 경우
    }

    // 충돌 처리 (Collider2D와 Rigidbody2D 필요)
    void OnTriggerEnter2D(Collider2D other)
    {
        // 수집 오브젝트와 충돌
        if (other.CompareTag("Collectible"))
        {
            // 점수 획득, 오브젝트 파괴 등 처리
            Debug.Log("오브젝트 획득!");
            GameManager.Instance?.AddScore(1); // GameManager를 만들었다면
            Destroy(other.gameObject);
        }
        // 장애물과 충돌
        else if (other.CompareTag("Obstacle"))
        {
            // 게임 오버 처리
            Debug.Log("장애물 충돌! 게임 오버!");
            GameManager.Instance?.GameOver(); // GameManager를 만들었다면
        }
    }
}