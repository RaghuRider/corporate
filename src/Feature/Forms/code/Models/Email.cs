﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIEnterprise.Feature.Forms.Models
{
    public class Email
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsAttachement { get; set; }
    }
}