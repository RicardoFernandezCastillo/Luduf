using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public float moveSpeed = 5f; // Velocidad de movimiento del jugador
	public float turnSpeed = 720f; // Velocidad de rotación del jugador
	public float mouseSensitivity = 100f; // Sensibilidad del ratón
	public Transform cameraTransform; // Referencia a la cámara

	private Vector3 moveDirection;
	private float xRotation = 0f;

	void Start()
	{
		if (cameraTransform == null)
		{
			cameraTransform = Camera.main.transform;
		}

		Cursor.lockState = CursorLockMode.Locked; // Bloquear el cursor en el centro de la pantalla
	}

	void Update()
	{
		// Rotar la cámara con el mouse
		float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

		xRotation -= mouseY;
		xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limitar la rotación vertical

		cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
		transform.Rotate(Vector3.up * mouseX);

		// Obtener la entrada del jugador
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		// Calcular la dirección de movimiento en relación con la cámara
		moveDirection = (transform.right * horizontal + transform.forward * vertical).normalized;

		if (moveDirection.magnitude >= 0.1f)
		{
			// Mover al jugador hacia adelante
			transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
		}

		// Mantener la cámara detrás del jugador
		cameraTransform.position = transform.position - transform.forward * 3f + Vector3.up * 2f;
		cameraTransform.LookAt(transform.position);
	}




}
