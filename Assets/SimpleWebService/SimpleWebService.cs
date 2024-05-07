/*
 * Simple Web Service
 * 
 * A simple HTTP request and response wrapper for interacting with RESTful JSON APIs. 
 * 
 * Built on top of UnityWebRequest and SimpleJSON.
 * 
 * https://github.com/maxfelker/SimpleWebService
 * 
*/

using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class SimpleWebService : MonoBehaviour
{

  public enum HttpMethod
  {
      GET,
      POST,
      PUT,
      DELETE,
      PATCH 
  }

  protected delegate void CallBack(JSONNode response);

  protected void Get(string url, CallBack callback) 
  {
    UnityWebRequest request = GenerateUnityWebRequest(url, HttpMethod.GET);
    StartCoroutine(SendRequest(request, callback));
  }

	protected void Post(string url, string json, CallBack callback)
  {
    UnityWebRequest request = GenerateUnityWebRequest(url, HttpMethod.POST, json);
    StartCoroutine(SendRequest(request, callback));
  }
	
  // Send a form post instead of just raw JSON POST above
	protected void PostForm(string url, Dictionary<string, string> payload, CallBack callback)
  {
		WWWForm formData = new WWWForm();
    foreach (KeyValuePair<String, String> data in payload)
    {
      formData.AddField(data.Key, data.Value);
    }
    UnityWebRequest request = UnityWebRequest.Post(url, formData);
    StartCoroutine(SendRequest(request, callback));
  }

	protected void Patch(string url, string json, CallBack callback)
  {
    UnityWebRequest request = GenerateUnityWebRequest(url, HttpMethod.PATCH, json);
    StartCoroutine(SendRequest(request, callback));
  }

	protected void Put(string url, string json, CallBack callback)
  {
    UnityWebRequest request = GenerateUnityWebRequest(url, HttpMethod.PUT, json);
    StartCoroutine(SendRequest(request, callback));
  }

	protected void Delete(string url, CallBack callback) 
  {
    UnityWebRequest request = GenerateUnityWebRequest(url, HttpMethod.DELETE);
    StartCoroutine(SendRequest(request, callback));
  }

	// Generates a UnityWebRequest with a configuration HTTP method and sets the request type to JSON
	private UnityWebRequest GenerateUnityWebRequest(string url, HttpMethod httpMethod, string json = null) 
  {
      UnityWebRequest request;
 
      switch (httpMethod)
      {
          case HttpMethod.GET:
              request = UnityWebRequest.Get(url);
              break;
          case HttpMethod.POST:
              request = UnityWebRequest.PostWwwForm(url, json);
              break;
          case HttpMethod.PUT:
              request = UnityWebRequest.Put(url, json);
              break;
          case HttpMethod.DELETE:
              request = UnityWebRequest.Delete(url);
              break;
          case HttpMethod.PATCH:
              // PATCH is not supported by UnityWebRequest, so we have to use a workaround
              request = new UnityWebRequest(url, "PATCH");
              byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
              request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
              request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
              break;
          default:
              throw new ArgumentException("Invalid HTTP method");
      }
      SetHeaders(request);
      return request;
  }

  private void SetHeaders(UnityWebRequest request)
  {
      request.SetRequestHeader("Content-Type", "application/json");
  }

  private IEnumerator SendRequest(UnityWebRequest request, CallBack callback)
  {
    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
    {
      Debug.LogError(request.error);
    }
    else
    {
      JSONNode responseJSON = JSON.Parse(request.downloadHandler.text);

      if (callback != null)
      {
        callback(responseJSON);
      }
    }
  }
}