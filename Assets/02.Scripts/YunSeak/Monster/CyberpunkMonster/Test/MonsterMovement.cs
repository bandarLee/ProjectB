using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class MonsterMovement : MonoBehaviour
{
    public Transform[] movePositions; // 이동할 위치들을 담을 배열

    private NavMeshAgent _navMeshAgent;
    private bool _isMoving; // 현재 이동 중인지 여부를 나타내는 변수

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        // 이동 시작
        StartCoroutine(MoveToRandomPosition());
    }

    private void Update()
    {
        // 이동 중이 아니라면
        if (!_isMoving)
        {
            // 새로운 위치로 이동하는 코루틴을 시작합니다.
            StartCoroutine(MoveToRandomPosition());
        }
    }

    // 무작위 위치로 이동하는 코루틴
    private IEnumerator MoveToRandomPosition()
    {
        _isMoving = true; // 이동 시작

        // 무작위 위치를 선택합니다.
        Vector3 randomPosition = movePositions[Random.Range(0, movePositions.Length)].position;

        // 내비메시 에이전트를 이 위치로 이동시킵니다.
        _navMeshAgent.SetDestination(randomPosition);

        // 내비메시 에이전트가 이동을 완료할 때까지 대기합니다.
        while (_navMeshAgent.pathPending || _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance)
        {
            yield return null;
        }

        _isMoving = false; // 이동 종료
    }
}