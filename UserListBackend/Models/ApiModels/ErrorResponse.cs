﻿using System.Net;

namespace UserListBackend.Models.ApiModels
{
    public class ErrorResponse
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public string Message { get; set; } = string.Empty;
        public IEnumerable<string> Errors { get; set; } = Array.Empty<string>();
        public DateTimeOffset DateTime { get; set; } = DateTimeOffset.Now;
    }
}
