using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B4D.UI.Utils
{
    public class IconRotation : MonoBehaviour
    {
        [SerializeField] float speed = -360f;

        private void Update()
        {
            transform.Rotate(Vector3.forward * (speed * Time.deltaTime));
        }
    }
}