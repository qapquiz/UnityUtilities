using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using UniRx;


public enum HttpMethod {
	GET,
	POST,
	PUT,
	HEAD
}


public class Service {
	protected readonly string m_baseUrl;

	public Service(string baseUrl) {
		m_baseUrl = baseUrl;
	}

	public UniRx.IObservable<T> Get<T>(string path, HttpMethod httpMethod) {
		return Observable.FromMicroCoroutine<T>((observer, cancellationToken) => Requester<T>(path, httpMethod, observer, cancellationToken));
	}

	public UniRx.IObservable<T[]> GetArray<T>(string path, HttpMethod httpMethod) {
		return Observable.FromMicroCoroutine<T[]>((observer, cancellationToken) => RequesterArray<T>(path, httpMethod, observer, cancellationToken));
	}

	public UniRx.IObservable<string> Create(string path, HttpMethod httpMethod, string requestJsonString) {
		byte[] bytesFormData = Encoding.UTF8.GetBytes(requestJsonString);
		return Observable.FromMicroCoroutine<string>((observer, cancellationToken) => Creator(path, httpMethod, bytesFormData, observer, cancellationToken));
	}

	public UniRx.IObservable<string> Update(string path, HttpMethod httpMethod, WWWForm formData) {
		return Observable.FromMicroCoroutine<string>((observer, cancellationToken) => Updater(path, httpMethod, formData, observer, cancellationToken));
	}

	private IEnumerator Requester<T>(string path, HttpMethod httpMethod, UniRx.IObserver<T> observer, CancellationToken cancellationToken) {
		string url = $"{m_baseUrl}{path}";
		
		switch (httpMethod)
		{
				case HttpMethod.GET:
					using (UnityWebRequest www = UnityWebRequest.Get(url)) {
						www.SendWebRequest();
						while (!www.isDone && !cancellationToken.IsCancellationRequested) {
							yield return null;
						}

						if (cancellationToken.IsCancellationRequested) {
							yield break;
						}

						if (www.isNetworkError || www.isHttpError) {
							observer.OnError(new Exception(www.error));
						} else {
							if (www.responseCode == 200) {
								observer.OnNext(JsonUtility.FromJson<T>(www.downloadHandler.text));
								observer.OnCompleted();
							} else {
								observer.OnNext(default(T));
								observer.OnCompleted();
							}
						}
					}
					break;
				
				default:
					break;
		}
	}

	private IEnumerator RequesterArray<T>(string path, HttpMethod httpMethod, UniRx.IObserver<T[]> observer, CancellationToken cancellationToken) {
		string url = $"{m_baseUrl}{path}";
		
		switch (httpMethod)
		{
				case HttpMethod.GET:
					using (UnityWebRequest www = UnityWebRequest.Get(url)) {
						www.SendWebRequest();
						while (!www.isDone && !cancellationToken.IsCancellationRequested) {
							yield return null;
						}

						if (cancellationToken.IsCancellationRequested) {
							yield break;
						}

						if (www.isNetworkError || www.isHttpError) {
							observer.OnError(new Exception(www.error));
						} else {
							if (www.responseCode == 200) {
								observer.OnNext(JsonHelper.FromJsonArray<T>(www.downloadHandler.text));
								observer.OnCompleted();
							} else {
								observer.OnNext(default(T[]));
								observer.OnCompleted();
							}
						}
					}
					break;
				
				default:
					break;
		}
	}

	private IEnumerator Updater(string path, HttpMethod httpMethod, WWWForm formData, UniRx.IObserver<string> observer, CancellationToken cancellationToken) {
		string url = $"{m_baseUrl}{path}";
		
		switch (httpMethod) {
			case HttpMethod.POST: 
				using (UnityWebRequest www = UnityWebRequest.Post(url, formData)) {
					www.SendWebRequest();
					while (!www.isDone && !cancellationToken.IsCancellationRequested) {
						yield return null;
					}

					if (cancellationToken.IsCancellationRequested) {
						yield break;
					}

					if (www.isNetworkError || www.isHttpError) {
						observer.OnError(new Exception(www.error));
					} else {
						observer.OnNext(www.downloadHandler.text);
						observer.OnCompleted();
					}
				}
				break;
			default:
				break;
		}
	}

	private IEnumerator Creator(string path, HttpMethod httpMethod, byte[] formData, UniRx.IObserver<string> observer, CancellationToken cancellationToken) {
		string url = $"{m_baseUrl}{path}";
		
		switch (httpMethod) {
			case HttpMethod.PUT:
				using (UnityWebRequest www = UnityWebRequest.Put(url, formData)) {
					www.SendWebRequest();
					while (!www.isDone && !cancellationToken.IsCancellationRequested) {
						yield return null;
					}

					if (cancellationToken.IsCancellationRequested) {
						yield break;
					}

					if (www.isNetworkError || www.isHttpError) {
						observer.OnError(new Exception(www.error));
					} else {
						observer.OnNext(www.downloadHandler.text);
						observer.OnCompleted();
					}
				}
				break;
			default:
				break;
		}
	}
}