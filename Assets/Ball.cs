using System;
using UnityEngine;


public class Ball : MonoBehaviour
{
    // Material of the Ball
    public Material material;

    // true if program should use real diff equation (d^2 θ / d^2 t = -g*sin(θ)/l),
    // false if program should use small angle approximation sin(θ)=θ, thus simplifying the differential equation to:
    // theta(t)= theta_0 * cos(sqrt (L/g) * theta)
    public bool useRealPendulum = true;
    // Position of the point to swing around
    public Vector3 pivotPos;
    // gravitational constant of earths acceleration
    public static readonly double g = 9.81f; // m/s^2
    // starting angle at which to drop the pendulum from
    public double startingAngle; // radians
    // the coefficient of air resistance
    public double airResistanceCoeff = 0.2f;
    // the starting velocity at which to drop the pendulum from
    public double startingVelocity;
    // angles, velocity, and acceleration updated every deltaT
    private double currentAngle = 0; // radians
    private double currentVelocity = 0; // radians/s
    private double currentAccel = 0; //radians/s^2

    // Time steps the simulation takes
    private double deltaT = 0.002; // s

    // time since pendulum is enabled
    private double time = 0;

    void OnEnable()
    {
        currentAngle = startingAngle;
        currentVelocity = startingVelocity;
    }

    // returns angular acceleration of pendulum (degrees/s^2)
    // angle - the previous angle, or in the case of the simple pendulum, the time
    // velocity - the current velocity of the 
    double DifferentialEquation(double angle, double velocity)
    {
        // length of the "string", or the distance from the weight to the point of rotation
        double length = Vector3.Distance(transform.position, pivotPos);
        if (useRealPendulum)
            return -(g * Math.Sin(angle) / length) - airResistanceCoeff * velocity;

        return startingAngle * Math.Cos(Math.Sqrt(length / g) * angle);
    }
    double GetAngle(double time)
    {
        // The equation for the simple pendulum doesnt require stepping through time
        if (!useRealPendulum)
            return DifferentialEquation(time, 0);
        currentAngle = startingAngle;
        currentVelocity = 0;
        currentAccel = 0;
        for (double i = 0; i <= time; i += deltaT)
        {
            currentAccel = DifferentialEquation(currentAngle, currentVelocity);
            currentVelocity += currentAccel * deltaT;
            currentAngle += currentVelocity * deltaT;

        }

        return currentAngle;
    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        // Destroys old line, then draws line from pivot point to the position of the ball
        Destroy(GameObject.Find("Line" + gameObject.name));
        LineRenderer lineRenderer = new GameObject("Line" + gameObject.name).AddComponent<LineRenderer>();
        lineRenderer.material = material;
        lineRenderer.gameObject.tag = "line";
        lineRenderer.startColor = UnityEngine.Color.blue;
        lineRenderer.endColor = UnityEngine.Color.blue;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;

        //For drawing line in the world space, provide the x,y,z values
        lineRenderer.SetPosition(0, pivotPos); //x,y and z position of the starting point of the line
        lineRenderer.SetPosition(1, transform.position); //x,y and z position of the end point of the line

        transform.position = PointOnCircle(3, -(float)GetAngle(time) + Mathf.PI / 2, pivotPos);

    }

    Vector3 PointOnCircle(float radius, float angle, Vector3 pointToRotateAround)
    {
        float x = (pointToRotateAround.x + radius * Mathf.Cos(angle));
        float y = (pointToRotateAround.y - radius * Mathf.Sin(angle));

        return new Vector3(x, y, 0);
    }
}
