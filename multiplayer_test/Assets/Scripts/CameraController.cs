using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static float WorldSize = 0.0f;

	private void LateUpdate()
    {
        var arenaCenterTransform = new Vector3(WorldSize / 2, WorldSize / 2, -10.0f);
        if (PlayerController.Local == null || !GameManager.IsConnected())
        {
            // Set the camera to be in middle of the arena if we are not connected or 
            // there is no local player
            transform.position = arenaCenterTransform;
            return;
        }

        var centerOfMass = PlayerController.Local.CenterOfMass();
        if (centerOfMass.HasValue)
        {
            // Set the camera to be the center of mass of the local player
            // if the local player has one
            transform.position = new Vector3
            {
                x = centerOfMass.Value.x,
                y = centerOfMass.Value.y,
                z = transform.position.z
            };
        } else {
            transform.position = arenaCenterTransform;
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 200, Time.deltaTime * 6f);
            return;
        }

		float targetCameraSize = CalculateCameraSize(PlayerController.Local);
        Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetCameraSize, 100f * Time.deltaTime);

		// Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetCameraSize, Time.deltaTime * 6f);
	}

    private float CalculateCameraSize(PlayerController player)
    {
        float mass = player.TotalMass();
        float zoom = mass / 100; // plus agressif
        return 40f + zoom;
    }



}