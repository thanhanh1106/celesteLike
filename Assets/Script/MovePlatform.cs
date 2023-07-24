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

    private void Start()
    {
        transform.position = PointA.position;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GameConst.PLAYER_TAG))
        {
            TriggerMovePlatform();
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GameConst.PLAYER_TAG))
        {
            if (Player.transform.parent == null)
            {
                Player.transform.SetParent(this.transform);
            }
            if (Player.transform.parent == this.transform && Player.IsJumping)
                Player.transform.SetParent(null);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GameConst.PLAYER_TAG))
        {
            if (Player.transform.parent == this.gameObject)
                Player.transform.SetParent(null);
        }
    }
    public void TriggerMovePlatform()
    {
        if(!isMoving)
            StartCoroutine(Move());
    }
    IEnumerator Move()
    {
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
    }
}
