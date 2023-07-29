using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour 
{
    public Transform PointA;
    public Transform PointB;
    [SerializeField] AnimationCurve curveAB;
    [SerializeField] AnimationCurve curveBA;

    public float TimeMoveAB;
    public float TimeMoveBA;
    float timeHasPassed;

    Vector3 startPosition;
    Vector3 TargetPosition;

    bool isMoving;

    public PlayerController Player;
    Animator animator;
    AnimationController animationController;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animationController = new AnimationController(animator);
        transform.position = PointA.position;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GameConst.PLAYER_TAG))
        {
            collision.transform.SetParent(this.transform);
            TriggerMovePlatform();
        }
    }
    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag(GameConst.PLAYER_TAG))
    //    {
    //        collision.gameObject.GetComponent<PlayerController>();
    //        if (collision.transform.parent == null)
    //        {
                
    //        }
    //        if (collision.gameObject.transform.parent == this.transform
    //            && collision.gameObject.GetComponent<PlayerController>().IsJumping || Player.IsDie)
    //            collision.transform.SetParent(null);
    //    }
    //}
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GameConst.PLAYER_TAG))
        {
            collision.transform.SetParent(null);
        }
    }
    public void TriggerMovePlatform()
    {
        if(!isMoving)
            StartCoroutine(Move());
    }
    IEnumerator Move()
    {
        animationController.ChangeAnimationState("Move");
        isMoving = true;
        yield return new WaitForSeconds(0.5f);
        timeHasPassed = 0;
        startPosition = transform.position;
        TargetPosition = PointB.position;
        while (timeHasPassed < TimeMoveAB)
        {
            timeHasPassed += Time.deltaTime;
            float percentageCompelete = timeHasPassed / TimeMoveAB;
            transform.position =
                Vector3.Lerp(startPosition, TargetPosition, curveAB.Evaluate(percentageCompelete));
            yield return null;
        }
        CameraShake.Instance.ShakeCamera(3f, 7f, 0.5f);
        yield return new WaitForSeconds(1f);
        timeHasPassed = 0;
        startPosition = PointB.position;
        TargetPosition = PointA.position;
        while (timeHasPassed < TimeMoveBA)
        {
            timeHasPassed += Time.deltaTime;
            float percentageCompelete = timeHasPassed / TimeMoveBA;
            transform.position =
                Vector3.Lerp(startPosition, TargetPosition, curveBA.Evaluate(percentageCompelete));
            yield return null;
        }
        isMoving = false;
        animationController.ChangeAnimationState("Idle");
    }
}
