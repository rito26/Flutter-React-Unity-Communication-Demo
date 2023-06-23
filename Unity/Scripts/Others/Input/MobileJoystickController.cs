using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rito.ut23
{
    public class MobileJoystickController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        /* 
            * [하이라키 구성]
            * 
            * - JoystickBG (원형 이미지)
            *     : MobileJoystickController 컴포넌트 위치
            *     : Pivot  (0.5, 0.5)
            *     : Anchor (0, 0, 0, 0)
            *     
            *   ㄴ JoystickKnob (작은 원형 이미지)
            *     : Pivot  (0.5, 0.5)
            *     : Anchor (0.5, 0.5, 0.5, 0.5)
            */
        [Header("Settings")]
        [SerializeField] private RectTransform joystickBg;
        [SerializeField] private RectTransform joystickKnob;

        [Header("Options")]
        [Range(0f, 1f)] public float offset = 1f; // 조이스틱 손잡이 이동 거리

        [Header("Output Values")]
        [SerializeField] private Vector2 _coordinate;
        public Vector2 Coord2D => _coordinate;
        public Vector3 Coord3D => new Vector3(_coordinate.x, 0f, _coordinate.y);

        // // 이미지의 가로세로 비율을 유지시키는 컴포넌트
        //private void OnEnable()
        //{
        //    if (!joystickBg.TryGetComponent<UnityEngine.UI.AspectRatioFitter>(out var fitter))
        //    {
        //        fitter = joystickBg.gameObject.AddComponent<UnityEngine.UI.AspectRatioFitter>();
        //        fitter.aspectMode = UnityEngine.UI.AspectRatioFitter.AspectMode.WidthControlsHeight;
        //    }
        //}

        public void OnBeginDrag(PointerEventData eventData)
        {

        }

        public void OnDrag(PointerEventData eventData)
        {
            _coordinate = new Vector2(
                (eventData.position.x - joystickBg.position.x) / ((joystickBg.rect.size.x - joystickKnob.rect.size.x) * 0.5f),
                (eventData.position.y - joystickBg.position.y) / ((joystickBg.rect.size.y - joystickKnob.rect.size.y) * 0.5f)
            );
            _coordinate = (_coordinate.magnitude > 1.0f) ? _coordinate.normalized : _coordinate;

            joystickKnob.transform.position = new Vector2(
                (_coordinate.x * ((joystickBg.rect.size.x - joystickKnob.rect.size.x) * 0.5f) * offset) + joystickBg.position.x,
                (_coordinate.y * ((joystickBg.rect.size.y - joystickKnob.rect.size.y) * 0.5f) * offset) + joystickBg.position.y
            );
        }
        public void OnPointerDown(PointerEventData eventData) => OnDrag(eventData);

        public void OnEndDrag(PointerEventData eventData)
        {
            _coordinate = new Vector2(0f, 0f);
            joystickKnob.transform.position = joystickBg.position;
        }
        public void OnPointerUp(PointerEventData eventData) => OnEndDrag(eventData);
    }
}