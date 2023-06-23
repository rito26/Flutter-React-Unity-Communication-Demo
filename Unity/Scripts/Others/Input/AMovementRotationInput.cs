using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rito.ut23
{
    public abstract class AMovementInput : MonoBehaviour, IMovementInput
    {
        public Vector2 Movement2D { get; }
        public Vector3 Movement3D { get; }
    }

    public abstract class ARotationInput : MonoBehaviour, IRotationInput
    {
        public Vector2 Rotation2D { get; }
    }

    // 다이아몬드 상속
    public abstract class AMovementRotationInput : MonoBehaviour, IMovementInput, IRotationInput
    {
        public Vector2 Movement2D { get; }
        public Vector3 Movement3D { get; }
        public Vector2 Rotation2D { get; }
    }
}
