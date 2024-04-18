using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishingBlock : MonoBehaviour
{
    [SerializeField] private GameObject platform;
    [SerializeField] private float timeToVanish;
    [SerializeField] private float respawnTime;

    private GameObject currentPlatform;
    private float counter = 0f;
    public bool platformTouched { get; set; }
    public bool platformDestroyed { get; set; }

    private void Start()
    {
        platform.SetActive(false);
        currentPlatform = Instantiate(platform);
        currentPlatform.SetActive(true);
        currentPlatform.transform.position = transform.position;
    }

    private void Update()
    {
        if (platformTouched && currentPlatform != null)
        {
            counter += Time.deltaTime;
            currentPlatform.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, (float)(1f - (counter / timeToVanish)));
            if (counter > timeToVanish)
            {
                Destroy(currentPlatform, 0.0f);
                platformDestroyed = true;
                platformTouched = false;
                currentPlatform = null;
                counter = 0f;
            }
        }
        if (platformDestroyed)
        {
            counter += Time.deltaTime;
            if (counter > respawnTime)
            {
                currentPlatform = Instantiate(platform);
                currentPlatform.SetActive(true);
                currentPlatform.transform.position = transform.position;
                platformDestroyed = false;
                counter = 0f;
            }
        }
    }


}
