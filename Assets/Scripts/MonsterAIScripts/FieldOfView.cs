using System;
using System.Collections;
using UnityEngine;

namespace MonsterAIScripts
{
    public class FieldOfView: MonoBehaviour
    {
        [SerializeField] private float _radius;
        
        [Range(0,360)] [SerializeField]private float _angle;

        [SerializeField]private GameObject _playerRef;

        [SerializeField]private LayerMask _targetMask;
        [SerializeField]private LayerMask _obstructionMask;

        private bool _canSeePlayer;
        
        public float Radius
        {
            get => _radius;
        }

        public float Angle
        {
            get => _angle;
        }

        public GameObject PlayerRef
        {
            get => _playerRef;
        }

        public bool CanSeePlayer
        {
            get => _canSeePlayer;
        }
        

        private void Update()
        {
            StartCoroutine(FOVRoutine());
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
}