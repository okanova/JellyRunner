using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emojis : MonoBehaviour
{
   public void EmojiClose()
   {
      for (int i = 0; i < transform.childCount; i++)
      {
         transform.GetChild(i).gameObject.SetActive(false);
      }
   }
}
