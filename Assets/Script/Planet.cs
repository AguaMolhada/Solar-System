using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;
using UnityEngine;

using Random = UnityEngine.Random;

public class Planet : MonoBehaviour
{
    [Tooltip("Put the object that have the controller script attach")]
    
    private Controller _controller;

    public bool isSun;

    [Header("Mass Controller")]
    [Range(0,10)]
    [Tooltip("Mass bteween 0 and 10 unitys")]
    public float MassUnity  = 5.9722f;
    [Header("POT")]
    [Tooltip("Used to multiply the mass to a higher value")]
    public int PowerOfTen = 24;
    [Tooltip("Mass value obtained by MU*POT")]
    private float _mass;

    private Planet nearest;

    public Vector3 Velocity = Vector3.zero;

    public float GetMass {get { return _mass; }}

    private void Start()
    {
        if (_controller == null)
        {
            _controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller>();
        }

        if (!isSun)
        {
            transform.position = new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), Random.Range(-20, 20));
            MassUnity = Random.Range(1f, 10f);
            PowerOfTen = Random.Range(20, 26);
            transform.localScale = new Vector3(PowerOfTen/_controller.SystemMassPoTScale,PowerOfTen/_controller.SystemMassPoTScale,PowerOfTen/_controller.SystemMassPoTScale);
            GetComponent<Renderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f),
            Random.Range(0f, 1f));
            transform.name = Ultility.nameGenerator();
            Velocity = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        }
        _mass = MassUnity * Mathf.Pow(10, PowerOfTen - _controller.SystemMassPoTScale);
        _controller.AddPlanet(this);
    }

    private void Update()
    {
        CalculateForces();

    }

    private void CalculateForces()
    {
        Vector3 GForce = Vector3.zero;
        List<Planet> allPlanets = _controller.ListPlanets;

        foreach (Planet item in allPlanets)
        {
            Vector3 GForceDirection = (item.transform.position - transform.position).normalized;

            if (item != this)
            {
                GForce += GForceDirection * _controller.GravitationalConstant * 
                    ((_mass * item._mass) / Mathf.Pow(Vector3.Distance(transform.position, item.transform.position), 2));
            }
        }

        Vector3 acceleration = (GForce / _mass) + AceleracaoGravitade();
        Velocity += acceleration * Time.deltaTime;
        transform.localPosition += Velocity * Time.deltaTime;

    }

    private Vector3 AceleracaoGravitade()
    {
        Vector3 accelG = Vector3.zero;
        List<Planet> allPlanets = _controller.ListPlanets;
        nearest = allPlanets[0];
        if (nearest != null)
        {
            Vector3 direction = Vector3.zero;
            foreach (Planet item in allPlanets)
            {
                if (item != this)
                {
                    if (Vector3.Distance(transform.position, nearest.transform.position) > Vector3.Distance(transform.position, item.transform.position))
                    {
                        nearest = item;
                    }
                }
            }
            direction = (nearest.transform.position - transform.position).normalized;
            accelG = direction * (_controller.GravitationalConstant*_mass) / Mathf.Pow(Vector3.Distance(transform.position, nearest.transform.position), 2f);
        }
        return accelG;
    }

    private void OnCollisionEnter(Collision col)
    {
        var colTemp = col.gameObject.GetComponent<Planet>();
        if (_mass > colTemp.GetMass)
        {
            _mass += colTemp.GetMass;
            if (PowerOfTen < colTemp.PowerOfTen)
            {
                PowerOfTen = colTemp.PowerOfTen;
            }
            MassUnity = _mass / Mathf.Pow(10, PowerOfTen - _controller.SystemMassPoTScale);
            _controller.RemovePlanet(col.gameObject.GetComponent<Planet>());
            Destroy(col.gameObject);
            transform.localScale = new Vector3(PowerOfTen / _controller.SystemMassPoTScale, PowerOfTen / _controller.SystemMassPoTScale, PowerOfTen / _controller.SystemMassPoTScale);
        }
    }

}

public class Ultility
{

