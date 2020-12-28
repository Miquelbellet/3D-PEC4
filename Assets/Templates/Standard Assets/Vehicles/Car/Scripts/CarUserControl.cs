using System;
using UnityEngine;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        public GameObject playerDoor;
        public bool drivingCar;

        private GameObject playerParent;
        private CarController m_Car; // the car controller we want to use
        private CarAudio m_CarAudio;
        private GameObject cameraContainer;


        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
            m_CarAudio = GetComponent<CarAudio>();
            cameraContainer = GameObject.FindGameObjectWithTag("ContainerCamera");
            playerParent = GameObject.FindGameObjectWithTag("PlayerParent");
        }


        private void FixedUpdate()
        {
            if (drivingCar)
            {
                // pass the input to the car!
                float h = CrossPlatformInputManager.GetAxis("Horizontal");
                float v = CrossPlatformInputManager.GetAxis("Vertical");
#if !MOBILE_INPUT
                float handbrake = CrossPlatformInputManager.GetAxis("Jump");
                m_Car.Move(h, v, v, handbrake);
#else
            m_Car.Move(h, v, v, 0f);
#endif
            }

            if (Input.GetKeyDown(KeyCode.E) && drivingCar)
            {
                try {
                    var player = playerDoor.transform.GetChild(0);
                    player.gameObject.SetActive(true);
                    player.transform.SetParent(playerParent.transform);
                    m_CarAudio.enabled = false;
                    cameraContainer.GetComponent<FreeLookCam>().SetTarget(player.transform);
                }
                catch{}
                Invoke("NotDriving", 0.5f);
            }
        }

        private void NotDriving()
        {
            drivingCar = false;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Player")
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (!drivingCar)
                    {
                        drivingCar = true;
                        other.gameObject.SetActive(false);
                        other.transform.SetParent(playerDoor.transform);
                        m_CarAudio.enabled = true;
                        cameraContainer.GetComponent<FreeLookCam>().SetTarget(transform);
                        try{ GetComponent<GhostCarScript>().enabled = false; }
                        catch{}
                    }
                }
            }
        }
    }
}
