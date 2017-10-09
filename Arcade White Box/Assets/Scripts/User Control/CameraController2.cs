using UnityEngine;
using System.Collections;

public class CameraController2 : MonoBehaviour 
{	
    public enum CameraMode { Locked, FreeMode }
    private CameraMode cameraMode = CameraMode.Locked;

	public float movementSpeed;
 	public float sensitivity;

	private float rotationX;
	private float rotationY;
	private Quaternion originalRotation;
	private Transform camTransform;
	private CursorLockMode cursorMode;

    private bool currentlyRotating;

    public float scrollSpeed;
    public float minXRotationLimit;
    public float maxXRotationLimit;
    public float panSpeed = 20f;

    void Start()
	{
	    originalRotation = transform.localRotation;
	    camTransform = transform;

	    cursorMode = CursorLockMode.None;
	    Cursor.lockState = cursorMode;

        currentlyRotating = false;

    }

	void Update()
	{

        Vector3 pos = transform.position;

        if (Input.GetKey("w"))
        {
            //pos.z += panSpeed * Time.deltaTime;
            transform.Translate(transform.forward * panSpeed * 0.01f, Space.World);        
        }

        if (Input.GetKey("s"))
        {
            pos.z -= panSpeed * Time.deltaTime;
            transform.Translate(-transform.forward * panSpeed * 0.01f, Space.World);

        }

        if (Input.GetKey("d"))
        {
            pos.x += panSpeed * Time.deltaTime;
            transform.Translate(transform.right * panSpeed * 0.01f, Space.World);
        }

        if (Input.GetKey("a"))
        {
            pos.x -= panSpeed * Time.deltaTime;
            transform.Translate(-transform.right * panSpeed * 0.01f, Space.World);
        }

        float xAxisValue = Input.GetAxis("Horizontal");
        float zAxisValue = Input.GetAxis("Vertical");

        camTransform.Translate(new Vector3(xAxisValue, 0.0f, zAxisValue) * Time.deltaTime * movementSpeed);

        //rotate left
        if (Input.GetKeyDown(KeyCode.N) && !currentlyRotating)
        {
            StartCoroutine(RotateMe(Vector3.up * 90, 1.0f));
        }

        //rotate right
        else if (Input.GetKeyDown(KeyCode.M) && !currentlyRotating)
        {
            StartCoroutine(RotateMe(Vector3.up * -90, 1.0f));
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;
        transform.localRotation *= Quaternion.Euler(5 * scroll * -300 * Time.deltaTime, 0, 0);
        //Vector3 tempRotation = transform.localRotation.eulerAngles;
        //tempRotation.x = Mathf.Clamp(tempRotation.x, 180, 270);
        //transform.localRotation = Quaternion.Euler(tempRotation);
       

    }

    IEnumerator RotateMe(Vector3 byAngles, float inTime)
    {
        currentlyRotating = true;
        roundRotations();
        var toAngle = transform.eulerAngles + byAngles;
        var currentAngle = transform.eulerAngles;
        var targetAngle = toAngle;

        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            transform.eulerAngles = new Vector3(
                Mathf.LerpAngle(currentAngle.x, targetAngle.x, t),
                Mathf.LerpAngle(currentAngle.y, targetAngle.y, t),
                Mathf.LerpAngle(currentAngle.z, targetAngle.z, t));

            yield return null;
        }

        currentlyRotating = false;
    }

    private void roundRotations()
    {
        var vec = transform.eulerAngles;
       // vec.x = Mathf.Round(vec.x / 90) * 90;
        vec.y = Mathf.Round(vec.y / 90) * 90;
        vec.z = Mathf.Round(vec.z / 90) * 90;
        transform.eulerAngles = vec;
    }


}
