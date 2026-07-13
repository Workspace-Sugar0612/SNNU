using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAssService
{
    public int currIdx { get; set; }
    public List<QuestionData> questionList { get; set; }
    public QuestionData GetCurrQuestion();
}
