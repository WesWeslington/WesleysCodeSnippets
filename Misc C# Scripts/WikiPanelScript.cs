using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WikiPanelScript : MonoBehaviour
{

    [SerializeField] private GameObject wikiPanelObject;

    //Players Instantaneous page settings
    [SerializeField] private int pageInt = 0;
    [SerializeField] private int enemyInt = 0;

    //The amount of panels per page
    [SerializeField] private List<EnemyPanel> enemyLeftPagePanels = new List<EnemyPanel>(5);

    //Name of the page
    [SerializeField] private List<string> mapNames;

    //Text of the page name
    [SerializeField] private TMP_Text mapText;

    //Nested List of a List of enemies
    [SerializeField] private List<EnemiesInArea> enemiesInArea;

    //Left and right buttons, 0 is left, 1 is right
    [SerializeField] private GameObject[] arrowButtons = new GameObject[2];

    [Header("Left Page")]

    [SerializeField] private List<EnemyPanel> enemyPanels;

    [Header("Right Page")]

    [SerializeField] private GameObject wikiDropRatePrefab;

    [SerializeField] private Transform itemDropRateParent;

    [SerializeField] private GameObject dropratesLockedObject;

    [SerializeField] private TMP_Text enemyNameText;

    [SerializeField] private TMP_Text enemyKillcountText;

    [SerializeField] private TMP_Text enemyStatsText;

    [SerializeField] private Image enemyImage;

    public Enemy _enemy;


    #region Left Page

    //Updating the left page automatically updates the right page to the first enemy on the list
    public void UpdateLeftPage(int _pageInt)
    {

        arrowButtons[0].SetActive(true);
        arrowButtons[1].SetActive(true);


        mapText.text = mapNames[_pageInt];

        pageInt = _pageInt;

        for(int i = 0; i < enemiesInArea[pageInt].enemyObjects.Count; i++)
        {
            Enemy _enemy = enemiesInArea[pageInt].enemyObjects[i].GetComponent<Enemy>();

            int _enemyID = _enemy.enemyID;
            int killcountAmount = PlayerStats.Instance.killcountList[_enemyID];

            //This line checks if the player has killed this enemy, if so it shows the color, otherwise its all black
            enemyPanels[i].enemyIcon.color = killcountAmount > 0 ? Color.white : Color.black;
            enemyPanels[i].enemyIcon.sprite = _enemy.enemyFaceSprite;
            enemyPanels[i].enemyName.text = killcountAmount <= 0 ? "????????" :_enemy.gameObject.name;

            if (i == 0)
            {
                UpdateRightPage(_enemy, i, killcountAmount);
            }
        }

        if (_pageInt == 0) {
            arrowButtons[0].SetActive(false);
        }
        else if(_pageInt==mapNames.Count-1)
        {
            arrowButtons[1].SetActive(false);
        }

    }

    #endregion

    #region Right Page

    public void OnSelectRightPageEnemy(int _int)
    {
        Enemy _enemy = enemiesInArea[pageInt].enemyObjects[_int].GetComponent<Enemy>();
        int _enemyID = _enemy.enemyID;
        int killcountAmount = PlayerStats.Instance.killcountList[_enemyID];
        UpdateRightPage(_enemy, _int, killcountAmount);
    }

    public void UpdateRightPage(Enemy _enemy, int enemyIndexInt, int killcountAmount)
    {
        ClearItemDrops();
        dropratesLockedObject.SetActive(killcountAmount < 25);
        EnemyStats _enemyStats = _enemy.GetComponent<EnemyStats>();

        enemyImage.color = killcountAmount > 0 ? Color.white : Color.black;
        enemyImage.sprite = _enemy.enemyFaceSprite;

        int _enemyID = _enemy.enemyID;
        enemyKillcountText.text = $"{killcountAmount.ToString("n0")} Kills";

        enemyNameText.text = killcountAmount <= 0 ? "????????" : enemyNameText.text = _enemy.gameObject.name;
        enemyStatsText.text = killcountAmount >= 10 ? $"{_enemyStats.maxHealth.GetDecrypted()} HP {_enemyStats.damage.GetValue()} ATK {_enemyStats.armor.GetValue()} DEF\n{_enemyStats.xpReward} exp {_enemyStats.moneyReward} monies" : $"You need 10 kills \n to see the stats";

        if (killcountAmount < 25) { return; }

        foreach(EnemyDrops _enemyDrop in _enemyStats.GetEnemyDrops())
        {
            AddItemIntoEnemyPanel(_enemyDrop.item, _enemyDrop.dropRate);
        }

        _enemyStats.GetEnemyDrops();
    }

    public void AddItemIntoEnemyPanel(Item _item, int rateInt) 
    {
     GameObject _newDropRateObject=  Instantiate(wikiDropRatePrefab, itemDropRateParent, true); 
        _newDropRateObject.GetComponent<WikiDropRateItem>().UpdateItem(_item, rateInt);
    }

    public void ClearItemDrops()
    {
        foreach(Transform _transform in itemDropRateParent)
        {
            Destroy(_transform.gameObject);
        }
    }

    #endregion

    #region 


    #endregion

    public void ToggleWikiPanel()
    {
        bool isEnabled = wikiPanelObject.activeSelf;

        if (!isEnabled)
        {
            wikiPanelObject.gameObject.SetActive(true);
            wikiPanelObject.GetComponentInParent<Image>().enabled = true;
            UpdateLeftPage(0);
        }
        else
        {
            wikiPanelObject.GetComponentInParent<Image>().enabled = false;
            wikiPanelObject.gameObject.SetActive(false);

        }

    }

   /// <summary>
   /// isNext boolean refers to if the button is the next or previous page button
   /// </summary>
   /// <param name="isNext"></param>
    public void OnNextPageButton(bool isNext)
    {
        if (isNext)
        {
            UpdateLeftPage(pageInt+1);
        }
        else
        {
            UpdateLeftPage(pageInt - 1);

        }
    }

}



[System.Serializable]
public class EnemyPanel
{
    public Image enemyIcon;
    public TMP_Text enemyName;
}

[System.Serializable]
public class EnemiesInArea
{

    public List<GameObject> enemyObjects;

}