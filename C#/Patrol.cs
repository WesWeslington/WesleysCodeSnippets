using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour, IPooledObject
{
    [SerializeField] private Animator enemyAnimator;
    public float currentSpeed;
    public float speed;
    public float ragingSpeed;
    private bool movingRight = true;
    //  [SerializeField] private Animator enemyAnimator;
    [SerializeField] private bool isProjectile = false;

    [SerializeField] private EnemyPalettes enemyColorPalettes;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRend;
    bool isRaging = false;

    private void Start()
    {
        currentSpeed = speed;
        ragingSpeed = speed * 2;

        if(!isProjectile)
        DecideEnemyDirection();
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.right * currentSpeed * Time.deltaTime);

    }

    public void OnObjectSpawn()
    {
        isRaging = false;

        if(!isProjectile)
        SetEnemyColors();

        enemyAnimator.speed = 1;
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "wall")
        {
            if (movingRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;

            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);

                movingRight = true;

            }
        }

        if(other.gameObject.tag == "pit")
        {
            enemyAnimator.speed = 2;
            RespawnEnemy();
            //sends enemy to a random spawn point
        }
    }

    private void RespawnEnemy()
    {
        if (isProjectile) { gameObject.SetActive(false);
            return;
        }
        isRaging = true;

        if (!isProjectile)
            SetEnemyColors();

        int _currentLevelInt = LevelManager.instance.currentLevelInt;
        Vector3 newSpawnPoint = LevelManager.instance.GetRandomEnemySpawnPoint();
        gameObject.transform.position = newSpawnPoint;

        gameObject.GetComponent<EnemyScript>().HealFull();
        currentSpeed = ragingSpeed;
    }

    public void DecideEnemyDirection()
    {
        int randomNumber = Random.Range(0, 2);
        if (randomNumber == 1)
        {
            movingRight = true;
            transform.eulerAngles = new Vector3(0, 0, 0);

        }
        else
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            movingRight = false;
        }
    }

    public void ResetSpeed()
    {
        currentSpeed = speed;
    }

    public void SetEnemyColors()
    {
        //if not raging 
        if (!skinnedMeshRend) { return; }
        List<Material> _enemyMaterials = new List<Material>();

        _enemyMaterials = GetEnemyMaterials(skinnedMeshRend.materials);

 
        if (!isRaging)
        {
            for (int i = 0; i < skinnedMeshRend.materials.Length; i++)
            {
                Material newMaterial = Instantiate(_enemyMaterials[i]);
                //make these foreaches into for loops

                newMaterial.SetColor("_BaseColor", enemyColorPalettes.normalEnemyColors[i]);
                skinnedMeshRend.materials[i].CopyPropertiesFromMaterial(newMaterial);

                Destroy(newMaterial);
            }
           

        }
        else
        {
            for (int i = 0; i < skinnedMeshRend.materials.Length; i++)
            {
                Material newMaterial = Instantiate(_enemyMaterials[i]);
                //make these foreaches into for loops

                newMaterial.SetColor("_BaseColor", enemyColorPalettes.ragingEnemyColors[i]);
                skinnedMeshRend.materials[i].CopyPropertiesFromMaterial(newMaterial);

                Destroy(newMaterial);
            }
        }
    }

    public List<Material> GetEnemyMaterials(Material[] _mats)
    {
        List<Material> _matsToReturn = new List<Material>();

        foreach(Material _mat in _mats)
        {
            _matsToReturn.Add(_mat);
        }

        return _matsToReturn;
    }
}