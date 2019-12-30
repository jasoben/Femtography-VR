using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomQuarkChooser : MonoBehaviour
{
    public List<ParticlePair> theseParticlePairs;
    // Start is called before the first frame update
    void Start()
    {
        ChooseRandomQuark();
    }
    public void ChooseRandomQuark()
    {
        int randomPair = Random.Range(0, 3);
        transform.GetChild(0).GetChild(0).GetComponent<SetMaterial>().particle = theseParticlePairs[randomPair].particles[0];
        transform.GetChild(0).GetChild(1).GetComponent<SetMaterial>().particle = theseParticlePairs[randomPair].particles[1];
    }
}
[System.Serializable]
public class ParticlePair
{
    public Particle[] particles;
}
