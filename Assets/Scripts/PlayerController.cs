using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject pistolObj;
    public GameObject bulletQuadPrefab;
    public GameObject gameOverCanvas;
    public AudioClip fireClip;
    public float playerLife = 100;
    public int maxAmmo = 32;
    public float weaponDamage;

    private GameObject gameController;
    private Animator animPLayer;
    private AudioSource audioSrcPlayer;
    private int maxCurrentAmmo = 12;
    private bool playerDead;
    private int totalAmmo;
    private int currentAmmo;
    private bool reloading;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        animPLayer = GetComponent<Animator>();
        audioSrcPlayer = GetComponent<AudioSource>();
        totalAmmo = maxAmmo;
        currentAmmo = maxCurrentAmmo;
    }

    void Update()
    {
        CharacterShoot();
    }

    private void CharacterShoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            Debug.DrawRay(transform.position, fwd * 50, Color.green);
            RaycastHit objectHit;
            if (Physics.Raycast(pistolObj.transform.position, transform.forward, out objectHit, 50) && !reloading)
            {
                if (objectHit.transform.tag == "Enemie")
                {
                    objectHit.transform.GetComponent<ZombieAiScript>().ZombieHitted(weaponDamage);
                }
                else
                {
                    GameObject bullet = Instantiate(bulletQuadPrefab, objectHit.point + objectHit.normal * 0.01f, Quaternion.FromToRotation(Vector3.forward, -objectHit.normal));
                    Destroy(bullet, 8f);
                }
            }

            if (currentAmmo > 0 && !reloading)
            {
                audioSrcPlayer.PlayOneShot(fireClip);
                currentAmmo--;
                totalAmmo--;
                gameController.GetComponent<UIScript>().SetAmmo(currentAmmo, totalAmmo);
            }
            if(currentAmmo == 0 && totalAmmo > 0)
            {
                reloading = true;
                Invoke("ReloadingAmmo", 3f);
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && !reloading && totalAmmo > 0 && currentAmmo < maxCurrentAmmo)
        {
            reloading = true;
            Invoke("ReloadingAmmo", 3f);
        }
    }

    private void ReloadingAmmo()
    {
        if (totalAmmo > maxCurrentAmmo)
        {
            gameController.GetComponent<UIScript>().ReloadAmmo(maxCurrentAmmo, totalAmmo);
            currentAmmo = maxCurrentAmmo;
        }
        else
        {
            gameController.GetComponent<UIScript>().ReloadAmmo(totalAmmo, totalAmmo);
            currentAmmo = totalAmmo;
        }
        reloading = false;
    }

    public void playerGetHit(float damage)
    {
        if (!playerDead)
        {
            playerLife -= damage;
            gameController.GetComponent<UIScript>().SetLifeBar(playerLife);
            animPLayer.SetTrigger("Hitted");
            if (playerLife <= 0)
            {
                playerDead = true;
                gameOverCanvas.SetActive(true);
                animPLayer.SetTrigger("Dead");
                Invoke("GoToMenu", 3f);
            }
        }
    }

    public void PlusHealth(float plusHealth)
    {
        playerLife += plusHealth;
        if (playerLife > 100)
        {
            playerLife = 100;
        }
        gameController.GetComponent<UIScript>().SetLifeBar(playerLife);
    }

    public void PlusAmmo(int plusAmmo)
    {
        totalAmmo += plusAmmo;
        gameController.GetComponent<UIScript>().SetAmmo(currentAmmo, totalAmmo);
    }

    private void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
