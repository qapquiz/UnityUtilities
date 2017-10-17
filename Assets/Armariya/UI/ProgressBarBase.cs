using UnityEngine;
using UnityEngine.UI;

namespace Armariya.UI {
	public class ProgressBarBase : MonoBehaviour {
		[SerializeField] protected Image LoadingFilledImage;
		[SerializeField] protected Text LoadingText;

		protected string FileName;
		protected float Progress;

		private void Update() {
			UpdateFillAmount(image: LoadingFilledImage, progress: Progress);
			UpdateLoadingText(text: LoadingText, fileName: FileName, progress: Progress);
		}

		private void UpdateFillAmount(Image image, float progress) {
			if (image.fillAmount < progress) {
				image.fillAmount += progress * Time.deltaTime;
			}
		}

		protected virtual void UpdateLoadingText(Text text, string fileName, float progress) {
			text.text = $"กำลังดาวน์โหลด {fileName} {progress}%";
		}
	}
}