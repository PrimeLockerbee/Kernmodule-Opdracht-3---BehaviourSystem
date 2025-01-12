using BehaviourTree;
using UnityEngine;

public class CheckEnemyInFOVRange : Node
{
    private static int _playerLayerMask = 1 << 6;
    private Transform _guardTransform;
    private Animator _animator;

    private float _fovAngle = 110f;
    private float _fovRange = 20f;

    [SerializeField] private GameObject _invisWall;

    public CheckEnemyInFOVRange(Transform transform, Animator animator, GameObject invisWall)
    {
        _guardTransform = transform;
        _animator = animator;
        _invisWall = invisWall;
    }

    public override NodeStatus Evaluate()
    {
        if (_invisWall.activeInHierarchy)
        {
            state = NodeStatus.FAILURE;
            return state;
        }

        object target = GetData("target");

        if (target == null)
        {
            Collider[] colliders = Physics.OverlapSphere(_guardTransform.position, _fovRange, _playerLayerMask);

            foreach (var collider in colliders)
            {
                Transform playerTransform = collider.transform;

                Vector3 directionToPlayer = (playerTransform.position - _guardTransform.position).normalized;
                float angleToPlayer = Vector3.Angle(_guardTransform.forward, directionToPlayer);

                //Debug.DrawRay(_transform.position + Vector3.up, directionToPlayer, Color.red);

                if (angleToPlayer < _fovAngle / 2f)
                {
                    Physics.queriesHitTriggers = true;

                    RaycastHit hit;
                    if (Physics.Raycast(_guardTransform.position + Vector3.up, directionToPlayer, out hit, _fovRange))
                    {
                        if (hit.transform == playerTransform)
                        {
                            _parent._parent.SetData("target", playerTransform);
                            _animator.SetBool("Walking", true);
                            state = NodeStatus.SUCCES;
                            return state;
                        }
                    }
                }
            }

            state = NodeStatus.FAILURE;
            return state;
        }

        state = NodeStatus.SUCCES;
        return state;
    }

    //private void DebugDrawFOV(Vector3 center, float range, float angle)
    //{
    //    float halfAngle = angle / 2f;
    //    Vector3 leftBoundary = Quaternion.Euler(0, -halfAngle, 0) * _transform.forward;
    //    Vector3 rightBoundary = Quaternion.Euler(0, halfAngle, 0) * _transform.forward;

    //    Debug.DrawRay(center, leftBoundary * range, Color.green);
    //    Debug.DrawRay(center, rightBoundary * range, Color.green);

    //    Debug.DrawRay(center, _transform.forward * range, Color.red);
    //}
}
