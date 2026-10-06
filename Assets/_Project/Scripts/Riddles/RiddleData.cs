using UnityEngine;
//helleo
[CreateAssetMenu(fileName = "NewRiddle", menuName = "Riddles/Riddle")]
public class RiddleData : ScriptableObject
{
    public string question;

    public string[] answers = new string[4];

    public int correctAnswerIndex;
}