using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveBloc : MonoBehaviour
{
    public static int count = 0;

    public enum State
    {
        Alternate,
        Fall,
        Dead
    }

    [Header("Public Field")]
    public float fallingSpeed = 10;
    public float alternateSpeed = 1;
    public new Rigidbody rigidbody;
    public GameObject ligthspot;


    [Header("Private Field")]
    public State state;
    public Vector3 initialPosition;

    [Serializable]
    public struct AlternateStateProps
    {
        public float t;
        public bool forward;
    };

    public AlternateStateProps alternateState;

    void Start()
    {
        initialPosition = transform.position;

        if (state == State.Dead)
        {
            return;
        }

        state = State.Alternate;
        alternateState = new AlternateStateProps
        {
            t = 0,
            forward = true
        };

        Score scoreSystem = FindObjectOfType<Score>();
        float t = (count % 20) / 20f;
        GetComponent<MeshRenderer>().material.color = Color.Lerp(scoreSystem.start, scoreSystem.end, t);

        count++;
    }

    void UdpateAlternateState()
    {
        if (alternateState.forward)
        {
            if (alternateState.t < 1)
            {
                alternateState.t += Time.deltaTime * alternateSpeed;
            }
            else
            {
                alternateState.forward = false;
                alternateState.t = 0.99f;
            }
        }
        else
        {
            if (alternateState.t > 0)
            {
                alternateState.t -= Time.deltaTime * alternateSpeed;
            }
            else
            {
                alternateState.t = 0.01f;
                alternateState.forward = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Dead)
        {
            transform.position = initialPosition;
            return;
        }

        if (state == State.Alternate)
        {
            Vector3 pos = Vector3.Lerp(initialPosition, initialPosition + Vector3.right * 2, alternateState.t);
            transform.position = pos;

            UdpateAlternateState();
        }
        else if (state == State.Fall)
        {
            transform.Translate(Vector3.down * Time.deltaTime * fallingSpeed);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (state != State.Dead && other.gameObject.CompareTag("Tower"))
        {
            state = State.Dead;
            initialPosition = transform.position;

            Destroy(ligthspot);

            Pubsub.Publish(Pubsub.Type.BlocTouchTower, this);
        }
    }

    public void TransitionToFall()
    {
        state = State.Fall;
    }
}
