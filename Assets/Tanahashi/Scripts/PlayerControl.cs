using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerControl : MonoBehaviour
{
    /*
    public enum PlayerState
    {
        Idle,
        Running,
        Jumping,
        Clear,
        Killed
    }
    */

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private float touchDeltaThreshold;

    [SerializeField]
    private float angleThreshold;

    private bool isRunning;
    private Rigidbody2D rb;
    private GameObject spriteObj;
    private Animator animator;
    private TouchManager touchManager;
    private GroundDetector groundDetector;


    void Start()
    {
        isRunning = false;
        rb = GetComponent<Rigidbody2D>();
        spriteObj = transform.Find("PlayerSprite").gameObject;
        animator = spriteObj.GetComponent<Animator>();
        touchManager = GetComponent<TouchManager>();
        groundDetector = transform.Find("GroundDetector").gameObject.GetComponent<GroundDetector>();
    }


    void Update()
    {
        if (groundDetector.onGround)
        {
            // 移動した場合の処理
            if ((touchManager.touchPhase == TouchPhase.Moved || touchManager.touchPhase == TouchPhase.Stationary) && touchManager.touchDelta > touchDeltaThreshold)
            {
                Run();
            }
            // ジャンプした場合の処理
            if (touchManager.touchPhase == TouchPhase.Ended && touchManager.touchDelta < touchDeltaThreshold)
            {
                touchManager.touchDelta = touchDeltaThreshold + 1f;
                Jump();
            }
        }
        else
        {
            // 移動中に足場から落ちた場合の処理
            if (isRunning && touchManager.touchDelta > touchDeltaThreshold)
            {
                Stop();
            }
        }

        // 移動後に指を離した場合の処理
        if (isRunning && touchManager.touchPhase == TouchPhase.Ended && touchManager.touchDelta > touchDeltaThreshold)
        {
            Stop();
        }

        if (groundDetector.onGround)
        {
            animator.SetBool("jump", false);
        }
        else
        {
            animator.SetBool("jump", true);
        }

        GravityRotate();
    }


    void Run()
    {
        animator.SetBool("run", true);
        isRunning = true;

        Vector3 rightVec = transform.right;
        Vector3 planeFrom = Vector3.ProjectOnPlane(rightVec, Vector3.forward);
        //Debug.Log(planeFrom);

        Vector3 worldTouchCurrentPos = Camera.main.ScreenToWorldPoint(touchManager.touchCurrentPos);
        Vector3 touchVec = worldTouchCurrentPos - transform.position;
        Vector3 planeTo = Vector3.ProjectOnPlane(touchVec, Vector3.forward);
        //Debug.Log(planeTo);

        float angle = Vector3.SignedAngle(planeFrom, planeTo, Vector3.forward);
        //Debug.Log(angle);

        // 右に移動
        if (angle > -90f + angleThreshold && angle < 90f - angleThreshold)
        {
            spriteObj.transform.localScale = new Vector3(-1.3f, 1.3f, 1);
            rb.velocity = transform.right * moveSpeed * Time.fixedDeltaTime;
        }
        else if (angle > 90f + angleThreshold || angle < -90f - angleThreshold)
        {
            spriteObj.transform.localScale = new Vector3(1.3f, 1.3f, 1);
            rb.velocity = transform.right * -moveSpeed * Time.fixedDeltaTime;
        }
        else
        {
            animator.SetBool("run", false);
            rb.velocity = new Vector2(0, 0);
        }
    }


    void Jump()
    {
        SEManager.Instance.PlaySE(SEManager.SEName.PlayerJump);
        Vector3 touchVec = touchManager.touchEndPos - touchManager.touchStartPos;
        rb.AddForce(touchVec.normalized * jumpForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }


    void Stop()
    {
        animator.SetBool("run", false);
        isRunning = false;
        rb.velocity = new Vector2(0, 0);
    }


    void GravityRotate()
    {
        GravityState state = GravityManager.instance.worldGravityState;

        if (state == GravityState.Left)
        {
            transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        else if (state == GravityState.Right)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (state == GravityState.Top)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (state == GravityState.Bottom)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }


    public void Killed()
    {
        rb.simulated = false;
        animator.SetBool("killed", true);
        GManager.Instance.Fail();
        //Destroy(this.gameObject);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "GimmicDoku")
        {
            Killed();
        }

        if (collision.gameObject.tag == "FallZone")
        {
            Killed();
        }
    }
}
