using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Managers")]
    private AudioManager m_AudioManager;

    [Header("Components")]
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask cornerCorrectLayer;

    [Header("Movement Variables")]
    [SerializeField] private float movementAcceleration;
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float linearDrag;

    [Header("Dash Parameters")]
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashLength = 0.3f;
    [SerializeField] private float dashBufferLength = 0.1f;
    private float dashBufferCounter;
    private bool isDashing;
    private bool hasDashed;
    private bool canDash => dashBufferCounter < 0f && !isDashing;

    [Header("Player Status")]
    [HideInInspector] public bool IsCarrying;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        m_AudioManager = AudioManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAudio();
        if (Input.GetButtonDown("Dash") && canDash) {
            dashBufferCounter = dashBufferLength;
            StartCoroutine(Dash(GetInput()));
        }
        else dashBufferCounter -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (!isDashing) {
            MoveCharacter();
            ApplyLinearDrag();
        }
    }

    void UpdateAudio()
    {
        if (Input.GetButtonDown("Interact"))
        {
            m_AudioManager.PlaySoundOnce("Scream");
        }
    }

    IEnumerator Dash(Vector2 dir) {
        float dashStartTime = Time.time;
        isDashing = true;   

        rb.velocity = Vector2.zero;
        rb.drag = 0f;

        while (Time.time < dashStartTime + dashLength) {
            rb.velocity = dir.normalized * dashSpeed;
            yield return null;
        }

        isDashing = false;
    }

    private static Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void MoveCharacter()
    {
        rb.velocity = GetInput().normalized*maxMoveSpeed;
    }

    private void ApplyLinearDrag()
    {
        //Only apply drag when you aren't moving the player.
        if (GetInput() == Vector2.zero) {
            rb.drag = linearDrag;
        } else {
            rb.drag = 0;
        }
    }   

    void CornerCorrect(float Yvelocity)
    {
        //RaycastHit2D _hit = Physics2D.Raycast(transform.position - )
    }
}
