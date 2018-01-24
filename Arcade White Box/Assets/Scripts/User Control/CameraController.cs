using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{    
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
    [SerializeField] private float dragSpeed;
    [SerializeField] private float resetSpeed;
    [SerializeField] private GameObject mainCamera;

    private bool lockForPlacing, dragging;
    private float scrollValue, scrollAngle, rotationAngle;
    private Vector3 movement, dragMovement, resetPosition;
    private Quaternion resetRotation;
	private Transform parentTransform, cameraTransform;
    private Camera cameraComponent;
	private CursorLockMode cursorMode;
    
	void Awake()
	{
		parentTransform = GetComponent<Transform>();
        cameraTransform = mainCamera.GetComponent<Transform>();
        cameraComponent = mainCamera.GetComponent<Camera>();
    }
	
    void Start()
	{
	    cursorMode = CursorLockMode.None;
	    Cursor.lockState = cursorMode;

        resetPosition = parentTransform.position;
        resetRotation = parentTransform.rotation;
        scrollValue = parentTransform.position.y;
        movement = parentTransform.position;
        lockForPlacing = false;
    }

    void Update()
    {
        GetUserInput();
    }

	void LateUpdate()
	{
        CameraScroll();
        CameraMovement();

        if(!lockForPlacing)
        {
            CameraRotation();
            CameraMouseDragging();
        }
    }

    private void GetUserInput()
    {
        float xValue = Input.GetAxis("Horizontal");
        float zValue = Input.GetAxis("Vertical");

        movement = new Vector3(xValue, 0.0f, zValue);

        scrollValue -= Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;
        scrollAngle -= Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;

        if(!lockForPlacing)
        {
            rotationAngle -= Input.GetAxis("Camera Rotation") * rotationSensitivity;
        }

        scrollValue = Mathf.Clamp(scrollValue, yMin, yMax);
        scrollAngle = Mathf.Clamp(scrollAngle, minScrollAngle, maxScrollAngle);

        if(Input.GetMouseButton(2))
        {
            dragging = true;

            dragMovement = parentTransform.position;
            dragMovement.x -= Input.GetAxis("Mouse X") * (dragSpeed * parentTransform.position.y) * Time.deltaTime;
            dragMovement.z -= Input.GetAxis("Mouse Y") * (dragSpeed * parentTransform.position.y) * Time.deltaTime;
        }
        else
        {
            dragging = false;
        }

        if(Input.GetButtonDown("Camera Reset"))
        {
            ResetTransform(false);
        }
    }

    private void CameraMovement()
    {   
        if(Input.GetButton("Camera Boost"))
        {
            parentTransform.Translate(movement * Time.deltaTime * (boostSpeed + parentTransform.position.y));
        }
        else
        {
            parentTransform.Translate(movement * Time.deltaTime * (movementSpeed + parentTransform.position.y));
        }
    }

    private void CameraScroll()
    {
        Vector3 newPosition = parentTransform.position;
        newPosition.y = Mathf.Lerp(newPosition.y, scrollValue, Time.deltaTime * scrollSpeed);

        Vector3 newRotation = cameraTransform.rotation.eulerAngles;
        newRotation.x = Mathf.Lerp(newRotation.x, scrollAngle, Time.deltaTime * scrollSpeed);

        cameraTransform.rotation = Quaternion.Euler(newRotation);
        parentTransform.position = newPosition;
    }

    private void CameraRotation()
    {
        Vector3 newRotation = parentTransform.rotation.eulerAngles;
        newRotation.y = rotationAngle * rotationSpeed;

        parentTransform.rotation = Quaternion.Lerp(parentTransform.rotation, Quaternion.Euler(newRotation), Time.deltaTime * rotationSpeed);
    }

    private void CameraMouseDragging()
    {   
        if(dragging)
        {
            parentTransform.position = dragMovement;
        }
    }

    private void ResetTransform(bool lerp)
    {
        if(!lerp)
        {
            parentTransform.position = resetPosition;
            parentTransform.rotation = resetRotation;
        }
        else
        {

        }
    }

    public void SetRotationLock(bool lockRotation) { this.lockForPlacing = lockRotation; }
}
