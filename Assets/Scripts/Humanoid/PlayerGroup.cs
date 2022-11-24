using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerGroup : MonoBehaviour
{
   [Header("Player")]
   public Player player;
   public float sizeChangerValue;
   public float changeTime;
   public ParticleSystem deathParticle;
   public ParticleSystem lavaParticle;
   [ReadOnly] public string state = "one";
   [ReadOnly] public List<Player> playerList;
   [ReadOnly] public int playerCount;

   [Space(10)]
   [Header("Movement")] 
   public Vector2 speed;
   public float oneClickTime;
   public float clamp;

   [Space(10)]
   [Header("Position Settings")]
   public float radius;
   public List<int> circleAngles;
   public float moveTime;
   public Transform position;
   [ReadOnly] public int maxPositionsCount;
   [ReadOnly] public List<Transform> positions;

   [Space(10)] 
   [Header("Wall")] 
   public float power;
   public int powerOfDistance;
   

   private void Start()
   {
      CreatePositions();
      Player temp = Instantiate(player);
      AddCharacter(temp);

      StartCoroutine("Movement");
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
      bool isNull = false;

      if (temp == null)
      {
         temp = Instantiate(player);
         isNull = true;
      }
      else
      {
         playerCount++;
      }

      temp.inList = true;
      
      playerList.Add(temp);
      temp.animator = temp.GetComponentInChildren<Animator>();
      temp.rigidbody = temp.GetComponentInChildren<Rigidbody>();
      temp.animator.Play("Run");
      
      temp.emojis.Play("AnimationMovement");
      int random = Random.Range(0, temp.emojis.transform.childCount);
      temp.emojis.transform.GetChild(random).gameObject.SetActive(true);

      for (int i = 0; i < positions.Count; i++)
      {
         if (positions[i].childCount == 0)
         {
            temp.transform.SetParent(positions[i]);

            if (isNull)
            {
               temp.transform.position = positions[0].position;
            }
            
            temp.transform.DOLocalMove(Vector3.zero, moveTime).SetEase(Ease.Linear).OnUpdate(() =>
            {
               temp.transform.LookAt(transform.parent);
            }).OnComplete(() =>
            {
               temp.transform.LookAt(transform.position + Vector3.forward);
            });
            return;
         }
      }
   }

   public void ChangeSize()
   {
      playerList[0].transform.DOScale((playerCount * sizeChangerValue * Vector3.one)
                                      + Vector3.one, changeTime / 4);
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
         ParticleSystem particle = Instantiate(deathParticle);
         particle.transform.position = temp.transform.position;
      }

      Destroy(temp.gameObject);
      playerList.Remove(temp);
     
   }
   

   public void ChangeState()
   {
      if (playerCount == 1)
          return;
      
      if (state == "one")
      {
         for (int i = 1; i < playerCount; i++)
         {
            AddCharacter(null);
         }

         playerList[0].transform.DOKill();
         playerList[0].transform.localScale = Vector3.one;
         
         state = "many";
         
      }
      else if (state == "many")
      {
         playerList[0].transform.DOScale((playerCount * sizeChangerValue * Vector3.one)
                                         + Vector3.one, changeTime);
         
         while (playerList.Count > 1)
         {
            RemoveCharacter(null);
         }
         
         state = "one";
      }
   }


   public IEnumerator Movement()
   {
      float timer = 0;
      Vector3 currentPos = Vector3.zero;
      float x = 0;
      float currentX = 0;

      while (true)
      {
         currentPos.z += speed.y / 1000f;
         timer += Time.deltaTime;

         if (Input.GetMouseButtonDown(0))
         {
            timer = 0;
            x = Input.mousePosition.x;
         }
         
         if (Input.GetMouseButton(0))
         {
            currentX = Input.mousePosition.x;
            currentPos.x = Mathf.Clamp(currentPos.x + ((currentX - x) * speed.x / 10000f), -clamp, clamp);
         }
         
         transform.rotation = Quaternion.LookRotation
            (transform.position + Vector3.forward);

         if (Input.GetMouseButtonUp(0))
         {
            if (timer < oneClickTime)
            {
               ChangeState();
            }
         }

         transform.position = currentPos;
         yield return null;
      }
   }
}
