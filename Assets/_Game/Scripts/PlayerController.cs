using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask mouseRaycastMask = default;

    public Vector2 MovementInput { get; private set; }
    public bool IsStun { get; private set; }
    public Team Team { get; private set; }

    public event Action onStunned;
    public event Action onStunSaved;
    public event Action<float> onStunProgress;

    private PlayerMovement movement;
    private PlayerAnimation animationController;

    private float stunStartTime;
    private float stunTime;

    private Camera cachedCamera;

    private Transform fov;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        animationController = GetComponent<PlayerAnimation>();
    }

    private void Start()
    {
        cachedCamera = Camera.main;
    }

    private void Update()
    {
        ProcessMouseRaycast();

        if (IsStun)
        {
            ProcessStunTime();
            return;
        }

        ProccessMovementInput();
    }

    public void SetFov(Transform fov)
    {
        this.fov = fov;
    }

    public void SetTeam(Team team)
    {
        this.Team = team;
    }

    private void ProccessMovementInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        MovementInput = new Vector2(h, v).normalized;
    }

    private void ProcessStunTime()
    {
        stunTime -= Time.deltaTime;
        onStunProgress.Invoke(stunTime/stunStartTime);
        if(stunTime < 0f)
        {
            IsStun = false;
            GetComponent<PhotonView>().RPC("RPC_Save", RpcTarget.All);
        }
    }

    private void ProcessMouseRaycast()
    {
        if (fov == null) return;

        Ray ray = cachedCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, mouseRaycastMask))
        {
            Vector3 hitPoint = hit.point;

            hitPoint.y = fov.position.y;
            fov.LookAt(hitPoint);
        }
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
    }

    private void FixedUpdate()
    {
        movement.Move(MovementInput);
        animationController.MoveAnimation(MovementInput);
    }

    public void Stun(float duration)
    {
        if (Time.timeSinceLevelLoad < 5f) return;

        if (IsStun) return;

        stunTime = duration;
        stunStartTime = duration;
        IsStun = true;
        MovementInput = Vector2.zero;

        onStunned.Invoke();
    }

    public void SaveStun()
    {
        IsStun = false;
        stunTime = 0f;

        onStunSaved.Invoke();
    }

}
