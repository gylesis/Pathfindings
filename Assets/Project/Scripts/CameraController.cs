using UnityEngine;

namespace Project
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _cameraParent;

        public Camera Camera => _camera;

        public void MoveTo(Vector3 pos)
        {
            _cameraParent.transform.position = pos;
        }

        public void SetHeight(float height)
        {
            Vector3 cameraParentPosition = _cameraParent.position;
            cameraParentPosition.y = height;

            _cameraParent.transform.position = cameraParentPosition;
        }
    }
}