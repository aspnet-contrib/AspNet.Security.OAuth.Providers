# Integrating the Twitter Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddTwitter(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";

            // Optionally request additional fields, if needed
            options.Expansions.Add("pinned_tweet_id");
            options.TweetFields.Add("text");
            options.UserFields.Add("created_at");
            options.UserFields.Add("pinned_tweet_id");
        });
```

## Required Additional Settings

_None._

## Optional Settings

| Property Name | Property Type | Description | Default Value |
|:-:|:-:|:-:|:-:|
| `Expansions` | `ISet<string>` | The optional list of additional data objects to expand from the user information endpoint. | None. |
| `TweetFields` | `ISet<string>` | The optional list of tweet fields to retrieve from the user information endpoint. | None. |
| `UserFields` | `ISet<string>` | The optional list of user fields to retrieve from the user information endpoint. | None. |
