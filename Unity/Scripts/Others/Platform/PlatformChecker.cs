using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rito.ut23
{
    public static class PlatformChecker
    {
#if !UNITY_EDITOR && UNITY_WEBGL
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern bool IsMobile();
#endif
        public static bool IsMobilePlatform()
        {
            var isMobile = false;

#if !UNITY_EDITOR && UNITY_WEBGL
            isMobile = IsMobile();
#endif
            return isMobile;
        }
    }
}
