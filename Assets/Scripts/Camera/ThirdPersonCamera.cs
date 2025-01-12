using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private float rotXSpeed = 90f;
    [SerializeField] private float rotYSpeed = 90f;
    [SerializeField] private Transform followTarget;

    private float angleX = 0;
    private float angleY = 0;

    private bool isFollowing = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (isFollowing && followTarget != null)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            angleX += mouseX * Time.deltaTime * rotXSpeed;
            angleY += mouseY * Time.deltaTime * rotYSpeed;
            angleY = Mathf.Clamp(angleY, -85f, 85f);
            transform.position = followTarget.position;
            transform.rotation = Quaternion.Euler(-angleY, angleX, 0);
        }
    }

    public void StopFollowing()
    {
        isFollowing = false;
    }
}
