using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Color start;
    public Color end;

    public int score = 0;
    public float bumpSpeed = 10;

    public float maxSize = 1f;

    public Text scoreLabel;

    private PubsubHandler OnNewBlocOnTower;

    void OnEnable()
    {
        OnNewBlocOnTower = PubsubHandler.HandleNewBlocOnTower((int count, GameObject _bloc) =>
        {
            score = count;
            scoreLabel.text = score.ToString();

            if (score > 1)
            {
                StartCoroutine(BumpText());

                if (score % 10 == 0)
                {
                    maxSize += 0.1f;
                }
            }
        });
    }

    void OnDisable()
    {
        OnNewBlocOnTower.Dispose();
    }

    void Start()
    {
        start = Color.Lerp(Color.green, Color.grey, Random.value);
        end = Color.Lerp(Color.blue, Color.grey, Random.value);
    }

    IEnumerator BumpText()
    {
        float t = 0;

        while (t < 1)
        {
            scoreLabel.transform.localScale = Vector3.Lerp(Vector3.one * maxSize, new Vector3(maxSize + .5f, maxSize + .5f, 0), t);
            t += Time.deltaTime * bumpSpeed;
            yield return null;
        }

        while (t > 0)
        {
            scoreLabel.transform.localScale = Vector3.Lerp(Vector3.one * maxSize, new Vector3(maxSize + .5f, maxSize + .5f, 0), t);
            t -= Time.deltaTime * bumpSpeed;
            yield return null;
        }

        scoreLabel.transform.localScale = Vector3.one * maxSize;
    }
}