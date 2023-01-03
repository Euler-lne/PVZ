using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private AudioSource audioSource;

    private Dictionary<string, AudioClip> dicAudio;//字典文件，key为文件路径，value为音频资源
    private void Awake()
    {
        instance = this;
        //初始化
        audioSource = GetComponent<AudioSource>();
        dicAudio = new Dictionary<string, AudioClip>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    //辅助函数，加载音频，需要将音频放到Resoures文件夹下
    private AudioClip LoadAudio(string path)
    {
        return (AudioClip)Resources.Load(path);
    }
    //辅助函数：加载音频，并缓存到dicAudio中，避免重复加载
    private AudioClip GetAudio(string path)
    {
        if(!dicAudio.ContainsKey(path))
        {
            dicAudio[path] = LoadAudio(path);
        }
        return dicAudio[path];
    }
    //背景音乐，一般只有一首
    public void PlayBGM(string name,float volume = 1.0f,bool loop = true)
    {
        audioSource.Stop();
        audioSource.clip = GetAudio(name);
        audioSource.Play();
        audioSource.volume = volume;
        audioSource.loop = loop;
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }

    //音效可以叠加
    public void PlaySound(string path , float volume = 1f)
    {
        audioSource.PlayOneShot(GetAudio(path),volume);
        // audioSource.volume = volume;
    }
    //3D效果：把自身的audioSource传入，使用挂载在物体自身的audioSource播放，以达到音效远近的效果
    public void PlaySound(AudioSource audioSource,string path,float volume = 1f)
    {
        audioSource.PlayOneShot(GetAudio(path),volume);
        // audioSource.volume = volume;
    }
}
