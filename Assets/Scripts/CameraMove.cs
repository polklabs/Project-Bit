using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMove : MonoBehaviour
{
    public bool useBorderPan = false;

    public float panSpeed = 20f;
    public float panBoarderThickness = 10f;
    public Vector2 panLimit;

    public float scrollSpeed = 20f;
    public float minY = 20f;
    public float maxY = 200f;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey(KeyCode.W) || (useBorderPan && Input.mousePosition.y >= Screen.height - panBoarderThickness))
        {
            pos.z += (panSpeed + (pos.y / 5)) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || (useBorderPan && Input.mousePosition.y <= panBoarderThickness))
        {
            pos.z -= (panSpeed + (pos.y / 5)) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || (useBorderPan && Input.mousePosition.x >= Screen.width - panBoarderThickness))
        {
            pos.x += (panSpeed + (pos.y / 5)) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || (useBorderPan && Input.mousePosition.x <= panBoarderThickness))
        {
            pos.x -= (panSpeed + (pos.y / 5)) * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            pos.y -= scroll * (scrollSpeed + (pos.y / 5)) * 100f * Time.deltaTime;
            if (pos.y > minY && pos.y < maxY)
            {
                pos.z += scroll * (scrollSpeed + (pos.z / 5)) * 100f * Time.deltaTime;
            }
        }

        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        transform.position = pos;
    }
}
