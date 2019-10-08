using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollisionRetrievers
{
    /// <summary>
    /// Gets all objects a <see cref="Collider"/> will hit when moving in a certain direction
    /// </summary>
    /// <param name="colliderObject">The object that the <see cref="Collider"/> is attached to, as a <see cref="Transform"/></param>
    /// <param name="collider">The <see cref="Collider"/> that will be checked</param>
    /// <param name="direction">The direction that <see cref="Collider"/> will move in. 
    /// Can easily be gotten by using <see cref="Vector3.normalized"/> with the difference between the target and starting locations</param>
    /// <param name="distance">The distance the <see cref="Collider"/> will move. 
    /// Can easily be gotten by using <see cref="Vector3.magnitude"/> with the difference between the target and starting locations</param>
    /// <returns></returns>
    public static RaycastHit[] GetCollisions(Transform colliderObject, Collider collider, Vector3 direction, float distance)
    {
        if (collider is CapsuleCollider)
        {
            return GetCapsuleCollisions(colliderObject, collider as CapsuleCollider, direction, distance);
        }

        return new RaycastHit[0];
    }

    /// <summary>
    /// Gets all objects a <see cref="CapsuleCollider"/> will hit when moving in a certain direction
    /// </summary>
    /// <param name="colliderObject">The object that the <see cref="CapsuleCollider"/> is attached to, as a <see cref="Transform"/></param>
    /// <param name="collider">The <see cref="CapsuleCollider"/> that will be checked</param>
    /// <param name="direction">The direction that <see cref="CapsuleCollider"/> will move in. 
    /// Can easily be gotten by using <see cref="Vector3.normalized"/> with the difference between the target and starting locations</param>
    /// <param name="distance">The distance the <see cref="CapsuleCollider"/> will move. 
    /// Can easily be gotten by using <see cref="Vector3.magnitude"/> with the difference between the target and starting locations</param>
    /// <returns></returns>
    public static RaycastHit[] GetCapsuleCollisions(Transform colliderObject, CapsuleCollider collider, Vector3 direction, float distance)
    {
        RaycastHit[] hits;

        Vector3 pointDistance = Vector3.up * (collider.height / 2 - collider.radius);
        Vector3 colliderTruePosition = colliderObject.position + collider.center;

        Vector3 point1 = colliderTruePosition + pointDistance;
        Vector3 point2 = colliderTruePosition - pointDistance;

        hits = Physics.CapsuleCastAll(point1, point2, collider.radius, direction, distance);

        return hits;
    }
}
