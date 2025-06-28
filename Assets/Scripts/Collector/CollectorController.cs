using UnityEngine;

public class CollectorController : MonoBehaviour
{
    public float maxDistanceFromPlayer = 5f; // 플레이어로부터 최대 거리
    private GameObject player; // 플레이어 오브젝트
    private LineRenderer lineRenderer; // 마우스 방향 선을 그릴 LineRenderer

    void Start()
    {
        // 플레이어 오브젝트를 태그로 찾아서, 이름 할당
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player 오브젝트가 'Player' 태그로 존재하지 않습니다!");
            enabled = false; // 플레이어를 찾지 못하면 스크립트 비활성화
            return;
        }

        // LineRenderer 컴포넌트 확인(없으면 오브젝트에 추가 필요)
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("이 오브젝트에 LineRenderer 컴포넌트를 추가해주세요!");
            enabled = false;
        }

        // LineRenderer 설정 (시작점, 끝점 정보를 인덱스에 맞게 설정)
        lineRenderer.positionCount = 2; // 선분의 점은 2개
    }

    void Update()
    {
        if (player == null || lineRenderer == null) return;

        // 1. 마우스 위치 월드 좌표로 변환
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 2. 플레이어 위치
        Vector2 playerPos = player.transform.position;

        // 3. 플레이어에서 마우스 방향 벡터 계산
        Vector2 direction = (mouseWorldPos - playerPos).normalized;

        // 4. 수집기 위치 갱신
        // 플레이어로부터 마우스 방향으로 최대 거리까지만 이동
        // 이 구간을 조절해서 마우스가 너무 멀리 있어도 일정 속도로만 이동하게 할 수 있습니다.
        // 여기서는 마우스 위치가 플레이어로부터 최대 거리를 넘으면 그 거리까지만 이동합니다.
        // Mathf.Min 은 두 값 중 작은 값을 반환합니다.
        float distance = Vector2.Distance(playerPos, mouseWorldPos);
        Vector2 targetCollectorPos = playerPos + direction * Mathf.Min(distance, maxDistanceFromPlayer);
        transform.position = targetCollectorPos;

        // 5. LineRenderer 갱신 (플레이어에서 수집기까지 선을 그림)
        // 또는 플레이어에서 마우스까지 선을 그리고 싶으면 아래 주석을 해제하세요. (기획에 따라 선택)
        lineRenderer.SetPosition(0, playerPos);
        lineRenderer.SetPosition(1, transform.position); // 수집기 위치
        // lineRenderer.SetPosition(1, mouseWorldPos); // 마우스 위치까지 선을 그림
    }

  void OnTriggerEnter2D(Collider2D other)
{
    // 수집 오브젝트와 충돌
    if (other.CompareTag("Collectible"))
    {
        Debug.Log("오브젝트 획득!");
        // GameManager가 없을 때 발생하는 오류이므로, 일단 이 코드를 주석 처리합니다.
        // GameManager.Instance?.AddScore(1);
        Destroy(other.gameObject);
    }
    // 장애물과 충돌
    else if (other.CompareTag("Obstacle"))
    {
        Debug.Log("장애물 충돌! 게임 오버!");
        // GameManager가 없을 때 발생하는 오류이므로, 일단 이 코드를 주석 처리합니다.
        // GameManager.Instance?.GameOver();
    }
}
}