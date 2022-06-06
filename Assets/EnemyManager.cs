using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public TileManager tm;
    public GameObject back;
    public GameObject enemy;

    public Sprite[] EnemySprite;

    public int enemyLevel;
    public int playerLevel;

    public GameObject enemyHPBar;
    public GameObject playerHPBar;

    public Text enemyDamageText;
    public Text playerDamageText;
    public Text stateText;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public IEnumerator Fight()
    {
        for(int x = 0; x < 20; x++)
        {
            back.transform.localScale = new Vector3(0.25f * x, 0.5f * x, 0);
            yield return null;
        }
        enemy.GetComponent<SpriteRenderer>().enabled = true;
        enemy.GetComponent<SpriteRenderer>().sprite = EnemySprite[0];

        float maxEnemyHP = enemyLevel * 10;
        float maxPlayerHP = playerLevel * 10;
        float enemyHP = maxEnemyHP;
        float playerHP = maxPlayerHP;

        enemyHPBar.SetActive(true);
        playerHPBar.SetActive(true);

        enemyHPBar.transform.localPosition = new Vector2(0, 0.1f);
        enemyHPBar.transform.localScale = new Vector2(3, 0.2f);
        playerHPBar.transform.localPosition = new Vector2(0, -0.2f);
        playerHPBar.transform.localScale = new Vector2(3 , 0.2f);

        yield return new WaitForSeconds(1f);

        while (enemyHP > 0 || playerHP > 0)
        {
            enemyDamageText.text = (playerLevel * 3).ToString("");
            enemyDamageText.gameObject.GetComponent<MomentText>().StartCoroutine("Moment");
            if (enemyHP - playerLevel * 3 <= 0)
            {
                enemyHP = 0;
                enemyHPBar.transform.localPosition = new Vector2(-0.15f + 0.15f * enemyHP / maxEnemyHP, 0.1f);
                enemyHPBar.transform.localScale = new Vector2(3 * enemyHP / maxEnemyHP, 0.2f);
                break;
            }
            if (enemyHP - playerLevel * 3 > 0) 
            {
                enemyHP -= playerLevel * 3;
                enemyHPBar.transform.localPosition = new Vector2(-0.15f + 0.15f * enemyHP / maxEnemyHP, 0.1f);
                enemyHPBar.transform.localScale = new Vector2(3 * enemyHP / maxEnemyHP, 0.2f);
            }

            yield return new WaitForSeconds(0.5f);

            playerDamageText.text = (enemyLevel * 3).ToString("");
            playerDamageText.gameObject.GetComponent<MomentText>().StartCoroutine("Moment");
            if (playerHP - enemyLevel * 3 <= 0)
            {
                playerHP = 0;
                playerHPBar.transform.localPosition = new Vector2(-0.15f + 0.15f * playerHP / maxPlayerHP, -0.2f);
                playerHPBar.transform.localScale = new Vector2(3 * playerHP / maxPlayerHP, 0.2f);
                break;
            }
            if (playerHP - enemyLevel * 3 > 0)
            {
                playerHP -= enemyLevel * 3;
                playerHPBar.transform.localPosition = new Vector2(-0.15f + 0.15f * playerHP / maxPlayerHP, -0.2f);
                playerHPBar.transform.localScale = new Vector2(3 * playerHP / maxPlayerHP, 0.2f);
            }

            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.5f);
        enemyHPBar.SetActive(false);
        playerHPBar.SetActive(false);

        enemy.GetComponent<SpriteRenderer>().enabled = false;
        for (int x = 20; x >= 0; x--)
        {
            back.transform.localScale = new Vector3(0.25f * x, 0.5f * x, 0);
            yield return null;
        }
        tm.fighting = false;
    }
}
