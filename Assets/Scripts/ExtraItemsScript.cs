using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraItemsScript : MonoBehaviour
{
    public GameObject lifeBoxPrefab;
    public float lifeBoxPlus;
    public GameObject ammoBoxPrefab;
    public int ammoBoxPlus;
    public AudioClip catchItemClip;
    public GameObject particlePlusLife;

    void Start()
    {

    }

    void Update()
    {

    }

    public void DropRandomItem(Vector3 pos)
    {
        var rand = Random.Range(0, 2);
        if (rand == 0) Instantiate(lifeBoxPrefab, pos, Quaternion.Euler(-90, 0, 0));
        else if (rand == 1) Instantiate(ammoBoxPrefab, pos, Quaternion.Euler(-90, 0, 0));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "LifeBox")
        {
            GetComponent<PlayerController>().PlusHealth(lifeBoxPlus);
            GetComponent<AudioSource>().PlayOneShot(catchItemClip);
            var particles = Instantiate(particlePlusLife, transform.position + Vector3.up, Quaternion.Euler(0, 0, 0), transform);
            Destroy(particles, 3f);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "AmmoBox")
        {
            GetComponent<PlayerController>().PlusAmmo(ammoBoxPlus);
            GetComponent<AudioSource>().PlayOneShot(catchItemClip);
            Destroy(collision.gameObject);
        }
    }
}
