using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rito.ut23;

public class PlayerController : MonoBehaviour
{
    private IMovementInput _moveInputPC;
    private IMovementInput _moveInputMobile;
    private IRotationInput _rotateInput;

    [Header("Options")]
    [Range(0f, 20f)]
    [SerializeField] private float _moveSpeed = 5f;
    [Range(0f, 100f)]
    [SerializeField] private float _rotateSpeed = 15f;

    [Header("Components")]
    [SerializeField] private Transform _rigY;
    [SerializeField] private Transform _camRigX;

    private Rigidbody _rb;

    private void Awake()
    {
        _moveInputPC     = UserInputPC.Instance;
        _moveInputMobile = UserInputMobile.Instance;
        //_rotateInput     = UserInputPC.Instance;
        _rotateInput     = UserInputMobile.Instance;
    }

    private void Start()
    {
        TryGetComponent(out _rb);
    }

    private void Update()
    {
#if !UNITY_EDITOR
        if (_moveInputMobile.Movement2D.sqrMagnitude < 0.01f)
#endif
            Rotate();
    }

    private void Rotate()
    {
        Vector2 mDiffDelta = _rotateInput.Rotation2D;

        float yRot = mDiffDelta.x * _rotateSpeed * Time.deltaTime;
        _rigY.Rotate(Vector3.up * yRot, Space.Self);

        float xRot = -mDiffDelta.y * _rotateSpeed * Time.deltaTime;
        Quaternion nextRotX = _camRigX.localRotation * Quaternion.Euler(xRot, 0f, 0f);

        float xAngle = nextRotX.eulerAngles.x;
        if (xAngle > 180f) xAngle -= 360f;
        if (-25f < xAngle && xAngle < 25f)
        {
            _camRigX.localRotation = nextRotX;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer(_moveInputPC);
        MovePlayer(_moveInputMobile);
    }

    private void MovePlayer(IMovementInput moveInput)
    {
        Vector3 move = _rigY.localToWorldMatrix * moveInput.Movement3D;
        Vector3 nextMoveDelta = move * _moveSpeed * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + nextMoveDelta);
    }
}
