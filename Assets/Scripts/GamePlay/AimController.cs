using UnityEngine;

public abstract class AimController : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private LayerMask obstacleMask;

    private Creature owner;

    protected float minDistance;


    public Transform Target
    {
        get => target;
        protected set => target = value;
    }

    protected virtual void Awake()
    {
        if (owner == null)
        {
            owner = transform.GetComponent<Creature>();
        }
    }


    public bool AssignTarget(Transform target)
    {
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        float distToTarget = Vector3.Distance(transform.position, target.position);
        if (minDistance != -1 && !(minDistance > distToTarget))
        {
            return false;
        }

        if (Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
        {
            return false;
        }

        minDistance = distToTarget;
        this.target = target;
        return true;
    }

    public bool IsTargetVisible()
    {
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        float dstToTarget = Vector3.Distance(transform.position, target.position);
        return !Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask);
    }

    public void ResetTarget()
    {
        target = null;
    }

    public void FollowTarget()
    {
        if (target == null)
        {
            return;
        }

        owner.transform.LookAt(target);
    }

    public virtual void HandleTarget()
    {
        if (Target == null)
        {
            Aim();
        }
        else if (!IsTargetVisible())
        {
            ResetTarget();
        }

        FollowTarget();
    }

    protected abstract bool Aim();
}