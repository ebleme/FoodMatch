using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{
    //[SerializeField]
    //private LevelData levelData;

    [SerializeField]
    private Dropdown levelDropdown;

    [SerializeField]
    private InputField levelInput;

    [SerializeField]
    private InputField xLimit;

    [SerializeField]
    private InputField yLimit;

    [SerializeField]
    private InputField zLimit;
    
    [SerializeField]
    private InputField timeText;

    [SerializeField]
    private InputField count;

    [Header("Right Panels")]
    [SerializeField]
    private GameObject viewChangerActions;
    [SerializeField]
    private GameObject elementCountViewPanel;

    [Header("Save Button")]
    [SerializeField]
    private GameObject saveButton;

    [Header("Counts")]

    [SerializeField] private Text avacodoCount;
    [SerializeField] private Text bellCount;
    [SerializeField] private Text blueBerryCount;
    [SerializeField] private Text bookCount;
    [SerializeField] private Text bottleCount;
    [SerializeField] private Text branchCount;
    [SerializeField] private Text cakeCount;
    [SerializeField] private Text carrotCount;
    [SerializeField] private Text cherryCount;
    [SerializeField] private Text cupCount;
    [SerializeField] private Text cupcakeCount;
    [SerializeField] private Text diamondCount;
    [SerializeField] private Text donutCount;
    [SerializeField] private Text iceCreamCount;


    public Vector3 cubeLimits;

    ElementSpawnerEditor elementSpawnerEditor;

    LevelData[] levelDatas;
    LevelData choosenLevelData;

    public int newLevelNumber;
    public int time;

    LevelSaver levelSaver;

    public event Action<LevelData> OnEditModeActive;

    private void Awake()
    {
        levelSaver = FindObjectOfType<LevelSaver>();
        levelSaver.OnLevelSaved += LevelSaver_OnLevelSaved;

        elementSpawnerEditor = FindObjectOfType<ElementSpawnerEditor>();
        elementSpawnerEditor.OnCountChange += ElementSpawnerEditor_OnCountChange;
    }
    private void Start()
    {
        GetLevelsFromResources();
    }

    private void LevelSaver_OnLevelSaved()
    {
        GetLevelsFromResources();

        viewChangerActions.SetActive(true);
        elementCountViewPanel.SetActive(false);
        saveButton.SetActive(false);
    }


    private void GetLevelsFromResources()
    {
        levelDatas = Resources.FindObjectsOfTypeAll<LevelData>();

        levelDropdown.options.Clear();

        foreach (var item in levelDatas)
        {
            levelDropdown.options.Add(new Dropdown.OptionData() { text = item.level.ToString() });
        }
    }

    // DropDown ile seçilirse Edit yapýlýr
    public void LevelChoose()
    {
        choosenLevelData = levelDatas[levelDropdown.value];
        xLimit.text = choosenLevelData.cubeLimits.x.ToString();
        yLimit.text = choosenLevelData.cubeLimits.y.ToString();
        zLimit.text = choosenLevelData.cubeLimits.z.ToString();

        newLevelNumber = choosenLevelData.level;
        time = choosenLevelData.time;

        timeText.text = time.ToString();

        OnEditModeActive?.Invoke(choosenLevelData);

        SpawnChoosenLevelData(choosenLevelData);

        saveButton.SetActive(true);
    }

    private void SpawnChoosenLevelData(LevelData choosenLevelData)
    {
        elementSpawnerEditor.SpawnElements(choosenLevelData);
    }

    public void NewLevel()
    {
        newLevelNumber = levelDatas.Length + 1;
        levelInput.text = newLevelNumber.ToString();
    }

    public void OnLevelInput_Change()
    {
        int.TryParse(levelInput.text, out newLevelNumber);
    } 
    
    public void OnTimeInput_Change()
    {
        int.TryParse(timeText.text, out time);
    }

    private void ElementSpawnerEditor_OnCountChange(int obj)
    {
        count.text = obj.ToString();
    }

    public void SpawnElements()
    {
        viewChangerActions.SetActive(true);
        elementCountViewPanel.SetActive(false);
        saveButton.SetActive(false);


        cubeLimits = new Vector3(Convert.ToInt32(xLimit.text), Convert.ToInt32(yLimit.text), Convert.ToInt32(zLimit.text));
        elementSpawnerEditor.SpawnElements(cubeLimits);
    }

    public void SetElementDatas()
    {
        elementSpawnerEditor.SetElementDatas();

        var keys = elementSpawnerEditor.dicElementTypes.Keys;

        foreach (var item in elementSpawnerEditor.dicElementTypes)
        {
            switch (item.Key)
            {
                case ElementTypes.Avacado:
                    avacodoCount.text = item.Value.ToString();
                    break;
                case ElementTypes.Bell:
                    bellCount.text = item.Value.ToString();
                    break;
                case ElementTypes.BlueBerry:
                    blueBerryCount.text = item.Value.ToString();
                    break;
                case ElementTypes.Book:
                    bookCount.text = item.Value.ToString();
                    break;
                case ElementTypes.Bottle:
                    bottleCount.text = item.Value.ToString();
                    break;
                case ElementTypes.Branch:
                    branchCount.text = item.Value.ToString();
                    break;
                case ElementTypes.Cake:
                    cakeCount.text = item.Value.ToString();
                    break;
                case ElementTypes.Carrot:
                    carrotCount.text = item.Value.ToString();
                    break;
                case ElementTypes.Cherry:
                    cherryCount.text = item.Value.ToString();
                    break;
                case ElementTypes.Cup:
                    cupCount.text = item.Value.ToString();
                    break;
                case ElementTypes.Cupcake:
                    cupcakeCount.text = item.Value.ToString();
                    break;
                case ElementTypes.Diamond:
                    diamondCount.text = item.Value.ToString();
                    break;
                case ElementTypes.Donut:
                    donutCount.text = item.Value.ToString();
                    break;
                case ElementTypes.IceCream:
                    iceCreamCount.text = item.Value.ToString();
                    break;
                default:
                    break;
            }
        }


        viewChangerActions.SetActive(false);
        elementCountViewPanel.SetActive(true);
        saveButton.SetActive(true);
    }

    public void ResetPosition()
    {
        elementSpawnerEditor.gameObject.transform.rotation = Quaternion.identity;
    }
}
