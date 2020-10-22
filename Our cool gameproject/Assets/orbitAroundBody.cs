using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class orbitAroundBody : MonoBehaviour
{
    public Transform bodyToOrbit;
    public float speed;
    public float speedFactor = 1;

    public bool setDistance;
    public float desiredDistance;
    
    // Start is called before the first frame update
    public void Start()
    {

        if (transform.parent != null)
        {
            bodyToOrbit = transform.parent;
        }

        desiredDistance = distanceToTarget();
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(bodyToOrbit.position, Vector3.forward, speedFactor * speed * Time.deltaTime / 100);

        // I think this minimizes flickering
        if (Mathf.Abs(distanceToTarget() - desiredDistance) > 0.1)
        {
            // Calculates the angle between the bodies
            Vector2 vectorDifference = bodyToOrbit.position - transform.position;
            float angle = Mathf.Atan2(vectorDifference.y, vectorDifference.x);

            // Adds the distance to the orbited body, offset by angle
            transform.position = new Vector2(bodyToOrbit.position.x + desiredDistance * -Mathf.Cos(angle), 
                                            bodyToOrbit.position.y + desiredDistance * -Mathf.Sin(angle));
        }
    }

    public float distanceToTarget()
    {
        return Vector2.Distance(bodyToOrbit.position, gameObject.transform.position);
    }

    public void OnValidate()
    {
        if (desiredDistance < 1)
        {
            desiredDistance = 1;
        }
    }
}


[ExecuteInEditMode]
[CustomEditor(typeof(orbitAroundBody))]
public class OrbitEditor : Editor
{
    // Custom editor to allow to dynamicly change which values are shown in the inspector
    public override void OnInspectorGUI()
    {
        orbitAroundBody orbitScript = target as orbitAroundBody;

        // Creates fields for all public variables
        orbitScript.bodyToOrbit = EditorGUILayout.ObjectField("Body to orbit", orbitScript.bodyToOrbit, typeof(Transform), true) as Transform;
        orbitScript.speed = EditorGUILayout.FloatField("Speed:", orbitScript.speed);
        orbitScript.speedFactor = EditorGUILayout.FloatField("Speed Factor:", orbitScript.speedFactor);
        orbitScript.setDistance = GUILayout.Toggle(orbitScript.setDistance, "Set distance");
        
        // Only shows the distance field if set distance is true
        if (orbitScript.setDistance)
        {
            orbitScript.desiredDistance = EditorGUILayout.FloatField("Distance:", orbitScript.desiredDistance);
        }

        // Needed to call onvalidate in custom inspector
        if (orbitScript != null) {
            orbitScript.OnValidate();
        }
    }
}

