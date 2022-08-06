using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class L1Script : LevelScript
{
     public Sprite plusA;
    public Sprite plusB;
    public GameObject confetti;
    public Sprite A;
    public Sprite B;
    public Sprite C;
    public Sprite F;
    public Image image;
   public override void  Interact(string label){
        label = label.ToLower();
        print("interacting");
        if(label.Contains("plusa")){
            Instantiate(confetti);
            print("interacted");
            image.sprite = plusA;
        }else if(label.Contains("plusb")){
             print("interacted");
            image.sprite = plusB;
            Instantiate(confetti);
        }else if(label.Contains("a") && !label.Contains("s")){
             print("interacted");
             Instantiate(confetti);
             image.sprite = A;
        }else if(label.Contains("b") && !label.Contains("s")){
             print("interacted");
            image.sprite = B;
        }else if(label.Contains("c")){
             print("interacted");
            image.sprite = C;
        }else{
             print("interacted");
            image.sprite = F;
        }
   }
}
