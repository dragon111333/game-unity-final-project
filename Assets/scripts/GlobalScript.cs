using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GlobalScript : MonoBehaviour
{
    public Text plantText;
    public bool enableCharacterMove ;
    public GameObject plantMenu;

    public Material selectedMat;

    public Transform prefab;

    public List<GameObject> characterHitOn = new List<GameObject>();
    public Dictionary<string, List<GameObject>> plantData;

    bool[,] planInDirt = new bool[4,4];

    private void Start()
    {
        this.enableCharacterMove = true;
        this.EnterPlantData();
    }

    private void EnterPlantData()
    {
        plantData = new Dictionary<string, List<GameObject>>();

        var allPlant = prefab.GetChild(0);

        for (int plantIndex = 0; plantIndex < allPlant.childCount; plantIndex++) {
           
            Transform plant = allPlant.GetChild(plantIndex);

            print($"Plant [{plantIndex}] : {plant.name}");

            List<GameObject> plantAge = new List<GameObject>();

            for (int plantAgeIndex = 0; plantAgeIndex < 3; plantAgeIndex++)
            {
                GameObject plantLife = plant.GetChild(plantAgeIndex).gameObject;
                print(plantLife.name);
                plantAge.Add(plantLife);
            }

            plantData.Add(plant.name, plantAge);
            print("---------------");
        }

    }

    public void SwitchPlantMenu(bool open)
    {
        this.plantMenu.SetActive(open);
        this.enableCharacterMove = (!open);

    }
    public void ClearAllMenu()
    {
        print("clear menu");
        this.SwitchPlantMenu(false);
    }

    public void ClearAllLabel()
    {
        plantText.enabled = false;
    }

    public void Plant(string plant)
    {
        if(characterHitOn.Count <= 0 || characterHitOn.Last() == null) return ;

        GameObject dirtInteract = characterHitOn.Last().gameObject;

        string [] pos = dirtInteract.name.Split("_");

        int x = int.Parse(pos[1]);
        int y = int.Parse(pos[2]);

        if (planInDirt[x, y]) return;

        planInDirt[x, y] = true;

        GameObject babyPlant = null;

        babyPlant = Instantiate(plantData[plant][0]);
        babyPlant.transform.parent = GameObject.Find("/planted").transform;
        babyPlant.transform.position = dirtInteract.transform.position;

        this.ClearAllMenu();


    }

}
