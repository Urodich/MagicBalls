using UnityEngine;

public interface IItem
{
    public void ShowInfo(){}
    public void CloseInfo(){}
    public void Take(GameObject player){}
}
