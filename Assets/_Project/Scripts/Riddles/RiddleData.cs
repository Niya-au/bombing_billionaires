using UnityEngine;

[CreateAssetMenu(fileName = "NewRiddle", menuName = "Riddles/Riddle")]
public class RiddleData : ScriptableObject
{
    public string question;
    public string correctAnswer;
}