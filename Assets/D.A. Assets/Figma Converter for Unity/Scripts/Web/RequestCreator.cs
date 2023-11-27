using DA_Assets.FCU.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace DA_Assets.FCU
{
    public class RequestCreator
    {
        public static Request CreateImageLinksRequest(string projectUrl, List<string> chunk, FigmaConverterUnity fcu)
        {
            string query = CreateImagesQuery(
                    chunk,
                    projectUrl,
                    fcu.Settings.MainSettings.ImageFormat.GetImageFormat(),
                    fcu.Settings.MainSettings.ImageScale);

            Request request = new Request
            {
                Query = query,
                RequestType = RequestType.Get,
                RequestHeader = new RequestHeader
                {
                    Name = "Authorization",
                    Value = $"Bearer {fcu.FigmaSession.Token}"
                }
            };

            return request;
        }

        public static string CreateImagesQuery(List<string> chunk, string projectId, string extension, float scale)
        {
            string joinedIds = string.Join(",", chunk);

            if (joinedIds[0] == ',')
                joinedIds = joinedIds.Remove(0, 1);

            string query = $"https://api.figma.com/v1/images/{projectId}?ids={joinedIds}&format={extension}&scale={scale.ToDotString()}";
            return query;
        }
        public static Request CreateTokenRequest(string code)
        {
            string tokenQueryLink = string.Format(FcuConfig.AuthUrl, FcuConfig.ClientId, FcuConfig.ClientSecret, FcuConfig.RedirectUri, code);

            Request request = new Request
            {
                Query = tokenQueryLink,
                RequestType = RequestType.Post,
                WWWForm = new WWWForm()
            };

            return request;
        }
        public static Request CreateProjectRequest(string token, string projectId)
        {
            string query = string.Format(FcuConfig.ApiLink, projectId);

            Request request = new Request
            {
                Query = query,
                RequestType = RequestType.Get,
                RequestHeader = new RequestHeader
                {
                    Name = "Authorization",
                    Value = $"Bearer {token}"
                }
            };

            return request;
        }
    }
}