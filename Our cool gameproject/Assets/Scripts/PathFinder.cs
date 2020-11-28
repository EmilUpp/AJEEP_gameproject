using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    /*
     * 
     */
    public static List<Vector2> GeneratePathList(GameObject origin, GameObject target, float baseSafeDistance, bool debugDraw)
    {
        List<Vector2> pathList = new List<Vector2>();
        pathList.Add(origin.transform.position);

        // Set currentOrigin to origin
        Vector2 currentOrigin = origin.transform.position;

        // Set current offset
        float currentOffset = 0;
        float step = Mathf.PI / 60;

        // Fire ray against target
        RaycastHit2D hitObjectInPath = FindObjectInPath(currentOrigin, target, 0, baseSafeDistance, debugDraw);
        RaycastHit2D lastHit = hitObjectInPath;

        float timeAccumulator = 0;

        // While object in path
        while (hitObjectInPath.collider != null && hitObjectInPath.collider.gameObject != target && timeAccumulator < 100)
        {
            // To stop potential endless loop, needs fix
            timeAccumulator += Time.deltaTime;

            // TODO fix step and offset
            // Fire new ray offset by a step in radians
            RaycastHit2D newHit = FindObjectInPath(currentOrigin, target, currentOffset, baseSafeDistance, debugDraw);

            // If if object still in path
            if (newHit.collider != null && newHit.collider.gameObject == lastHit.collider.gameObject)
            {
                // Save hit as lastHit (for use later)
                lastHit = newHit;

                currentOffset += step;
            }

            // If no or other object is in path
            else
            {
                // Add the normal multiplied by safedistance to point
                Vector2 newPoint = lastHit.point + lastHit.normal * baseSafeDistance;

                // Add offset point to PathList
                if (!pathList.Contains(newPoint))
                {
                    pathList.Add(newPoint);
                }

                // Set point to current origin
                currentOrigin = newPoint;

                currentOffset = 0;
            }

        }

        if (timeAccumulator > 25)
        {
            Debug.Log(timeAccumulator);
        }

        pathList.Add(target.transform.position);

        return pathList;
    }


    /*
     * Fires a ray from origin at target returns any object in the way
     * The ray is "rotated" by and angle in radians given by rotation
     */
    public static RaycastHit2D FindObjectInPath(Vector3 originPosition, GameObject target, float rotation, float baseSafeDistance, bool debugDraw)
    {

        float maxDistance = 10000;

        // Calculate direction
        Vector3 direction = target.transform.position - originPosition;

        // Rotate the direction vector
        direction = RotateVector(direction, rotation);

        // Cast ray
        RaycastHit2D hit = Physics2D.Raycast(originPosition, direction, maxDistance);

        // If it's an object in the way
        if (hit.collider != null
            && hit.collider.gameObject != target.gameObject)
        {
            if (debugDraw)
            {
                Debug.DrawRay(originPosition, direction.normalized * hit.distance, Color.red);

                float safeDistance = Mathf.Min(baseSafeDistance, hit.distance);

                // Shoots ray perpendicular to surface hit using the hit's normal
                Debug.DrawRay(originPosition + direction.normalized * hit.distance, hit.normal.normalized * safeDistance, Color.blue);
            }
        }
        // If it's the target
        else if (hit.collider != null
                && hit.collider.gameObject == target)
        {
            if (debugDraw)
            {
              Debug.DrawRay(originPosition, direction.normalized * Vector2.Distance(originPosition, target.transform.position), Color.cyan);
            }
        }
        else
        {
            if (debugDraw)
            {
                Debug.DrawRay(originPosition, direction.normalized * maxDistance, Color.yellow);
            }
        }

        return hit;
    }



    /*
     * Rotates a vector by matrix miltiplication
     */
    public static Vector3 RotateVector(Vector3 vectorToRotate, float rotation)
    {
        Vector3 rotatedVector = new Vector3(vectorToRotate.x * Mathf.Cos(rotation) - vectorToRotate.y * Mathf.Sin(rotation),
                                            vectorToRotate.x * Mathf.Sin(rotation) + vectorToRotate.y * Mathf.Cos(rotation),
                                            0);

        return rotatedVector;
    }

    public static float setNewOffset(float currentOffset, float step)
    {
        if (currentOffset > 0)
        {
            return -currentOffset;
        }

        return currentOffset * -1 + step;
    }
}
