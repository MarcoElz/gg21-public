using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhotonSyncView : MonoBehaviourPun, IPunObservable
{
    [SerializeField] ParticleSystem stepsParticles = default;

    private Vector3 lastNetworkPosition;
    private Vector2 lastNetworkInput;

    private Rigidbody rb;
    private PlayerController controller;
    private PlayerMovement movement;
    private PlayerAnimation animationController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<PlayerController>();
        movement = GetComponent<PlayerMovement>();
        animationController = GetComponent<PlayerAnimation>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //Envia datos
            stream.SendNext(controller.MovementInput);
            stream.SendNext(rb.position);
        }
        else
        {
            //Lee datos
            lastNetworkInput = (Vector2)stream.ReceiveNext();
            lastNetworkPosition = (Vector3)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void RPC_SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    [PunRPC]
    public void RPC_SetTeam(Team team)
    {
        controller.SetTeam(team);
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            if (lastNetworkInput.x != 0 || lastNetworkInput.y != 0)
            {
                if (!stepsParticles.isPlaying)
                    stepsParticles.Play();
                rb.position = Vector3.MoveTowards(rb.position, lastNetworkPosition, Time.fixedDeltaTime * 2f);
                movement.Move(lastNetworkInput);
            }
            else
            {
                if (stepsParticles.isPlaying)
                    stepsParticles.Stop();
                rb.position = lastNetworkPosition;
            }

            animationController.MoveAnimation(lastNetworkInput);
        }
    }
}
