using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Fish", menuName = "New Fish", order = 1)]
public class FishData : ItemData
{
    public Sprite ShowToPnjSprite() {
        return Resources.Load<Sprite>($"Sprites/ShowToPnj/{name}");
    }
}
