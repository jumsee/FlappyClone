using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Tube : MonoBehaviour
{
    public float moveSpeed;
    public float minX;
    public float spawnPosX;
    public GameObject upperTube;
    public GameObject lowerTube;
    private bool playerPassed = false;

    // Start is called before the first frame update
    private void Awake()
    {
        transform.position = new Vector3(spawnPosX, 0, 1);
        float tubeDist = Mathf.Max(GameManager.instance.tubeDistance, GameManager.instance.minTubeDistance);
        float lowerTubeY = Random.Range(2, 8);

        lowerTube.transform.position = new Vector3(spawnPosX, lowerTubeY + 1.5F, 1);
        upperTube.transform.position = new Vector3(spawnPosX,
            lowerTubeY + tubeDist + 1.5F, 1);
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameManager.instance.playerController.isPaused)
        {
            return;
        }

        Vector3 transformPosition = transform.position;
        if (transformPosition.x <= minX)
        {
            Destroy(this);
            GameManager.instance.tubeDistance -= 0.2F;
        }
        else
        {
            transformPosition = new Vector3(transformPosition.x - moveSpeed * Time.deltaTime, transformPosition.y,
                transformPosition.z);
            transform.position = transformPosition;
        }

        if (!playerPassed && transform.position.x <= GameManager.instance.playerController.transform.position.x)
        {
            playerPassed = !playerPassed;
            GameManager.instance.IncrementScore();
        }
    }
}