SimpleWebSevice
---

A simple HTTP request and response wrapper to make it easier when interacting with RESTful JSON APIs. 

After writing custom request/response lifecycles for a few Unity Projects, it became apparent that a common approach could streamline implementation. The goal was to implement a simple wrapper around [UnityWebRequest](https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest.html) and provide flexible JSON parsing [using SimpleJSON](https://github.com/Bunny83/SimpleJSON) so that teams could focus on integrating web APIs without starting from scratch each time. This tool is free to use, modify and distribute as needed.

## UnityWebRequest - A building block, not a solution 
[UnityWebRequest](https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest.html), the successor of [WWW](https://docs.unity3d.com/ScriptReference/WWW.html), is responsibile for the request/response lifecyle between Unity 3D and web accessible property using HTTP. Unity provides some examples on how to make common requests but in order to bring the implementation beyond proof-of-concept, we must manage the request/response lifecule. This involves establishing a coroutine per request/response as well as delegating and firing a callback method upon coroutine completion.

## JSON Support - Not flexible, still lagging 
The majority of modern web APIs will respond in the JSON. The need to support JSON natively inside Unity is increasing continually. JSON Serialization and specifically the [JSONUtility](https://docs.unity3d.com/ScriptReference/JsonUtility.html) aree a good start but require a 1:1 translation of JSON structure to class structure. This is fine if the API response will never change but during iterative development or integrating with an API that is not under a team's control can drastically reduce progress. Furthermore, a successful request the `UnityWebRequest.downloadHandler` provides the JSON response as a string which needs to be transformed into useful format that can be accessed by Unity. 

# Getting Started
Add this to your project by [using Unity's Package Manager to import via Git Url](https://docs.unity3d.com/Manual/upm-ui-giturl.html) with the following URL:

```
ssh://git@github.com:maxfelker/SimpleWebService.git
```

## Usage
The _SimpleWebService_ class is meant to be a starting point for your own custom web services. Below is an example of the one such extension where we make a GET request to a mock JSON API and retrieve a list of todos. 

```C#
public class MyTodoAPIExample : SimpleWebService {

    // Get a list of todos when the script initializes 
    void Start() {
        GetTodoList();
    }

    // Make a request to the API to get a list of todos 
    void GetTodoList() {
        string URL = "https://jsonplaceholder.typicode.com/todos";
        base.Get(URL, TodoListSuccess);
    }

     // Log the API response of GetTodoList()
    void TodoListSuccess(JSONNode response) {
        Debug.Log(response);
    }
}
```


## What's Going On
In the above example, we see that `GetTodoList` implements the a `Get` call to a specific URL and provides a callback method of `TodoListSuccess`:

```C#
string URL = "https://jsonplaceholder.typicode.com/todos";
base.Get(URL, TodoListSuccess);
```

Under the hood, a coroutine is established for the HTTP request/response lifecycle. Once the response from the API is recieved, response text is parsed into a [JSONNode](https://github.com/Bunny83/SimpleJSON/blob/master/SimpleJSON.cs#L62) object. This object is passed to the supplied callback method as single argument and when the coroutine completes, the callback method is fired:

```C#
void TodoListSuccess(JSONNode response) {
    Debug.Log(response);
}
```
From here, we can interacte with the _response_ data. Working with JSONNode is similar to the Javascript bracket notation pattern. You can see examples of this in the [_User Service Example_](https://github.com/mw-felker/SimpleWebService/blob/master/Assets.SimpleWebService/Example/UserServiceExample.cs) that is included in this project.

## Available methods

When the _SimpleWebService_ class is extended, you can utilizing the following base methods:

- `base.Get(string url, delegate callback)`
- `base.Post(string url, string json, delegate callback)`
- `base.Patch(string url, string json, delegate callback)`
- `base.Put(string url, string json, delegate callback)`
- `base.Delete(string url, delegate callback)`

# Contributing
Download the Unity project by using `git clone`:

```bash
git clone git@github.com:maxfelker/SimpleWebService.git
```

This will create a `SimpleWebService/` folder which contains the Unity project. Open Unity 3D and select this folder as a project. Once the project is loaded, browse to:

> Assets/SimpleWebService/Example

Open up the `Example` scene and press play. If everything works correctly, the Unity app should make a HTTP GET request to [a mock JSON User API](https://jsonplaceholder.typicode.com/) and when the API responds, it displays the JSON data inthe Unity console.


