using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
	public Transform player;  // Referencia al Transform del jugador
	public Vector3 offset = new Vector3(0, 15.9f, -11.83f);  // Offset para posicionar la cámara
	public float smoothSpeed = 0.125f;  // Velocidad de suavizado para el movimiento de la cámara

	void LateUpdate()
	{
		// Calcula la posición deseada de la cámara sumando el offset a la posición del jugador
		Vector3 desiredPosition = player.position + offset;
		// Suaviza el movimiento de la cámara hacia la posición deseada
		Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
		// Actualiza la posición de la cámara con la posición suavizada
		transform.position = smoothedPosition;
	}
}
