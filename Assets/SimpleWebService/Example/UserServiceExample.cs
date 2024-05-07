/* 
 * Post Example
 * ------------------------------
 * An example of how to extend the SimpleWebService class and make 
 * a request to web API that returns JSON. This examples
 * uses https://jsonplaceholder.typicode.com/ and it's 
 * free User API to illustrate the round trip.
 *
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class UserServiceExample : SimpleWebService
{

    public int userID = 1;
    string baseURL = "https://jsonplaceholder.typicode.com/users";

    // Get a list of todos when the script initializes 
    void Start()
    {
        GetUser();
    }

    // Make a request to the API to get specific User by ID
    void GetUser() 
    {
        Debug.Log("Getting User ID " + userID.ToString() + " from API...");
        string url = baseURL + "/" + userID.ToString();
        base.Get(url, LogUser);
    }

    void AddUser() 
    {
        string jsonPayload = "{\"name\":\"My Test User\",\"email\":\"mytest@yoursite.com\", \"address\": { \"city\": \"Portland\", \"zipcode\": \"04101\"}}";
        base.Post(baseURL , jsonPayload, LogUser);
    }

    void PatchUser() 
    {
        string url = baseURL + "/" + userID.ToString();
        string jsonPayload = "{\"email\":\"updatedemail@fromunitypatch.com\"}";
        base.Patch(url, jsonPayload, LogUser);
    }

    // Log the API response 
    void LogUser(JSONNode response) 
    {
        Debug.Log("User Id: " + response["id"]);
        Debug.Log("User Email: " + response["email"]);
        Debug.Log("User Location: " + response["address"]["city"] + ", " + response["address"]["zipcode"]);

    }

    void DeleteUser() 
    {
        string url = baseURL + "/" + userID.ToString();
        base.Delete(url, UserWasDeleted);
    }

    void UserWasDeleted(JSONNode response) 
    {
        Debug.Log("User was deleted!");
        Debug.Log(response);
    }


}
