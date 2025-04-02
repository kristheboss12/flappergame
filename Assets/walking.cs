using UnityEngine;

public class Walker : MonoBehaviour
{
    public float speed = 2f;
    public float rotationSpeed = 5f;

    private bool isTurning = false;
    private Rigidbody rb;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        animator.Play("Walk"); // Replace with your actual animation name
    }

    private void FixedUpdate()
    {
        if (!isTurning)
        {
            // Move forward respecting physics
            Vector3 move = transform.forward * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);
        }
        else
        {
            // Smooth turn
            Quaternion targetRotation = Quaternion.Euler(0, 180, 0) * transform.rotation;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                isTurning = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Automatically turn when hitting anything with a collider (except itself if needed)
        if (!isTurning)
        {
            isTurning = true;
        }
    }
}
