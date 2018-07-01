using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Tower : MonoBehaviour
{
    public GameObject blocPrefab;

    public List<InteractiveBloc> tblocs = new List<InteractiveBloc>();

    public float speed = 0;
    public float bounceDepth = 0.5f;

    private PubsubHandler OnGameOver;
    private PubsubHandler OnBlocTouchTower;

    void OnEnable()
    {
        OnGameOver = PubsubHandler.HandleGameOver((ignore) =>
        {
            Destroy(gameObject);
        });

        OnBlocTouchTower = PubsubHandler.HandleBlocTouchTower((bloc) =>
        {
            AddBloc(bloc);
            StartCoroutine(BounceOff(this.speed));
        });
    }

    void OnDisable()
    {
        OnGameOver.Dispose();
        OnBlocTouchTower.Dispose();
    }

    void Start()
    {
        InstantiateNewBloc();
        AddBloc(transform.GetComponentInChildren<InteractiveBloc>());
    }

    void AddBloc(InteractiveBloc bloc)
    {
        tblocs.Add(bloc);
        bloc.transform.SetParent(transform);
        bloc.tag = "Tower";
        bloc.GetComponent<Rigidbody>().Sleep();
        // bloc.GetComponent<Rigidbody>().mass = 1000;

        Pubsub.Publish(Pubsub.Type.NewBlocOnTower, new object[] { tblocs.Count, bloc.gameObject });
    }

    void InstantiateNewBloc()
    {
        GameObject bloc = Instantiate(blocPrefab);
        Pubsub.Publish(Pubsub.Type.NewInteractiveBloc, bloc);
    }

    IEnumerator BounceOff(float speed)
    {
        Vector3 initial = transform.position;

        float t = 0;

        while (t < 1)
        {
            transform.position = Vector3.Lerp(initial, initial + Vector3.down * 0.1f, t);
            t += Time.deltaTime * speed;
            yield return null;
        }

        transform.position = initial + Vector3.down * 0.1f;

        InstantiateNewBloc();
    }
}