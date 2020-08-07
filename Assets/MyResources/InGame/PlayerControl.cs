using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplay
{
    public class PlayerControl : MonoBehaviour
    {
        [Header("Move Values")]
        public CharacterController characterController;
        public Transform camTr;
        public float speed = 10.0f;
        public float ySpeed = 100f;

        float yVelocity = 0.0f;
        public float gravity = -20.0f;
        public float jumpSpeed = 20.0f;

        float rotY = 0.0f;

        void Update()
        {
            Move();
            Fire();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.LoadLevel("lobby");
            }
        }

        void Move()
        {
            float y = Input.GetAxis("Mouse X");

            rotY += y * ySpeed * Time.deltaTime;

            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");

            Vector3 dir = new Vector3(x, 0.0f, z);
            dir = camTr.TransformDirection(dir);
            dir.y = 0.0f;
            dir.Normalize();

            if (characterController.isGrounded == true)
            {
                yVelocity = 0.0f;
            }

            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpSpeed;
            }

            yVelocity += gravity * Time.deltaTime;
            dir.y = yVelocity;

            characterController.Move(dir * speed * Time.deltaTime);

            transform.eulerAngles = new Vector3(0.0f, rotY, 0.0f);
        }

        public PhotonView _photonView;
        [Header("Fire Values")]
        public GameObject fireObj;
        public Transform fireTr;
        public float fireSpeed = 10.0f;
        void Fire()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                // 나는 내가 쏘고
                FireRPC(fireTr.position, fireTr.forward);

                // 다른 상대방들에게는 RPC로 쏜다 : RpcTarget.Others를 쓰는 이유
                if (PhotonNetwork.IsConnected)
                {                    
                    _photonView.RPC("FireRPC", RpcTarget.Others, fireTr.position, fireTr.forward);
                }
            }
        }

        [PunRPC]
        void FireRPC(Vector3 firePos, Vector3 fireDir)
        {
            if (gameObject.activeSelf == false)
                return;

            GameObject obj = Instantiate(fireObj) as GameObject;
            obj.transform.position = fireTr.position;

            Rigidbody rb = obj.GetComponent<Rigidbody>();
            rb.velocity = fireTr.forward * fireSpeed;

            // 발사한 사람을 기록한다.
            SpikeBall sb = obj.GetComponent<SpikeBall>();
            sb.actorNumber = _photonView.Owner.ActorNumber;            
        }               
    }

}