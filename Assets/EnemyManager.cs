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
    public float exp;
    public float maxExp;

    public GameObject enemyHPBar;
    public GameObject playerHPBar;

    public Text enemyDamageText;
    public Text playerDamageText;
    public Text stateText;
    public Text playerLevelText;
    float nextLevel;

    public float playerAttack;
    public float playerDefense;
    public float playerSpeed;
    public float playerMaxHP;
    public float playerHP;

    public float enemyAttack;
    public float enemyDefense;
    public float enemySpeed;
    public float enemyMaxHP;
    public float enemyHP;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public IEnumerator Fight(int kind)
    {
        for(int x = 0; x < 20; x++)
        {
            back.transform.localScale = new Vector3(0.25f * x, 0.5f * x, 0);
            yield return null;
        }
        enemy.GetComponent<SpriteRenderer>().enabled = true;
        enemy.GetComponent<SpriteRenderer>().sprite = EnemySprite[kind];

        enemyAttack = Mathf.FloorToInt(2 * Mathf.Pow(1.1f, tm.stairNum - 1));
        enemyLevel = tm.stairNum;
        enemyMaxHP = 10 * Mathf.Pow(1.1f, enemyLevel - 1);
        enemyHP = enemyMaxHP;

        if (kind == 1)
        {
            enemyAttack = Mathf.FloorToInt(enemyAttack * 1.5f);
            enemyDefense = Mathf.FloorToInt(enemyDefense * 1.5f);
            enemySpeed = Mathf.FloorToInt(enemySpeed * 1.5f);
            enemyHP = Mathf.FloorToInt(enemyHP * 2f);
            enemyMaxHP = Mathf.FloorToInt(enemyMaxHP * 2f);
        }

        enemyHPBar.SetActive(true);
        playerHPBar.SetActive(true);
        enemyHPBar.transform.localPosition = new Vector2(-0.15f + 0.15f * enemyHP / enemyMaxHP, 0.1f);
        enemyHPBar.transform.localScale = new Vector2(3 * enemyHP / enemyMaxHP, 0.2f);
        playerHPBar.transform.localPosition = new Vector2(-0.15f + 0.15f * playerHP / playerMaxHP, -0.2f);
        playerHPBar.transform.localScale = new Vector2(3 * playerHP / playerMaxHP, 0.2f);

        float speedLimit = enemySpeed + playerSpeed;
        float playerSpeedGauge = 0;
        float enemySpeedGauge = 0;

        yield return new WaitForSeconds(0.3f);

        while (enemyHP > 0 && playerHP > 0)
        {
            playerSpeedGauge += playerSpeed * 0.1f;
            enemySpeedGauge += enemySpeed * 0.1f;
            if (playerSpeedGauge >= speedLimit)
            {
                PlayerAttack();
                playerSpeedGauge = 0;
                yield return new WaitForSeconds(0.3f);
                continue;
            }
            if (enemySpeedGauge >= speedLimit && enemyHP > 0)
            {
                EnemyAttack();
                enemySpeedGauge = 0;
                yield return new WaitForSeconds(0.3f);
                continue;
            }
        }
        enemyHPBar.SetActive(false);
        playerHPBar.SetActive(false);

        StartCoroutine("LevelManage",enemyLevel);

        enemy.GetComponent<SpriteRenderer>().enabled = false;
        for (int x = 20; x >= 0; x--)
        {
            back.transform.localScale = new Vector3(0.25f * x, 0.5f * x, 0);
            yield return null;
        }
        tm.fighting = false;
    }


    void EnemyAttack()
    {
        StartCoroutine("Attacked", enemy);
        if (enemyAttack - playerDefense > 1)
        {
            if (playerHP - enemyAttack <= 0) playerHP = 0;
            if (playerHP - enemyAttack > 0) playerHP -= enemyAttack - playerDefense;
            playerDamageText.text = (enemyAttack - playerDefense).ToString("");
        }
        if (enemyAttack - playerDefense <= 1)
        {
            playerHP -= 1;
            playerDamageText.text = 1.ToString("");
        }

        playerDamageText.gameObject.GetComponent<MomentText>().StartCoroutine("Moment");
        playerHPBar.transform.localPosition = new Vector2(-0.15f + 0.15f * playerHP / playerMaxHP, -0.2f);
        playerHPBar.transform.localScale = new Vector2(3 * playerHP / playerMaxHP, 0.2f);
    }

    void PlayerAttack()
    {
        StartCoroutine("Damaged", enemy);
        if(playerAttack - enemyDefense > 1)
        {
            if (enemyHP - playerAttack <= 0) enemyHP = 0;
            if (enemyHP - playerAttack > 0) enemyHP -= playerAttack - enemyDefense;
            enemyDamageText.text = (playerAttack - enemyDefense).ToString("");
        }
        if (playerAttack - enemyDefense <= 1)
        {
            enemyHP -= 1;
            enemyDamageText.text = 1.ToString("");
        }

        enemyDamageText.gameObject.GetComponent<MomentText>().StartCoroutine("Moment");
        enemyHPBar.transform.localPosition = new Vector2(-0.15f + 0.15f * enemyHP / enemyMaxHP, 0.1f);
        enemyHPBar.transform.localScale = new Vector2(3 * enemyHP / enemyMaxHP, 0.2f);
    }


    IEnumerator Damaged(GameObject obj)
    {
        Vector2 startPos = obj.transform.localPosition;
        int d = 1;
        float w = 0.1f;
        for(int x = 0; x < 4; x++)
        {
            d *= -1;
            w *= 0.8f;
            obj.transform.localPosition = startPos + new Vector2(0.1f * w * d, 0);
            yield return new WaitForSeconds(0.03f);
        }
        obj.transform.localPosition = startPos;
    }

    IEnumerator Attacked(GameObject obj)
    {
        Vector2 startPos = obj.transform.localPosition;
        float w = 0.05f;
        for (int x = 0; x < 10; x++) 
        {
            w *= 0.8f;
            obj.transform.localPosition = startPos + new Vector2(0, -w);
            yield return null;
        }
        obj.transform.localPosition = startPos;
    }

    IEnumerator LevelManage(float plus)
    {
        maxExp = playerLevel * 2;
        while(exp + plus >= maxExp)
        {
            exp = 0;
            plus -= maxExp;
            playerLevel += 1;
            playerLevelText.text = playerLevel.ToString("");
            yield return new WaitForSeconds(0.05f);
            maxExp = playerLevel * 2;

            playerAttack = Mathf.Floor(5 * Mathf.Pow(1.1f, playerLevel));
            playerMaxHP = Mathf.Floor(10 * Mathf.Pow(1.1f, playerLevel));
            playerHP = playerMaxHP;
        }
        if (exp + plus < maxExp) exp += plus;
    }
}
