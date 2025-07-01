using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimulateTrajectory : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private int _maxPoints = 20;
    [SerializeField] private float _timeStep = 0.1f;
    [SerializeField] private LayerMask _collisionLayerMask;

    private void Start()
    {
        if (_lineRenderer == null) 
        {
            _lineRenderer = GetComponent<LineRenderer>();
            Debug.LogError("LineRenderer is not assigned in the inspector.");
        }
    }

    public void SimulateTr(Vector3 direction, Vector3 origin)
    {
        List<Vector3> points = new List<Vector3>();
        var currentPosition = origin;

        points.Add(currentPosition);

        for (int i = 0; i < _maxPoints; i++)
        {
            _timeStep = i * 0.1f; 
            var position = origin + direction * _timeStep + Physics.gravity * _timeStep * _timeStep / 2f;
            points.Add(position);

            var directionToNext = position - currentPosition;
            var distanceToNext = directionToNext.magnitude;

            if (Physics.Raycast(currentPosition, directionToNext.normalized, out RaycastHit hit, distanceToNext, _collisionLayerMask))
            {
                points[i] = hit.point;
                break;
            }
        }

        _lineRenderer.positionCount = points.Count;
        _lineRenderer.SetPositions(points.ToArray());
    }
}

