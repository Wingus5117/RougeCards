using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public bool isPlayerTurn;
    public bool isEnemyTurn;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isPlayerTurn)
            {
                isPlayerTurn = true;
                isEnemyTurn = false;

                // get all Player units and tell them that there is a new turn for them
                GameObject[] playerUnits = GameObject.FindGameObjectsWithTag("PlayerUnit");
                foreach (GameObject playerunits in playerUnits)
                { 
                    PlayerUnit playerUnit = playerunits.GetComponent<PlayerUnit>();
                    playerUnit.NewPlayerTurn();
                }
            }
            else
            {
                isPlayerTurn = false;
                isEnemyTurn = true;
                
                // get all enemy units and tell them that there is a new turn for them
                GameObject[] enemyUnits = GameObject.FindGameObjectsWithTag("EnemyUnit");
                foreach (GameObject enemyunits in enemyUnits)
                {
                    EnemyUnit enemyUnit = enemyunits.GetComponent<EnemyUnit>();
                    enemyUnit.NewEnemyTurn();
                }
            }
        }
    }
}
