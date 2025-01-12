using BehaviourTree;
using System.Collections;
using UnityEngine;

public class ThrowSmokeBombTask : Node
{
    private Transform _ninjaTransform;
    private float _throwRadius = 500f; //Radius within which to affect the guard, extra high so to make sure it detects the guard
    private bool _isThrown = false;

    private GameObject _invisSmokeWall;
    private GameObject _smokeObject;

    private MonoBehaviour _monoBehaviour;

    public ThrowSmokeBombTask(Transform transform, GameObject gameobject, MonoBehaviour monoBehaviour, GameObject smokeObject)
    {
        _ninjaTransform = transform;

        _invisSmokeWall = gameobject;

        _monoBehaviour = monoBehaviour;

        _smokeObject = smokeObject;
    }

    public override NodeStatus Evaluate()
    {
        if (!_isThrown)
        {
            //Debug.Log("Throwing smoke bomb!");

            Collider[] guards = Physics.OverlapSphere(_ninjaTransform.position, _throwRadius, LayerMask.GetMask("Guard"));

            //Debug.Log($"Found {guards.Length} guards within radius.");

            foreach (var guardCollider in guards)
            {
                if (guardCollider.TryGetComponent<Guard>(out var guard))
                {

                    _monoBehaviour.StartCoroutine(InvisWallTimer());

                    GameObject smokeInstance = GameObject.Instantiate(_smokeObject, guard.transform.position, Quaternion.identity);

                    GameObject.Destroy(smokeInstance, 4f);

                    //Log to verify the effect
                    //Debug.Log($"Guard {guard.name} is confused for 5 seconds.");
                }
            }

            _isThrown = true;
            state = NodeStatus.RUNNING;
        }
        else
        {
            _isThrown= false;
            state = NodeStatus.SUCCES;
        }

        return state;
    }

    IEnumerator InvisWallTimer()
    {
        _invisSmokeWall.SetActive(true);

        yield return new WaitForSeconds(5);

        _invisSmokeWall.SetActive(false);
    }
}
