using System;
using System.Collections;
using GameLogicScripts;
using PlayerControlScripts;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;


namespace MonsterAIScripts
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Animator))]
    public class MonsterMovement : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private Animator _animator;
        private AudioSource _audioSource;
        [SerializeField] private float _sprintRatio = 1.25f;
        [SerializeField] private float _range;
        [SerializeField] private Transform _centrePoint;
        private bool _isDead;
        private FieldOfView _fieldOfView;
        private IEnumerator _coroutine;
        [SerializeField] private GameObject _player;
        private PlayerController _playerController;
        private DamageScreen _screen;
        private static event Action SeePlayer;
        public UnityEvent dead;
        
        private void OnEnable()
        {
            SeePlayer += OnSeePlayer;
        }

        private void OnDisable()
        {
            SeePlayer -= OnSeePlayer;
        }

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
            _fieldOfView = GetComponent<FieldOfView>();
            _playerController = _player.GetComponent<PlayerController>();
            _screen = _player.GetComponent<DamageScreen>();
            _coroutine = IdleRoutine();
        }


        private void Update()
        {
            if (!_isDead)
            {
                StartCoroutine(_coroutine);
                if (_fieldOfView.CanSeePlayer)
                    SeePlayer?.Invoke();
            }
            else
            {
                _audioSource.Stop();
                StopCoroutine(_coroutine);
                Debug.Log("ты умер лох");
            }
        }

        private void OnSeePlayer()
        {
            float distance = Vector3.Distance(transform.position, _fieldOfView.PlayerRef.transform.position);
            if (distance > _agent.stoppingDistance)
            {
                _agent.speed *= _sprintRatio;
                _agent.SetDestination(_fieldOfView.PlayerRef.transform.position);
            }
            else
            {
                _isDead = true;
                _animator.SetBool("isIdle", false);
                _animator.SetBool("isWalking", false);
                _animator.SetTrigger("Attack");
                StartCoroutine(OnDamage());
            }
        }

        private IEnumerator OnDamage()
        {
            _screen.DurationTimer = 0;
            _screen.Overlay.color = new Color(_screen.Overlay.color.r, _screen.Overlay.color.g,
                _screen.Overlay.color.b, 1);
            yield return new WaitForSeconds(3);
            dead?.Invoke();
        }
        
        private bool RandomPoint(Vector3 center, float range, out Vector3 result)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            if (NavMesh.SamplePosition(randomPoint, out var hit, 1.0f, NavMesh.AllAreas))
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
                yield return new WaitForSeconds(1f);
                if (_agent.isStopped)
                {
                    _animator.SetBool("isIdle", true);
                    _audioSource.Stop();
                    _animator.SetBool("isWalking", false);
                }

                _agent.speed = 3.5f;
                if (_agent.remainingDistance <= _agent.stoppingDistance)
                {
                    Vector3 point;
                    if (RandomPoint(_centrePoint.position, _range, out point))
                    {
                        Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                        _animator.SetBool("isIdle", false);
                        _audioSource.Play();
                        _animator.SetBool("isWalking", true);
                        _agent.SetDestination(point);
                    }
                }
            }
        }
    }
}