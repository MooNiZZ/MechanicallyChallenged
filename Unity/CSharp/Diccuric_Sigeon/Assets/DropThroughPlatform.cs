using System.Collections.Generic;
using UnityEngine;

public class DropThroughPlatform : MonoBehaviour
{
    public Collider Collider;
    public Vector3 TriggerScale = new Vector3(1.05f, 1.05f, 1.05f);
    public LayerMask DropThroughLayer;
    public List<Collider> OverlappedColliders {get; set;}
    public List<Collider> IgnoredCollders {get; set;}
    private bool _isStarted;
    private Collider _trigger;
    private GameObject _triggerGO;

    public string DefaultLayer {get; set;}

    private void Start()
    {
        DefaultLayer = LayerMask.LayerToName(gameObject.layer);
        IgnoredCollders = new List<Collider>();
        OverlappedColliders = new List<Collider>();

        _triggerGO = new GameObject();
        _triggerGO.transform.SetParent(transform);
        _triggerGO.name = "DropThroughTrigger";
        _triggerGO.transform.localPosition = Vector3.zero;
        _triggerGO.transform.localScale = TriggerScale;

        var triggerHandler = _triggerGO.AddComponent<DropThroughPlatformTrigger>();
        triggerHandler.DropThroughPlatform = this;
        _triggerGO.layer = gameObject.layer;

        UpdateTrigger();
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if(Collider == default)
        {
            Collider = GetComponent<Collider>();
        }
    }
    #endif

    public void StartDrop()
    {
        _isStarted = true;
    }

    public void StopDrop()
    {
        if(!_isStarted) return;

        _isStarted = false;
    }

    private void Update()
    {
        if(_isStarted)
        {
            Ignore();
        }
    }

    private void Ignore()
    {
        for(var i = 0; i < OverlappedColliders.Count; i++)
        {
            IgnoredCollders.Add(OverlappedColliders[i]);
            Physics.IgnoreCollision(Collider, OverlappedColliders[i], true);
        }

        OverlappedColliders.Clear();
    }

    public void UpdateTrigger()
    {
        if(_trigger != default)
        {
            Destroy(_trigger);
        }
        
        _trigger = Collider.Clone(_triggerGO);

        if(_trigger != default)
        {
            _trigger.isTrigger = true;
        }
    }
}

public static class ColliderUtils
{
    public static Collider Clone(this Collider collider, GameObject target)
    {
        Collider clone = default;

        switch (collider)
        {
            case SphereCollider c:
                clone = CreateSphere(c, target);
            break;
            case BoxCollider c:
                clone = CreateBox(c, target);
            break;
            case CapsuleCollider c:
                clone = CreateCapsule(c, target);
            break;
            case CharacterController c:
                clone = CreateCharacterController(c, target);
            break;
            case MeshCollider c:
                clone = CreateMesh(c, target);
            break;
            default:
                Debug.LogWarning($"Collider {collider.GetType()} not supported :(", collider.gameObject);
            break;
        }

        return clone;
    }


    private static Collider CreateSphere(SphereCollider c, GameObject target)
    {
        var clone = target.AddComponent<SphereCollider>();
        clone.radius = c.radius;
        clone.center = new Vector3(c.center.x / target.transform.localScale.x,
                                   c.center.y / target.transform.localScale.y,
                                   c.center.z / target.transform.localScale.z);

        return clone;
    }

    private static Collider CreateBox(BoxCollider c, GameObject target)
    {
        var clone = target.AddComponent<BoxCollider>();
        clone.size = c.size;
        clone.center = new Vector3(c.center.x / target.transform.localScale.x,
                                   c.center.y / target.transform.localScale.y,
                                   c.center.z / target.transform.localScale.z);

        return clone;
    }

    private static Collider CreateCapsule(CapsuleCollider c, GameObject target)
    {
        var clone = target.AddComponent<CapsuleCollider>();
        clone.radius = c.radius;
        clone.direction = c.direction;
        clone.height = c.height;
        clone.center = new Vector3(c.center.x / target.transform.localScale.x,
                                   c.center.y / target.transform.localScale.y,
                                   c.center.z / target.transform.localScale.z);

        return clone;
    }

    private static Collider CreateCharacterController(CharacterController c, GameObject target)
    {
        var clone = target.AddComponent<CapsuleCollider>();
        clone.radius = c.radius + c.skinWidth;
        clone.height = c.height + c.skinWidth;
        clone.center = new Vector3(c.center.x / target.transform.localScale.x,
                                   c.center.y / target.transform.localScale.y,
                                   c.center.z / target.transform.localScale.z);

        return clone;
    }

    private static Collider CreateMesh(MeshCollider c, GameObject target)
    {   
        var clone = target.AddComponent<MeshCollider>();
        clone.sharedMesh = c.sharedMesh;
        clone.convex = c.convex;

        return clone;
    }
}
