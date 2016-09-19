﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Experimental.Networking;

namespace AzureServicesForUnity.Helpers
{
    public static class Utilities
    {
        public static T[] DeserializeJsonArray<T>(string json)
        {
            json = json.Trim();
            if (json == "[]")
                return new T[0]; //just return an empty array
            else
            {
                //string will be of the format "[{...},{...},{...},{...}]"

                if (json[0] != '[' || json[json.Length - 1] != ']')
                    return null;

                //http://stackoverflow.com/questions/20243621/regular-expression-for-a-json-type-string
                Match match = Regex.Match(json, "({.*?})");

                List<T> objectInstances = new List<T>();

                while(match.Success)
                {
                    try
                    {
                        objectInstances.Add(JsonUtility.FromJson<T>(match.Value));
                    }
                    catch
                    {
                        //if you cannot deserialize a single object contained in the array, suppress
                    }
                    match = match.NextMatch();
                }
                return objectInstances.ToArray();
            }
        }

        public static void ValidateForNull(params object[] objects)
        {
            foreach (object obj in objects)
            {
                if (obj == null)
                {
                    throw new Exception("Argument null");
                }
            }

        }

        public static bool IsWWWError(UnityWebRequest www)
        {
            return www.isError || (www.responseCode >= 400L && www.responseCode <= 511L);
        }

        public static void BuildResponseObjectOnFailure(CallbackResponse response, UnityWebRequest www)
        {
            if (www.responseCode == 404L)
                response.Status = CallBackResult.NotFound;
            else
                response.Status = CallBackResult.Failure;

            Exception ex = new Exception(www.error ?? Constants.ErrorOccurred);
            response.Exception = ex;
        }

        public static void BuildResponseObjectOnException(CallbackResponse response, Exception ex)
        {
            response.Status = CallBackResult.LocalException;
            response.Exception = ex;
        }

        /// <summary>
        /// Builds and returns a UnityWebRequest object
        /// </summary>
        /// <param name="url">Url to hit</param>
        /// <param name="method">POST,GET, etc.</param>
        /// <param name="json">Any JSON to send</param>
        /// <param name="authenticationToken">Authentication token for the headers</param>
        /// <returns>A UnityWebRequest object</returns>
        public static UnityWebRequest BuildWebRequest(string url, string method, string json, string authenticationToken)
        {
            UnityWebRequest webRequest = new UnityWebRequest(url, method);
            webRequest.SetRequestHeader(Constants.Accept, Constants.ApplicationJson);
            webRequest.SetRequestHeader(Constants.Content_Type, Constants.ApplicationJson);
            webRequest.SetRequestHeader(Constants.ZumoString, Constants.ZumoVersion);

            if (!string.IsNullOrEmpty(authenticationToken))
            {
                webRequest.SetRequestHeader(Constants.ZumoAuth, authenticationToken.Trim());
            }

            webRequest.downloadHandler = new DownloadHandlerBuffer();

            if (!string.IsNullOrEmpty(json))
            {
                byte[] payload = Encoding.UTF8.GetBytes(json);
                UploadHandler handler = new UploadHandlerRaw(payload);
                handler.contentType = Constants.ApplicationJson;
                webRequest.uploadHandler = handler;
            }
            return webRequest;
        }
    }

    public enum HttpMethod
    {
        Post,
        Get,
        Patch,
        Delete,
        Put
    }
}