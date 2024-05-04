using UnityEngine;

public class DropThroughPlatformTrigger : MonoBehaviour
{
    public DropThroughPlatform DropThroughPlatform {get; set;}

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.transform == DropThroughPlatform.Collider.transform)
        {
            return;
        }

        if(DropThroughPlatform.OverlappedColliders.Contains(collider) || collider.isTrigger)
        {
            return;
        }
        
        if((DropThroughPlatform.DropThroughLayer & (1 << collider.gameObject.layer)) != 0)
        {
            DropThroughPlatform.OverlappedColliders.Add(collider);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        DropThroughPlatform.OverlappedColliders.Remove(collider);

        if(DropThroughPlatform.IgnoredCollders.Remove(collider))
        {
            Physics.IgnoreCollision(DropThroughPlatform.Collider, collider, false);
        }
    }
}
