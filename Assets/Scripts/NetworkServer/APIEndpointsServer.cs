using UnityEngine;
using System.Collections;
using System;
using SimpleJSON;

public class APIEndpointsServer : MonoBehaviour{

	const string URLHost = "http://lovepoints.stevedecoded.com";
	public string Auth_token = "";
	//public string JSONReturn = "";

	void Start () {

		//Check if Auth code exists if not then
		//create account with passcode entered.
		if(AuthCodeExist())
			Auth_token = RetrieveAuthCode();
		else
			NewAccountPasscode (1234); // <- To be changes to the users passcode they enter first time
	}


	/// <summary>
	/// Creation of a new account
	/// </summary>
	/// <param name="PasscodeValue">Passcode value.</param>
	public void NewAccountPasscode(int PasscodeValue)
	{
		string url = "/account/passcode";
		
		WWWForm form = new WWWForm();
		form.AddField("passcode", PasscodeValue.ToString());
		WWW www = new WWW(URLHost + url, form);
		
		StartCoroutine(AuthTokenRequest(www));
	}

	/// <summary>
	/// Registers the details.
	/// </summary>
	/// <param name="sex">Sex.</param>
	/// <param name="firstName">First name.</param>
	/// <param name="secondName">Second name.</param>
	/// <param name="region">Region.</param>
	/// <param name="phoneNumber">Phone number.</param>
	/// <param name="email">Email.</param>
	public void RegisterDetails(string sex, string firstName, string secondName, string region, string phoneNumber, string email)
	{
		string url = "/account/you";
		
		WWWForm form = new WWWForm();
		form.AddField("sex", sex);
		form.AddField("firstName", firstName);
		form.AddField("secondName", secondName);
		form.AddField("region", region);
		form.AddField("phoneNumber", phoneNumber);
		form.AddField ("email", email);
		WWW www = new WWW(URLHost + url, form);
		
		StartCoroutine(WebFormRequest(www));
	}

	/// <summary>
	/// Accounts login with the phone number and passcode
	/// </summary>
	/// <param name="phoneNumber">Phone number.</param>
	/// <param name="passcode">Passcode.</param>
	public void AccountLoginPhoneNumber(string phoneNumber, int passcode)
	{
		string url = "/account/login";
		
		WWWForm form = new WWWForm();
		form.AddField("phoneNumber", phoneNumber);
		form.AddField("passcode", passcode.ToString());

		WWW www = new WWW(URLHost + url, form);
		
		StartCoroutine(AuthTokenRequest(www));
	}

	/// <summary>
	/// Accounts login with the email and passcode
	/// </summary>
	/// <param name="email">Email.</param>
	/// <param name="passcode">Passcode.</param>
	public void AccountLoginEmail(string email, int passcode)
	{
		string url = "/account/login";
		
		WWWForm form = new WWWForm();
		form.AddField("email", email);
		form.AddField("passcode", passcode.ToString());
		
		WWW www = new WWW(URLHost + url, form);
		
		StartCoroutine (AuthTokenRequest (www));
	}

	//!UNSURE OF RESPONSE!
	/// <summary>
	/// Retrieves passcode from phone number
	/// </summary>
	/// <param name="phoneNumber">Phone number.</param>
	public void ForgottenPasscodePhoneNumber(string phoneNumber)
	{
		string url = "/passcode/resend";
		
		WWWForm form = new WWWForm();
		form.AddField("phoneNumber", phoneNumber);
		
		WWW www = new WWW(URLHost + url, form);
		
		StartCoroutine(AuthTokenRequest(www));
	}

	/// <summary>
	/// Retrieves passcode from email
	/// </summary>
	/// <param name="email">Email.</param>
	public void ForgottenPasscodeEmail(string email)
	{
		string url = "/passcode/resend";
		
		WWWForm form = new WWWForm();
		form.AddField("email", email);
		
		WWW www = new WWW(URLHost + url, form);
		
		StartCoroutine(AuthTokenRequest(www));
	}
	
	/// <summary>
	/// Saves the auth token.
	/// </summary>
	/// <param name="JSONText">JSON text.</param>
	void SaveAuthToken(string JSONText)
	{
		var jsonReader = JSON.Parse (JSONText);
		Debug.Log(jsonReader["data"]["auth_token"]);

		//Parse the token to remove the excess unicode characters
		Auth_token = jsonReader["data"]["auth_token"].ToString().Replace("\"","");

		//Saves to the devices Player Prefs
		AddToPlayerPref ("Auth_Token", Auth_token);

	}

	/// <summary>
	/// Method to test if the app has been started before
	/// </summary>
	/// <returns><c>true</c>, if code exist was authed, <c>false</c> otherwise.</returns>
	bool AuthCodeExist()
	{
		if(RetrieveAuthCode() != "")
			return true;
		else
			return false;
	}
	/// <summary>
	/// Retrieves the auth code.
	/// </summary>
	/// <returns>The auth code.</returns>
	string RetrieveAuthCode()
	{
		return PlayerPrefs.GetString ("Auth_Token");
	}
	/// <summary>
	/// Adds string player preference.
	/// </summary>
	/// <param name="ValueName">Value name.</param>
	/// <param name="Value">Value.</param>
	void AddToPlayerPref(string ValueName, string Value)
	{
		PlayerPrefs.SetString(ValueName, Value);
	}
	
	/// <summary>
	/// Adds int to player preference.
	/// </summary>
	/// <param name="ValueName">Value name.</param>
	/// <param name="Value">Value.</param>
	void AddToPlayerPref(string ValueName, int Value)
	{
		PlayerPrefs.SetInt(ValueName, Value);
	}
	
	/// <summary>
	/// Adds float to player preference.
	/// </summary>
	/// <param name="ValueName">Value name.</param>
	/// <param name="Value">Value.</param>
	void AddToPlayerPref(string ValueName, float Value)
	{
		PlayerPrefs.SetFloat(ValueName, Value);
	}

	/// <summary>
	/// Web Request to Retrieve the Auth Token
	/// </summary>
	/// <returns>The for request.</returns>
	/// <param name="www">Www.</param>
	IEnumerator AuthTokenRequest(WWW www)
	{
		//wait for connection message
		yield return www;
			
		// check for errors
		if(www.error == null)
		{
			Debug.Log("WWW Success");
			Debug.Log(www.text);
			SaveAuthToken(www.text);
		} 
		else
		{
			Debug.Log("WWW Failed");
			Debug.Log(www.error);
		}    
	}

	IEnumerator WebFormRequest(WWW www)
	{
		//wait for connection message
		yield return www;
		
		// check for errors
		if(www.error == null)
		{
			Debug.Log("WWW Success");
			Debug.Log(www.text);
		} 
		else
		{
			Debug.Log("WWW Failed");
			Debug.Log(www.error);
		}    
	}
	
} 