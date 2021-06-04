using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkField : MonoBehaviour
{
    GameObject player;
    public float shrinkSpeed;

    public GameObject growRoom;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        GetComponent<MeshRenderer>().enabled = true;
        player = other.gameObject;

        StartCoroutine(ShrinkCharacter());
    }

    IEnumerator ShrinkCharacter()
    {
        float shrinkage = 0;
        Vector3 playerSize = player.transform.localScale;
        float cameraField = Camera.main.fieldOfView;

        Vector3 roomSize = growRoom.transform.localScale;
        Vector3 roomFinalSize = growRoom.transform.localScale * 100;

        player.SetActive(false);
        growRoom.SetActive(true);

        while (true)
        {
            shrinkage += shrinkSpeed;

            growRoom.transform.localScale = Vector3.Lerp(roomSize, roomFinalSize, shrinkage);

            if (shrinkage > 1)
                yield break;

            yield return new WaitForEndOfFrame();
        }

    }
}
