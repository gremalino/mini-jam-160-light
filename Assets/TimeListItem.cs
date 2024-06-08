using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeListItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _labelText;
    [SerializeField] private TMP_Text _timeDiffText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private Image _activeHighlight;
    
    public void SetHighlightActive(bool isActive)
    {
        _activeHighlight.DOFade(isActive ? 1 : 0, 0.3f).SetEase(Ease.OutExpo);
    }
}
