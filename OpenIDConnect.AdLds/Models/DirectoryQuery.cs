﻿namespace OpenIDConnect.AdLds.Models
{
    public class DirectoryQuery
    {
        public DirectoryQuery(string filter)
        {
            this.Filter = filter;
        }

        public string Filter { get; }
    }
}
