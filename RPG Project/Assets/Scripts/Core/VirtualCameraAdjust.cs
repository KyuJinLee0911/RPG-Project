using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class VirtualCameraAdjust : MonoBehaviour
{
        CinemachineVirtualCamera vc;
        public float distance;

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
            vc = GetComponent<CinemachineVirtualCamera>();
            
        }

        private void LateUpdate()
        {
            // transform.position = target.position;
            // camTransform.localPosition = distVec.normalized * distance;
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
            CinemachineFramingTransposer framingTransposer = vc.GetCinemachineComponent<CinemachineFramingTransposer>();
            framingTransposer.m_CameraDistance -= context.ReadValue<float>() * 0.01f;
            if (framingTransposer.m_CameraDistance > maxDist)
                framingTransposer.m_CameraDistance = maxDist;

            if (framingTransposer.m_CameraDistance < minDist)
                framingTransposer.m_CameraDistance = minDist;
        }


        float beforeValue;
        float afterValue;
        void OnLook(InputAction.CallbackContext context)
        {
            beforeValue = context.ReadValue<float>();

            if (context.performed)
            {
                if (Mathf.Abs(beforeValue - afterValue) > 0.001f)
                    transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y + (beforeValue - afterValue) * camRotSpeed, transform.localRotation.eulerAngles.z);
            }

            afterValue = context.ReadValue<float>();
        }
}
