using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization; 
using UnityEngine;
using System;
using System.IO;
using UnityEngine.AI;

public class Loader : MonoBehaviour
{
    static Loader inst;
    [SerializeField] string datapath;
    string filename;

    [SerializeField] GameObject[] Rooms;
    public GameObject player;
    public static Loader GetInstance(){
        return inst;
    }
    void Start(){
        if(inst==null) inst=this;
        datapath = Application.dataPath + "/Saves";
        filename = "/" + Application.loadedLevel + ".xml";

        if(!Directory.Exists(datapath))
        Directory.CreateDirectory(datapath);
		
    }
    public void Save(){
        
        Debug.Log("SAVED");
        SavedData savedData = new();
        List<RoomState> roomStates = new List<RoomState>(); 
        foreach(GameObject room in Rooms){
            RoomState State = new();
            State.Name=room.name;
            foreach(Transform elem in room.GetComponentInChildren<Transform>())
                State.AddItem(new PositData(elem.gameObject.name, elem.gameObject.active));
            roomStates.Add(State);
        }
        savedData.rooms=roomStates;
        savedData.playerData = new PlayerData(player);

        player.GetComponent<player_script>().PosDispell();
        player.GetComponent<player_script>().NegDispell();
        player.GetComponent<inventory_script>().DropAll();
        player.GetComponent<buffs_script>().Save();

        Type[] extraTypes= { typeof(PositData), typeof(RoomState), typeof(PlayerData)};
        XmlSerializer serializer = new XmlSerializer(typeof(SavedData), extraTypes);

        using ( FileStream fs = new FileStream(datapath+filename, FileMode.Create))
		    serializer.Serialize(fs, savedData); 
    }
    public void SaveWithDelay()=>Invoke("Save", 0.5f);
    public bool Load(){
        if(!File.Exists(datapath+filename)) return false;

        Type[] extraTypes= { typeof(PositData), typeof(RoomState), typeof(PlayerData)};
        XmlSerializer serializer = new XmlSerializer(typeof(SavedData), extraTypes);

        SavedData state;
        using ( FileStream fs = new FileStream(datapath+filename, FileMode.Open))
		    state = (SavedData)serializer.Deserialize(fs);  
        
        foreach(GameObject room in Rooms){
            RoomState roomState = state.rooms.Find((elem)=>elem.Name==room.name);
            if(roomState==null) {room.SetActive(false); continue;}

            foreach(Transform elem in room.GetComponentInChildren<Transform>()){
                if (roomState.objects.Find((el)=>el.Name==elem.name)==null) elem.gameObject.SetActive(false);
                else if (!roomState.objects.Find((el)=>el.Name==elem.name).IsActive) elem.gameObject.SetActive(false);
            }
        }
        NavMeshAgent navMesh = player.GetComponent<NavMeshAgent>();
        navMesh.enabled=false;
        player.transform.position=state.playerData.position;
        navMesh.enabled=true;

        player.GetComponent<buffs_script>().Load();

        foreach (string item in state.playerData.items){
                Instantiate(Resources.Load<Item>("Item_inst/"+item)).Take(player);
            };

        return true;
    }
    public void Delete(){
        if(File.Exists(datapath+filename)) File.Delete(datapath+filename);
    }
}
[XmlRoot("Save")]
[XmlInclude(typeof(RoomState))] 
[XmlInclude(typeof(PlayerData))] 
public class SavedData{
    [XmlArray("Rooms")]
	[XmlArrayItem("Room")]
    public List<RoomState> rooms = new List<RoomState>();
    
    [XmlElement("PlayerData")]
    public PlayerData playerData = new();

    public SavedData() {}
}

[XmlType("RoomState")]
[XmlInclude(typeof(PositData))] 
public class RoomState {		//  класс, содержащий состояние комнаты в целом

	[XmlArray("objects")]
	[XmlArrayItem("arrayItem")]
	public List<PositData> objects = new List<PositData>(); // список из перемещаемых предметов

    [XmlElement("Name")]
	public string Name { get; set; }
	public RoomState() { }

	public void AddItem(PositData item) {
		objects.Add(item);
	}
	
}

[XmlType("PositionData")]
public class PositData
{
	[XmlElement("Name")]
	public string Name { get; set; }  // это будет название префаба из Resourses
	
	[XmlElement("IsActive")]
    public bool IsActive { get; set; }

	public PositData() { }
	
	public PositData(string name, bool IsActive)
	{
		this.Name = name;
		this.IsActive = IsActive;
	}
	
}

public class PlayerData
{
    [XmlElement("Position")]
    public Vector3 position { get; set; }
    public List<string> items = new List<string>();

    public PlayerData() {}

    public PlayerData(GameObject player){
        inventory_script inventory = player.GetComponent<inventory_script>();
        position=player.transform.position;
        foreach(Item item in inventory.GetItems()){
            if(item!=null)items.Add(item.name.Replace("(Clone)",""));
        }
    }

}