using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   public Animator animator;
   public bool inList;

   private void OnTriggerEnter(Collider other)
   {
      Player otherPlayer = other.GetComponent<Player>();
      
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
      else if (other.CompareTag("Obstacle"))
      {
         if ( GameManager.Instance.playerGroup.playerCount - 1 == 0)
         {
            GameManager.Instance.Lose();
         }
         else
         {
            if ( GameManager.Instance.playerGroup.state == "many")
            {
               Player temp =  GameManager.Instance.playerGroup.playerList[
                  GameManager.Instance.playerGroup.playerList.Count - 1];
               GameManager.Instance.playerGroup.RemoveCharacter(temp);
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
