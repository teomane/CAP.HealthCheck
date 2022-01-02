﻿using System;

namespace DotNetCore.CAP.HealthCheck.PostgreSql.Received
{
    public class PostgreSqlReceivedTableData
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public object Content { get; set; }
        public int Retries { get; set; }
        public DateTime Added { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}