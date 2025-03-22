using System;
using System.Collections;
using Junyoung;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    private static LoadingManager m_instance;
    public static LoadingManager Instance
    {
        get
        {
            if(m_instance is null)
            {
                LoadingManager loading_manager = FindAnyObjectByType<LoadingManager>();
                if(loading_manager is not null)
                {
                    m_instance = loading_manager;
                }
                else
                {
                    m_instance = Create();
                }
            }

            return m_instance;
        }
    }

    private static LoadingManager Create()
    {
        return Instantiate(Resources.Load<LoadingManager>("Loading UI"));
    }

    [SerializeField] private CanvasGroup m_canvas_group;
    [SerializeField] private TMP_Text m_loading_rate_label;
    [SerializeField] private TMP_Text m_tool_tip_label;
    [SerializeField] [TextArea] private string[] m_tool_tips;

    private string m_load_scene_name;
    private Action m_on_scene_load_action;

    private void Awake()
    {
        if(Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string scene_name, Action action = null)
    {
        EventBus.Publish(GameEventType.LOADING);

        gameObject.SetActive(true);
        
        SceneManager.sceneLoaded += OnSceneLoaded;

        m_load_scene_name = scene_name;

        m_tool_tip_label.text = m_tool_tips[UnityEngine.Random.Range(0, m_tool_tips.Length - 1)];

        StartCoroutine(LoadSceneProcess());
    }

    private IEnumerator LoadSceneProcess()
    {
        m_loading_rate_label.text = "0%";

        yield return StartCoroutine(Fade(true));

        AsyncOperation op = SceneManager.LoadSceneAsync(m_load_scene_name);
        op.allowSceneActivation = false;

        float process = 0f;
        while(!op.isDone)
        {
            yield return null;

            if(op.progress < 0.9f)
            {
                m_loading_rate_label.text = (op.progress * 100).ToString("F0") + "%";
            }
            else
            {
                process += Time.deltaTime;
                m_loading_rate_label.text = (Mathf.Lerp(0.9f, 1f, process) * 100).ToString("F0") + "%";

                if(m_loading_rate_label.text == "100%")
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if(arg0.name == m_load_scene_name)
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();

        m_on_scene_load_action?.Invoke();
    }

    private IEnumerator Fade(bool is_fade_in)
    {
        float process = 0f;

        if(!is_fade_in)
        {
            StartCoroutine(LateStart());
        }

        while(process < 1f)
        {
            process += Time.unscaledDeltaTime;

            m_canvas_group.alpha = is_fade_in ? Mathf.Lerp(0f, 1f, process) : Mathf.Lerp(1f, 0f, process);

            yield return null;
        }

        if(!is_fade_in)
        {
            gameObject.SetActive(false);
        }
    }
}
