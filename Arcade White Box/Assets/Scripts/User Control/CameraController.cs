using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
    public enum CameraMode { FreeCamera, RotationLock }
    
    [SerializeField] private float movementSpeed;
    [SerializeField] private float boostSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float rotationSensitivity;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float scrollSensitivity;
    [SerializeField] private float yMin;
    [SerializeField] private float yMax;
    [SerializeField] private float minScrollAngle;
    [SerializeField] private float maxScrollAngle;
    [SerializeField] private Transform cameraTransform;

    private float scrollValue, scrollAngle, rotationAngle;
    private Vector3 movement;
	private Transform parentTransform;
	private CursorLockMode cursorMode;
	private CameraMode cameraMode;
    
	void Awake()
	{
		parentTransform = GetComponent<Transform>();
	}
	
    void Start()
	{
	    cursorMode = CursorLockMode.None;
	    Cursor.lockState = cursorMode;

        scrollValue = parentTransform.position.y;
        movement = parentTransform.position;
    }

    void Update()
    {
        GetUserInput();
    }

	void LateUpdate()
	{
        CameraScroll();
        CameraMovement();
        CameraRotation();
    }


    private void GetUserInput()
    {
        float xValue = Input.GetAxis("Horizontal");
        float zValue = Input.GetAxis("Vertical");

        movement = new Vector3(xValue, 0.0f, zValue);

        scrollValue -= Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;
        scrollAngle -= Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;
        rotationAngle += Input.GetAxis("Camera Rotation") * rotationSensitivity;

        scrollValue = Mathf.Clamp(scrollValue, yMin, yMax);
        scrollAngle = Mathf.Clamp(scrollAngle, minScrollAngle, maxScrollAngle);
    }

    private void CameraMovement()
    {   
        if(Input.GetButton("Camera Boost"))
        {
            parentTransform.Translate(movement * Time.deltaTime * boostSpeed);
        }
        else
        {
            parentTransform.Translate(movement * Time.deltaTime * movementSpeed);
        }
        
    }

    private void CameraScroll()
    {
        Vector3 newPosition = parentTransform.position;
        newPosition.y = Mathf.Lerp(newPosition.y, scrollValue, Time.deltaTime * scrollSpeed);

        Vector3 newRotation = cameraTransform.rotation.eulerAngles;
        newRotation.x = Mathf.LerpAngle(newRotation.x, scrollAngle, Time.deltaTime * scrollSpeed);

        cameraTransform.rotation = Quaternion.Euler(newRotation);
        parentTransform.position = newPosition;
    }

    private void CameraRotation()
    {
        Vector3 newRotation = parentTransform.rotation.eulerAngles;
        newRotation.y = Mathf.LerpAngle(newRotation.y, rotationAngle, Time.deltaTime * rotationSpeed);

        parentTransform.rotation = Quaternion.Euler(newRotation);
    }
}
