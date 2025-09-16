using System;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine; 

public class NetworkController : MonoBehaviour
{
    [SerializeField] private TextAsset _text;
    public async UniTask<Question[]> GetQuestions()
    {
        var questions = await RequestQuestions();
        return JsonConvert.DeserializeObject<Question[]>(questions);
    }
    private async UniTask<string> RequestQuestions()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        TextAsset textAsset = Resources.Load("Text") as TextAsset;
        return textAsset.text;
    }
}
