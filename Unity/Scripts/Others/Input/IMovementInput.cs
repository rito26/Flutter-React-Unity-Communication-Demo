using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rito.ut23
{
    public interface IMovementInput
    {
        public Vector2 Movement2D { get; }
        public Vector3 Movement3D { get; }
    }
}
