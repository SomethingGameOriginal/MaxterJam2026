using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    public float speed;

    float velocityY;
    float gravityForce = -9.81f;

    public float jumpHeight = 1.5f;
    public float fallMultiplier;

    Gravity gravity;
    Vector3 gravityDirection;


    public float groundCheckDistance;
    public float gravityCheckDistance;


    public Transform respawn;
    public GameObject deadPlayer;
    Coroutine gravityResetCoroutine;
    public int attempts;
    public TextMeshProUGUI attemptsText;
    bool isDead = false;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        gravity = FindFirstObjectByType<Gravity>();
    }
    void Update()
    {
        // Прочие
        gravityDirection = Quaternion.Euler(gravity.gravityRotation) * Vector3.down;
        //transform.rotation = Quaternion.Euler(gravity.gravityRotation);

        RaycastHit hit;
        bool isGrounded = Physics.Raycast(transform.position, gravityDirection, out hit, groundCheckDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

        Debug.DrawRay(transform.position, gravityDirection * groundCheckDistance, Color.red);

        if (isGrounded && velocityY < 0)
        {
            velocityY = 0;
        }

        attemptsText.text = attempts.ToString();


        // Движение
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, gravityDirection).normalized;
        Vector3 right = Vector3.ProjectOnPlane(transform.right, gravityDirection).normalized;

        Vector3 move = (right * moveX + forward * moveZ).normalized;

        controller.Move(move * speed * Time.deltaTime);


        // Гравитация
        if (velocityY < 0)
            velocityY += gravityForce * fallMultiplier * Time.deltaTime;
        else
            velocityY += gravityForce * Time.deltaTime;

        controller.Move(gravityDirection * -velocityY * Time.deltaTime);


        // Прыжок
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            velocityY = Mathf.Sqrt(jumpHeight * -2 * gravityForce);



        // Сдох
        if (attempts <= 0)
            SceneManager.LoadScene(3);

        if (Input.GetKeyDown(KeyCode.R) && !isDead)
        {
            isDead = true;
            StartCoroutine(Dead());
        }

        // Делаем магию священого ХУЯ у прыжка
        //RaycastHit gravityHit;
        //Debug.DrawRay(transform.position, gravityDirection * gravityCheckDistance, Color.blue);

        //if (Physics.Raycast(transform.position, gravityDirection, out gravityHit, gravityCheckDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide))
        //{
        //    Wall_And_GravityTrigger gravityTrigger = gravityHit.collider.GetComponent<Wall_And_GravityTrigger>();

        //    if (gravityTrigger == null)
        //    {
        //        if (gravityResetCoroutine == null)
        //        {
        //            gravityResetCoroutine = StartCoroutine(Aga());
        //        }
        //    }
        //    else
        //    {
        //        if (gravityResetCoroutine != null)
        //        {
        //            StopCoroutine(gravityResetCoroutine);
        //            gravityResetCoroutine = null;
        //        }
        //    }
        //}
    }
    //IEnumerator Aga()
    //{
    //    yield return new WaitForSeconds(1f);
    //    gravity.gravityRotation = Vector3.zero;
    //    gravityResetCoroutine = null;
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spikes") && !isDead)
        {
            isDead = true;
            StartCoroutine(Dead());
        }
    }
    IEnumerator Dead()
    {
        yield return new WaitForSeconds(.2f);
        Instantiate(deadPlayer, transform.position, Quaternion.EulerRotation(0, 0, 0));
        controller.enabled = false;
        transform.position = respawn.position;
        controller.enabled = true;
        attempts--;
        isDead = false;
    }
}
