/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.VkId;

/// <summary>
/// List of <see href="https://id.vk.com/about/business/go/docs/vkid/latest/vk-id/connection/work-with-user-info/scopes"> scopes </see>.
/// </summary>
public static class VkIdAuthenticationScopes
{
    /// <summary>
    /// Grants access to personal information.
    /// </summary>
    public const string PersonalInfo = "vkid.personal_info";

    /// <summary>
    /// Grants access to user's email.
    /// </summary>
    public const string Email = "email";

    /// <summary>
    /// Grants access to user's phone number.
    /// </summary>
    public const string Phone = "phone";

    /// <summary>
    /// Grants access to <see href="https://dev.vk.com/method/friends">Friends API</see>.
    /// </summary>
    public const string Friends = "friends";

    /// <summary>
    /// Grants access to <see href="https://dev.vk.com/api/posts">Posts API</see>.
    /// </summary>
    public const string Posts = "wall";

    /// <summary>
    /// Grants access to <see href="https://dev.vk.com/method/groups">Groups API</see>.
    /// </summary>
    public const string Groups = "groups";

    /// <summary>
    /// Grants access to <see href="https://dev.vk.com/method/stories">Stories API</see>.
    /// </summary>
    public const string Stories = "stories";

    /// <summary>
    /// Grants access to <see href="https://dev.vk.com/method/docs">Docs API</see>.
    /// </summary>
    public const string Docs = "docs";

    /// <summary>
    /// Grants access to <see href="https://dev.vk.com/method/photos">Photos API</see>.
    /// </summary>
    public const string Photos = "photos";

    /// <summary>
    /// Grants access to <see href="https://dev.vk.com/method/ads">Ads API</see>.
    /// </summary>
    public const string Ads = "ads";

    /// <summary>
    /// Grants access to <see href="https://dev.vk.com/method/video">Video API</see>.
    /// </summary>
    public const string Video = "video";

    /// <summary>
    /// Grants access to <see href="https://dev.vk.com/method/status">Status API</see>.
    /// </summary>
    public const string Status = "status";

    /// <summary>
    /// Grants access to <see href="https://dev.vk.com/method/market">Market API</see>.
    /// </summary>
    public const string Market = "market";

    /// <summary>
    /// Grants access to <see href="https://dev.vk.com/method/pages">Pages API</see>.
    /// </summary>
    public const string Pages = "pages";

    /// <summary>
    /// Grants access to <see href="https://dev.vk.com/method/notifications">Notifications API</see>.
    /// </summary>
    public const string Notifications = "notifications";

    /// <summary>
    /// Grants access to <see href="https://dev.vk.com/method/stats">Stats API</see>.
    /// </summary>
    public const string Stats = "stats";

    /// <summary>
    /// Grants access to <see href="https://dev.vk.com/method/notes">Notes API</see>.
    /// </summary>
    public const string Notes = "notes";
}