    public static string nameGenerator()
    {
        int pattern = Random.Range(0, 100);
        if (pattern >= 0 && pattern < 40)
        {
            return StartName() + NameEnd();
        }
        else if (pattern >= 40 && pattern < 50)
        {
            return NameVowel() + NameLink() + NameEnd();
        }
        else if (pattern >= 50 && pattern < 60)
        {
            return StartName() + NameVowel() + NameLink() + NameEnd();
        }
        else if (pattern >= 60 && pattern < 70)
        {
            return NameVowel() + NameLink() + NameVowel() + NameLink() + NameEnd();
        }
        else if (pattern >= 70 && pattern < 80)
        {
            return StartName() + NameVowel() + NameLink() + NameVowel() + NameLink() + NameEnd();
        }
        else if (pattern >= 80 && pattern < 90)
        {
            return NameVowel() + NameLink() + NameVowel() + NameLink() + NameVowel() + NameLink() + NameEnd();
        }
        else if (pattern >= 90 && pattern < 100)
        {
            return StartName() + NameVowel() + NameLink() + NameVowel() + NameLink() + NameVowel() + NameLink() +
                   NameEnd();
        }
        return null;
    }

    private static string StartName()
    {
        int a, b;
        string[,] startName = new string[3, 19]
        {
            {"B", "C", "D", "F", "G", "H", "J", "K", "L", "M", "N", "P", "R", "S", "T", "V", "W", "X", "Z"},
            {"B", "C", "Ch", "D", "F", "G", "K", "P", "Ph", "S", "T", "V", "Z", "R", "L", "", "", "", ""},
            {"Ch", "St", "Th", "Ct", "Ph", "Qu", "Squ", "Sh", "", "", "", "", "", "", "", "", "", "", ""}
        };
        a = Random.Range(0, startName.GetLength(0));
        for (int i = 0; i < 1; i++)
        {
            b = Random.Range(0, startName.GetLength(1));
            if (startName[a, b] == "")
            {
                i--;
            }
            else
            {
                return startName[a, b];
            }
        }
        return null;
    }

    private static string NameVowel()
    {
        int a, b;
        string[,] nameVowel = new string[2, 22]
        {
            {"a", "e", "i", "o", "u", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
            {
                "ao", "ae", "ai", "au", "ay", "eo", "ea", "ei", "ey", "io", "ia", "iu", "oa", "oe", "oi", "ou", "oy", "ui",
                "uo", "uy", "ee", "oo"
            }
        };
        a = Random.Range(0, nameVowel.GetLength(0));
        for (int i = 0; i < 1; i++)
        {
            b = Random.Range(0, nameVowel.GetLength(1));
            if (nameVowel[a, b] == "")
            {
                i--;
            }
            else
            {
                return nameVowel[a, b];
            }
        }
        return null;
    }

    private static string NameLink()
    {
        int a, b;
        string[,] nameLink = new string[3, 34]
        {
            {
                "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "r", "s", "t", "v", "w", "x", "z", "", "", "",
                "", "", "", "", "", "", "", "", "", "", "", ""
            },
            {
                "b", "c", "ch", "d", "f", "g", "k", "p", "ph", "r", "s", "t", "v", "z", "r", "l", "n", "", "", "", "", "",
                "", "", "", "", "", "", "", "", "", "", "", ""
            },
            {
                "ch", "rt", "rl", "rs", "rp", "rb", "rm", "st", "th", "ct", "ph", "qu", "tt", "bb", "nn", "mm", "gg", "cc",
                "dd", "ff", "pp", "rr", "ll", "vv", "ww", "ck", "squ", "lm", "sh", "wm", "wb", "wt", "lb", "rg"
            }
        };
        a = Random.Range(0, nameLink.GetLength(0));
        for (int i = 0; i < 1; i++)
        {
            b = Random.Range(0, nameLink.GetLength(1));
            if (nameLink[a, b] == "")
            {
                i--;
            }
            else
            {
                return nameLink[a, b];
            }
        }
        return null;
    }

    private static string NameEnd()
    {
        int a;
        string[] nameEnd = new string[42]
        {
            "id", "ant", "on", "ion", "an", "in", "at", "ate", "us", "oid", "aid", "al", "ark", "ork", "irk", "as",
            "os", "e", "o", "a", "y", "or", "ore", "es", "ot", "at", "ape", "ope", "el", "er", "ex", "ox", "ax", "ie",
            "eep", "ap", "op", "oop", "aut", "ond", "ont", "oth"
        };
        a = Random.Range(0, nameEnd.Length);
        return nameEnd[a];
    }
}