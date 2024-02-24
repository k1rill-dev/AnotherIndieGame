using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MonsterAI : MonoBehaviour
{
    private NavMeshAgent _agent;
    [SerializeField] private float _sprintRatio = 1.25f;
    [SerializeField] private float _range;
    [SerializeField] private Transform _centrePoint;
    [SerializeField] private float _radius;
    [Range(0,360)] [SerializeField]private float _angle;

    [SerializeField]private GameObject _playerRef;

    [SerializeField]private LayerMask _targetMask;
    [SerializeField]private LayerMask _obstructionMask;

    [SerializeField]private bool _canSeePlayer;

    private static event Action _seePlayer;
    private static event Action _death; 
    public float Radius
    {
        get => _radius;
        set => _radius = value;
    }

    public float Angle
    {
        get => _angle;
        set => _angle = value;
    }

    public GameObject PlayerRef
    {
        get => _playerRef;
        set => _playerRef = value;
    }

    public bool CanSeePlayer
    {
        get => _canSeePlayer;
        set => _canSeePlayer = value;
    }

    void Start()
    {
        _seePlayer += OnSeePlayer;
        _death += OnDeath;
        _agent = GetComponent<NavMeshAgent>();
    }

    
    void Update()
    {
        if (!_canSeePlayer)
        {
            StartCoroutine(IdleRoutine());
            StartCoroutine(FOVRoutine());
            
        }
        else
        {
            StopCoroutine(IdleRoutine());
            StopCoroutine(FOVRoutine());
            _seePlayer?.Invoke();
        }
    }

    private void OnSeePlayer()
    {
        float distance = Vector3.Distance(transform.position ,_playerRef.transform.position);
        if (distance > _agent.stoppingDistance)
        {
            _agent.speed *= _sprintRatio;
            _agent.SetDestination(_playerRef.transform.position);
        }
        else
        {
            _death?.Invoke();
        }
    }

    private void OnDeath()
    {
        Debug.Log("ты умер лох");
    }
    
    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) 
        { 
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private IEnumerator IdleRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            _agent.speed = 3.5f;
            if(_agent.remainingDistance <= _agent.stoppingDistance) 
            {
                Vector3 point;
                if (RandomPoint(_centrePoint.position, _range, out point)) 
                {
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); 
                    _agent.SetDestination(point);
                }
            }
        }
    }
    private IEnumerator FOVRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, _radius, _targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < _angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstructionMask))
                    _canSeePlayer = true;
                else
                    _canSeePlayer = false;
            }
            else
                _canSeePlayer = false;
        }
        else if (_canSeePlayer)
            _canSeePlayer = false;
    }

}
