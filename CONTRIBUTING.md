This is a community generated project and we appreciate contributions from other developers. If you would like to contribute, this document will help you.

## Getting started

First thing is to clone the repo and ensure you have everything running locally. Here are some steps to guide you:

* To install the DNVM and DNX see [Getting Started with ASP.NET 5 and DNX](https://github.com/aspnet/Home/blob/dev/README.md)
* Create a fork of this repository.
* Clone you fork to your computer. Go to a command prompt and CD to your project folder. 
* Ensure that you are running the latest unstable dev feed of the runtime by running the command `dnvm upgrade -u`.
* Restore all packages by running the command `dnu restore`
* At this point you should be able to open the solution in Visual Studio and run the sample project 
    
If you want to add a new provider, look at what providers are available for you to pick up in [the issues](https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/issues). If you see one you want to work on, leave a comment stating that you are picking it up so we don't get people working on the same things. If you don't see the provider you want to work on listed, then just create an issue for it and then leave a comment that you’re implementing it.

## Using the Yeoman generator

To create a new provider, we have created a Yeoman generator to create the basic scaffolding for you. To install it please ensure you have completed the following:

* Install NodeJS from [their website](https://nodejs.org/en/download/).
* Install Yeoman by running `npm install -g yo`
* Next install the generator by running `npm install -g generator-aspnet-oauth`

Great, now you are ready to create your first provider. In this step through I will create one for [Buffer](https://buffer.com): 

* Go to the command line and CD to the folder where you have cloned the repo before. Ensure you are in the `/src` folder. For example `C:\Development\jerriep\AspNet.Security.OAuth.Providers\src>`
* Now run the Generator by running the command `yo aspnet-oauth`
* You will be prompted by the generator for various pieces of information:
	* For the name of the provider, type the name in Pascal casing, e.g. `Buffer`
	* Enter your own name, e.g. `Jerrie Pelser`
	* Enter the authorization endpoint, e.g. `https://bufferapp.com/oauth2/authorize`
	* Enter the token endpoint, e.g. `https://api.bufferapp.com/1/oauth2/token.json`
	* Enter the user information endpoint. This will be the API call that will be made to retrieve information about the user of that particular service so you can populate the relevant Claims. E.g. `https://api.bufferapp.com/1/user.json`
* The generator will complete and you will see that it has created a folder for your provider along with the required files. The folder will be based on the name which you supplied. In my example this will be `AspNet.Security.OAuth.Buffer`
* Next you can open the solution in Visual Studio 2015 and add the generated project for the solution. Right click on the `/src` folder in your Solution Explorer and select Add | Existing Project. Browse to the folder that was generated for you, e.g. `src/AspNet.Security.OAuth.Buffer` and add the project, e.g, `AspNet.Security.OAuth.Buffer.xproj`.
* Next go to the sample project (Mvc.Client) and open the `project.json` file. Look under the dependencies section for where the references are added for all the other providers and add a reference you your provider, e.g. `"AspNet.Security.OAuth.Buffer": "1.0.0-*"`. Save the file and give VS an opportunity to restore the dependencies.
* You should now be able to go to `Startup.cs` file of the demo project and configure your provider.

## Completing the provider 

At this point you only have the basic skeleton for your provider. Usually there would be one more piece for you to complete and that is to extract the correct values from the call to the User Information Endpoint. 

You will need to look at `*AuthenticationHandler.cs` file in your project and in the `CreateTicketAsync` method you will see there is a TODO section for you to add any extra claims. To extract the claims from the payload returned by the User Information Endpoint, we make use of a helper class in the file `*AuthenticationHelper.cs`. You will see that it contains one demo method to show you how you can retrieve the ID for a user. This method may not be correct, and you will need to ensure that you look at the actual payload returned to extract the ID correctly.

This ID field may or may not actually be called ID, depending on the particular service. What is important is that we need to extract a unique identifier for the user (typically an integer or GUID value) and assign that to the `NameIdentifier` claim. Refer to the API documentation for the service and ensure that you retrieve the correct value.

**NB:** The `NameIdentifier` is very important for ASP.NET Identity to work correctly, so please ensure that it works correctly and that you retrieve the correct value.

Another typical claim which you can retrieve is the `Name` claim. This will normally be the username of the user of that particular service. If you extract more claims you can check whether an appropriate built-in claim type exists for that information, and if so you can use it. If not you can create you own one. We typically use the format `"urn:<service>:<claim name>"`, e.g. `"urn:github:name"` is used to store the full name of the user in the case of the GitHub provider.

For an example you can look at the following relevant files on the GitHub provider:

* [GitHubAuthenticationHelper.cs](https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/blob/dev/src/AspNet.Security.OAuth.GitHub/GitHubAuthenticationHelper.cs)
* [GitHubAuthenticationHandler.cs](https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/blob/dev/src/AspNet.Security.OAuth.GitHub/GitHubAuthenticationHandler.cs)  

You can also look at the actual payload for the GitHub provider [on their documentation](https://developer.github.com/v3/users/). 

Another example where a little bit more complex payload structure was returned is the Yahoo provider:

* [YahooAuthenticationHelper.cs](https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/blob/dev/src/AspNet.Security.OAuth.Yahoo/YahooAuthenticationHelper.cs)
* [YahooAuthenticationHandler.cs](https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/blob/dev/src/AspNet.Security.OAuth.Yahoo/YahooAuthenticationHandler.cs)

You can look at the [actual Yahoo API documentation](https://developer.yahoo.com/social/rest_api_guide/extended-profile-resource.html) to see the documentation for this particular payload. 

## Submit a pull request

Once you are happy that your provider is tested and works, you can commit the changes and push it up to your local fork. At that point you can submit a pull request. 