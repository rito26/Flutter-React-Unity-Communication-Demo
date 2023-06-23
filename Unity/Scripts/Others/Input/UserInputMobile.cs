using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Input = InputWrapper.Input;

namespace Rito.ut23
{
    public class UserInputMobile : SingletonMonoBehavior<UserInputMobile>, IMovementInput, IRotationInput
    {
        private const float ROT_POWER = 1000f;

        public MobileJoystickController joystick;

        Vector2 IMovementInput.Movement2D => joystick.Coord2D;
        Vector3 IMovementInput.Movement3D => joystick.Coord3D;
        Vector2 IRotationInput.Rotation2D => _dragDiffDelta;

        [Header("Show Only - Screen Drag")]
        [SerializeField] private Vector2 _dragBeginPos;
        [SerializeField] private Vector2 _dragCurPos;
        [SerializeField] private Vector2 _dragDiff;
        [SerializeField] private Vector2 _dragDiffBefore;
        [SerializeField] private Vector2 _dragDiffDelta;
        [SerializeField] private Vector2 _touchPos;
        [SerializeField] private bool[] _beginTouchOnUIs = new bool[10];

        private void Update()
        {
            HandleScreenTouchDrag();
        }

        // 뷰포트 내 마우스 드래그 감지
        private void HandleScreenTouchDrag()
        {
            _dragDiffDelta = Vector2.zero;

            int touchCount = Input.touchCount;
            //if (touchCount > 2) return;

            for (int i = 0; i < touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                _touchPos = new Vector2(touch.position.x / Screen.width, touch.position.y / Screen.height);

                bool isTouchBegin = touch.phase == TouchPhase.Began;
                if (isTouchBegin)
                {
                    _beginTouchOnUIs[i] = (_touchPos.x < 0.2f && _touchPos.y < 0.4f);
                }

                // UI 터치 || 조이스틱 터치 -> 터치 무시
                if (IsTouchOverUI(touch)) continue;
                if (_beginTouchOnUIs[i])  continue;

                CalculateDelta(touch);
                //_dragDiffDelta = touch.deltaPosition;
            }
        }

        private void CalculateDelta(in Touch touch)
        {
            if (touch.phase == TouchPhase.Began)
            {
                _dragBeginPos = GetTouchPos(touch);
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                _dragCurPos = GetTouchPos(touch);
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
            if (PlatformChecker.IsMobilePlatform())
                _dragDiffDelta = -_dragDiffDelta;
        }

        private static bool IsTouchOverUI(in Touch touch)
        {
            int id = touch.fingerId;
            return EventSystem.current.IsPointerOverGameObject(id);
        }

        private static Vector2 GetTouchPos(in Touch touch)
        {
            return new Vector2(touch.position.x / Screen.width, touch.position.y / Screen.height);
        }
    }
}