using UnityEngine;

public class UI : MonoBehaviour {
    public bool m_LookAtCamera = true;    // Whether the UI element should rotate to face the camera.
    public Transform m_UIElement;         // The transform of the UI to be affected.
    public Transform m_Camera;            // The transform of the camera.
    public bool m_RotateWithCamera;       // Whether the UI should rotate with the camera so it is always in front.
    public float m_FollowSpeed = 10f;     // The speed with which the UI should follow the camera.
    public float m_DistanceFromCamera;   // The distance the UI should stay from the camera when rotating with it.

    private void Start() {
        // Find the distance from the UI to the camera so the UI can remain at that distance.
        if (m_Camera == null) {
            m_Camera = GameObject.Find("VRCamera").transform;
        }
    }

    private void Update() {
        // If the UI should look at the camera set it's rotation to point from the UI to the camera.
        if (m_LookAtCamera)
            m_UIElement.rotation = Quaternion.LookRotation(m_UIElement.position - m_Camera.position);

        // If the UI should rotate with the camera...
        if (m_RotateWithCamera) {
            // Find the direction the camera is looking
            Vector3 targetDirection = m_Camera.forward;

            // Calculate a target position from the camera in the direction at the same distance from the camera as it was at Start.
            Vector3 targetPosition = m_Camera.position + targetDirection * m_DistanceFromCamera;

            // Set the target position  to be an interpolation of itself and the UI's position.
            targetPosition = Vector3.Lerp(m_UIElement.position, targetPosition, m_FollowSpeed * Time.deltaTime);

            // Set the UI's position to the calculated target position.
            m_UIElement.position = targetPosition;
        }
    }
}