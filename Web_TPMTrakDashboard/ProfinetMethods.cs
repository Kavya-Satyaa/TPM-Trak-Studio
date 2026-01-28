using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S7.Net;

namespace Web_TPMTrakDashboard
{
    class Profinet
    {
        const bool isPLCOffline = false;


        internal static bool GetString(string ipAddr, string dbnum, string addr, int nchars, out string val)
        {
            bool ret = false;
            string va = String.Empty;
            string u = "";
            val = "";

            if (!isPLCOffline)
            {
                Plc client = new Plc(CpuType.S71200, ipAddr, 0, 1);
                int retries = 0;
                while (!ret && retries < 2)
                {
                    try
                    {
                        if (client.IsAvailable)
                        {
                            if (client.IsConnected == false)
                            {
                                var error = client.Open();
                                if (error != ErrorCode.NoError)
                                {
                                    Logger.WriteDebugLog(error.ToString());
                                }
                                else
                                {
                                    u = client.Read(DataType.DataBlock, int.Parse(dbnum), int.Parse(addr) + 2, VarType.String, nchars).ToString().Trim(char.MinValue).Trim();

                                    if (client.LastErrorCode == ErrorCode.NoError)
                                    {
                                        val = u;
                                        ret = true;
                                    }
                                    else
                                    {
                                        Logger.WriteDebugLog(string.Format("GetWord error: {0}", client.LastErrorCode));
                                    }
                                }
                            }
                        }
                        else
                        {
                            Logger.WriteDebugLog("Not able to Ping the Machine: " + ipAddr);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteErrorLog(ex.Message);
                    }
                    finally
                    {
                        if (client.IsConnected)
                        {
                            client.Close();
                        }
                    }
                    retries += 1;
                }
            }
            return ret;
        }

        internal static bool PutString(string ipAddr, string dbnum, string addr, int nchars, string str)
        {
            Plc client = new Plc(CpuType.S71200, ipAddr, 0, 1);
            bool ret = false;
            string va = String.Empty;

            int retries = 0;
            while (!ret && retries < 2)
            {
                try
                {
                    if (client.IsAvailable)
                    {
                        if (client.IsConnected == false)
                        {
                            var error = client.Open();
                            if (error != ErrorCode.NoError)
                            {
                                Logger.WriteDebugLog(error.ToString());
                            }
                            else
                            {
                                client.WriteBytes(DataType.DataBlock, int.Parse(dbnum), int.Parse(addr), new[] { (byte)20, (byte)str.Length });
                                str = str.PadRight(nchars, char.MinValue);
                                ErrorCode er = client.Write(DataType.DataBlock, int.Parse(dbnum), int.Parse(addr) + 2, str);

                                if (er == ErrorCode.NoError)
                                {
                                    ret = true;
                                }
                                else
                                {
                                    Logger.WriteDebugLog(string.Format("PutString error: {0}", er));
                                }
                            }
                        }
                    }
                    else
                    {
                        Logger.WriteDebugLog("Not able to Ping the Machine: " + ipAddr);
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog(ex.Message);
                }
                finally
                {
                    if (client.IsConnected)
                    {
                        client.Close();
                    }
                }
                retries += 1;
            }
            return ret;
        }

        internal static bool GetBit(string ipAddr, string dbnum, string addr, out bool val)
        {
            bool ret = false;
            string va = String.Empty;
            bool u = false;
            val = false;
            if (!isPLCOffline)
            {
                Plc client = new Plc(CpuType.S71200, ipAddr, 0, 1);
                int retries = 0;
                while (!ret && retries < 2)
                {
                    try
                    {
                        if (client.IsAvailable)
                        {
                            if (client.IsConnected == false)
                            {
                                var error = client.Open();
                                if (error != ErrorCode.NoError)
                                {
                                    Logger.WriteDebugLog(error.ToString());
                                }
                                else
                                {
                                    va = String.Format("DB{0}.DBX{1}", dbnum, addr);
                                    u = (bool)client.Read(va);

                                    if (client.LastErrorCode == ErrorCode.NoError)
                                    {
                                        val = u;
                                        ret = true;
                                    }
                                    else
                                    {
                                        Logger.WriteDebugLog(string.Format("GetBit error: {0}", client.LastErrorCode));
                                    }
                                }
                            }
                        }
                        else
                        {
                            Logger.WriteDebugLog("Not able to Ping the Machine: " + ipAddr);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteErrorLog(ex.Message);
                    }
                    finally
                    {
                        if (client.IsConnected)
                        {
                            client.Close();
                        }
                    }
                    retries += 1;
                }
            }
            return ret;
        }

        internal static bool PutBit(string ipAddr, string dbnum, string addr, string val)
        {
            Plc client = new Plc(CpuType.S71200, ipAddr, 0, 1);
            bool ret = false;
            string va = String.Empty;

            int retries = 0;
            while (!ret && retries < 2)
            {
                try
                {
                    if (client.IsAvailable)
                    {
                        if (client.IsConnected == false)
                        {
                            var error = client.Open();
                            if (error != ErrorCode.NoError)
                            {
                                Logger.WriteDebugLog(error.ToString());
                            }
                            else
                            {
                                va = String.Format("DB{0}.DBX{1}", dbnum, addr);
                                ErrorCode er = client.Write(va, val.Equals("True"));

                                if (er == ErrorCode.NoError)
                                {
                                    ret = true;
                                }
                                else
                                {
                                    Logger.WriteDebugLog(string.Format("PutBit error: {0}", er));
                                }
                            }
                        }
                    }
                    else
                    {
                        Logger.WriteDebugLog("Not able to Ping the Machine: " + ipAddr);
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog(ex.Message);
                }
                finally
                {
                    if (client.IsConnected)
                    {
                        client.Close();
                    }
                }
                retries += 1;
            }
            return ret;
        }

        internal static bool GetWord(string ipAddr, string dbnum, string addr, out ushort val)
        {
            bool ret = false;
            string va = String.Empty;
            ushort u = 0;
            val = 0;
            if (isPLCOffline) return false;


            Plc client = new Plc(CpuType.S71200, ipAddr, 0, 1);

            int retries = 0;
            while (!ret && retries < 2)
            {
                try
                {
                    if (client.IsAvailable)
                    {
                        if (client.IsConnected == false)
                        {
                            var error = client.Open();
                            if (error != ErrorCode.NoError)
                            {
                                Logger.WriteDebugLog(error.ToString());
                            }
                            else
                            {
                                va = String.Format("DB{0}.DBW{1}", dbnum, addr);
                                u = (ushort)client.Read(va);

                                if (client.LastErrorCode == ErrorCode.NoError)
                                {
                                    val = u;
                                    ret = true;
                                }
                                else
                                {
                                    Logger.WriteDebugLog(string.Format("GetWord error: {0}", client.LastErrorCode));
                                }
                            }
                        }
                    }
                    else
                    {
                        Logger.WriteDebugLog("Not able to Ping the Machine: " + ipAddr);
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog(ex.Message);
                }
                finally
                {
                    if (client.IsConnected)
                    {
                        client.Close();
                    }
                }
                retries += 1;
            }
            return ret;
        }

        internal static bool PutWord(string ipAddr, string dbnum, string addr, string val)
        {
            Plc client = new Plc(CpuType.S71200, ipAddr, 0, 1);
            bool ret = false;
            string va = String.Empty;

            int retries = 0;
            while (!ret && retries < 2)
            {
                try
                {
                    if (client.IsAvailable)
                    {
                        if (client.IsConnected == false)
                        {
                            var error = client.Open();
                            if (error != ErrorCode.NoError)
                            {
                                Logger.WriteDebugLog(error.ToString());
                            }
                            else
                            {
                                va = String.Format("DB{0}.DBW{1}", dbnum, addr);
                                ErrorCode er = client.Write(va, val);

                                if (er == ErrorCode.NoError)
                                {
                                    ret = true;
                                }
                                else
                                {
                                    Logger.WriteDebugLog(string.Format("PutWord error: {0}", er));
                                }
                            }
                        }
                    }
                    else
                    {
                        Logger.WriteDebugLog("Not able to Ping the Machine: " + ipAddr);
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog(ex.Message);
                }
                finally
                {
                    if (client.IsConnected)
                    {
                        client.Close();
                    }
                }
                retries += 1;
            }
            return ret;
        }

        internal static bool GetDWord(string ipAddr, string dbnum, string addr, out uint val)
        {

            bool ret = false;
            string va = String.Empty;
            uint u = 0;
            val = 0;

            if (isPLCOffline) return false;


            Plc client = new Plc(CpuType.S71200, ipAddr, 0, 1);

            int retries = 0;
            while (!ret && retries < 2)
            {
                try
                {
                    if (client.IsAvailable)
                    {
                        if (client.IsConnected == false)
                        {
                            var error = client.Open();
                            if (error != ErrorCode.NoError)
                            {
                                Logger.WriteDebugLog(error.ToString());
                            }
                            else
                            {
                                va = String.Format("DB{0}.DBD{1}", dbnum, addr);
                                u = (uint)client.Read(va);

                                if (client.LastErrorCode == ErrorCode.NoError)
                                {
                                    val = u;
                                    ret = true;
                                }
                                else
                                {
                                    Logger.WriteDebugLog(string.Format("GetDWord error: {0}", client.LastErrorCode));
                                }
                            }
                        }
                    }
                    else
                    {
                        Logger.WriteDebugLog("Not able to Ping the Machine: " + ipAddr);
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog(ex.Message);
                }
                finally
                {
                    if (client.IsConnected)
                    {
                        client.Close();
                    }
                }
                retries += 1;
            }
            return ret;
        }

        internal static bool PutDWord(string ipAddr, string dbnum, string addr, string val)
        {
            Plc client = new Plc(CpuType.S71200, ipAddr, 0, 1);
            bool ret = false;
            string va = String.Empty;

            int retries = 0;
            while (!ret && retries < 2)
            {
                try
                {
                    if (client.IsAvailable)
                    {
                        if (client.IsConnected == false)
                        {
                            var error = client.Open();
                            if (error != ErrorCode.NoError)
                            {
                                Logger.WriteDebugLog(error.ToString());
                            }
                            else
                            {
                                va = String.Format("DB{0}.DBD{1}", dbnum, addr);
                                ErrorCode er = client.Write(va, val);

                                if (er == ErrorCode.NoError)
                                {
                                    ret = true;
                                }
                                else
                                {
                                    Logger.WriteDebugLog(string.Format("PutDWord error: {0}", er));
                                }
                            }
                        }
                    }
                    else
                    {
                        Logger.WriteDebugLog("Not able to Ping the Machine: " + ipAddr);
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog(ex.Message);
                }
                finally
                {
                    if (client.IsConnected)
                    {
                        client.Close();
                    }
                }
                retries += 1;
            }
            return ret;
        }
    }
}