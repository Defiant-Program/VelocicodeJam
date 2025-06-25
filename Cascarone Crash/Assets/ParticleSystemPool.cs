using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemPool : MonoBehaviour
{
    public ParticleSystem confettiSystem; // Reference to your central particle system

    public void EmitFromCascarone(Vector3 cascaronePositions, Vector3 cascaroneDirections)
    {
        Vector3 position = cascaronePositions;
        //Vector3 direction = GetComponent<Rigidbody>()?.velocity.normalized ?? transform.forward;

        var emitParams = new ParticleSystem.EmitParams
        {
            position = position,
            applyShapeToPosition = true

        };
        confettiSystem.transform.rotation = Quaternion.LookRotation(cascaroneDirections);

        confettiSystem.Emit(emitParams, 200); // Emit 20 particles at this location
    }
}
