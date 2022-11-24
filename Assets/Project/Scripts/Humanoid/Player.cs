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
   public TrailRenderer trail;

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

            ParticleSystem particle = Instantiate(GameManager.Instance.playerGroup.deathParticle);
            particle.transform.position = transform.position;
            
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
      else if (other.gameObject.CompareTag("Wall"))
      {
         if (GameManager.Instance.playerGroup.state == "one")
         {
            Rigidbody[] rigidbodies = other.gameObject.GetComponentsInChildren<Rigidbody>();

            foreach (var rb in rigidbodies)
            {
               rb.isKinematic = false;
               Vector3 targetPos = transform.position;
               targetPos.y = 0;
               Vector3 dir = (rb.transform.position - targetPos);
               float power = (1 / Mathf.Pow(Vector3.Distance(rb.transform.position, targetPos), 
                  GameManager.Instance.playerGroup.powerOfDistance)) * GameManager.Instance.playerGroup.power;
               rb.AddForce(dir * power);
            }
         }
         else
         {
            if (GameManager.Instance.playerGroup.playerCount - 1 == 0)
            {
               GameManager.Instance.Lose();

               ParticleSystem particle = Instantiate(GameManager.Instance.playerGroup.deathParticle);
               particle.transform.position = transform.position;
               
               Destroy(gameObject);
            }
            else
            {
               GameManager.Instance.playerGroup.RemoveCharacter(this);
            }
         }
      }
      else if (other.gameObject.CompareTag("End"))
      {
         GameManager.Instance.Win();
         
         ParticleSystem particle = Instantiate(GameManager.Instance.playerGroup.deathParticle);
         particle.transform.position = transform.position;
         
         Destroy(gameObject);
      }
      else if (other.gameObject.CompareTag("Gold"))
      {
         other.gameObject.GetComponent<MeshRenderer>().enabled = false;
         other.gameObject.GetComponentInChildren<ParticleSystem>().Play();
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
         
         transform.DOLocalMoveY(2f + transform.localPosition.y, 1.25f).SetEase(Ease.OutSine)
            .OnComplete(() =>
            {
               transform.DOLocalMoveY(0, 1.25f).SetEase(Ease.InSine);
            });
      }
      else if (other.CompareTag("Lava"))
      {
         ParticleSystem lava = Instantiate(GameManager.Instance.playerGroup.lavaParticle);
         lava.transform.position = transform.position;
         
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
               GameManager.Instance.Lose();
               Destroy(gameObject);
            }
         }
      }
      else if (other.CompareTag("Finish"))
      {
         if (GameManager.Instance.playerGroup.state != "one")
            GameManager.Instance.playerGroup.ChangeState();

         GameManager.Instance.playerGroup.state = "onlyOne";
         trail.GetComponentInChildren<TrailRenderer>(true);
         trail.gameObject.SetActive(true);
      }
   }

   private void OnTriggerStay(Collider other)
   {
      if (other.CompareTag("Space"))
      {
         if (GameManager.Instance.playerGroup.state == "one")
            return;
         
         rigidbody.constraints = RigidbodyConstraints.None;
         rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX
                                                                     | RigidbodyConstraints.FreezePositionZ;
      }
   }
}
