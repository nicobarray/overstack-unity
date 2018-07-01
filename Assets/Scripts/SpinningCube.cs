using UnityEngine;
using System.Collections;

public class SpinningCube : MonoBehaviour
{
    public float m_Speed = 20f;

    private Vector3 m_RotationDirection = Vector3.up;

    public void ToggleRotationDirection()
    {
        if (m_RotationDirection == Vector3.up)
        {
            m_RotationDirection = Vector3.down;
        }
        else
        {
            m_RotationDirection = Vector3.up;
        }
    }

    void Update()
    {
        transform.Rotate(m_RotationDirection * Time.deltaTime * m_Speed);
    }
}
