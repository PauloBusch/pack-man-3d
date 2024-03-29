using Assets.Constants;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Agent : WayPointBase
{
    public Image AvatarPrefab;
    public Player Player;
    public GameController GameController;
    public float MinDistanceAgent = 0.5f;
    public float MinDistancePlayer = 2f;
    public bool Chasing => _chasing;

    private float _speed;
    private bool _chasing;
    private NavMeshAgent _navMeshAgent;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed += _speed;
        GoToNext();
    }

    void Update()
    {
        if (Chasing || Vector3.Distance(transform.position, Player.transform.position) <= MinDistancePlayer)
            GoToPlayer();
        else if (Vector3.Distance(transform.position, _navMeshAgent.destination) <= MinDistanceAgent)
            GoToNext();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Chasing) return;

        if (collision.gameObject.CompareTag(Tags.Agent))
            GoToNext();
    }

    private void GoToNext()
    {
        var position = GetRandom().transform.position;
        _navMeshAgent.SetDestination(position);
    }

    private void GoToPlayer()
    {
        if (!Chasing) GameController.BeginChasePlayer(this);

        _navMeshAgent.SetDestination(Player.transform.position);
        _chasing = true;
    }

    public void StopChasing()
    {
        _chasing = false;
        GoToNext();
    }

    public void IncrementSpeed(float speed)
    {
        if (_navMeshAgent == null) _speed = speed;
        else _navMeshAgent.speed += speed;
    }
}
