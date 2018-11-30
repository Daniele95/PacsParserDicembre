﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PacsParserDicembre
{

    public class ImageLevelQuery : QueryObject
    {
        public string QueryRetrieveLevel { get; set; } = "IMAGE";
        public string StudyInstanceUID { get; set; } = "";
        public string SeriesInstanceUID { get; set; } = "";
        public string InstanceNumber { get; set; } = "";
    }

        public class SeriesLevelQuery : QueryObject
    {
        public string QueryRetrieveLevel { get; set; } = "SERIES";
        public string StudyInstanceUID { get; set; } = "";
        public string SeriesInstanceUID { get; set; } = "";
        public string SeriesDescription { get; set; } = "";
        public string PatientID { get; set; } = "";


        public SeriesLevelQuery() : base()
        { }

        public SeriesLevelQuery(StudyLevelQuery studyQuery) : base()
        {
            SetField("StudyInstanceUID", studyQuery.GetField("StudyInstanceUID"));
            SetField("PatientID", studyQuery.GetField("PatientID"));
        }

    }

    public class StudyLevelQuery : QueryObject
    {
        public string QueryRetrieveLevel { get; set; } = "STUDY";
        public string PatientName { get; set; } = "";
        public string PatientBirthDate { get; set; } = "";
        public string PatientID { get; set; } = "";
        public string Modality { get; set; } = "";
        public string StudyInstanceUID { get; set; } = "";
        public string StudyDate { get; set; } = "";
        public string AccessionNumber { get; set; } = "";
        public string StudyDescription { get; set; } = "";
    }

    public abstract class QueryObject
    {

        Dictionary<string, string> queryData = new Dictionary<string, string>();

        public QueryObject()
        {
            init();
        }

        void init()
        {
            PropertyInfo[] properties = this.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                queryData.Add(property.Name, property.GetValue(this).ToString());
            }
        }

        public List<string> getKeys()
        {
            return new List<string>(queryData.Keys);
        }

        public string GetField(string tag)
        {
            return queryData[tag];
        }

        public void SetField(string tag, string value)
        {
            PropertyInfo prop = this.GetType().GetProperty(tag, BindingFlags.Public | BindingFlags.Instance);

            if (null != prop && prop.CanWrite)
                prop.SetValue(this, value, null);

            queryData[tag] = value;
        }

    }

    public abstract class Publisher
    {
        public delegate void EventHandler(QueryObject s);
        public event EventHandler Event;

        public void RaiseEvent(QueryObject s)
        {
            Event(s);
        }
    }
}