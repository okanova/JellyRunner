using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   public Animator animator;
   public bool inList;
   public Animator emojis;
   
   
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
}
