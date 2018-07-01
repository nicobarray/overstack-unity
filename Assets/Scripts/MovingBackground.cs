using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackground : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject movingBackgroundPrefab;

    [Header("Parameter")]
    public float step = 1;

    private int blocCount = 0;
    private PubsubHandler OnNewBlocOnTower;

    void OnEnable()
    {
        OnNewBlocOnTower = PubsubHandler.HandleNewBlocOnTower((int count, GameObject bloc) =>
        {
            blocCount = count;
            transform.Translate(Vector3.down * step);

            if (blocCount % 10 == 0)
            {
                GameObject movingBackground = Instantiate(movingBackgroundPrefab);

                movingBackground.transform.position = new Vector3(
                    transform.position.x,
                    5,
                    transform.position.z
                );
            }
        });
    }

    void OnDisable()
    {
        OnNewBlocOnTower.Dispose();
    }

    void Start()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform child = transform.GetChild(i);
            child.rotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler((i * 45) % 180, (90 - i * 45) % 180, 0), Random.value);
            child.position = Vector3.Lerp(child.position + Vector3.left, child.position + Vector3.right, Random.value);
        }
    }
}
