using UnityEngine;
using System.Collections;
using TMPro;

    public class TextConsoleSimulator : MonoBehaviour
    {
#if DEBUG
        private TMP_Text _textComponent;
        private int _visibleCount;
        private int _totalVisibleCharacters;
        private Coroutine _revealCharactersCoroutine;
        private void Awake()
        {
            _textComponent = gameObject.GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            _revealCharactersCoroutine = StartCoroutine(RevealCharacters(_textComponent));
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
        }

        private void OnDisable()
        {
            StopCoroutine(_revealCharactersCoroutine);
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
        }


        // Event received when the text object has changed.
        private void ON_TEXT_CHANGED(Object obj)
        {
            _totalVisibleCharacters = _textComponent.textInfo.characterCount;
        }


        /// <summary>
        /// Method revealing the text one character at a time.
        /// </summary>
        /// <returns></returns>
        private IEnumerator RevealCharacters(TMP_Text textComponent)
        {
            textComponent.ForceMeshUpdate();
            while (true)
            {
                if (_visibleCount <= _totalVisibleCharacters)
                {
                    textComponent.maxVisibleCharacters = _visibleCount;
                    _visibleCount += 1;
                }

                yield return null;
            }
        }

        
        /// <summary>
        /// 清屏操作
        /// </summary>
        public void ClearScreen()
        {
            _visibleCount = 0;
            _textComponent.text = "";
        }

        /// <summary>
        /// 清屏操作
        /// </summary>
        public void AddText(string Text)
        {
            _textComponent.text = _textComponent.text + $"\n{Text}";
        }
        
#endif
    }