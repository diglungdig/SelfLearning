using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsSimulationPlacement : MonoBehaviour {

    public int maxIterations = 1000;
	


    [ContextMenu("Run Simulation")]
    public void RunSimulation()
    {
        Physics.autoSimulation = false;
        for (int i = 0; i < maxIterations; i++)
        {
            Physics.Simulate(Time.fixedDeltaTime);
        }
        Physics.autoSimulation = true;




    }



}
