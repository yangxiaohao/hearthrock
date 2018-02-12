﻿// <copyright file="RockApiClient.cs" company="https://github.com/yangyuan">
//     Copyright (c) The Hearthrock Project. All rights reserved.
// </copyright>

namespace Hearthrock.Communication
{
    using System;
    using System.Net;
    using System.Threading;

    /// <summary>
    /// The API client to communicate with trace service and bot.
    /// </summary>
    public class RockApiClient
    {
        /// <summary>
        /// ContentType of Json
        /// </summary>
        private const string JsonContentType = "application/json";

        /// <summary>
        /// Initializes a new instance of the <see cref="RockApiClient" /> class.
        /// </summary>
        public RockApiClient()
        {
        }

        /// <summary>
        /// Post a object to remote and get the result.
        /// </summary>
        /// <typeparam name="T">Return Type.</typeparam>
        /// <param name="endpoint">The api endpoint.</param>
        /// <param name="obj">The object to be posted.</param>
        /// <returns>The return object.</returns>
        public T Post<T>(string endpoint, object obj)
        {
            var ret = this.Post(endpoint, RockJsonSerializer.Serialize(obj));
            return RockJsonSerializer.Deserialize<T>(ret);
        }

        /// <summary>
        /// Post a object to remote and without getting the result.
        /// </summary>
        /// <param name="endpoint">The api endpoint.</param>
        /// <param name="obj">The object to be posted.</param>
        public void Post(string endpoint, object obj)
        {
            this.Post(endpoint, RockJsonSerializer.Serialize(obj));
        }

        /// <summary>
        /// Post a json to remote and get the result string.
        /// </summary>
        /// <param name="endpoint">The api endpoint.</param>
        /// <param name="json">The json to be posted.</param>
        /// <returns>The return string.</returns>
        public string Post(string endpoint, string json)
        {
            using (var webClient = new WebClient())
            {
                webClient.Headers[HttpRequestHeader.ContentType] = JsonContentType;
                return webClient.UploadString(endpoint, json);
            }
        }

        /// <summary>
        /// Post a object to remote.
        /// </summary>
        /// <param name="endpoint">The api endpoint.</param>
        /// <param name="obj">The object to be posted.</param>
        public void PostAsync(string endpoint, object obj)
        {
            new Thread(delegate()
            {
                try
                {
                    this.Post(endpoint, RockJsonSerializer.Serialize(obj));
                }
                catch (Exception e)
                {
                    // for any exception
                    Console.WriteLine(e);
                }
            }).Start();
        }
    }
}
