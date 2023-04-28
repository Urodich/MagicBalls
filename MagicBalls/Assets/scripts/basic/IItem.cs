using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    public void ShowInfo(){}
    public void CloseInfo(){}
    public void Take(GameObject player){}
}
