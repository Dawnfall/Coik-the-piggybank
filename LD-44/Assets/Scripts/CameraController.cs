using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private PlayerController m_playerController;
    public PlayerController PlayController
    {
        get
        {
            if (!m_playerController)
                m_playerController = FindObjectOfType<PlayerController>();
            return m_playerController;
        }
    }

    private Camera m_camera;
    public Camera Camera
    {
        get
        {
            if (!m_camera)
                m_camera = GetComponent<Camera>();
            return m_camera;
        }
    }

    public Vector3 zOffset;
    public float distancePercent;
    public float cameraMoveSpeed;

    private void Update()
    {
        transform.position = PlayController.transform.position + zOffset;
        //just for now ...TODO.....camera viewpoint drag
    }

    public Vector3 LookPoint
    {
        get
        {
            return transform.position + PlayController.CrossHair.localPosition * distancePercent;
        }
    }
    public Vector3 GetMouseWorldPosition()
    {
        return Camera.ScreenToWorldPoint(Input.mousePosition);
    }
}
