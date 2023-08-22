using Entities;
using Managers;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour, IEntityNotify {

    public Animator anim;
    public Rigidbody rb;
    private AnimatorStateInfo currentBaseState;

    public Entity entity;
    public UnityEngine.Vector3 position;
    public UnityEngine.Vector3 direction;
    Quaternion rotation;

    public UnityEngine.Vector3 lastPosition;

    Quaternion lastRotation;

    public float speed;
    public float animSpeed = 1.5f;
    public float jumpPower = 3.0f;

    public bool isPlayer = false;

    public RideController rideController;

    private int currentRide = 0;
    public Transform rideBone;

    void Start()
    {
        if (entity != null)
        {
            EntityManager.Instance.RegisterEntityChangeNotify(entity.entityId, this);
            this.UpdateTransform();
        }
        if (!this.isPlayer)
        {
            rb.useGravity = false;
        }
    }

    void UpdateTransform()
    {
        this.position = GameObjectTool.LogicToWorld(entity.position);
        this.direction = GameObjectTool.LogicToWorld(entity.direction);

        this.rb.MovePosition(this.position);
        this.transform.forward = this.direction;
        this.lastPosition = this.position;
        this.lastRotation = this.rotation;
    }
    void OnDestroy()
    {
        if (entity != null)
        {
            Debug.LogFormat("{0} OnDestroy :ID:{1} POS:{2} DIR:{3} SPD:{4}", this.name, entity.entityId, entity.position, entity.direction, entity.speed);
        }
        if(UIWorldElementManager.Instance != null)
        {
            UIWorldElementManager.Instance.RemoveCharacterNameBar(this.gameObject.GetComponent<UICharacterHead>().head.transform);
        }
    }


    void FixedUpdate()
    {
        if (this.entity == null)
        {
            return;
        }
        this.entity.OnUpdate(Time.fixedDeltaTime);

        if (!this.isPlayer)
        {
            this.UpdateTransform();
        }
    }


    public void OnEntityRemoved()
    {
        if (UIWorldElementManager.Instance != null)
        {
            UIWorldElementManager.Instance.RemoveCharacterNameBar(this.transform);
        }
        Destroy(this.gameObject);
    }

    public void OnEntityChanged(Entity entity)
    {
        Debug.LogFormat("OnEntityChanged :ID:{0} POS:{1} DIR{2} SPD:{3} ", entity.entityId, entity.position, entity.direction, entity.speed);
    }

    public void OnEntityEvent(EntityEvent entityEvent, int param)
    {
        switch (entityEvent)
        {
            case EntityEvent.Idle:
                {
                    anim.SetBool("Move", false);
                    anim.SetBool("Move_Forward", false);
                    anim.SetBool("Move_Back", false);
                    anim.SetTrigger("Idle");
                    break;
                }
            case EntityEvent.MoveFwd:
                {
                    anim.SetBool("Move", true);
                    anim.SetBool("Move_Forward", true);
                    break;
                }
            case EntityEvent.MoveBack:
                {
                    anim.SetBool("Move", true);
                    anim.SetBool("Move_Back", true);
                    break;
                }
            case EntityEvent.Jump:
                {
                    anim.SetTrigger("Jump");
                    break;
                }
            case EntityEvent.Ride:
                {
                    //param在此分支为坐骑id，若为0，则取消坐骑
                    this.Ride(param);
                    break;
                }
        }
        if (this.rideController != null) this.rideController.OnEntityEvent(entityEvent);
    }

    public void Ride(int rideId)
    {
        if (currentRide == rideId) return;
        
        if (rideId > 0)
        {
            if (currentRide != 0)
            {
                Destroy(this.rideController.gameObject);
            }
            this.rideController = GameObjectManager.Instance.LoadRide(rideId, this.transform);
        }
        else
        {
            Destroy(this.rideController.gameObject);
            this.rideController = null;
        }

        if (this.rideController == null)
        {
            this.anim.transform.localPosition = Vector3.zero;
            this.anim.SetLayerWeight(1, 0);
        }
        else
        {
            this.rideController.SetRider(this);
            this.anim.SetLayerWeight(1, 1);
        }
        currentRide = rideId;
    }

    public void SetRidePosition(Vector3 position)
    {
        this.anim.transform.position = position + (this.anim.transform.position - this.rideBone.position);
    }
}
