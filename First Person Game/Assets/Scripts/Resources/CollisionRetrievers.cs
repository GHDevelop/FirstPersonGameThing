using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A set of shortcuts for Unity's ColliderCastAll and OverlapCollider methods, as well as a method to remove triggers from a list of colliders
/// </summary>
public static class CollisionRetrievers
{
    /// <summary>
    /// Gets all objects a <see cref="Collider"/> will hit when moving in a certain direction. Currently only implemented for <see cref="CapsuleCollider"/>
    /// </summary>
    /// <param name="colliderObject">The object that the <see cref="CapsuleCollider"/> is attached to, as a <see cref="Vector3"/> position</param>
    /// <param name="collider">The <see cref="Collider"/> that will be checked</param>
    /// <param name="direction">The direction that <see cref="Collider"/> will move in. 
    /// Can easily be gotten by using <see cref="Vector3.normalized"/> with the difference between the target and starting locations</param>
    /// <param name="distance">The distance the <see cref="Collider"/> will move. 
    /// Can easily be gotten by using <see cref="Vector3.magnitude"/> with the difference between the target and starting locations</param>
    /// <returns></returns>
    public static RaycastHit[] GetCollisions(Vector3 colliderObject, Collider collider, Vector3 direction, float distance)
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
    /// <param name="colliderObject">The object that the <see cref="CapsuleCollider"/> is attached to, as a <see cref="Vector3"/> position</param>
    /// <param name="collider">The <see cref="CapsuleCollider"/> that will be checked</param>
    /// <param name="direction">The direction that <see cref="CapsuleCollider"/> will move in. 
    /// Can easily be gotten by using <see cref="Vector3.normalized"/> with the difference between the target and starting locations</param>
    /// <param name="distance">The distance the <see cref="CapsuleCollider"/> will move. 
    /// Can easily be gotten by using <see cref="Vector3.magnitude"/> with the difference between the target and starting locations</param>
    /// <returns></returns>
    public static RaycastHit[] GetCapsuleCollisions(Vector3 colliderObject, CapsuleCollider collider, Vector3 direction, float distance)
    {
        Vector3 pointDistance = Vector3.up * (collider.height / 2 - collider.radius);
        Vector3 colliderTruePosition = colliderObject + collider.center;

        Vector3 point1 = colliderTruePosition + pointDistance;
        Vector3 point2 = colliderTruePosition - pointDistance;

        RaycastHit[] hits = Physics.CapsuleCastAll(point1, point2, collider.radius, direction, distance);

        return hits;
    }


    /// <summary>
    /// Gets all objects a <see cref="Collider"/> will hit at a certain position. Currently only implemented for <see cref="CapsuleCollider"/><br/>
    /// If you already know the type of collider you are using, you should use the specific methods instead
    /// </summary>
    /// <param name="colliderObject">The object that the <see cref="CapsuleCollider"/> is attached to, as a <see cref="Vector3"/> position</param>
    /// <param name="collider">The <see cref="Collider"/> that will be checked</param>
    /// <seealso cref="GetBoxOverlap(Vector3, BoxCollider)"/>
    /// <seealso cref="GetCapsuleOverlap(Vector3, CapsuleCollider)"/>
    /// <seealso cref="GetSphereOverlap(Vector3, SphereCollider)"/>
    public static Collider[] GetColliderOverlap(Vector3 colliderObject, Collider collider)
    {
        if (collider is CapsuleCollider)
        {
            return GetCapsuleOverlap(colliderObject, collider as CapsuleCollider);
        }
        else if (collider is BoxCollider)
        {
            return GetBoxOverlap(colliderObject, collider as BoxCollider);
        }
        else if (collider is SphereCollider)
        {
            return GetSphereOverlap(colliderObject, collider as SphereCollider);
        }

        return new Collider[0];
    }

    /// <summary>
    /// Gets all objects a <see cref="CapsuleCollider"/> will hit at a certain position
    /// </summary>
    /// <param name="colliderObject">The object that the <see cref="CapsuleCollider"/> is attached to, as a <see cref="Vector3"/> position</param>
    /// <param name="collider">The <see cref="CapsuleCollider"/> that will be checked</param>
    public static Collider[] GetCapsuleOverlap(Vector3 colliderObject, CapsuleCollider collider)
    {
        Vector3 pointDistance = Vector3.up * (collider.height / 2 - collider.radius);
        Vector3 colliderTruePosition = colliderObject + collider.center;

        Vector3 point1 = colliderTruePosition + pointDistance;
        Vector3 point2 = colliderTruePosition - pointDistance;

        Collider[] hits = Physics.OverlapCapsule(point1, point2, collider.radius);

        return hits;
    }

    /// <summary>
    /// Gets all objects a <see cref="BoxCollider"/> will hit at a certain position
    /// </summary>
    /// <param name="colliderObject">The object that the <see cref="BoxCollider"/> is attached to, as a <see cref="Vector3"/> position</param>
    /// <param name="collider">The <see cref="BoxCollider"/> that will be checked</param>
    public static Collider[] GetBoxOverlap(Vector3 colliderObject, BoxCollider collider)
    {
        Collider[] hits = Physics.OverlapBox(colliderObject + collider.center, collider.size / 2);

        return hits;
    }

    /// <summary>
    /// Gets all objects a <see cref="SphereCollider"/> will hit at a certain position
    /// </summary>
    /// <param name="colliderObject">The object that the <see cref="SphereCollider"/> is attached to, as a <see cref="Vector3"/> position</param>
    /// <param name="collider">The <see cref="SphereCollider"/> that will be checked</param>
    public static Collider[] GetSphereOverlap(Vector3 colliderObject, SphereCollider sphere)
    {
        Collider[] hits = Physics.OverlapSphere(colliderObject + sphere.center, sphere.radius);

        return hits;
    }

    /// <summary>
    /// Removes triggers from a list of <see cref="Collider"/>s
    /// </summary>
    /// <param name="colliders"></param>
    /// <returns></returns>
    public static Collider[] RemoveTriggers(Collider[] colliders)
    {
        List<Collider> collidersOnly = new List<Collider>(colliders.Length);

        for (int index = 0; index < colliders.Length; index++)
        {
            if (colliders[index].isTrigger == false)
            {
                collidersOnly.Add(colliders[index]);
            }
        }

        return collidersOnly.ToArray();
    }
}
