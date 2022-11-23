using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
   public Animator animator;
   public bool inList;
   public Animator emojis;
   public Rigidbody rigidbody;
   
   
   private void OnCollisionEnter(Collision other)
   {
      Player otherPlayer = other.gameObject.GetComponent<Player>();
      
      if (otherPlayer != null)
      {
         if (otherPlayer.inList)
            return;
         
         if (GameManager.Instance.playerGroup.state == "many")
         {
            GameManager.Instance.playerGroup.AddCharacter(otherPlayer);
         }
         else
         {
            GameManager.Instance.playerGroup.playerCount++;
            GameManager.Instance.playerGroup.ChangeSize();
            Destroy(otherPlayer.gameObject);
         }
      }
      else if (other.gameObject.CompareTag("Obstacle"))
      {
         if (GameManager.Instance.playerGroup.playerCount - 1 == 0)
         {
            GameManager.Instance.Lose();
            Destroy(gameObject);
         }
         else
         {
            if ( GameManager.Instance.playerGroup.state == "many")
            {
               GameManager.Instance.playerGroup.RemoveCharacter(this);
            }
            else
            {
               GameManager.Instance.playerGroup.playerCount = 
                  Mathf.Max(GameManager.Instance.playerGroup.playerCount - 3, 2);
               GameManager.Instance.playerGroup.ChangeSize();
            }
         }
      }
   }


   private void OnTriggerEnter(Collider other)
   {

      if (other.CompareTag("TriggerY"))
      {
         rigidbody.constraints = RigidbodyConstraints.None;
         rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX
                                                                     | RigidbodyConstraints.FreezePositionZ;
      }
      
      else if (other.CompareTag("Fan"))
      {
         if (GameManager.Instance.playerGroup.state == "one")
            return;
         
         rigidbody.constraints = RigidbodyConstraints.FreezeAll;
         
         transform.DOLocalMoveY(1.5f + transform.localPosition.y, 1.1f).SetEase(Ease.OutSine)
            .OnComplete(() =>
            {
               transform.DOLocalMoveY(0, 1.2f).SetEase(Ease.InSine);
            });
      }
      
      else if (other.CompareTag("Space"))
      {
         if (GameManager.Instance.playerGroup.state == "one")
            return;
         
         rigidbody.constraints = RigidbodyConstraints.None;
         rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX
                                                                     | RigidbodyConstraints.FreezePositionZ;
      }
   }
}
