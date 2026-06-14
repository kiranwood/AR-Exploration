using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PhysicsPlaneManager : MonoBehaviour
{
    [SerializeField] private ARPlaneManager _planeManager;
    [SerializeField] private float _minCalibratedSize; // The combined size of planes needed in order to start game
    [SerializeField] private GameObject _calibratingPanel;
    [SerializeField] private TextMeshProUGUI _percentageTextUI;

    private bool _isCalibrating;
    private float _currentCalibratedSize;
    private float _calibratedPercentage => _currentCalibratedSize / _minCalibratedSize;

    private void OnEnable()
    {
        _isCalibrating = true;
        _currentCalibratedSize = 0;
    }

    private void Update()
    {
        float sumPlaneSize = 0;
        foreach (ARPlane plane in _planeManager.trackables)
        {
            EnsureCollider(plane);

            // Only takes size of horizonal planes
            if (plane.alignment == UnityEngine.XR.ARSubsystems.PlaneAlignment.HorizontalUp)
            {
                sumPlaneSize += GetPlaneSize(plane);
            }
        }

        _currentCalibratedSize = sumPlaneSize;
        if (_isCalibrating == true)
        {
            UpdatePercentageText();
        }

        // Starts game
        if (_calibratedPercentage >= 1
            && GameManager.Instance.IsPlaying == false)
        {
            GameManager.Instance.StartGame();
            _calibratingPanel.SetActive(false);
            _isCalibrating = false;
        }

        // No longer has the right size for playing
        else if (   _calibratedPercentage < 0
                    && GameManager.Instance.IsPlaying == true)
        {
            GameManager.Instance.PauseGame();
            _calibratingPanel.SetActive(true);
            _isCalibrating = true;
        }
    }

    // Makes sure each AR plane has a mesh collider and filter
    private void EnsureCollider(ARPlane plane)
    {
        MeshCollider collider = plane.GetComponent<MeshCollider>();

        // Adds collider if doesn't exist
        if (collider == null)
        {
            collider = plane.gameObject.AddComponent<MeshCollider>();
        }

        MeshFilter meshFilter = plane.GetComponent<MeshFilter>();
        // Makes sure collider and filter has the same shared mesh
        if (meshFilter != null && meshFilter.sharedMesh != null && collider.sharedMesh != meshFilter.sharedMesh)
        {
            collider.sharedMesh = meshFilter.sharedMesh;
        }
    }

    // Gets the size of the planes
    private float GetPlaneSize(ARPlane plane)
    {
        return plane.size.x * plane.size.y;
    }

    // Updates the text of percentages
    private void UpdatePercentageText()
    {
        _percentageTextUI.text = Mathf.FloorToInt(_calibratedPercentage * 100).ToString("00") + "% Complete";
    }
}
