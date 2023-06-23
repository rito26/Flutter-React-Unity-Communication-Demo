using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rito.ut23
{
    public class UserInputPC : SingletonMonoBehavior<UserInputPC>, IMovementInput, IRotationInput
    {
        // 모바일 touch delta와 비율 보정
        private const float ROT_POWER = 1000f;

        Vector2 IMovementInput.Movement2D => _move2d;
        Vector3 IMovementInput.Movement3D => _move3d;
        Vector2 IRotationInput.Rotation2D => _dragDiffDelta;

        [Header("Options")]
        [SerializeField] private bool _useWASD = true;
        [SerializeField] private bool _useCursorMove = true;
        [SerializeField] private bool _useScreenDrag = true;

        [Header("Show Only - WASD")]
        [SerializeField] private float _v;
        [SerializeField] private float _h;
        [SerializeField] private Vector3 _move2d;
        [SerializeField] private Vector3 _move3d;

        [Header("Show Only - Screen Drag")]
        [SerializeField] private Vector2 _dragBeginPos;
        [SerializeField] private Vector2 _dragCurPos;
        [SerializeField] private Vector2 _dragDiff;
        [SerializeField] private Vector2 _dragDiffBefore;
        [SerializeField] private Vector2 _dragDiffDelta;

        [Header("Show Only - Screen Cursor Move")]
        [SerializeField] private Vector2 _curPosPrev;
        [SerializeField] private Vector2 _curPosNow;
        [SerializeField] private Vector2 _curPosDelta;

        private void Update()
        {
            if(_useWASD) HandleWASD();
            if(_useCursorMove) HadleCursorMove();
            if(_useScreenDrag) HandleScreenDrag();
        }

        // WASD, 방향키 입력
        private void HandleWASD()
        {
            _v = Input.GetAxisRaw("Vertical");
            _h = Input.GetAxisRaw("Horizontal");
            _move2d = new Vector2(_h, _v).normalized;
            _move3d = new Vector3(_h, 0f, _v).normalized;
        }

        // 뷰포트 내 커서 이동 감지
        private void HadleCursorMove()
        {
            _curPosNow = GetCursorPos();
            _curPosDelta = _curPosNow - _curPosPrev;
            _curPosPrev  = _curPosNow;
        }

        // 뷰포트 내 마우스 드래그 감지
        private void HandleScreenDrag()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _dragBeginPos = GetCursorPos();
            }
            if (Input.GetMouseButton(0))
            {
                _dragCurPos = GetCursorPos();
                _dragDiff = _dragCurPos - _dragBeginPos;

                _dragDiffDelta = _dragDiff - _dragDiffBefore;
                _dragDiffBefore = _dragDiff;
            }
            else
            {
                _dragDiff = Vector2.zero;
                _dragDiffBefore = Vector2.zero;
                _dragDiffDelta = Vector2.zero;
            }

            _dragDiffDelta *= ROT_POWER;
        }

        private static Vector2 GetCursorPos()
        {
            return new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
        }
    }
}
