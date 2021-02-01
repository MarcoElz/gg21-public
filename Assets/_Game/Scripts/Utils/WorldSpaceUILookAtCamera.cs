using UnityEngine;

namespace Ignita.Utility.WorldUI
{
    public class WorldSpaceUILookAtCamera : MonoBehaviour
    {
        private Camera cachedCamera;

        private void Start()
        {
            cachedCamera = Camera.main;
        }

        private void LateUpdate()
        {
            LookAtCamera();
        }

        public void LookAtCamera()
        {
            transform.rotation = cachedCamera.transform.rotation;
        }
    }
}