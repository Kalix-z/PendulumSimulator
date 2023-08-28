using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public GameObject pendulum;

    public Text startingAngleText;
    public Text startingVelocityText;
    public Text startingAirResistanceText;

    public Slider startingAngleSlider;
    public Slider startingVelocitySlider;
    public Slider startingAirResistanceSlider;

    public void onStartingAngleChange()
    {
        startingAngleText.text = "Starting Angle: " + startingAngleSlider.value;
    }

    public void onStartingVeloChange()
    {
        startingVelocityText.text = "Starting Velocity: " + Math.Round(startingVelocitySlider.value, 2);
    }

    public void onStartingAirResistanceChange()
    {
        startingAirResistanceText.text = "Starting Air Resistance Coefficient: " + Math.Round(startingAirResistanceSlider.value, 2);
    }

    public void OnStartPress()
    {
        pendulum.GetComponent<Ball>().startingAngle = startingAngleSlider.value * Math.PI / 180;
        pendulum.GetComponent<Ball>().airResistanceCoeff = startingAirResistanceSlider.value;
        pendulum.GetComponent<Ball>().startingVelocity = startingVelocitySlider.value;
        pendulum.SetActive(true);
        GameObject.Find("PreSimulation").SetActive(false);
    }

    public void OnBackPress()
    {
        print("t");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
