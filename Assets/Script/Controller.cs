using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Controller IController;

    public float GravitationalConstant;

    public GameObject PlanetGameObject;

    [Header("POT")]
    [Tooltip("Used to adjust the mass on a planet")]
    public float SystemMassPoTScale = 24;
    [Header("Time Scale")]
    [Tooltip("Used to make the time faster (only to see all the planets moving :P)")]
    public float TimeScale = 100;

    public List<Planet> ListPlanets = new List<Planet>();

    void Awake()
    {

        GravitationalConstant = 6.67f * Mathf.Pow(10, -11);
    }

    void Update()
    {
        Time.timeScale = TimeScale;
        if (Input.GetKeyDown(KeyCode.S))
            SpawnPlanet();
    }

    void SpawnPlanet()
    {
        GameObject newPlanet = Instantiate(PlanetGameObject);
    }

    public void AddPlanet(Planet x)
    {
        ListPlanets.Add(x);
    }

    public void RemovePlanet(Planet x)
    {
        ListPlanets.Remove(x);
    }
}
