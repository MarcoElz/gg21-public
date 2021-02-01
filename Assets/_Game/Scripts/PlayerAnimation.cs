using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator animator = default;
    [SerializeField] SpriteRenderer spriteRenderer = default;
    private int isWalkingAnimationId;

    private void Awake()
    {
        isWalkingAnimationId = Animator.StringToHash("isWalking");
    }

    public void MoveAnimation(Vector2 input)
    {
        //Animation
        animator.SetBool(isWalkingAnimationId, input.x != 0f || input.y != 0f);

        //Flip
        bool isWalkingLeft = input.x < 0f;
        bool isWalkingRight = input.x > 0f;

        if(isWalkingLeft)
            spriteRenderer.flipX = false;  
        else if(isWalkingRight)
            spriteRenderer.flipX = true;
        
    }

}
