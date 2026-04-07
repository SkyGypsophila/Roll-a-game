using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
	public float speed = 10;
	public TextMeshProUGUI countText;
	public GameObject winTextObject;
	private Rigidbody rb;
	private int count;
	private float movementX;
	private float movementY;

	public AudioClip hitSound;
	public AudioClip pickupSound;
	private AudioSource audioSource;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        rb = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
		count = 0;

		SetCountText();
		winTextObject.SetActive(false);
	}

	public void OnMove(InputValue movementValue)
	{
		Vector2 movementVector = movementValue.Get<Vector2>();
		movementX = movementVector.x;
		movementY = movementVector.y;
	}

	void SetCountText()
	{
		countText.text = "Count: " + count.ToString();
		if (count >= 12)
		{
			winTextObject.SetActive(true);
			Destroy(GameObject.FindGameObjectWithTag("Enemy"));
		}
	}

	private void FixedUpdate()
	{
		Vector3 movement = new Vector3(movementX, 0.0f, movementY);

		rb.AddForce(movement * speed);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Enemy"))
		{
			audioSource.PlayOneShot(hitSound);
			Destroy(gameObject, 0.1f);
			winTextObject.gameObject.SetActive(true);
			winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("PickUp"))
		{
			audioSource.PlayOneShot(pickupSound);
			other.gameObject.SetActive(false);
			count = count + 1;
			SetCountText();
		}
	}
}
