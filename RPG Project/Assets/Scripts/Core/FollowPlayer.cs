using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Core
{
    public class FollowPlayer : MonoBehaviour
    {
        [SerializeField]
        Transform target;

        [SerializeField]
        Transform camTransform;
        public float distance;
        Vector3 distVec;

        [SerializeField]
        float minDist = 5f;
        [SerializeField]
        float maxDist = 25f;

        RPGProject playerInput;
        InputAction look;
        InputAction zoom;

        public float camRotSpeed = 0.15f;

        private void Awake()
        {
            playerInput = new RPGProject();
            playerInput.Player.Enable();
            look = playerInput.Player.Look;
            zoom = playerInput.Player.Zoom;
        }

        private void OnEnable()
        {

            look.started += OnLook;
            look.performed += OnLook;
            look.canceled += OnLook;

            zoom.performed += OnZoom;
        }

        private void Start()
        {
            camTransform = transform.GetChild(0);
            distVec = camTransform.position - transform.position;
            distance = Vector3.Distance(camTransform.position, target.position);

        }

        private void LateUpdate()
        {
            transform.position = target.position;
            camTransform.localPosition = distVec.normalized * distance;
        }

        private void OnDisable()
        {

            look.started -= OnLook;
            look.performed -= OnLook;
            look.canceled -= OnLook;

            zoom.performed -= OnZoom;

            playerInput.Player.Disable();
        }

        void OnZoom(InputAction.CallbackContext context)
        {
            distance -= context.ReadValue<float>() * 0.01f;
            if (distance > maxDist)
                distance = maxDist;

            if (distance < minDist)
                distance = minDist;
        }


        float beforeValue;
        float afterValue;
        void OnLook(InputAction.CallbackContext context)
        {
            beforeValue = context.ReadValue<float>();

            if (context.performed)
            {
                if (Mathf.Abs(beforeValue - afterValue) > 0.001f)
                    transform.localRotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y + (beforeValue - afterValue) * camRotSpeed, 0);
            }

            afterValue = context.ReadValue<float>();
        }
    }

}