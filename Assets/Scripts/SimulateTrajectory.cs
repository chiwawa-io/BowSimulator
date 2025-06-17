using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SimulateTrajectory : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;

    private void Start()
    {
        if (_lineRenderer == null)
            Debug.LogError("LineRenderer is not assigned in the inspector.");
    }

    public void SimulateTr(Vector3 direction, Vector3 origin)
    {
        _lineRenderer.positionCount = 20;

        for (int i = 0; i < 20; i++)
        {
            float time = i * 0.1f; // Adjust time step as needed
            Vector3 position = origin + direction * time + Physics.gravity * time * time / 2f;
            _lineRenderer.SetPosition(i, position);
        }
    }
}

