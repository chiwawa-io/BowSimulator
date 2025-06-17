using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulateTrajectory : MonoBehaviour
{
    private Scene _simulatedScene;
    private PhysicsScene _physicsScene;
    [SerializeField] private Transform _trajectoryParent;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private GameObject _arrowPrefab;

    private void Start()
    {
        _simulatedScene = SceneManager.CreateScene("SimulatedTrajectoryScene", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        _physicsScene = _simulatedScene.GetPhysicsScene();
        
        if (_lineRenderer == null)
            Debug.LogError("LineRenderer is not assigned in the inspector.");
        if (_arrowPrefab == null)
            Debug.LogError("Arrow prefab is not assigned in the inspector.");
    }

    public void SimulateTr(Vector3 direction, Vector3 origin)
    {
        var simulatedArrow = Instantiate(_arrowPrefab, origin, Quaternion.identity, _trajectoryParent);
        var arrowScript = simulatedArrow.GetComponent<Arrow>();

        arrowScript.Shoot(direction, 20f); 

        Vector3[] positions = new Vector3[20];
        for (int i = 0; i < positions.Length; i++)
        {
            float time = i * 0.1f; // Adjust time step as needed
            Vector3 position = origin + direction * time + Physics.gravity * time * time / 2f;
            positions[i] = position;
        }
        _lineRenderer.positionCount = positions.Length;
        _lineRenderer.SetPositions(positions);

        Destroy(simulatedArrow, 3f); 
    }
}

