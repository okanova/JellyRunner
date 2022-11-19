using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Collections;
using UnityEngine;

public class PlayerGroup : MonoBehaviour
{
   [Header("Player")]
   [ReadOnly] public List<Player> playerList;
   public Player player;
   [ReadOnly] public int playerCount;
   public float sizeChangerValue;
   public float changeTime;
   [ReadOnly] public string state = "one";
   
   [Header("Position Settings")] 
   [ReadOnly] public int maxPositionsCount;
   [ReadOnly] public List<Transform> positions;
   public float radius;
   public List<int> circleAngles;
   public float moveTime;
   public Transform position;

   private void Start()
   {
      CreatePositions();
      Player temp = Instantiate(player);
      AddCharacter(temp);
   }


   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.A) && maxPositionsCount + 1 > playerList.Count)
      {
         Player temp = Instantiate(player);
         AddCharacter(temp);
      }

      if (Input.GetKeyDown(KeyCode.D) && playerList.Count > 0)
      {
         if (playerList.Count - 1 == 0)
         {
            
         }
         else
         {
            Player temp = playerList[playerList.Count - 1];
            RemoveCharacter(temp);
         }
      }

      if (Input.GetKeyDown(KeyCode.E))
      {
         ChangeState();
      }
   }

   
   public void CreatePositions()
   {
      int totalAngle = 0;
      int angleCount = 0;

      float currentRadius = radius;
      float radians;
      float x;
      float y;

      for (int i = 0; i < circleAngles.Count; i++)
      {
         maxPositionsCount += 360 / circleAngles[i];
      }

      Transform temp = Instantiate(position);
      temp.SetParent(transform);
      temp.transform.position = Vector3.zero;
      positions.Add(temp);

      for (int i = 0; i < maxPositionsCount; i++)
      {
         totalAngle += circleAngles[angleCount];
         radians = totalAngle * Mathf.Deg2Rad;
         x = Mathf.Cos(radians);
         y = Mathf.Sin(radians);
         temp = Instantiate(position);
         temp.SetParent(transform);
         temp.transform.position = new Vector3(x, 0, y) * currentRadius;
         positions.Add(temp);

         if (totalAngle == 360)
         {
            totalAngle = 0;
            currentRadius += radius;
            angleCount++;
         }
      }
   }
   
   
   public void AddCharacter(Player temp)
   {
      if (temp == null)
      {
         temp = Instantiate(player);
      }
      else
      {
         playerCount++;
      }
      
      if (state == "one")
      {
         playerList[0].transform.DOScale((playerCount * sizeChangerValue * Vector3.one) + Vector3.one, changeTime / 4);
      }
      else
      {
         playerList.Add(temp);
         temp.animator = temp.GetComponentInChildren<Animator>();
         temp.animator.Play("Run");
         
         for (int i = 0; i < positions.Count; i++)
         {
            if (positions[i].childCount == 0)
            {
               temp.transform.SetParent(positions[i]);
               temp.transform.DOLocalMove(Vector3.zero, moveTime).SetEase(Ease.Linear);
               return;
            }
         }
      }
     
   }


   public void RemoveCharacter(Player temp)
   {
      if (temp == null)
      {
         temp = playerList[playerList.Count - 1];
      }
      else
      {
         playerCount--;
      }

      if (state == "one")
      {
         playerList[0].transform.DOScale((playerCount * sizeChangerValue * Vector3.one) + Vector3.one, changeTime / 4);
      }
      else
      {
         Destroy(temp.gameObject);
         playerList.Remove(temp);
      }
     
   }
   

   public void ChangeState()
   {
      if (playerCount == 1)
          return;
      
      if (state == "one")
      {
         state = "many";

         for (int i = 1; i < playerCount; i++)
         {
            AddCharacter(null);
         }

         playerList[0].transform.DOKill();
         playerList[0].transform.localScale = Vector3.one;
         
      }
      else if (state == "many")
      {
         state = "one";
         playerList[0].transform.DOScale((playerCount * sizeChangerValue * Vector3.one) + Vector3.one, changeTime);
         
         while (playerList.Count > 1)
         {
            RemoveCharacter(null);
         }
      }
   }


   public void Movement()
   {
      
   }
}
