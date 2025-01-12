using BehaviourTree;
using System.Collections;
using UnityEngine;

public class ThrowSmokeBombTask : Node
{
    private Transform _transform;
    private float _throwRadius = 500f; // Radius within which to affect the guard
    private bool _isThrown = false;

    private GameObject _invisSmokeWall;
    private GameObject _smokeObject;

    private MonoBehaviour _monoBehaviour;

    public ThrowSmokeBombTask(Transform transform, GameObject gameobject, MonoBehaviour monoBehaviour, GameObject smokeObject)
    {
        _transform = transform;

        _invisSmokeWall = gameobject;

        _monoBehaviour = monoBehaviour;

        _smokeObject = smokeObject;
    }

    public override NodeStatus Evaluate()
    {
        if (!_isThrown)
        {
            // Logic to throw a smoke bomb at the guard
            Debug.Log("Throwing smoke bomb!");

            // Find nearby guards within the radius
            Collider[] guards = Physics.OverlapSphere(_transform.position, _throwRadius, LayerMask.GetMask("Guard"));

            Debug.Log($"Found {guards.Length} guards within radius.");

            foreach (var guardCollider in guards)
            {
                if (guardCollider.TryGetComponent<Guard>(out var guard))
                {

                    _monoBehaviour.StartCoroutine(InvisWallTimer());

                    GameObject smokeInstance = GameObject.Instantiate(_smokeObject, guard.transform.position, Quaternion.identity);

                    // Destroy the smoke effect after 2 seconds
                    GameObject.Destroy(smokeInstance, 4f);

                    // Log to verify the effect
                    Debug.Log($"Guard {guard.name} is confused for 5 seconds.");
                }
            }

            _isThrown = true; // Mark that the smoke bomb has been thrown
            state = NodeStatus.RUNNING;  // Ensure the task remains in running state until the bomb is thrown
        }
        else
        {
            // Once the smoke bomb is thrown, return to following the player
            _isThrown= false;
            state = NodeStatus.SUCCES;  // This signals that the task has finished successfully
        }

        return state;
    }

    // For visualization, this will show the smoke bomb throw radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_transform.position, _throwRadius);
    }

    IEnumerator InvisWallTimer()
    {
        _invisSmokeWall.SetActive(true);

        yield return new WaitForSeconds(5);

        _invisSmokeWall.SetActive(false);
    }
}
