﻿/*
 * Copyright 2011 Christopher Gooley / LessNeglect.com
 *
 * Author(s):
 *  Christopher Gooley / LessNeglect (gooley@lessneglect.com)
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Net;
using System.Web;
using System.Security.Cryptography;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace LessNeglect
{
    public class LessNeglectApi
    {
        private static string api_endpoint = "https://api.preact.io/api/v2";
        private static Encoding encoding = Encoding.UTF8;

        private string ProjectCode { get; set; }
        private string ProjectApiSecret { get; set; }

        #region Constructors
        public LessNeglectApi() :
            this(System.Configuration.ConfigurationManager.AppSettings["LessNeglectProjectCode"],
                System.Configuration.ConfigurationManager.AppSettings["LessNeglectProjectApiSecret"]) { }

        public LessNeglectApi(string projectCode, string secretKey)
        {
            if (string.IsNullOrEmpty(projectCode) || string.IsNullOrEmpty(secretKey))
                throw new ArgumentException("Missing ProjectCode or ProjectApiSecret");

            ProjectCode = projectCode;
            ProjectApiSecret = secretKey;
        }
        #endregion

        #region Static Helpers
        private static LessNeglectApi _client;

        public static LessNeglectApi Client
        {
            get
            {
                if (_client == null) { _client = new LessNeglectApi(); }
                return _client;
            }
        }
        #endregion

        public void CreateMessage(MessageCreateRequest request)
        {
            // sign the request 
            request.SignRequest(ProjectCode, ProjectApiSecret);

            string url = String.Format("{0}/events", api_endpoint);
            Helpers.SendData(url, "POST", JObject.FromObject(request));
        }

        public void CreateActionEvent(ActionEventCreateRequest request)
        {
            // sign the request 
            request.SignRequest(ProjectCode, ProjectApiSecret);

            string url = String.Format("{0}/events", api_endpoint);
            JObject param = JObject.FromObject(request);
            Helpers.SendData(url, "POST", param);
        }

        public void UpdatePerson(Person person)
        {
            PersonUpdateRequest request = new PersonUpdateRequest().WithPerson(person);

            // sign the request 
            request.SignRequest(ProjectCode, ProjectApiSecret);

            string url = String.Format("{0}/people", api_endpoint);
            JObject param = JObject.FromObject(request);
            Helpers.SendData(url, "POST", param);
        }
    }
}
