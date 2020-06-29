using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AgentController : MonoBehaviour
{
    public NavMeshAgent[] agents;
    public Transform destination;

    private void Update()
    {
        SetDestination();
    }

    public void SetDestination()
    {
        for (int i = 0; i < agents.Length; i++)
        {
            agents[i].SetDestination(destination.position);
        }
       
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(AgentController))]
public class AgentControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AgentController myScript = (AgentController)target;
        if (GUILayout.Button("Set new destination"))
        {
            myScript.SetDestination();
        }

    }
}
#endif
