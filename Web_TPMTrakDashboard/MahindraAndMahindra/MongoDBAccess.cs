using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.MahindraAndMahindra
{
    public class MongoDBAccess
    {
        private readonly static IMongoClient _MongoClient = null;
        private readonly static IMongoDatabase _MongoDatabase;
        private static string mongoDBConnString = ConfigurationManager.AppSettings["mongoDBConString"].ToString();
        static MongoDBAccess()
        {
            _MongoClient = new MongoClient(mongoDBConnString);
            _MongoDatabase = _MongoClient.GetDatabase(ConfigurationManager.AppSettings["mongoDBName"].ToString());
        }
        public static DataTable getParameterData(string fromDate,string toDate,string parameter, string machine)
        {
            DataTable dt = new DataTable();
            DataColumn dtColumn;
            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(string);
            dtColumn.ColumnName = "UpdatedtimeStamp";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(string);
            dtColumn.ColumnName = "ParameterValue";
            dt.Columns.Add(dtColumn);
            try
            {

                var match = new BsonDocument
                            {
                            {
                            "$match", new BsonDocument
                            {
                                { "$and" ,  new BsonArray{ 
                                    new BsonDocument{
                                    {"$and", new BsonArray{ new BsonDocument{
                                        { "UpdatedTS", new BsonDocument{ {"$gte", Util.GetDateTime(fromDate).ToUniversalTime() } } } },
                                         new BsonDocument{{ "UpdatedTS", new BsonDocument{ { "$lte", Util.GetDateTime(toDate).ToUniversalTime() } } }
                                        } } } },
                                      new BsonDocument{{"MachineID",machine} },
                                       new BsonDocument{{ "ParameterID", parameter} },
                                 } },
                            }
                            }
                            };
                var sort = new BsonDocument{
                                    {
                                        "$sort" ,new BsonDocument {{ "UpdatedTS" , 1}}
                                    }
                                };

                var group = new BsonDocument{
                                    {
                                        "$group" ,new BsonDocument {
                                             { "_id" , new BsonDocument{
                                                 {"ParameterID","$ParameterID" },
                                                  {"MachineID","$MachineID" }
                                             } },
                                            { "MachineID",new BsonDocument{ { "$last", "$MachineID" } } },
                                            { "ParameterID",new BsonDocument{ { "$last", "$ParameterID" } } },
                                            { "UpdatedTS",new BsonDocument{ { "$last", "$UpdatedTS" } } },
                                            { "ParameterValue",new BsonDocument{ { "$last", "$ParameterValue" } } }
                                        }
                                    }
                                };

                var project = new BsonDocument{
                                    {
                                        "$project" ,new BsonDocument {{ "_id", 0}}
                                    }
                                };
                AggregateOptions aggregateOptions = new AggregateOptions { AllowDiskUse = true };
                PipelineDefinition<BsonDocument, BsonDocument> pipeline = new BsonDocument[]
                     { match, sort };
                //{ match,sort,group,project };
                IAsyncCursor<BsonDocument> result = _MongoDatabase.GetCollection<BsonDocument>("ProcessParameterTransaction").Aggregate(pipeline, aggregateOptions);
                var resultSet = IAsyncCursorExtensions.ToList<BsonDocument>(result);
                foreach (BsonDocument doc in resultSet)
                {
                    DataRow rows = dt.NewRow();
                    rows["ParameterValue"] = doc["ParameterValue"].ToString();
                    rows["UpdatedtimeStamp"] = doc["UpdatedTS"].AsLocalTime;
                    dt.Rows.Add(rows);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return dt;
        }

        public static List<DTO> getLatestParameterData(DataTable dtProcessMasterData,string machine)
        {
            List<DTO> listLatestParam = new List<DTO>();
            try
            {
                BsonArray mongoParameter = new BsonArray();
                mongoParameter.Add("SpindleLoad");
                mongoParameter.Add("SpindleVibration");

                var match = new BsonDocument
                            {
                            {
                            "$match", new BsonDocument
                            {
                                { "$and" ,  new BsonArray{
                                      new BsonDocument{{"MachineID",machine} },
                                       new BsonDocument{{ "ParameterID", new BsonDocument { { "$in", mongoParameter } } } },
                                 } },
                            }
                            }
                            };
                var sort = new BsonDocument{
                                    {
                                        "$sort" ,new BsonDocument {{ "UpdatedTS" , 1}}
                                    }
                                };

                var group = new BsonDocument{
                                    {
                                        "$group" ,new BsonDocument {
                                             { "_id" , new BsonDocument{
                                                 {"ParameterID","$ParameterID" },
                                                  {"MachineID","$MachineID" }
                                             } },
                                            { "MachineID",new BsonDocument{ { "$last", "$MachineID" } } },
                                            { "ParameterID",new BsonDocument{ { "$last", "$ParameterID" } } },
                                            { "UpdatedTS",new BsonDocument{ { "$last", "$UpdatedTS" } } },
                                            { "ParameterValue",new BsonDocument{ { "$last", "$ParameterValue" } } },
                                            { "HighGreenLimit",new BsonDocument{ { "$last", "HighGreenLimit" } } },
                                            { "LowerGreenLimit",new BsonDocument{ { "$last", "LowerGreenLimit" } } }
                                        }
                                    }
                                };

                var project = new BsonDocument{
                                    {
                                        "$project" ,new BsonDocument {{ "_id", 0}}
                                    }
                                };
                AggregateOptions aggregateOptions = new AggregateOptions { AllowDiskUse = true };
                PipelineDefinition<BsonDocument, BsonDocument> pipeline = new BsonDocument[]
                { match,sort,group,project };
                IAsyncCursor<BsonDocument> result = _MongoDatabase.GetCollection<BsonDocument>("ProcessParameterTransaction").Aggregate(pipeline, aggregateOptions);
                var resultSet = IAsyncCursorExtensions.ToList<BsonDocument>(result);
                foreach (BsonDocument doc in resultSet)
                {
                    DTO data = new DTO();
                    data.ParameterId = doc["ParameterID"].ToString();
                    System.Type type = doc["ParameterValue"].GetType();
                    if (type.Name == "BsonDouble")
                    {
                        data.ParameterValue = Math.Round(doc["ParameterValue"].AsDouble, 2).ToString();
                    }
                    else
                    {
                        data.ParameterValue = doc["ParameterValue"].ToString();
                    }
                    var masterRow=dtProcessMasterData.AsEnumerable().Where(k => k.Field<string>("ParameterID").Equals(data.ParameterId,StringComparison.OrdinalIgnoreCase)).ToList();
                    string parametercolor = "";
                    if (masterRow != null)
                    {
                        if (masterRow.Count > 0)
                        {
                            foreach (var row in masterRow)
                            {
                                data.ParameterName = row["ParameterName"].ToString();
                                data.DisplayText = row["DisplayText"].ToString();
                                data.MaxValue = row["HighRedLimit"].ToString();
                                data.MinValue = row["LowerRedLimit"].ToString();
                                data.TemplateType = row["TemplateType"].ToString();
                                data.Unit = row["Unit"].ToString();
                                switch (data.TemplateType)
                                {
                                    case "Text":
                                        data.Visibility = "none";
                                        break;
                                    case "High/Low Limits":
                                        data.Visibility = "";
                                        break;
                                    case "High/Low":
                                        data.Visibility = "";
                                        break;
                                    default:
                                        data.Visibility = "none";
                                        break;
                                }
                                double lowGreen=0, highGreen=0, highRed=0, lowRed=0;
                                if (!string.IsNullOrEmpty(row["LowerGreenLimt"].ToString()))
                                    lowGreen = Convert.ToDouble(row["LowerGreenLimt"].ToString());
                                if (!string.IsNullOrEmpty(row["HighGreenLimit"].ToString()))
                                    highGreen = Convert.ToDouble(row["HighGreenLimit"].ToString());
                                if (!string.IsNullOrEmpty(row["LowerRedLimit"].ToString()))
                                    lowRed = Convert.ToDouble(row["LowerRedLimit"].ToString());
                                if (!string.IsNullOrEmpty(row["HighRedLimit"].ToString()))
                                    highRed = Convert.ToDouble(row["HighRedLimit"].ToString());
                              
                                if (string.IsNullOrEmpty(data.ParameterValue))
                                {
                                    //parametercolor = "#4069A6";
                                    parametercolor = "White";
                                }
                                if(lowGreen ==0 && highGreen==0 && highRed==0 && lowRed==0)
                                {
                                    //parametercolor = "#4069A6";
                                    parametercolor = "White";
                                }
                                else
                                {
                                    double parameterVal = Convert.ToDouble(data.ParameterValue);
                                    if (parameterVal > lowGreen && parameterVal < highGreen)
                                    {
                                        parametercolor = "Green";
                                    }
                                    else if ((parameterVal > highGreen && parameterVal < highRed) || (parameterVal < lowGreen && parameterVal > lowRed))
                                    {
                                        parametercolor = "Yellow";
                                    }
                                    else if (parameterVal > highRed || parameterVal < lowRed)
                                    {
                                        parametercolor = "Red";
                                    }
                                }
                                
                                break;
                            }
                        }
                    }
                    string paramColor = parametercolor;
                    //data.BackgroundColor = !string.IsNullOrEmpty(parametercolor) ? parametercolor : "#4069A6";
                    //data.ForeColor = data.BackgroundColor.Equals("Red", StringComparison.OrdinalIgnoreCase) ? "#FFFFFF" : "#2C639B";
                    data.BackgroundColor = !string.IsNullOrEmpty(paramColor) ? string.Equals(paramColor, "Green", StringComparison.OrdinalIgnoreCase) ? "#28bb28" : string.Equals(paramColor, "white", StringComparison.OrdinalIgnoreCase) ? "#2775ea" : paramColor : "#2775ea";
                    data.ForeColor = paramColor.Equals("Green", StringComparison.OrdinalIgnoreCase) ? "#555" : paramColor.Equals("Red", StringComparison.OrdinalIgnoreCase) ? "#FFFFFF" : "#2C639B";
                    data.HeaderForeColor = paramColor.Equals("Yellow", StringComparison.OrdinalIgnoreCase) || paramColor.Equals("Green", StringComparison.OrdinalIgnoreCase) ? System.Drawing.Color.Black : System.Drawing.Color.White;
                    listLatestParam.Add(data);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return listLatestParam;
        }
    }
}