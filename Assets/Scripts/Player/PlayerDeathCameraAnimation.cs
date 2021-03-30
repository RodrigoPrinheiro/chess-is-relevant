using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathCameraAnimation : MonoBehaviour
{
    private PlayerActor _player;
    private PlayerController _controller;
    private bool _popped;

    private void Awake() {
        _player = GetComponent<PlayerActor>();
        _controller = GetComponent<PlayerController>();
    } 

    private void OnEnable() {
        _player.DeathEvent += Animate;
    }

    private void OnDisable() {
        _player.DeathEvent -= Animate;
    }

    public void Animate(Actor actor)
    {
        if (_popped) return;
        _controller.MainCamera.SetParent(null, true);
        _controller.Disable = true;

        Transform camera = _controller.MainCamera;
        Rigidbody rb = camera.gameObject.AddComponent<Rigidbody>();
        SphereCollider col = camera.gameObject.AddComponent<SphereCollider>();
        
        col.radius = 0.5f;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        rb.angularDrag = 3f;
        rb.AddForce(camera.up * 4f + camera.right * 3f, ForceMode.VelocityChange);
        _popped = true;
    }

    private IEnumerator SlowDown(Rigidbody rb)
    {
        while(rb.angularVelocity.magnitude > 0)
        {
            rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, Time.deltaTime * 3f);
            yield return null;
        }
    }
    
}
