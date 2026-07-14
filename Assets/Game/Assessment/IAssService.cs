using SUG.Essentials;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Injectable] public interface IAssService
{
    public int currIdx { get; set; }
    public List<QuestionData> questionList { get; set; }
    public QuestionData GetCurrQuestion();
}
