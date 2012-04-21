﻿using System;
using FluentMigrator;

namespace phiNdus.fundus.DbMigrations
{
    public abstract class MigrationBase : Migration
    {
        private string _schemaName = String.Empty;

        protected string SchemaName
        {
            get { return _schemaName; }
            set { _schemaName = value; }
        }
    }
}