using UnityEngine.UI;
using Armariya.UI;
using UniRx;

public class ProgressBar : ProgressBarBase {

  private readonly ReactiveProperty<string> _stubFileName = new ReactiveProperty<string>();
  private readonly ReactiveProperty<float> _stubProgress = new ReactiveProperty<float>();

  private bool _isFileNameOrProgressChanged;
  
  private void Start() {
    SubscribeToUpdateFileName();
    SubscribeToUpdateProgress();
  }

  private void SubscribeToUpdateFileName() {
    _stubFileName
      .TakeUntilDestroy(this)
      .Where(fileName => !string.IsNullOrEmpty(fileName))
      .Do(_ => _isFileNameOrProgressChanged = true)
      .Subscribe(fileName => FileName = fileName);
      
  }

  private void SubscribeToUpdateProgress() {
    _stubProgress
      .TakeUntilDestroy(this)
      .Do(_ => _isFileNameOrProgressChanged = true)
      .Subscribe(progress => Progress = progress);
  }

  protected sealed override void UpdateLoadingText(Text text, string fileName, float progress) {
    if (_isFileNameOrProgressChanged) {
      base.UpdateLoadingText(text: text, fileName: fileName, progress: progress);
    }
  }
}
