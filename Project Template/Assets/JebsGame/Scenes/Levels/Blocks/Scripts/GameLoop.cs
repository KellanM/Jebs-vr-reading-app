using UnityEngine;

public class GameLoop : MonoBehaviour
{
    public TMPro.TextMeshProUGUI timeLeftLabel, goalLabel, bestLabel;

    [Tooltip("How long we think is enough for the player to get one block (used to calculate round time)")]
    public float expectedBlockAchieveTime = 20;
    [Tooltip("The number of blocks the player needs to stack in order to win")]
    public int requiredStackCount = 6;

    private float roundStartTime = -1;

    public float roundTime { get { return expectedBlockAchieveTime * requiredStackCount; } }

    [Tooltip("How many frames to skip before checking win condition")]
    public int checkStacksEvery = 4;
    private int framesPassed;

    public UnityEngine.Events.UnityEvent onWin;
    public UnityEngine.Events.UnityEvent onLose;

    private int currentTallestStack;

    void Update()
    {
        float timeLeft = 0;

        if (roundStartTime >= 0)
        {
            float timePassed = Time.time - roundStartTime;
            timeLeft = roundTime - timePassed;

            if (framesPassed >= checkStacksEvery)
            {
                currentTallestStack = GetHighestStackCount();
                if (currentTallestStack >= requiredStackCount)
                {
                    EndRound();
                    onWin?.Invoke();
                }
                else if (timePassed >= roundTime)
                {
                    EndRound();
                    onLose?.Invoke();
                }
                framesPassed = 0;
            }
        }
        else
            currentTallestStack = 0;

        timeLeftLabel.text = Mathf.Round(timeLeft) + " s";
        goalLabel.text = requiredStackCount.ToString();
        bestLabel.text = currentTallestStack.ToString();

        framesPassed++;
    }

    public void StartRound()
    {
        roundStartTime = Time.time;
        framesPassed = 0;
    }
    public void EndRound()
    {
        roundStartTime = -1;
    }

    public int GetHighestStackCount()
    {
        var blocks = FindObjectsOfType<StackStability>();
        
        int largestStack = 0;
        foreach (var block in blocks)
        {
            int currentStackCount = 1;
            var nextUndercube = block.undercube;
            while (nextUndercube != null)
            {
                currentStackCount++;
                nextUndercube = nextUndercube.undercube;
            }
            largestStack = Mathf.Max(currentStackCount, largestStack);
        }

        return largestStack;
    }
}
