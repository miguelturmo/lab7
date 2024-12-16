using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
public class AgenteML : Agent
{
    [SerializeField]
    private float _fuerzaMovimieto = 200;
    [SerializeField]
    private Transform _target;
    public bool _training = true;
    private Rigidbody _rb;
    public override void Initialize()
    {
        _rb = GetComponent<Rigidbody>();
        //MaxStep forma parte de la clase Agent
        if (!_training) MaxStep = 0;
    }
    public override void OnEpisodeBegin()
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        MoverPosicionInicial();
    }
   
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        var continuousActions = actionBuffers.ContinuousActions;
        Vector3 movimiento = new Vector3(continuousActions[0], 0f, continuousActions[1]);

        _rb.AddForce(movimiento * _fuerzaMovimieto * Time.deltaTime);
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        //Calcular cuanto nos queda hasta el objetivo
        Vector3 alObjetivo = _target.position - transform.position;
        //Un vector ocupa 3 observaciones. 
        sensor.AddObservation(alObjetivo.normalized);
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continousActions = actionsOut.ContinuousActions;
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
        continousActions[0] = movement.x;
        continousActions[1] = movement.z;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_training)
        {
            if (other.CompareTag("premio"))
            {
                AddReward(1f);
            }
            if (other.CompareTag("borders"))
            {
                AddReward(-0.1f);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (_training)
        {
            if (other.CompareTag("premio"))
            {
                AddReward(0.5f);
            }
            if (other.CompareTag("borders"))
            {
                AddReward(-0.05f);
            }
        }
    }
    private void MoverPosicionInicial()
    {
        bool posicionEncontrada = false;
        int intentos = 100;
        Vector3 posicionPotencial = Vector3.zero;
        while (!posicionEncontrada || intentos >= 0)
        {
            intentos--;
            posicionPotencial = new Vector3(
                transform.parent.position.x + UnityEngine.Random.Range(-3f, 3f),
                1,
                transform.parent.position.z + UnityEngine.Random.Range(-3f, 3f));
            //en el caso de que tengamos mas cosas en el escenario checker que no choca
            Collider[] colliders = Physics.OverlapSphere(posicionPotencial, 0.05f);
            if (colliders.Length == 0)
            {
                transform.position = posicionPotencial;
                posicionEncontrada = true;
            }
        }
    }
}