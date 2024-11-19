using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
public class MoveToBallAgent : Agent
{

    [SerializeField] private Transform targetTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;
    

    public override void OnEpisodeBegin() {
        transform.localPosition = new Vector3(Random.Range(-2f, +2f),1f,Random.Range(-2f, +2f));
        targetTransform.localPosition = new Vector3(Random.Range(0f, +8f),0.5f, -2f);
    }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);

    }
    public override void OnActionReceived(ActionBuffers actions) {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        float moveSpeed = 2f;
        //transform.GetComponent<Rigidbody>().MovelocalPosition(transform.localPosition +  new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed);
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        
        var continuousActions = actionsOut.ContinuousActions;

        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Trigger");
        if (other.TryGetComponent<Goal>(out Goal goal)) {
            Debug.Log("Goal Hit");
            SetReward(1f);
            EndEpisode();
        }
        if (other.TryGetComponent<Wall>(out Wall wall)) {
            Debug.Log("Wall Hit");
            SetReward(-1f);
            EndEpisode();
        }
        
    }

    private void OnCollisionEnter(Collision other) {
        if (other.collider.tag == "Goal") {
            Debug.Log("Goal CHit");
            SetReward(1f);
            floorMeshRenderer.material = winMaterial;
            EndEpisode();
        }
        if (other.collider.tag == "Wall") {
            Debug.Log("Wall CHit");
            SetReward(-1f);
            floorMeshRenderer.material = loseMaterial;
            EndEpisode();
        }
        
    }

}
