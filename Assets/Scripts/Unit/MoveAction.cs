using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    private Vector3 targetPosition;

    [SerializeField] private Animator unitAnimator;
    private readonly float _moveSpeed = 4f;
    private readonly float _rotateSpeed = 10f;

    private void Awake() {
         targetPosition = transform.position;
    }
    private void Update(){
        float stopingDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) > stopingDistance)
        {
            Vector3 moveDir = (targetPosition - transform.position).normalized;  
            
            transform.position += _moveSpeed * Time.deltaTime * moveDir;
            transform.forward = Vector3.Lerp(transform.forward,moveDir,Time.deltaTime * _rotateSpeed);
            unitAnimator.SetBool("IsWalking", true);
        }
        else
        {
            unitAnimator.SetBool("IsWalking", false);
        }
    }

    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    public List<GridPosition> GetValidActionGridPositionList(){
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        return validGridPositionList;
    }


}
